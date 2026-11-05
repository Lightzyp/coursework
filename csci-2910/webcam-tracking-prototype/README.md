# Human Tracking & Object Detection Dataset

Clean presentation app for tracking humans, detecting common objects, and saving dataset-style camera samples.

## Requirements

- Python 3.8+
- See `requirements.txt` for packages.

## Run

1. Install dependencies:

```bash
pip install -r requirements.txt
```

2. Start the fullscreen presentation:

```bash
python webcam_object_tracker.py
```

Press `S` to save a presentation snapshot plus a YOLO-format image/label pair in `human_object_dataset`.

3. Or start the dashboard:

```bash
python app.py
```

4. Open http://localhost:5000 in your browser.

## Features

- Human tracking using YOLO person detections
- Object detection and tracking for the model's known object classes
- Minimal recruiter-facing fullscreen presentation
- Dataset sample export with images and YOLO label files
