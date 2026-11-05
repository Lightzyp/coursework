# CSCI 1200 - Lab 3: Campus Lost and Found Management System
# Saves lost item reports to a text file, then views, searches, and counts them.
# Run this program from the folder that contains lost_items.txt.


def read_records():
    # Read the file and return a list of good records.
    # Each record is a list: [name, item, location, date]
    records = []
    try:
        # "r" opens the file for reading only
        with open("lost_items.txt", "r") as file:
            for line in file:
                line = line.strip()
                if line != "":
                    try:
                        # This line raises ValueError if there are not exactly 4 fields
                        name, item, location, date = line.split(",")
                        records.append([name.strip(), item.strip(), location.strip(), date.strip()])
                    except ValueError:
                        print("Warning: skipping bad record (it needs 4 fields):", line)
    except FileNotFoundError:
        print("The file lost_items.txt was not found. No reports have been saved yet.")
        print("Choose option 1 to report a lost item and create the file.")
    return records


def report_item():
    print("\n--- Report a Lost Item ---")
    prompts = ["Student name: ", "Item name: ", "Location lost: ", "Date reported (MM/DD/YYYY): "]
    answers = []
    for prompt in prompts:
        answer = input(prompt).strip()
        # Keep asking until the answer is not blank and has no commas
        while answer == "" or "," in answer:
            if answer == "":
                print("This field cannot be blank. Please try again.")
            else:
                print("Please do not use commas. Commas separate the fields in the file.")
            answer = input(prompt).strip()
        answers.append(answer)

    # "a" adds to the end of the file, so earlier reports are not erased
    with open("lost_items.txt", "a") as file:
        file.write(answers[0] + ", " + answers[1] + ", " + answers[2] + ", " + answers[3] + "\n")
    print("Report saved successfully for", answers[0] + ".")


def view_items():
    print("\n--- All Reported Items ---")
    records = read_records()
    if len(records) == 0:
        print("There are no valid reports to display.")
    for record in records:
        print("Student:  ", record[0])
        print("Item:     ", record[1])
        print("Location: ", record[2])
        print("Date:     ", record[3])
        print("--------------------------------------")


def search_item():
    print("\n--- Search for an Item ---")
    search = input("Enter the item name to search for: ").strip().lower()
    found = False
    for record in read_records():
        # Compare in lowercase so "WALLET" matches "Wallet"
        if record[1].lower() == search:
            print("Student:  ", record[0])
            print("Item:     ", record[1])
            print("Location: ", record[2])
            print("Date:     ", record[3])
            print("--------------------------------------")
            found = True
    if found == False:
        print("No matching item found.")


def count_reports():
    print("\n--- Total Number of Reports ---")
    records = read_records()
    print("Total number of valid reports:", len(records))


def main():
    choice = ""
    while choice != "5":
        print()
        print("CAMPUS LOST & FOUND SYSTEM")
        print("======================================")
        print("1. Report a Lost Item")
        print("2. View All Reported Items")
        print("3. Search for an Item")
        print("4. View Total Number of Reports")
        print("5. Exit")
        print("======================================")
        choice = input("Enter your choice: ").strip()

        if choice == "1":
            report_item()
        elif choice == "2":
            view_items()
        elif choice == "3":
            search_item()
        elif choice == "4":
            count_reports()
        elif choice == "5":
            print("Goodbye! Thank you for using the Lost & Found System.")
        else:
            print("Invalid choice. Please enter a number from 1 to 5.")


main()
