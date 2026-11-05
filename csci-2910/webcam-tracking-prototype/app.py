import atexit
import threading
import time
import os
import urllib.request
from datetime import datetime

from flask import Flask, render_template, Response, jsonify, send_from_directory
import cv2
import mediapipe as mp
from ultralytics import YOLO

from camera_utils import DEFAULT_EXTERNAL_CAMERA_NAME, open_preferred_camera


APP_ROOT = os.path.dirname(os.path.abspath(__file__))
SNAPSHOT_FOLDER = os.path.join(APP_ROOT, "snapshots")
HAND_MODEL_PATH = os.path.join(APP_ROOT, "hand_landmarker.task")
HAND_MODEL_URL = "https://storage.googleapis.com/mediapipe-models/hand_landmarker/hand_landmarker/float16/1/hand_landmarker.task"
CAMERA_RETRY_SECONDS = 3
os.makedirs(SNAPSHOT_FOLDER, exist_ok=True)


def ensure_hand_model(model_path=HAND_MODEL_PATH):
    if os.path.exists(model_path):
        return model_path

    print(f"[INFO] Downloading hand landmarker model to {model_path}")
    urllib.request.urlretrieve(HAND_MODEL_URL, model_path)
    return model_path


class VideoCamera:
    def __init__(
        self,
        model_path="yolo11n.pt",
        camera_index=None,
        conf=0.30,
        preferred_camera_name=DEFAULT_EXTERNAL_CAMERA_NAME,
    ):
        self.cap, self.camera_index, self.camera_backend = open_preferred_camera(
            requested_index=camera_index,
            preferred_camera_name=preferred_camera_name,
        )
        self._configure_capture()

        self.model = YOLO(model_path)
        self.conf = conf
        self.enable_hand_tracking = False
        if self.enable_hand_tracking:
            hand_model_path = ensure_hand_model()
            self.mp_drawing = mp.tasks.vision.drawing_utils
            self.mp_drawing_styles = mp.tasks.vision.drawing_styles
            self.hand_connections = mp.tasks.vision.HandLandmarksConnections
            self.hand_landmarker = mp.tasks.vision.HandLandmarker.create_from_options(
                mp.tasks.vision.HandLandmarkerOptions(
                    base_options=mp.tasks.BaseOptions(model_asset_path=hand_model_path),
                    running_mode=mp.tasks.vision.RunningMode.VIDEO,
                    num_hands=2,
                    min_hand_detection_confidence=0.5,
                    min_hand_presence_confidence=0.5,
                    min_tracking_confidence=0.5,
                )
            )
        else:
            self.hand_landmarker = None
        self.frame_timestamp_ms = 0

        self.lock = threading.Lock()
        self.last_frame = None
        self.last_jpeg = None
        self.last_detection_count = 0
        self.last_human_count = 0
        self.last_hand_error = None

        self.running = True
        self.thread = threading.Thread(target=self._capture_loop, daemon=True)
        self.thread.start()

    def _configure_capture(self):
        self.cap.set(cv2.CAP_PROP_FOURCC, cv2.VideoWriter_fourcc(*"YUY2"))
        self.cap.set(cv2.CAP_PROP_FRAME_WIDTH, 640)
        self.cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)
        self.cap.set(cv2.CAP_PROP_FPS, 30)
        self.cap.set(cv2.CAP_PROP_BUFFERSIZE, 1)

    def _draw_overlay_text(self, frame, detection_count, human_count):
        lines = [
            f"Detections: {detection_count}",
            f"Humans: {human_count}",
        ]

        y = 30
        for line in lines:
            cv2.putText(
                frame,
                line,
                (10, y),
                cv2.FONT_HERSHEY_SIMPLEX,
                0.7,
                (255, 255, 255),
                2,
                cv2.LINE_AA,
            )
            y += 30

    def _annotate_hands(self, frame):
        rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=rgb_frame)
        hand_results = self.hand_landmarker.detect_for_video(mp_image, self.frame_timestamp_ms)
        hand_count = 0
        self.frame_timestamp_ms += 33

        if hand_results.hand_landmarks:
            hand_count = len(hand_results.hand_landmarks)
            for index, landmarks in enumerate(hand_results.hand_landmarks):
                self.mp_drawing.draw_landmarks(
                    frame,
                    landmarks,
                    self.hand_connections.HAND_CONNECTIONS,
                    self.mp_drawing_styles.get_default_hand_landmarks_style(),
                    self.mp_drawing_styles.get_default_hand_connections_style(),
                )

                if hand_results.handedness and index < len(hand_results.handedness):
                    handedness = hand_results.handedness[index][0].category_name
                    x_coordinates = [landmark.x for landmark in landmarks]
                    y_coordinates = [landmark.y for landmark in landmarks]
                    text_x = int(min(x_coordinates) * frame.shape[1])
                    text_y = max(int(min(y_coordinates) * frame.shape[0]) - 10, 20)
                    cv2.putText(
                        frame,
                        handedness,
                        (text_x, text_y),
                        cv2.FONT_HERSHEY_SIMPLEX,
                        0.6,
                        (88, 205, 54),
                        2,
                        cv2.LINE_AA,
                    )

        return frame, hand_count

    def _capture_loop(self):
        while self.running:
            ret, frame = self.cap.read()
            if not ret:
                time.sleep(0.05)
                continue

            # Run tracking / detection on the frame
            try:
                results = self.model.track(
                    source=frame,
                    persist=True,
                    conf=self.conf,
                    verbose=False,
                    imgsz=640,
                    max_det=30,
                    tracker="bytetrack.yaml",
                )
                result = results[0]
                annotated = result.plot()
                detection_count = 0
                human_count = 0
                if result.boxes is not None and len(result.boxes) > 0:
                    detection_count = len(result.boxes)
                    class_ids = result.boxes.cls.detach().cpu().numpy().astype(int)
                    human_count = int((class_ids == 0).sum())
            except Exception:
                # If model fails, fall back to raw frame
                annotated = frame
                detection_count = 0
                human_count = 0

            self._draw_overlay_text(annotated, detection_count, human_count)

            # Encode to JPEG
            ret2, jpeg = cv2.imencode('.jpg', annotated)
            if not ret2:
                time.sleep(0.01)
                continue

            with self.lock:
                self.last_frame = annotated
                self.last_jpeg = jpeg.tobytes()
                self.last_detection_count = detection_count
                self.last_human_count = human_count

            # small sleep to avoid pegging CPU
            time.sleep(0.01)

    def get_jpeg(self):
        with self.lock:
            return self.last_jpeg

    def get_stats(self):
        with self.lock:
            return {
                "detection_count": self.last_detection_count,
                "hand_count": self.last_human_count,
                "camera_index": self.camera_index,
                "camera_backend": self.camera_backend,
            }

    def save_snapshot(self):
        with self.lock:
            jpeg = self.last_jpeg
            frame = self.last_frame

        if jpeg is None or frame is None:
            return None

        timestamp = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
        filename = f"snapshot_{timestamp}.jpg"
        path = os.path.join(SNAPSHOT_FOLDER, filename)
        # write the already-annotated frame (BGR) as JPG
        cv2.imwrite(path, frame)
        return filename

    def stop(self):
        self.running = False
        self.thread.join(timeout=1.0)
        try:
            if self.hand_landmarker is not None:
                self.hand_landmarker.close()
        except Exception:
            pass
        try:
            self.cap.release()
        except Exception:
            pass


app = Flask(__name__)
camera = None
camera_error = None
last_camera_start_attempt = 0


def get_camera():
    global camera, camera_error, last_camera_start_attempt

    if camera is not None:
        return camera

    current_time = time.time()
    if current_time - last_camera_start_attempt < CAMERA_RETRY_SECONDS:
        return None

    last_camera_start_attempt = current_time
    try:
        camera = VideoCamera(
            model_path=os.environ.get("YOLO_MODEL", "yolo11n.pt"),
            camera_index=None,
            conf=float(os.environ.get("YOLO_CONFIDENCE", "0.30")),
            preferred_camera_name=os.environ.get(
                "PREFERRED_CAMERA_NAME",
                DEFAULT_EXTERNAL_CAMERA_NAME,
            ),
        )
        camera_error = None
    except Exception as exc:
        camera_error = f"{type(exc).__name__}: {exc}"
        print(f"[ERROR] Camera startup failed: {camera_error}")

    return camera


def stop_camera():
    global camera

    if camera is None:
        return

    camera.stop()
    camera = None


atexit.register(stop_camera)


@app.route('/')
def index():
    return render_template('index.html')


def mjpeg_generator():
    while True:
        active_camera = get_camera()
        if active_camera is None:
            time.sleep(0.1)
            continue
        jpeg = active_camera.get_jpeg()
        if jpeg is None:
            time.sleep(0.05)
            continue

        yield (b"--frame\r\nContent-Type: image/jpeg\r\nContent-Length: "
               + f"{len(jpeg)}".encode()
               + b"\r\n\r\n" + jpeg + b"\r\n")


@app.route('/video_feed')
def video_feed():
    return Response(mjpeg_generator(), mimetype='multipart/x-mixed-replace; boundary=frame')


@app.route('/snapshot', methods=['POST'])
def snapshot():
    active_camera = get_camera()
    if active_camera is None:
        return jsonify({'ok': False, 'error': camera_error or 'camera not ready'}), 503
    filename = active_camera.save_snapshot()
    if filename is None:
        return jsonify({'ok': False, 'error': 'no frame available'}), 500
    return jsonify({'ok': True, 'filename': filename})


@app.route('/stats')
def stats():
    active_camera = get_camera()
    snapshots = sorted(os.listdir(SNAPSHOT_FOLDER), reverse=True)
    snapshots = snapshots[:20]

    if active_camera is None:
        return jsonify({
            'detection_count': 0,
            'hand_count': 0,
            'camera_error': camera_error,
            'snapshot_count': len(snapshots),
            'snapshots': snapshots,
        })

    s = active_camera.get_stats()
    return jsonify({
        'detection_count': s.get('detection_count', 0),
        'hand_count': s.get('hand_count', 0),
        'camera_index': s.get('camera_index'),
        'camera_backend': s.get('camera_backend'),
        'snapshot_count': len(snapshots),
        'snapshots': snapshots,
    })


@app.route('/snapshots/<path:filename>')
def serve_snapshot(filename):
    return send_from_directory(SNAPSHOT_FOLDER, filename)


if __name__ == '__main__':
    get_camera()
    try:
        app.run(
            host=os.environ.get("HOST", "0.0.0.0"),
            port=int(os.environ.get("PORT", "5000")),
            debug=False,
        )
    finally:
        stop_camera()
