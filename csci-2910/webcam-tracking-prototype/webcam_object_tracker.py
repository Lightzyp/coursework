import os
import time
import urllib.request
import ctypes
from collections import deque
from collections import Counter
from datetime import datetime

import cv2
import mediapipe as mp
import numpy as np
from ultralytics import YOLO

from camera_utils import (
    DEFAULT_EXTERNAL_CAMERA_NAME,
    open_preferred_camera,
)


APP_ROOT = os.path.dirname(os.path.abspath(__file__))
HAND_MODEL_URL = "https://storage.googleapis.com/mediapipe-models/hand_landmarker/hand_landmarker/float16/1/hand_landmarker.task"
HAND_MODEL_PATH = os.path.join(APP_ROOT, "hand_landmarker.task")


def ensure_hand_model(model_path: str = HAND_MODEL_PATH) -> str:
    if os.path.exists(model_path):
        return model_path

    print(f"[INFO] Downloading hand landmarker model to {model_path}")
    urllib.request.urlretrieve(HAND_MODEL_URL, model_path)
    return model_path


class WebcamObjectTracker:
    def __init__(
        self,
        model_path: str = "yolo11n.pt",
        camera_index: int | None = None,
        min_confidence: float = 0.30,
        window_name: str = "Human Tracking & Object Detection Dataset",
        save_folder: str = "human_object_dataset",
        preferred_camera_name: str = DEFAULT_EXTERNAL_CAMERA_NAME,
    ):
        """
        Real-time webcam object detection and tracking.

        Args:
            model_path: YOLO model file name or local path.
            camera_index: Webcam index override. Leave as None to auto-prefer the external onn. webcam.
            min_confidence: Minimum confidence threshold for displayed detections.
            window_name: Name of the OpenCV display window.
            save_folder: Folder where snapshots will be stored.
            preferred_camera_name: Friendly device name to prefer when auto-selecting a webcam.
        """
        self.model_path = model_path
        self.preferred_camera_name = preferred_camera_name
        self.camera_index = camera_index
        self.min_confidence = min_confidence
        self.window_name = window_name
        self.save_folder = save_folder
        self.snapshot_folder = os.path.join(self.save_folder, "snapshots")
        self.dataset_image_folder = os.path.join(self.save_folder, "images")
        self.dataset_label_folder = os.path.join(self.save_folder, "labels")

        os.makedirs(self.save_folder, exist_ok=True)
        os.makedirs(self.snapshot_folder, exist_ok=True)
        os.makedirs(self.dataset_image_folder, exist_ok=True)
        os.makedirs(self.dataset_label_folder, exist_ok=True)

        print(f"[INFO] Loading model: {self.model_path}")
        self.model = YOLO(self.model_path)
        self.known_object_names = [
            name for _, name in sorted(self.model.names.items())
        ]
        print(
            f"[INFO] Object model loaded with {len(self.known_object_names)} known classes: "
            f"{', '.join(self.known_object_names)}"
        )
        hand_model_path = ensure_hand_model()
        self.mp_drawing = mp.tasks.vision.drawing_utils
        self.mp_drawing_styles = mp.tasks.vision.drawing_styles
        self.hand_connections = mp.tasks.vision.HandLandmarksConnections
        self.hands = mp.tasks.vision.HandLandmarker.create_from_options(
            mp.tasks.vision.HandLandmarkerOptions(
                base_options=mp.tasks.BaseOptions(model_asset_path=hand_model_path),
                running_mode=mp.tasks.vision.RunningMode.VIDEO,
                num_hands=2,
                min_hand_detection_confidence=0.5,
                min_hand_presence_confidence=0.5,
                min_tracking_confidence=0.5,
            )
        )
        self.frame_timestamp_ms = 0
        self.frame_index = 0
        self.hand_detection_interval = 4
        self.object_detection_interval = 1
        self.max_tracked_objects = 30
        self.pause_tracking_for_gestures = False
        self.show_hand_landmarks = False
        self.cached_hand_result = None
        self.cached_hand_count = 0
        self.cached_fingers_up = 0
        self.cached_hand_summary = []
        self.cached_detections = []
        self.cached_scene_objects = []
        self.cached_primary_detection = None
        self.object_results_ready = False
        self.finger_count_history = deque(maxlen=6)
        self.object_label_history = deque(maxlen=8)
        self.object_conf_history = deque(maxlen=8)
        self.object_absence_frames = 0
        # The onn. webcam returns black/corrupt frames when forced to 1280x720.
        # Its stable DirectShow mode is 640x480 YUY2, so keep capture and
        # processing at that size instead of stretching a broken feed.
        self.capture_size = (640, 480)
        self.processing_size = (640, 480)
        self.detection_imgsz = 640
        self.cached_background = None
        self.cached_background_size = None
        self.fps_history = deque(maxlen=20)

        self.cap, self.camera_index, self.camera_backend = open_preferred_camera(
            requested_index=camera_index,
            preferred_camera_name=self.preferred_camera_name,
        )

        self.configure_camera()

        self.last_snapshot_time = 0
        self.snapshot_cooldown_seconds = 1
        self.previous_frame_time = time.time()
        self.smoothed_fps = 0.0
        self.is_fullscreen = True
        self.screen_width, self.screen_height = self.get_screen_size()
        self.ui_colors = {
            "bg": (14, 14, 14),
            "panel": (24, 24, 24),
            "panel_soft": (36, 36, 36),
            "panel_glow": (48, 48, 48),
            "cyan": (212, 212, 212),
            "blue": (210, 166, 92),
            "gold": (90, 216, 142),
            "pink": (220, 220, 220),
            "human": (90, 216, 142),
            "object": (210, 166, 92),
            "text": (246, 246, 246),
            "muted": (168, 168, 168),
            "line": (82, 82, 82),
        }

    def configure_camera(self):
        """
        Try to get the cleanest webcam feed the device can reliably provide.
        """
        self.cap.set(cv2.CAP_PROP_FOURCC, cv2.VideoWriter_fourcc(*"YUY2"))
        self.cap.set(cv2.CAP_PROP_FRAME_WIDTH, self.capture_size[0])
        self.cap.set(cv2.CAP_PROP_FRAME_HEIGHT, self.capture_size[1])
        self.cap.set(cv2.CAP_PROP_FPS, 30)
        self.cap.set(cv2.CAP_PROP_BUFFERSIZE, 1)

        # Not every webcam supports these controls, so failures are harmless.
        try:
            self.cap.set(cv2.CAP_PROP_AUTOFOCUS, 1)
        except Exception:
            pass

        try:
            self.cap.set(cv2.CAP_PROP_AUTO_EXPOSURE, 0.75)
        except Exception:
            pass

    def get_ui_scale(self, frame):
        """
        Scale HUD elements to the active display size.
        """
        height, width = frame.shape[:2]
        return max(0.82, min(width / 1600.0, height / 900.0))

    def fit_text(self, text, max_chars):
        """
        Keep HUD text compact enough for small layout regions.
        """
        if len(text) <= max_chars:
            return text
        return text[: max(0, max_chars - 3)] + "..."

    def draw_status_card(self, frame, x, y, width, height, title, value, accent):
        """
        Draw a compact footer card.
        """
        overlay = frame.copy()
        cv2.rectangle(overlay, (x, y), (x + width, y + height), self.ui_colors["panel"], -1)
        cv2.addWeighted(overlay, 0.82, frame, 0.18, 0, frame)
        cv2.rectangle(frame, (x, y), (x + width, y + height), accent, 1)

        scale = self.get_ui_scale(frame)
        cv2.putText(
            frame,
            self.fit_text(title.upper(), 18),
            (x + 14, y + 22),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.42 * scale,
            self.ui_colors["muted"],
            1,
            cv2.LINE_AA,
        )
        cv2.putText(
            frame,
            self.fit_text(value, 24),
            (x + 14, y + height - 14),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.58 * scale,
            self.ui_colors["text"],
            2,
            cv2.LINE_AA,
        )

    def get_screen_size(self):
        """
        Get the current screen size so the display can fill the window cleanly.
        """
        try:
            user32 = ctypes.windll.user32
            return user32.GetSystemMetrics(0), user32.GetSystemMetrics(1)
        except Exception:
            return 1920, 1080

    def configure_window(self):
        """
        Create a resizable OpenCV window and launch it in fullscreen mode.
        """
        cv2.namedWindow(self.window_name, cv2.WINDOW_NORMAL)
        cv2.setWindowProperty(
            self.window_name,
            cv2.WND_PROP_FULLSCREEN,
            cv2.WINDOW_FULLSCREEN,
        )

    def toggle_fullscreen(self):
        """
        Toggle between fullscreen and windowed mode.
        """
        self.is_fullscreen = not self.is_fullscreen
        mode = cv2.WINDOW_FULLSCREEN if self.is_fullscreen else cv2.WINDOW_NORMAL
        cv2.setWindowProperty(self.window_name, cv2.WND_PROP_FULLSCREEN, mode)
        if not self.is_fullscreen:
            cv2.resizeWindow(self.window_name, 1400, 900)

    def write_yolo_labels(self, label_path, detections, image_shape):
        """
        Write dataset labels in YOLO normalized xywh format.
        """
        height, width = image_shape[:2]
        lines = []
        for detection in detections:
            class_id = detection.get("class_id")
            if class_id is None:
                continue

            x1, y1, x2, y2 = [float(value) for value in detection["box"]]
            x1 = max(0.0, min(x1, width - 1.0))
            x2 = max(0.0, min(x2, width - 1.0))
            y1 = max(0.0, min(y1, height - 1.0))
            y2 = max(0.0, min(y2, height - 1.0))

            box_width = max(0.0, x2 - x1)
            box_height = max(0.0, y2 - y1)
            if box_width <= 0.0 or box_height <= 0.0:
                continue

            x_center = (x1 + x2) / 2.0 / width
            y_center = (y1 + y2) / 2.0 / height
            normalized_width = box_width / width
            normalized_height = box_height / height
            lines.append(
                f"{class_id} {x_center:.6f} {y_center:.6f} "
                f"{normalized_width:.6f} {normalized_height:.6f}"
            )

        with open(label_path, "w", encoding="utf-8") as label_file:
            label_file.write("\n".join(lines))
            if lines:
                label_file.write("\n")

    def save_snapshot(self, presentation_frame, detections=None, dataset_frame=None):
        """
        Save a polished screenshot and a dataset-ready image/label pair.
        """
        current_time = time.time()
        if current_time - self.last_snapshot_time < self.snapshot_cooldown_seconds:
            return

        timestamp = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
        snapshot_filename = os.path.join(self.snapshot_folder, f"presentation_{timestamp}.jpg")
        image_filename = os.path.join(self.dataset_image_folder, f"sample_{timestamp}.jpg")
        label_filename = os.path.join(self.dataset_label_folder, f"sample_{timestamp}.txt")

        dataset_frame = dataset_frame if dataset_frame is not None else presentation_frame
        detections = detections or []

        cv2.imwrite(snapshot_filename, presentation_frame)
        cv2.imwrite(image_filename, dataset_frame)
        self.write_yolo_labels(label_filename, detections, dataset_frame.shape)
        self.last_snapshot_time = current_time
        print(f"[INFO] Presentation snapshot saved: {snapshot_filename}")
        print(f"[INFO] Dataset sample saved: {image_filename}")
        print(f"[INFO] Dataset labels saved: {label_filename}")

    def draw_help_text(self, frame):
        """
        Draw keyboard instructions on the screen.
        """
        instructions = [
            ("Q", "Quit"),
            ("S", "Save snapshot"),
            ("F", "Fullscreen toggle"),
        ]

        scale = self.get_ui_scale(frame)
        panel_width = int(300 * scale)
        panel_height = int(146 * scale)
        overlay = frame.copy()
        x1, y1 = int(24 * scale), int(24 * scale)
        x2, y2 = x1 + panel_width, y1 + panel_height
        cv2.rectangle(overlay, (x1, y1), (x2, y2), self.ui_colors["bg"], -1)
        cv2.rectangle(overlay, (x1, y1), (x2, y1 + int(30 * scale)), self.ui_colors["panel_glow"], -1)
        cv2.addWeighted(overlay, 0.8, frame, 0.2, 0, frame)
        cv2.rectangle(frame, (x1, y1), (x2, y2), self.ui_colors["cyan"], max(1, int(2 * scale)))
        cv2.line(
            frame,
            (x1 + int(16 * scale), y1 + int(16 * scale)),
            (x1 + int(42 * scale), y1 + int(16 * scale)),
            self.ui_colors["pink"],
            max(1, int(2 * scale)),
        )
        cv2.line(
            frame,
            (x1 + int(16 * scale), y1 + int(22 * scale)),
            (x1 + int(34 * scale), y1 + int(22 * scale)),
            self.ui_colors["gold"],
            max(1, int(2 * scale)),
        )

        cv2.putText(
            frame,
            "VISION CONTROL",
            (x1 + int(50 * scale), y1 + int(25 * scale)),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.58 * scale,
            self.ui_colors["cyan"],
            max(1, int(2 * scale)),
            cv2.LINE_AA,
        )
        cv2.putText(
            frame,
            "Keyboard shortcuts",
            (x1 + int(20 * scale), y1 + int(52 * scale)),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.42 * scale,
            self.ui_colors["muted"],
            1,
            cv2.LINE_AA,
        )

        x = x1 + int(22 * scale)
        y = y1 + int(82 * scale)
        for key_label, action_label in instructions:
            cv2.rectangle(
                frame,
                (x - int(2 * scale), y - int(18 * scale)),
                (x + int(28 * scale), y + int(4 * scale)),
                self.ui_colors["panel_soft"],
                -1,
            )
            cv2.rectangle(
                frame,
                (x - int(2 * scale), y - int(18 * scale)),
                (x + int(28 * scale), y + int(4 * scale)),
                self.ui_colors["blue"],
                1,
            )
            cv2.putText(
                frame,
                key_label,
                (x + int(3 * scale), y),
                cv2.FONT_HERSHEY_SIMPLEX,
                0.54 * scale,
                self.ui_colors["gold"],
                max(1, int(2 * scale)),
                cv2.LINE_AA,
            )
            cv2.putText(
                frame,
                action_label,
                (x + int(46 * scale), y),
                cv2.FONT_HERSHEY_SIMPLEX,
                0.5 * scale,
                self.ui_colors["text"],
                max(1, int(1.5 * scale)),
                cv2.LINE_AA,
            )
            y += int(28 * scale)

    def extract_detections(self, result):
        """
        Convert YOLO output into a simple list of visible detections.
        """
        boxes = result.boxes
        if boxes is None or len(boxes) == 0:
            return []

        detections = []
        confidences = boxes.conf.detach().cpu().numpy()
        xyxy_boxes = boxes.xyxy.detach().cpu().numpy().astype(int)
        class_ids = boxes.cls.detach().cpu().numpy().astype(int)
        track_ids = None
        if boxes.id is not None:
            track_ids = boxes.id.detach().cpu().numpy().astype(int)

        for index, confidence in enumerate(confidences):
            class_id = int(class_ids[index])
            box = xyxy_boxes[index]
            center_x = int((box[0] + box[2]) / 2)
            center_y = int((box[1] + box[3]) / 2)
            detections.append(
                {
                    "label": self.model.names.get(class_id, str(class_id)),
                    "class_id": class_id,
                    "confidence": float(confidence),
                    "track_id": int(track_ids[index]) if track_ids is not None else None,
                    "box": box,
                    "center": (center_x, center_y),
                }
            )

        detections.sort(key=lambda item: item["confidence"], reverse=True)
        return detections

    def summarize_scene_objects(self, detections):
        """
        Return a stable, readable list of visible object labels.
        """
        scene_labels = []
        label_counts = Counter(detection["label"] for detection in detections)
        for label, count in label_counts.most_common(8):
            if count > 1:
                scene_labels.append(f"{count}x {label}")
            else:
                scene_labels.append(label)
        return scene_labels

    def get_box_area(self, box):
        """
        Calculate area for an xyxy box.
        """
        return max(0, box[2] - box[0]) * max(0, box[3] - box[1])

    def get_overlap_ratio(self, box_a, box_b):
        """
        Measure overlap relative to the smaller box.
        """
        x1 = max(box_a[0], box_b[0])
        y1 = max(box_a[1], box_b[1])
        x2 = min(box_a[2], box_b[2])
        y2 = min(box_a[3], box_b[3])
        if x2 <= x1 or y2 <= y1:
            return 0.0

        intersection = (x2 - x1) * (y2 - y1)
        min_area = max(1, min(self.get_box_area(box_a), self.get_box_area(box_b)))
        return intersection / min_area

    def get_detection_priority_score(self, detection, hand_summary):
        """
        Score detections so held objects beat broad scene detections like person.
        """
        label = detection["label"].lower()
        box = detection["box"]
        box_area = max(1.0, float(self.get_box_area(box)))
        frame_area = float(self.processing_size[0] * self.processing_size[1])
        area_ratio = box_area / max(frame_area, 1.0)

        score = (detection["confidence"] * 100.0) + min(area_ratio, 0.35) * 40.0

        if label == "person":
            score += 20.0

        if label in {"cell phone", "bottle", "cup", "book", "remote", "mouse", "sports ball"}:
            score += 8.0

        if hand_summary:
            best_overlap = 0.0
            best_distance = 1.0
            frame_diag = max(
                (self.processing_size[0] ** 2 + self.processing_size[1] ** 2) ** 0.5,
                1.0,
            )

            for hand in hand_summary:
                overlap = self.get_overlap_ratio(hand["box"], box)
                hand_center = (
                    (hand["box"][0] + hand["box"][2]) / 2.0,
                    (hand["box"][1] + hand["box"][3]) / 2.0,
                )
                det_center = detection["center"]
                distance = (
                    ((hand_center[0] - det_center[0]) ** 2 + (hand_center[1] - det_center[1]) ** 2) ** 0.5
                ) / frame_diag
                best_overlap = max(best_overlap, overlap)
                best_distance = min(best_distance, distance)

            score += best_overlap * 130.0
            score += max(0.0, 1.0 - min(best_distance, 1.0)) * 28.0

            if best_overlap > 0.12 and label != "person":
                score += 20.0

        return score

    def get_primary_detection(self, detections, hand_summary):
        """
        Return the most relevant detection, favoring held objects over the user.
        """
        if not detections:
            return None

        close_hand_objects = []
        if hand_summary:
            for detection in detections:
                if detection["label"].lower() == "person":
                    continue
                if any(
                    self.get_overlap_ratio(hand["box"], detection["box"]) > 0.10
                    for hand in hand_summary
                ):
                    close_hand_objects.append(detection)

        candidate_pool = close_hand_objects if close_hand_objects else detections
        prioritized = sorted(
            candidate_pool,
            key=lambda item: self.get_detection_priority_score(item, hand_summary),
            reverse=True,
        )
        return prioritized[0].copy()

    def get_detection_color(self, detection, is_primary=False):
        """
        Use a quiet two-color system: humans and objects.
        """
        if detection["label"].lower() == "person":
            return self.ui_colors["human"]
        return self.ui_colors["object"]

    def is_primary_detection(self, detection, primary_detection):
        """
        Decide whether a detection is the selected object lock.
        """
        if primary_detection is None:
            return False

        if (
            detection["track_id"] is not None
            and primary_detection["track_id"] is not None
        ):
            return detection["track_id"] == primary_detection["track_id"]

        center_x, center_y = detection["center"]
        primary_x, primary_y = primary_detection["center"]
        return (
            detection["label"] == primary_detection["label"]
            and abs(center_x - primary_x) <= 8
            and abs(center_y - primary_y) <= 8
        )

    def draw_tracked_detections(self, frame, detections, primary_detection=None):
        """
        Draw clean dataset-style boxes for every tracked item.
        """
        for detection in detections:
            is_primary = self.is_primary_detection(detection, primary_detection)
            color = self.get_detection_color(detection, is_primary=is_primary)
            thickness = 2 if detection["label"].lower() == "person" else 1
            x1, y1, x2, y2 = [int(value) for value in detection["box"]]
            label = detection["label"]
            confidence = detection["confidence"]
            track_id = detection["track_id"]

            cv2.rectangle(frame, (x1, y1), (x2, y2), color, thickness)
            cv2.circle(frame, detection["center"], 3, color, -1)

            display_label = "human" if label.lower() == "person" else label
            badge_text = f"{display_label} {confidence:.2f}"
            if track_id is not None:
                badge_text += f" #{track_id}"

            font_scale = 0.48
            font_thickness = 1
            (text_width, text_height), _ = cv2.getTextSize(
                badge_text,
                cv2.FONT_HERSHEY_SIMPLEX,
                font_scale,
                font_thickness,
            )
            badge_x1 = max(0, min(x1, frame.shape[1] - text_width - 16))
            badge_y1 = max(y1 - text_height - 12, 4)
            badge_x2 = min(frame.shape[1] - 1, badge_x1 + text_width + 12)
            badge_y2 = badge_y1 + text_height + 8
            overlay = frame.copy()
            cv2.rectangle(overlay, (badge_x1, badge_y1), (badge_x2, badge_y2), self.ui_colors["bg"], -1)
            cv2.addWeighted(overlay, 0.72, frame, 0.28, 0, frame)
            cv2.rectangle(frame, (badge_x1, badge_y1), (badge_x2, badge_y2), color, 1)
            cv2.putText(
                frame,
                badge_text,
                (badge_x1 + 6, badge_y2 - 6),
                cv2.FONT_HERSHEY_SIMPLEX,
                font_scale,
                self.ui_colors["text"],
                font_thickness,
                cv2.LINE_AA,
            )

    def get_detection_counts(self, detections):
        human_count = sum(1 for detection in detections if detection["label"].lower() == "person")
        object_count = max(0, len(detections) - human_count)
        return human_count, object_count

    def draw_primary_detection(self, frame, detection):
        """
        Draw the single selected object with a clearer HUD-style label.
        """
        if detection is None:
            return

        x1, y1, x2, y2 = detection["box"]
        label = detection["label"]
        confidence = detection["confidence"]
        track_id = detection["track_id"]
        box_color = (76, 168, 255)

        cv2.rectangle(frame, (x1, y1), (x2, y2), box_color, 3)
        cv2.circle(frame, detection["center"], 5, (124, 240, 200), -1)

        badge_text = f"{label} {confidence:.2f}"
        if track_id is not None:
            badge_text += f"  ID {track_id}"

        (text_width, text_height), _ = cv2.getTextSize(
            badge_text,
            cv2.FONT_HERSHEY_SIMPLEX,
            0.6,
            2,
        )
        badge_y1 = max(y1 - text_height - 18, 8)
        badge_y2 = badge_y1 + text_height + 12
        badge_x2 = x1 + text_width + 18
        cv2.rectangle(frame, (x1, badge_y1), (badge_x2, badge_y2), box_color, -1)
        cv2.putText(
            frame,
            badge_text,
            (x1 + 8, badge_y2 - 8),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.6,
            (8, 17, 31),
            2,
            cv2.LINE_AA,
        )

    def stabilize_detection(self, detection):
        """
        Smooth noisy class predictions so small objects do not flicker between labels.
        """
        if detection is None:
            self.object_label_history.clear()
            self.object_conf_history.clear()
            return None

        self.object_label_history.append(detection["label"])
        self.object_conf_history.append(detection["confidence"])
        label_counts = Counter(self.object_label_history)
        stable_label, stable_votes = label_counts.most_common(1)[0]

        if stable_votes >= 3:
            detection["label"] = stable_label

        detection["confidence"] = max(self.object_conf_history)
        return detection

    def should_show_object_ui(self, detection):
        """
        Keep object-tracking chrome visible only when objects are actively present.
        """
        if detection is not None:
            self.object_absence_frames = 0
            return True

        self.object_absence_frames += 1
        return self.object_absence_frames <= 8

    def get_finger_states_for_hand(self, landmarks, handedness_label):
        """
        Detect which fingers are extended for one hand.
        """
        finger_states = {
            "thumb": False,
            "index": False,
            "middle": False,
            "ring": False,
            "pinky": False,
        }

        fingertip_pairs = [
            ("index", 8, 6, 5),
            ("middle", 12, 10, 9),
            ("ring", 16, 14, 13),
            ("pinky", 20, 18, 17),
        ]

        for finger_name, tip_idx, pip_idx, mcp_idx in fingertip_pairs:
            if (
                landmarks[tip_idx].y < landmarks[pip_idx].y
                and landmarks[pip_idx].y < landmarks[mcp_idx].y
            ):
                finger_states[finger_name] = True

        thumb_tip = landmarks[4]
        thumb_ip = landmarks[3]
        thumb_mcp = landmarks[2]
        index_mcp = landmarks[5]
        pinky_mcp = landmarks[17]
        wrist = landmarks[0]
        palm_center_x = (wrist.x + index_mcp.x + pinky_mcp.x) / 3.0
        palm_center_y = (wrist.y + index_mcp.y + pinky_mcp.y) / 3.0

        def distance(point_a, point_b):
            return ((point_a.x - point_b.x) ** 2 + (point_a.y - point_b.y) ** 2) ** 0.5

        def distance_to_xy(point, x_value, y_value):
            return ((point.x - x_value) ** 2 + (point.y - y_value) ** 2) ** 0.5

        def joint_angle(point_a, point_b, point_c):
            vector_ba = np.array([point_a.x - point_b.x, point_a.y - point_b.y], dtype=float)
            vector_bc = np.array([point_c.x - point_b.x, point_c.y - point_b.y], dtype=float)
            denominator = np.linalg.norm(vector_ba) * np.linalg.norm(vector_bc)
            if denominator == 0:
                return 0.0
            cosine_value = np.clip(np.dot(vector_ba, vector_bc) / denominator, -1.0, 1.0)
            return float(np.degrees(np.arccos(cosine_value)))

        thumb_angle = joint_angle(thumb_tip, thumb_ip, thumb_mcp)
        palm_width = max(distance(index_mcp, pinky_mcp), 1e-6)
        thumb_extension = distance(thumb_tip, thumb_mcp) / palm_width
        thumb_index_distance = distance(thumb_tip, index_mcp)
        thumb_wrist_distance = distance(thumb_tip, wrist) / palm_width
        thumb_palm_distance = distance_to_xy(thumb_tip, palm_center_x, palm_center_y) / palm_width
        thumb_progression_ok = (
            distance_to_xy(thumb_tip, palm_center_x, palm_center_y)
            > distance_to_xy(thumb_ip, palm_center_x, palm_center_y)
            > distance_to_xy(thumb_mcp, palm_center_x, palm_center_y)
        )
        thumb_spread_x = abs(thumb_tip.x - thumb_mcp.x) / palm_width
        thumb_spread_y = abs(thumb_tip.y - thumb_mcp.y) / palm_width

        if (
            thumb_progression_ok
            and thumb_angle > 128
            and thumb_extension > 0.34
            and thumb_index_distance > (0.26 * palm_width)
            and thumb_wrist_distance > 0.85
            and thumb_palm_distance > 0.72
            and (thumb_spread_x > 0.18 or thumb_spread_y > 0.22)
        ):
            finger_states["thumb"] = True

        return finger_states

    def classify_gesture(self, finger_states):
        """
        Create a simple demo-friendly gesture label from finger states.
        """
        fingers_up = sum(int(is_up) for is_up in finger_states.values())

        if fingers_up == 5:
            return "Open Palm"
        if fingers_up == 0:
            return "Fist"
        if finger_states["thumb"] and not any(
            finger_states[name] for name in ["index", "middle", "ring", "pinky"]
        ):
            return "Thumbs Up"
        if (
            finger_states["index"]
            and finger_states["middle"]
            and not finger_states["ring"]
            and not finger_states["pinky"]
        ):
            return "Peace"
        if finger_states["index"] and not any(
            finger_states[name] for name in ["middle", "ring", "pinky"]
        ):
            return "Point"

        return f"{fingers_up} Fingers"

    def should_pause_object_tracking(self, hand_summary, detections=None):
        """
        Pause object tracking only for clear free-hand gestures, not while holding items.
        """
        if not self.pause_tracking_for_gestures:
            return False

        free_hands = hand_summary
        if detections is not None:
            free_hands = self.filter_free_hands(hand_summary, detections)

        for hand in free_hands:
            if hand["gesture"] in {"Open Palm", "Peace", "Point", "4 Fingers"}:
                return True
            if hand["fingers_up"] >= 3:
                return True
        return False

    def summarize_hands(self, hand_results):
        """
        Build a compact summary of detected hands and raised fingers.
        """
        summary = []
        if not hand_results or not hand_results.hand_landmarks:
            return summary

        for index, landmarks in enumerate(hand_results.hand_landmarks):
            handedness_label = "Hand"
            if hand_results.handedness and index < len(hand_results.handedness):
                handedness_label = hand_results.handedness[index][0].category_name

            finger_states = self.get_finger_states_for_hand(landmarks, handedness_label)
            fingers_up = sum(int(is_up) for is_up in finger_states.values())
            x_coordinates = [landmark.x for landmark in landmarks]
            y_coordinates = [landmark.y for landmark in landmarks]
            hand_box = [
                int(min(x_coordinates) * self.processing_size[0]),
                int(min(y_coordinates) * self.processing_size[1]),
                int(max(x_coordinates) * self.processing_size[0]),
                int(max(y_coordinates) * self.processing_size[1]),
            ]
            summary.append(
                {
                    "label": handedness_label,
                    "fingers_up": fingers_up,
                    "gesture": self.classify_gesture(finger_states),
                    "finger_states": finger_states,
                    "box": hand_box,
                    "landmarks": landmarks,
                }
            )

        return summary

    def filter_free_hands(self, hand_summary, detections):
        """
        Only keep hands that look free and not busy holding an object.
        """
        free_hands = []
        for hand in hand_summary:
            open_hand = hand["fingers_up"] >= 3 or hand["gesture"] in {"Open Palm", "4 Fingers"}
            overlaps_object = any(
                self.get_overlap_ratio(hand["box"], detection["box"]) > 0.22
                for detection in detections
            )
            if open_hand and not overlaps_object:
                free_hands.append(hand)
        return free_hands

    def update_finger_count_cache(self, fingers_up):
        """
        Smooth the finger count so it is less jumpy frame to frame.
        """
        self.finger_count_history.append(int(fingers_up))
        counts = {}
        for value in self.finger_count_history:
            counts[value] = counts.get(value, 0) + 1
        return max(counts, key=lambda key: (counts[key], key))

    def update_fps(self):
        """
        Update the smoothed FPS counter.
        """
        current_time = time.time()
        frame_delta = max(current_time - self.previous_frame_time, 1e-6)
        instant_fps = 1.0 / frame_delta
        self.fps_history.append(instant_fps)
        history_average = sum(self.fps_history) / len(self.fps_history)
        if self.smoothed_fps == 0.0:
            self.smoothed_fps = history_average
        else:
            self.smoothed_fps = (self.smoothed_fps * 0.9) + (history_average * 0.1)
        self.previous_frame_time = current_time

    def draw_hands(self, frame, hand_summary):
        """
        Draw cached or fresh hand landmarks on the current frame.
        """
        total_fingers_up = 0
        for hand in hand_summary:
            landmarks = hand["landmarks"]
            total_fingers_up += hand["fingers_up"]

            self.mp_drawing.draw_landmarks(
                frame,
                landmarks,
                self.hand_connections.HAND_CONNECTIONS,
                self.mp_drawing_styles.get_default_hand_landmarks_style(),
                self.mp_drawing_styles.get_default_hand_connections_style(),
            )

            x_coordinates = [landmark.x for landmark in landmarks]
            y_coordinates = [landmark.y for landmark in landmarks]
            text_x = int(min(x_coordinates) * frame.shape[1])
            text_y = max(int(min(y_coordinates) * frame.shape[0]) - 14, 24)
            hand_text = f"{hand['fingers_up']} up"
            shadow_position = (text_x + 2, text_y + 2)
            cv2.putText(
                frame,
                hand_text,
                shadow_position,
                cv2.FONT_HERSHEY_SIMPLEX,
                0.66,
                self.ui_colors["bg"],
                4,
                cv2.LINE_AA,
            )
            cv2.putText(
                frame,
                hand_text,
                (text_x, text_y),
                cv2.FONT_HERSHEY_SIMPLEX,
                0.66,
                self.ui_colors["cyan"],
                2,
                cv2.LINE_AA,
            )

        return len(hand_summary), total_fingers_up

    def get_hand_results(self, frame):
        """
        Run hand detection every few frames and reuse the most recent result between runs.
        """
        should_refresh = (
            self.cached_hand_result is None
            or self.frame_index % self.hand_detection_interval == 0
        )

        if should_refresh:
            rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=rgb_frame)
            self.cached_hand_result = self.hands.detect_for_video(mp_image, self.frame_timestamp_ms)
            self.cached_hand_summary = self.summarize_hands(self.cached_hand_result)
            self.cached_hand_count = len(self.cached_hand_summary)
            self.cached_fingers_up = sum(hand["fingers_up"] for hand in self.cached_hand_summary)
            self.frame_timestamp_ms += 33 * self.hand_detection_interval

        return (
            self.cached_hand_result,
            self.cached_hand_summary,
            self.cached_hand_count,
            self.cached_fingers_up,
        )

    def get_object_results(self, frame, hand_summary):
        """
        Run YOLO tracking and reuse the latest detections between refreshes.
        """
        should_refresh = (
            not self.object_results_ready
            or self.frame_index % self.object_detection_interval == 0
        )

        if should_refresh:
            results = self.model.track(
                source=frame,
                persist=True,
                conf=self.min_confidence,
                verbose=False,
                imgsz=self.detection_imgsz,
                max_det=self.max_tracked_objects,
                tracker="bytetrack.yaml",
            )
            result = results[0]
            self.cached_detections = self.extract_detections(result)
            self.cached_scene_objects = self.summarize_scene_objects(self.cached_detections)
            self.cached_primary_detection = self.get_primary_detection(
                self.cached_detections,
                hand_summary,
            )
            self.object_results_ready = True

        cached_primary = self.cached_primary_detection.copy() if self.cached_primary_detection is not None else None
        return self.cached_detections, self.cached_scene_objects, cached_primary

    def draw_showcase_badges(self, frame, hand_summary, detection, scene_objects):
        """
        Draw a simple demo badge and current gesture for live showcase moments.
        """
        object_count = len(self.cached_detections)
        if self.should_pause_object_tracking(hand_summary, self.cached_detections):
            headline = hand_summary[0]["gesture"]
            subline = "Object tracking paused while hand gesture is active"
            accent = self.ui_colors["gold"]
        elif detection is not None:
            object_word = "object" if object_count == 1 else "objects"
            headline = f"Tracking {object_count} {object_word}"
            subline = f"Visible: {', '.join(scene_objects) if scene_objects else detection['label']}"
            accent = self.ui_colors["gold"]
        elif hand_summary:
            headline = hand_summary[0]["gesture"]
            subline = "Hand tracking active"
            accent = self.ui_colors["cyan"]
        else:
            headline = "Scanning scene"
            subline = "Waiting for an object or hand in frame"
            accent = self.ui_colors["blue"]

        scale = self.get_ui_scale(frame)
        overlay = frame.copy()
        badge_w = int(320 * scale)
        badge_h = int(74 * scale)
        badge_x = frame.shape[1] - badge_w - int(28 * scale)
        badge_y = int(28 * scale)
        cv2.rectangle(overlay, (badge_x, badge_y), (badge_x + badge_w, badge_y + badge_h), self.ui_colors["bg"], -1)
        cv2.addWeighted(overlay, 0.76, frame, 0.24, 0, frame)
        cv2.rectangle(frame, (badge_x, badge_y), (badge_x + badge_w, badge_y + badge_h), accent, max(1, int(2 * scale)))
        cv2.rectangle(
            frame,
            (badge_x, badge_y),
            (badge_x + badge_w, badge_y + int(26 * scale)),
            self.ui_colors["panel_soft"],
            -1,
        )
        cv2.putText(
            frame,
            "DEMO STATUS",
            (badge_x + int(14 * scale), badge_y + int(19 * scale)),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.46 * scale,
            self.ui_colors["cyan"],
            max(1, int(1.5 * scale)),
            cv2.LINE_AA,
        )
        cv2.putText(
            frame,
            self.fit_text(headline, 24),
            (badge_x + int(14 * scale), badge_y + int(48 * scale)),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.7 * scale,
            self.ui_colors["text"],
            max(1, int(2 * scale)),
            cv2.LINE_AA,
        )
        cv2.putText(
            frame,
            self.fit_text(subline, 44),
            (badge_x + int(14 * scale), badge_y + int(65 * scale)),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.38 * scale,
            self.ui_colors["muted"],
            1,
            cv2.LINE_AA,
        )

    def draw_footer_bar(self, frame, detection, hand_count, fingers_up, total_candidates, hand_summary, scene_objects):
        """
        Draw one organized bottom HUD with all real-time data.
        """
        scale = self.get_ui_scale(frame)
        bar_height = int(174 * scale)
        overlay = frame.copy()
        cv2.rectangle(
            overlay,
            (0, frame.shape[0] - bar_height),
            (frame.shape[1], frame.shape[0]),
            self.ui_colors["bg"],
            -1,
        )
        cv2.addWeighted(overlay, 0.75, frame, 0.25, 0, frame)
        cv2.line(
            frame,
            (0, frame.shape[0] - bar_height),
            (frame.shape[1], frame.shape[0] - bar_height),
            self.ui_colors["blue"],
            2,
        )

        show_object_ui = self.should_show_object_ui(detection)
        tracking_paused = self.should_pause_object_tracking(hand_summary, self.cached_detections)
        gesture_text = hand_summary[0]["gesture"] if hand_summary else "None"
        object_text = ", ".join(scene_objects) if scene_objects else str(total_candidates)
        time_text = datetime.now().strftime("%H:%M:%S")

        if tracking_paused:
            cards = [
                ("Mode", "Tracking paused", self.ui_colors["gold"]),
                ("Gesture", gesture_text, self.ui_colors["cyan"]),
                ("Hands", str(hand_count), self.ui_colors["blue"]),
                ("Fingers Up", str(fingers_up), self.ui_colors["pink"]),
                ("Objects", object_text, self.ui_colors["muted"]),
                ("Time", time_text, self.ui_colors["muted"]),
            ]
            status_note = "Lower your hand to resume object lock"
        elif show_object_ui and detection is not None:
            cards = [
                ("Tracking", f"{total_candidates} object{'s' if total_candidates != 1 else ''}", self.ui_colors["gold"]),
                ("Lock", detection["label"], self.ui_colors["cyan"]),
                ("Track ID", str(detection["track_id"]) if detection["track_id"] is not None else "--", self.ui_colors["blue"]),
                ("Confidence", f"{detection['confidence']:.2f}", self.ui_colors["pink"]),
                ("Hands", str(hand_count), self.ui_colors["muted"]),
                ("Known Classes", str(len(self.known_object_names)), self.ui_colors["muted"]),
            ]
            status_note = f"Objects in scene: {self.fit_text(object_text, 52)}"
        else:
            cards = [
                ("Mode", "No object locked", self.ui_colors["gold"]),
                ("Gesture", gesture_text, self.ui_colors["cyan"]),
                ("Hands", str(hand_count), self.ui_colors["blue"]),
                ("Fingers Up", str(fingers_up), self.ui_colors["pink"]),
                ("Objects", object_text, self.ui_colors["muted"]),
                ("Time", time_text, self.ui_colors["muted"]),
            ]
            status_note = "Object tracking will appear automatically"

        cv2.putText(
            frame,
            "LIVE STATUS",
            (int(32 * scale), frame.shape[0] - bar_height + int(28 * scale)),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.6 * scale,
            self.ui_colors["cyan"],
            max(1, int(2 * scale)),
            cv2.LINE_AA,
        )

        inner_x = int(32 * scale)
        inner_y = frame.shape[0] - bar_height + int(44 * scale)
        inner_w = frame.shape[1] - int(64 * scale)
        gap = int(14 * scale)
        card_width = max(140, int((inner_w - (gap * 2)) / 3))
        card_height = int(46 * scale)

        for index, (title, value, accent) in enumerate(cards):
            row = index // 3
            col = index % 3
            card_x = inner_x + (col * (card_width + gap))
            card_y = inner_y + (row * (card_height + gap))
            self.draw_status_card(frame, card_x, card_y, card_width, card_height, title, value, accent)

        note_y = inner_y + (2 * (card_height + gap)) + int(8 * scale)
        cv2.putText(
            frame,
            f"FPS {self.smoothed_fps:.1f}   |   {self.fit_text(status_note, 70)}",
            (inner_x, note_y),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.5 * scale,
            self.ui_colors["muted"],
            max(1, int(1.5 * scale)),
            cv2.LINE_AA,
        )

    def draw_modern_frame_chrome(self, frame):
        """
        Add subtle modern framing accents for a cleaner demo look.
        """
        h, w = frame.shape[:2]
        accent = self.ui_colors["blue"]
        glow = self.ui_colors["cyan"]
        corner = 28
        line = 4

        corners = [
            ((24, 24), (24 + corner, 24), (24, 24 + corner)),
            ((w - 24, 24), (w - 24 - corner, 24), (w - 24, 24 + corner)),
            ((24, h - 24), (24 + corner, h - 24), (24, h - 24 - corner)),
            ((w - 24, h - 24), (w - 24 - corner, h - 24), (w - 24, h - 24 - corner)),
        ]
        for origin, horizontal, vertical in corners:
            cv2.line(frame, origin, horizontal, accent, line)
            cv2.line(frame, origin, vertical, glow, line)

    def build_display_frame(self, frame):
        """
        Scale the camera frame into a full-screen presentation canvas.
        """
        screen_w = max(self.screen_width, 1280)
        screen_h = max(self.screen_height, 720)
        frame_h, frame_w = frame.shape[:2]

        scale = min(screen_w / frame_w, screen_h / frame_h)
        scaled_w = max(1, int(frame_w * scale))
        scaled_h = max(1, int(frame_h * scale))
        resized = cv2.resize(frame, (scaled_w, scaled_h), interpolation=cv2.INTER_LINEAR)

        if self.cached_background is None or self.cached_background_size != (screen_w, screen_h):
            self.cached_background = np.full(
                (screen_h, screen_w, 3),
                self.ui_colors["bg"],
                dtype=np.uint8,
            )
            self.cached_background_size = (screen_w, screen_h)

        canvas = self.cached_background.copy()

        x_offset = (screen_w - scaled_w) // 2
        y_offset = (screen_h - scaled_h) // 2
        canvas[y_offset:y_offset + scaled_h, x_offset:x_offset + scaled_w] = resized

        cv2.rectangle(
            canvas,
            (max(0, x_offset - 1), max(0, y_offset - 1)),
            (min(screen_w - 1, x_offset + scaled_w + 1), min(screen_h - 1, y_offset + scaled_h + 1)),
            self.ui_colors["line"],
            1,
        )

        return canvas

    def draw_metric_chip(self, frame, x, y, label, value, accent):
        """
        Draw a compact, minimal metric chip.
        """
        width = 152
        height = 48
        overlay = frame.copy()
        cv2.rectangle(overlay, (x, y), (x + width, y + height), self.ui_colors["panel"], -1)
        cv2.addWeighted(overlay, 0.86, frame, 0.14, 0, frame)
        cv2.rectangle(frame, (x, y), (x + width, y + height), self.ui_colors["line"], 1)
        cv2.line(frame, (x, y), (x, y + height), accent, 3)
        cv2.putText(
            frame,
            label.upper(),
            (x + 14, y + 18),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.38,
            self.ui_colors["muted"],
            1,
            cv2.LINE_AA,
        )
        cv2.putText(
            frame,
            str(value),
            (x + 14, y + 39),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.7,
            self.ui_colors["text"],
            2,
            cv2.LINE_AA,
        )

    def draw_presentation_overlay(self, frame, detections, scene_objects):
        """
        Draw the recruiter-facing minimal header and footer.
        """
        human_count, object_count = self.get_detection_counts(detections)
        width = frame.shape[1]
        compact_header = width < 1500
        top_height = 136 if compact_header else 84
        bottom_height = 44

        overlay = frame.copy()
        cv2.rectangle(overlay, (0, 0), (width, top_height), self.ui_colors["bg"], -1)
        cv2.rectangle(
            overlay,
            (0, frame.shape[0] - bottom_height),
            (width, frame.shape[0]),
            self.ui_colors["bg"],
            -1,
        )
        cv2.addWeighted(overlay, 0.82, frame, 0.18, 0, frame)
        cv2.line(frame, (0, top_height), (width, top_height), self.ui_colors["line"], 1)
        cv2.line(
            frame,
            (0, frame.shape[0] - bottom_height),
            (width, frame.shape[0] - bottom_height),
            self.ui_colors["line"],
            1,
        )

        cv2.putText(
            frame,
            "Human Tracking & Object Detection Dataset",
            (28, 34),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.78,
            self.ui_colors["text"],
            2,
            cv2.LINE_AA,
        )
        cv2.putText(
            frame,
            f"YOLO tracking | camera {self.camera_index} {self.camera_backend} | "
            f"{len(self.known_object_names)} known classes",
            (30, 62),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.44,
            self.ui_colors["muted"],
            1,
            cv2.LINE_AA,
        )

        metrics = [
            ("Humans", human_count, self.ui_colors["human"]),
            ("Objects", object_count, self.ui_colors["object"]),
            ("Tracks", len(detections), self.ui_colors["text"]),
            ("FPS", f"{self.smoothed_fps:.1f}", self.ui_colors["muted"]),
        ]
        chip_gap = 12
        chip_width = 152
        total_width = (chip_width * len(metrics)) + (chip_gap * (len(metrics) - 1))
        start_x = 28 if compact_header else max(28, width - total_width - 28)
        chip_y = 76 if compact_header else 18
        for index, (label, value, accent) in enumerate(metrics):
            self.draw_metric_chip(
                frame,
                start_x + index * (chip_width + chip_gap),
                chip_y,
                label,
                value,
                accent,
            )

        visible_text = ", ".join(scene_objects) if scene_objects else "No tracked detections"
        footer_text = (
            f"Visible: {self.fit_text(visible_text, 72)}   |   "
            "S save dataset sample   F fullscreen   Q quit"
        )
        cv2.putText(
            frame,
            footer_text,
            (28, frame.shape[0] - 16),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.46,
            self.ui_colors["muted"],
            1,
            cv2.LINE_AA,
        )

    def process_frame(self, frame):
        """
        Run tracking on a single frame and return an annotated frame.
        """
        annotated_frame = frame.copy()
        if self.show_hand_landmarks:
            _, all_hand_summary, hand_count, fingers_up = self.get_hand_results(frame)
        else:
            all_hand_summary = []
            hand_count = 0
            fingers_up = 0

        detections, scene_objects, primary_detection = self.get_object_results(frame, all_hand_summary)
        if self.show_hand_landmarks:
            hand_count, fingers_up = self.draw_hands(annotated_frame, all_hand_summary)

        fingers_up = self.update_finger_count_cache(
            fingers_up if all_hand_summary else 0
        )

        tracking_paused = self.should_pause_object_tracking(all_hand_summary, detections)
        if tracking_paused:
            primary_detection = None
            self.object_label_history.clear()
            self.object_conf_history.clear()
        else:
            primary_detection = self.stabilize_detection(primary_detection)
            if detections:
                self.draw_tracked_detections(
                    annotated_frame,
                    detections,
                    primary_detection,
                )

        self.update_fps()
        total_candidates = len(detections)

        return (
            annotated_frame,
            primary_detection,
            hand_count,
            fingers_up,
            total_candidates,
            all_hand_summary,
            scene_objects,
        )

    def run(self):
        """
        Start the webcam loop.
        """
        print("[INFO] Starting webcam detection. Press 'q' to quit.")
        self.configure_window()

        try:
            while True:
                ret, frame = self.cap.read()
                if not ret:
                    print("[ERROR] Failed to read frame from webcam.")
                    break

                if (frame.shape[1], frame.shape[0]) != self.processing_size:
                    frame = cv2.resize(frame, self.processing_size, interpolation=cv2.INTER_LINEAR)
                dataset_frame = frame.copy()
                self.frame_index += 1

                annotated_frame, primary_detection, hand_count, fingers_up, detection_count, hand_summary, scene_objects = self.process_frame(frame)
                display_frame = self.build_display_frame(annotated_frame)
                self.draw_presentation_overlay(
                    display_frame,
                    self.cached_detections,
                    scene_objects,
                )
                cv2.imshow(self.window_name, display_frame)

                key = cv2.waitKey(1) & 0xFF
                if key == ord("q"):
                    print("[INFO] Quitting...")
                    break
                elif key == ord("s"):
                    self.save_snapshot(display_frame, self.cached_detections, dataset_frame)
                elif key == ord("f"):
                    self.toggle_fullscreen()

        finally:
            self.hands.close()
            self.cap.release()
            cv2.destroyAllWindows()
            print("[INFO] Webcam released and windows closed.")


if __name__ == "__main__":
    tracker = WebcamObjectTracker(
        model_path="yolo11n.pt",   # Lighter model keeps the webcam feed smoother
        camera_index=None,         # Auto-select the external onn. webcam when it is connected
        min_confidence=0.30,
        window_name="Human Tracking & Object Detection Dataset",
        save_folder="human_object_dataset",
        preferred_camera_name=DEFAULT_EXTERNAL_CAMERA_NAME,
    )
    tracker.run()
