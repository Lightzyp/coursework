# Lab3_Documentation

Text extracted from the completed coursework document. Screenshots, comments and document metadata are omitted.

CSCI 1200 Lab 3 Campus Lost and Found
Pseudocode
FILE_NAME = lost_items.txt beside the program; call main() when run directly
FUNCTION main()
    choice = ""; WHILE choice is not "5"
        show_menu(); read choice and strip spaces
        IF "1": report_item(); ELIF "2": view_items()
        ELIF "3": search_item(); ELIF "4": count_reports()
        ELIF "5": print goodbye; ELSE: print invalid-choice message
FUNCTION show_menu()
    print campus title and options 1 Report, 2 View, 3 Search, 4 Count, 5 Exit
FUNCTION ask_field(prompt)
    REPEAT read and strip an answer
        IF blank or contains a comma: print the reason and ask again
        ELSE return the answer
FUNCTION report_item()
    use ask_field for name, item, location and date (MM/DD/YYYY prompt)
    OPEN FILE_NAME in append mode "a"; write four comma-separated fields
    end the new line and print the save confirmation
FUNCTION read_records()
    records = empty list
    TRY OPEN FILE_NAME in read mode "r"  <- FileNotFoundError may occur
        FOR each line: strip spaces and skip a blank line
            TRY split on commas; IF field count is not 4: raise ValueError
                add the four stripped fields to records
            EXCEPT ValueError: print line-number warning and skip the line
    EXCEPT FileNotFoundError: print missing-file message and option 1 hint
    return records
FUNCTION show_record(record)
    print Student, Item, Location, Date and a separator
FUNCTION view_items()
    records = read_records()
    IF empty: print "There are no valid reports to display."
    ELSE FOR each record: show_record(record)
FUNCTION search_item()
    search = lowercase ask_field("item name"); found = false
    FOR each record in read_records()
        IF lowercase item equals search: show_record(record); found = true
    IF not found: print "No matching item found."
FUNCTION count_reports()
    print "Total number of valid reports:" and the size of read_records()
Exception Handling
If lost_items.txt is missing, FileNotFoundError is caught and a helpful message appears. If a saved line does not have four fields, ValueError is caught and that line is skipped. Blank report fields are requested again, and the menu stays available after these errors.

Saving and test overview


Viewing and counting reports



Searching and recovering from invalid input



Testing file exceptions
