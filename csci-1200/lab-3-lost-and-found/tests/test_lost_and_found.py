"""Independent black-box tests for the CSCI 1200 Lab 3 Campus Lost and Found program.

Every test copies the program into a fresh temporary folder with its own
lost_items.txt, runs it in a *different* working directory, and feeds menu
input through stdin. The real submission data is never touched.

Usage:
    python test_lost_and_found.py                      # auto-discover program
    python test_lost_and_found.py --program PATH -v    # explicit program path
"""
import argparse
import os
import re
import shutil
import subprocess
import sys
import tempfile
import unittest
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
OUTPUT_DIR = ROOT
PROVISIONAL = OUTPUT_DIR / "firstName_lost_and_found.py"
DATA_NAME = "lost_items.txt"
TIMEOUT = 15

HANDOUT_LINES = [
    "Mary Johnson, Laptop, Library, 10/20/2026",
    "David Smith, Wallet, Student Center,10/21/2026",
    "Sarah Brown, Keys, Parking Lot, 10/22/2026",
]
HANDOUT_DATA = "\n".join(HANDOUT_LINES) + "\n"

MENU_OPTIONS = [
    "Report a Lost Item",
    "View All Reported Items",
    "Search for an Item",
    "View Total Number of Reports",
    "Exit",
]

MISSING_MSG = r"not found|does not exist|doesn't exist|no (saved )?(reports|items|records|file)|missing|(can ?not|can't|could not|couldn't) (find|open)"
EMPTY_MSG = r"no (saved |valid )?(reports|items|records)|empty|nothing"
INVALID_MENU_MSG = r"invalid|error|not a valid|please (enter|choose|select)|try again"
BLANK_MSG = r"blank|empty|required|cannot be|can't be|must not|try again"
MALFORMED_MSG = r"skip|invalid|malformed|bad|error|warning|corrupt|format"


def resolve_program(explicit=None):
    """Explicit path wins; else the single *_lost_and_found.py in outputs; else the provisional name."""
    if explicit:
        return Path(explicit).resolve()
    found = sorted(OUTPUT_DIR.glob("*_lost_and_found.py"))
    if PROVISIONAL in found or not found:
        return PROVISIONAL
    return found[0]


PROGRAM = resolve_program()


def run_program(program, stdin_text, data=HANDOUT_DATA, remove_data=False):
    """Run a temp copy of program. Returns (CompletedProcess, script_dir, cwd_dir, tempdir_obj)."""
    tmp = tempfile.TemporaryDirectory(prefix="lab3_", dir=Path(__file__).resolve().parent)
    script_dir = Path(tmp.name) / "app"
    cwd_dir = Path(tmp.name) / "elsewhere"
    script_dir.mkdir()
    cwd_dir.mkdir()
    copy = script_dir / program.name
    shutil.copy2(program, copy)
    if not remove_data:
        (script_dir / DATA_NAME).write_text(data, encoding="utf-8")
    env = dict(os.environ, PYTHONIOENCODING="utf-8", PYTHONUTF8="1")
    proc = subprocess.run(
        [sys.executable, "-u", str(copy)],
        input=stdin_text, capture_output=True, text=True, encoding="utf-8",
        cwd=cwd_dir, env=env, timeout=TIMEOUT,
    )
    return proc, script_dir, cwd_dir, tmp


def action_outputs(stdout):
    """Text printed by each menu action: after 'Enter your choice:' up to the next menu banner."""
    parts = re.split(r"enter your choice:?", stdout, flags=re.I)[1:]
    return [re.split(r"CAMPUS LOST", p, flags=re.I)[0] for p in parts]


def fields(line):
    return [f.strip() for f in line.split(",")]


def count_line_matches(text, expected):
    """True if some line mentioning reports/total states exactly the number `expected` (dates ignored)."""
    for line in text.splitlines():
        if not re.search(r"report|total|count|record|item", line, re.I):
            continue
        numbers = re.findall(r"\d+", re.sub(r"\d{1,2}/\d{1,2}/\d{4}", "", line))
        if numbers == [str(expected)]:
            return True
    return False


class LostAndFoundTests(unittest.TestCase):
    def setUp(self):
        if not PROGRAM.is_file():
            self.fail(f"Program not found: {PROGRAM}")

    def run_app(self, stdin_text, **kw):
        try:
            proc, script_dir, cwd_dir, tmp = run_program(PROGRAM, stdin_text, **kw)
        except subprocess.TimeoutExpired:
            self.fail(f"Program did not exit within {TIMEOUT}s for input {stdin_text!r}")
        self.addCleanup(tmp.cleanup)
        out = proc.stdout + proc.stderr
        self.assertNotIn("Traceback", out, f"Program crashed:\n{out}")
        self.assertEqual(proc.returncode, 0, f"Non-zero exit:\n{out}")
        return proc.stdout, script_dir, cwd_dir

    def data_lines(self, script_dir):
        return [l for l in (script_dir / DATA_NAME).read_text(encoding="utf-8").splitlines() if l.strip()]

    def assertMatches(self, pattern, text, why):
        self.assertRegex(text.lower(), pattern, f"{why}. Output was:\n{text}")

    # --- menu -----------------------------------------------------------
    def test_menu_shows_all_five_options_and_exits_on_5(self):
        stdout, _, _ = self.run_app("5\n")
        self.assertIn("CAMPUS LOST & FOUND SYSTEM", stdout)
        for option in MENU_OPTIONS:
            self.assertIn(option.lower(), stdout.lower(), f"menu missing {option!r}")
        self.assertIn("enter your choice", stdout.lower())

    def test_invalid_menu_choice_shows_error_and_menu_repeats(self):
        stdout, _, _ = self.run_app("9\nabc\n5\n")
        outs = action_outputs(stdout)
        self.assertGreaterEqual(len(outs), 3, "menu did not repeat after invalid choices")
        self.assertMatches(INVALID_MENU_MSG, outs[0], "no error message for choice 9")
        self.assertMatches(INVALID_MENU_MSG, outs[1], "no error message for choice 'abc'")

    # --- view -----------------------------------------------------------
    def test_view_shows_every_handout_record(self):
        stdout, _, _ = self.run_app("2\n5\n")
        view = action_outputs(stdout)[0]
        for line in HANDOUT_LINES:
            for value in fields(line):
                self.assertIn(value, view, f"view missing {value!r}")

    # --- report ---------------------------------------------------------
    def test_report_appends_fourth_record_without_losing_earlier_ones(self):
        stdout, script_dir, _ = self.run_app("1\nAlex Lee\nWater Bottle\nGym\n10/23/2026\n5\n")
        lines = self.data_lines(script_dir)
        self.assertEqual(lines[:3], HANDOUT_LINES, "earlier records changed or lost")
        self.assertEqual(len(lines), 4)
        self.assertEqual(fields(lines[3]), ["Alex Lee", "Water Bottle", "Gym", "10/23/2026"])
        self.assertMatches(r"saved|added|recorded|success", action_outputs(stdout)[0], "no save confirmation")

    def test_count_after_append_is_four(self):
        stdout, _, _ = self.run_app("1\nAlex Lee\nWater Bottle\nGym\n10/23/2026\n4\n5\n")
        self.assertTrue(count_line_matches(action_outputs(stdout)[1], 4), f"count not 4:\n{stdout}")

    def test_blank_field_is_rejected_and_reasked(self):
        stdout, script_dir, _ = self.run_app("1\n\nBen Ng\n   \nPhone\nCafe\n10/25/2026\n5\n")
        lines = self.data_lines(script_dir)
        self.assertEqual(len(lines), 4, "blank input should not create extra or partial records")
        self.assertEqual(fields(lines[3]), ["Ben Ng", "Phone", "Cafe", "10/25/2026"])
        self.assertMatches(BLANK_MSG, action_outputs(stdout)[0], "no message for blank field")

    def test_comma_in_field_is_rejected_and_reasked(self):
        stdout, script_dir, _ = self.run_app("1\nSmith, John\nJohn Smith\nUmbrella\nLibrary\n10/26/2026\n5\n")
        lines = self.data_lines(script_dir)
        self.assertEqual(len(lines), 4)
        for line in lines:
            self.assertEqual(len(fields(line)), 4, f"record without exactly 4 fields: {line!r}")
        self.assertEqual(fields(lines[3]), ["John Smith", "Umbrella", "Library", "10/26/2026"])
        self.assertMatches(r"comma|invalid|not allowed|try again", action_outputs(stdout)[0], "no comma message")

    def test_data_file_lives_beside_script_not_in_cwd(self):
        _, script_dir, cwd_dir = self.run_app("1\nAlex Lee\nWater Bottle\nGym\n10/23/2026\n5\n")
        self.assertEqual(len(self.data_lines(script_dir)), 4, "record not saved beside the script")
        self.assertFalse((cwd_dir / DATA_NAME).exists(), "program wrote lost_items.txt into the cwd")

    # --- search ---------------------------------------------------------
    def test_search_is_case_insensitive_and_shows_all_matches(self):
        data = HANDOUT_DATA + "Tom Ray, keys, Gym, 10/24/2026\n"
        stdout, _, _ = self.run_app("3\nKEYS\n5\n", data=data)
        found = action_outputs(stdout)[0]
        self.assertIn("Sarah Brown", found)
        self.assertIn("Tom Ray", found)
        self.assertNotIn("Mary Johnson", found)
        self.assertNotIn("No matching item found.", found)

    def test_search_without_match_prints_exact_message(self):
        stdout, _, _ = self.run_app("3\nUmbrella\n5\n")
        self.assertIn("No matching item found.", action_outputs(stdout)[0])

    # --- count ----------------------------------------------------------
    def test_count_handout_records_is_three(self):
        stdout, _, _ = self.run_app("4\n5\n")
        self.assertTrue(count_line_matches(action_outputs(stdout)[0], 3), f"count not 3:\n{stdout}")

    # --- error handling -------------------------------------------------
    def test_missing_file_is_handled_helpfully_and_menu_continues(self):
        stdout, _, _ = self.run_app("2\n3\nKeys\n4\n5\n", remove_data=True)
        outs = action_outputs(stdout)
        self.assertGreaterEqual(len(outs), 4, "menu did not continue after missing file")
        self.assertMatches(MISSING_MSG, outs[0], "view: no helpful missing-file message")
        self.assertMatches(MISSING_MSG + r"|no matching item found\.", outs[1], "search: no helpful message")
        self.assertMatches(MISSING_MSG, outs[2], "count: no helpful missing-file message")

    def test_malformed_lines_are_skipped_and_processing_continues(self):
        data = "\n".join([
            HANDOUT_LINES[0],
            "this line has no commas",
            "Only, three, fields",
            HANDOUT_LINES[2],
            "Too, many, fields, here, 10/01/2026",
        ]) + "\n"
        stdout, _, _ = self.run_app("2\n4\n3\nkeys\n5\n", data=data)
        view, count, search = action_outputs(stdout)[:3]
        self.assertIn("Mary Johnson", view)
        self.assertIn("Sarah Brown", view, "valid record after a bad line was not shown")
        self.assertMatches(MALFORMED_MSG, view, "no message about malformed line")
        self.assertTrue(count_line_matches(count, 2), f"valid count should be 2:\n{count}")
        self.assertIn("Sarah Brown", search)

    def test_empty_file_is_handled(self):
        stdout, _, _ = self.run_app("2\n4\n3\nKeys\n5\n", data="")
        view, count, search = action_outputs(stdout)[:3]
        self.assertMatches(EMPTY_MSG, view, "view: no helpful empty-file message")
        self.assertTrue(count_line_matches(count, 0), f"empty count should be 0:\n{count}")
        self.assertMatches(EMPTY_MSG + r"|no matching item found\.", search, "search: no helpful message")


if __name__ == "__main__":
    parser = argparse.ArgumentParser(add_help=False)
    parser.add_argument("--program")
    args, rest = parser.parse_known_args()
    PROGRAM = resolve_program(args.program)
    print(f"Testing program: {PROGRAM} (exists={PROGRAM.is_file()})", file=sys.stderr)
    unittest.main(argv=[sys.argv[0]] + rest)

