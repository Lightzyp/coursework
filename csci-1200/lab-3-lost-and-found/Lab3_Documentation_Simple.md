# Lab3_Documentation_Simple

Text extracted from the completed coursework document. Screenshots, comments and document metadata are omitted.

CSCI 1200 Lab 3 Campus Lost and Found
Run Daniel_lost_and_found.py from the folder that contains both the program and lost_items.txt.
Pseudocode
FUNCTION read_records()
    TRY open lost_items.txt in read mode "r"
        FOR each non-blank line: TRY splitting it into 4 comma-separated fields
            EXCEPT ValueError (not 4 fields): print a warning, skip the line
    EXCEPT FileNotFoundError: print a file-not-found message
    RETURN the list of good records
FUNCTION report_item()
    FOR name, item, location, date: ask again while blank or has a comma
    OPEN lost_items.txt in append mode "a" and write one line
    PRINT "Report saved successfully"
FUNCTION view_items()
    FOR each record from read_records(): print name, item, location, date
    IF there are no records: print "There are no valid reports to display."
FUNCTION search_item()
    ask for an item name
    FOR each record: IF its item equals the search, ignoring case: print it
    IF nothing matched: print "No matching item found."
FUNCTION count_reports()
    PRINT how many records read_records() returned
FUNCTION main()
    choice = ""; WHILE choice is not "5"
        show the menu with options 1-5 and read the choice
        1 report_item, 2 view_items, 3 search_item, 4 count_reports
        5 print goodbye; any other choice: print "Invalid choice"
Saving and Reading the File
Mode "a" adds a report without erasing earlier reports. Mode "r" reads the saved file.
Exception Handling
If lost_items.txt is missing, FileNotFoundError is caught and a helpful message appears. If a saved line does not have four fields, ValueError is caught and that line is skipped. Blank report fields are requested again, and the menu stays available after these errors.

These screenshots show a read-only viewer of actual Python run output with the supplied input echoed. Selected excerpts omit repeated menus and final goodbye lines.
Saving a report

Viewing all reports


Searching for an item

Counting reports

Error test
