"""Independent subprocess checks of the handout's observable requirements.

Run: python test_independent_lost_and_found.py --program PATH -v
Without --program, discover the sole *_lost_and_found.py submission file.
Only temporary copies and temporary lost_items.txt files are executed/modified.
"""
import argparse
import ast
from pathlib import Path
import re
import shutil
import subprocess
import sys
import tempfile
import unittest

ROOT = Path(__file__).resolve().parents[1]
SAMPLES = [
    "Mary Johnson, Laptop, Library, 10/20/2026",
    "David Smith, Wallet, Student Center,10/21/2026",
    "Sarah Brown, Keys, Parking Lot, 10/22/2026",
]


def discover_program():
    folder = ROOT
    candidates = sorted(folder.glob("*_lost_and_found.py"))
    if len(candidates) == 1:
        return candidates[0]
    if not candidates:
        return folder / "firstName_lost_and_found.py"
    raise RuntimeError("Several candidate scripts exist; pass --program explicitly.")


PROGRAM = discover_program().resolve()


def normalized_records(text):
    return [[field.strip() for field in line.split(",")] for line in text.splitlines() if line.strip()]


class LostAndFoundContract(unittest.TestCase):
    def setUp(self):
        self.temp = tempfile.TemporaryDirectory(prefix="lab3_test_", dir=Path(__file__).resolve().parent)
        self.addCleanup(self.temp.cleanup)
        self.sandbox = Path(self.temp.name)
        self.script_dir = self.sandbox / "program"
        self.script_dir.mkdir()
        self.other_cwd = self.sandbox / "other_directory"
        self.other_cwd.mkdir()
        self.program = self.script_dir / PROGRAM.name
        shutil.copy2(PROGRAM, self.program)
        self.data = self.script_dir / "lost_items.txt"
        self.write_records(SAMPLES)

    def write_records(self, records):
        self.data.write_text("\n".join(records) + ("\n" if records else ""), encoding="utf-8")

    def run_session(self, answers):
        result = subprocess.run(
            [sys.executable, str(self.program)], input="\n".join(answers) + "\n",
            text=True, capture_output=True, cwd=self.other_cwd, timeout=10,
        )
        self.assertEqual(result.returncode, 0, result.stderr + result.stdout)
        self.assertNotIn("Traceback", result.stderr + result.stdout)
        self.assertEqual(result.stderr, "")
        self.assertIn("CAMPUS LOST & FOUND SYSTEM", result.stdout)
        return result.stdout

    def assert_count(self, output, count):
        self.assertRegex(output, rf"(?im)^(?!\s*4\.).*(?:total[^\n]*reports|number[^\n]*reports|reports?[^\n]*count)[^\d\n]*{count}\b")

    def assert_helpful_missing(self, output):
        self.assertRegex(output.lower(), r"not found|does not exist|missing|no reports.*saved|no saved reports")
        self.assertGreaterEqual(output.count("CAMPUS LOST & FOUND SYSTEM"), 2)

    def test_exit_and_exact_menu_options(self):
        out = self.run_session(["5"])
        for label in (
            "1. Report a Lost Item", "2. View All Reported Items",
            "3. Search for an Item", "4. View Total Number of Reports", "5. Exit",
        ):
            self.assertIn(label, out)
        self.assertEqual(out.count("CAMPUS LOST & FOUND SYSTEM"), 1)

    def test_view_all_three_sample_records(self):
        out = self.run_session(["2", "5"])
        for record in normalized_records("\n".join(SAMPLES)):
            for field in record:
                self.assertIn(field, out)
        self.assertGreaterEqual(out.count("CAMPUS LOST & FOUND SYSTEM"), 2)

    def test_append_fourth_preserves_first_three_and_count(self):
        before = self.data.read_bytes()
        out = self.run_session(["1", "Alex Green", "Phone", "Cafeteria", "10/23/2026", "4", "5"])
        self.assertTrue(self.data.read_bytes().startswith(before), "Existing data was changed instead of appended.")
        self.assertEqual(normalized_records(self.data.read_text(encoding="utf-8")), normalized_records("\n".join(SAMPLES)) + [["Alex Green", "Phone", "Cafeteria", "10/23/2026"]])
        self.assertRegex(out.lower(), r"saved|success|recorded")
        self.assert_count(out, 4)

    def test_search_is_case_insensitive_and_shows_every_match(self):
        self.write_records(SAMPLES + ["Jordan Lee, wAlLeT, Gym, 10/23/2026"])
        out = self.run_session(["3", "WaLlEt", "5"])
        self.assertIn("David Smith", out)
        self.assertIn("Jordan Lee", out)
        self.assertNotIn("Mary Johnson", out)
        self.assertNotIn("Sarah Brown", out)

    def test_no_match_uses_required_exact_message(self):
        out = self.run_session(["3", "Umbrella", "5"])
        self.assertIn("No matching item found.", out)
        for name in ("Mary Johnson", "David Smith", "Sarah Brown"):
            self.assertNotIn(name, out)

    def test_count_three_valid_samples(self):
        self.assert_count(self.run_session(["4", "5"]), 3)

    def test_invalid_menu_input_and_blank_menu_return_to_menu(self):
        out = self.run_session(["9", "", "hello", "4", "5"])
        self.assertRegex(out.lower(), r"invalid|choose.*1.*5|enter.*1.*5")
        self.assertGreaterEqual(out.count("CAMPUS LOST & FOUND SYSTEM"), 5)
        self.assert_count(out, 3)

    def test_blank_fields_are_retried_before_record_is_saved(self):
        out = self.run_session(["1", "", "Alex Green", " ", "Phone", "", "Cafeteria", "  ", "10/23/2026", "5"])
        records = normalized_records(self.data.read_text(encoding="utf-8"))
        self.assertEqual(len(records), 4)
        self.assertEqual(records[-1], ["Alex Green", "Phone", "Cafeteria", "10/23/2026"])
        self.assertGreaterEqual(len(re.findall(r"blank|empty|required", out.lower())), 4)

    def test_commas_are_rejected_to_preserve_four_field_format(self):
        out = self.run_session(["1", "Alex, Green", "Alex Green", "Phone", "Cafeteria", "10/23/2026", "5"])
        records = normalized_records(self.data.read_text(encoding="utf-8"))
        self.assertEqual(records[-1], ["Alex Green", "Phone", "Cafeteria", "10/23/2026"])
        self.assertEqual(len(records), 4)
        self.assertRegex(out.lower(), r"comma")

    def test_missing_file_view_recovers(self):
        self.data.unlink()
        self.assert_helpful_missing(self.run_session(["2", "5"]))

    def test_missing_file_search_recovers(self):
        self.data.unlink()
        self.assert_helpful_missing(self.run_session(["3", "Wallet", "5"]))

    def test_missing_file_count_is_zero_and_recovers(self):
        self.data.unlink()
        out = self.run_session(["4", "5"])
        self.assert_helpful_missing(out)
        self.assert_count(out, 0)

    def test_saving_creates_missing_data_file(self):
        self.data.unlink()
        self.run_session(["1", "Alex Green", "Phone", "Cafeteria", "10/23/2026", "5"])
        self.assertEqual(normalized_records(self.data.read_text(encoding="utf-8")), [["Alex Green", "Phone", "Cafeteria", "10/23/2026"]])

    def test_malformed_records_skipped_and_later_valid_records_displayed(self):
        self.write_records([SAMPLES[0], "bad,three,fields", SAMPLES[1], "five,fields,in,this,line", SAMPLES[2]])
        before = self.data.read_bytes()
        out = self.run_session(["2", "4", "5"])
        self.assertRegex(out.lower(), r"bad record|malformed|invalid record|skipp|expected.*4|four fields")
        for name in ("Mary Johnson", "David Smith", "Sarah Brown"):
            self.assertIn(name, out)
        self.assert_count(out, 3)
        self.assertEqual(self.data.read_bytes(), before)

    def test_search_passes_malformed_records_and_finds_later_match(self):
        self.write_records(["bad,record", SAMPLES[1], "too,many,comma,separated,fields", "Jordan Lee, Wallet, Gym, 10/23/2026"])
        out = self.run_session(["3", "wAlLeT", "5"])
        self.assertIn("David Smith", out)
        self.assertIn("Jordan Lee", out)
        self.assertRegex(out.lower(), r"bad record|malformed|invalid record|skipp|expected.*4|four fields")

    def test_empty_file_view_search_count_and_menu_recover(self):
        self.write_records([])
        out = self.run_session(["2", "3", "Wallet", "4", "5"])
        self.assertRegex(out.lower(), r"no.*reports|empty|nothing.*display")
        self.assertIn("No matching item found.", out)
        self.assert_count(out, 0)
        self.assertGreaterEqual(out.count("CAMPUS LOST & FOUND SYSTEM"), 4)

    def test_file_lives_beside_script_even_when_cwd_differs(self):
        misleading = self.other_cwd / "lost_items.txt"
        misleading.write_text("Wrong Person, Laptop, Wrong Place, 01/01/2000\n", encoding="utf-8")
        original = misleading.read_bytes()
        out = self.run_session(["2", "1", "Alex Green", "Phone", "Cafeteria", "10/23/2026", "4", "5"])
        self.assertIn("Mary Johnson", out)
        self.assertNotIn("Wrong Person", out)
        self.assert_count(out, 4)
        self.assertEqual(misleading.read_bytes(), original)
        self.assertEqual(len(normalized_records(self.data.read_text(encoding="utf-8"))), 4)

    def test_required_named_functions_loop_and_exception_handlers_exist(self):
        tree = ast.parse(PROGRAM.read_text(encoding="utf-8-sig"))
        functions = {node.name for node in ast.walk(tree) if isinstance(node, ast.FunctionDef)}
        self.assertTrue({"report_item", "view_items", "search_item", "count_reports"}.issubset(functions))
        self.assertTrue(any(isinstance(node, ast.While) for node in ast.walk(tree)))
        self.assertTrue(any(isinstance(node, ast.For) for node in ast.walk(tree)))
        handler_names = set()
        for node in ast.walk(tree):
            if isinstance(node, ast.ExceptHandler):
                if isinstance(node.type, ast.Name):
                    handler_names.add(node.type.id)
                elif isinstance(node.type, ast.Tuple):
                    handler_names.update(value.id for value in node.type.elts if isinstance(value, ast.Name))
        self.assertTrue({"FileNotFoundError", "ValueError"}.issubset(handler_names))


if __name__ == "__main__":
    parser = argparse.ArgumentParser(add_help=False)
    parser.add_argument("--program", type=Path)
    args, remaining = parser.parse_known_args()
    PROGRAM = (args.program or discover_program()).resolve()
    unittest.main(argv=[sys.argv[0]] + remaining, verbosity=2)

