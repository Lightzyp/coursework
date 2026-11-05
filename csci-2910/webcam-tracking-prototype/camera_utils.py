import os
import subprocess
import time

import cv2


DEFAULT_EXTERNAL_CAMERA_NAME = "onn. USB 2.0 webcam"
DEFAULT_CAMERA_INDEX_ENV = "WEBCAM_INDEX"


def get_windows_camera_names():
    """
    Return camera friendly names in the order Windows reports them.
    OpenCV usually follows this order for DirectShow camera indices.
    """
    if os.name != "nt":
        return []

    command = (
        "Get-PnpDevice -Class Camera | "
        "Select-Object -ExpandProperty FriendlyName"
    )
    try:
        result = subprocess.run(
            ["powershell", "-NoProfile", "-Command", command],
            capture_output=True,
            text=True,
            check=False,
            timeout=5,
        )
    except Exception:
        return []

    return [line.strip() for line in result.stdout.splitlines() if line.strip()]


def is_camera_connected(camera_name):
    """
    Check whether a named camera appears in Windows device listings.
    """
    target_name = camera_name.strip().lower()
    return any(
        target_name in device_name.lower()
        for device_name in get_windows_camera_names()
    )


def get_env_camera_index(env_var=DEFAULT_CAMERA_INDEX_ENV):
    value = os.environ.get(env_var, "").strip()
    if not value:
        return None

    try:
        return int(value)
    except ValueError as exc:
        raise ValueError(f"{env_var} must be a webcam index number, got {value!r}") from exc


def get_preferred_camera_index_from_windows(
    preferred_camera_name=DEFAULT_EXTERNAL_CAMERA_NAME,
):
    target_name = preferred_camera_name.strip().lower()
    for index, device_name in enumerate(get_windows_camera_names()):
        if target_name in device_name.lower():
            return index
    return None


def _unique_ordered(values):
    seen = set()
    ordered = []
    for value in values:
        if value is None or value in seen:
            continue
        seen.add(value)
        ordered.append(value)
    return ordered


def get_camera_index_candidates(
    requested_index=None,
    preferred_camera_name=DEFAULT_EXTERNAL_CAMERA_NAME,
    max_index=5,
):
    env_index = get_env_camera_index()
    preferred_index = get_preferred_camera_index_from_windows(preferred_camera_name)

    leading_candidates = []
    if requested_index is not None:
        leading_candidates.append(requested_index)
    elif env_index is not None:
        leading_candidates.append(env_index)
    elif preferred_index is not None:
        leading_candidates.append(preferred_index)

    all_candidates = list(range(max_index + 1))
    return _unique_ordered(leading_candidates + all_candidates)


def get_backend_candidates():
    if os.name == "nt":
        return [
            ("DirectShow", cv2.CAP_DSHOW),
            ("Media Foundation", cv2.CAP_MSMF),
            ("Auto", cv2.CAP_ANY),
        ]
    return [("Auto", cv2.CAP_ANY)]


def _create_capture(index, backend):
    if backend == cv2.CAP_ANY:
        return cv2.VideoCapture(index)
    return cv2.VideoCapture(index, backend)


def _can_read_frame(capture, attempts=5):
    for _ in range(attempts):
        ok, frame = capture.read()
        if ok and frame is not None:
            return True
        time.sleep(0.05)
    return False


def open_preferred_camera(
    requested_index=None,
    preferred_camera_name=DEFAULT_EXTERNAL_CAMERA_NAME,
    max_index=5,
):
    """
    Open the preferred webcam and return (capture, index, backend_name).

    The old code assumed the preferred external webcam was always index 1.
    That breaks as soon as Windows has a virtual camera or built-in webcam
    ahead of it, so this probes the likely index and verifies it returns frames.
    """
    camera_names = get_windows_camera_names()
    if camera_names:
        print("[INFO] Windows cameras:")
        for index, device_name in enumerate(camera_names):
            print(f"[INFO]   {index}: {device_name}")

    candidates = get_camera_index_candidates(
        requested_index=requested_index,
        preferred_camera_name=preferred_camera_name,
        max_index=max_index,
    )
    backends = get_backend_candidates()
    attempted = []

    for index in candidates:
        for backend_name, backend in backends:
            attempted.append(f"{index} ({backend_name})")
            print(f"[INFO] Trying webcam index {index} with {backend_name}")
            capture = _create_capture(index, backend)
            if capture.isOpened() and _can_read_frame(capture):
                print(f"[INFO] Using webcam index {index} with {backend_name}")
                return capture, index, backend_name
            capture.release()

    raise RuntimeError(
        "Could not open a webcam that returns frames. "
        f"Tried: {', '.join(attempted)}. "
        f"Set the {DEFAULT_CAMERA_INDEX_ENV} environment variable to 0, 1, 2, etc. to force a camera."
    )


def get_preferred_camera_index(
    preferred_camera_name=DEFAULT_EXTERNAL_CAMERA_NAME,
    max_index=5,
):
    """
    Return the first working preferred camera index.
    """
    capture, index, _ = open_preferred_camera(
        requested_index=None,
        preferred_camera_name=preferred_camera_name,
        max_index=max_index,
    )
    capture.release()
    return index
