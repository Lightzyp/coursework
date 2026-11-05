def welcome_student(name):
    print(f"Welcome, {name}!")


def show_student_info(name, major, workshops):
    print(f"{name} | {major} | Workshops: {workshops}")


def goodbye_student(name):
    print(f"{name}, your check-in is complete!")


def main():
    total_workshops = 0

    # Get a valid student count.
    students = int(input("How many students are checking in? "))

    while students < 1:
        print("Please enter at least 1 student.")
        students = int(input("How many students are checking in? "))

    # Process each student.
    for student in range(1, students + 1):
        print(f"\nStudent {student}")

        name = input("Name: ")
        major = input("Major: ")
        workshops = int(input("How many workshops? "))

        # Get a valid workshop count.
        while workshops < 1:
            print("Please enter at least 1 workshop.")
            workshops = int(input("How many workshops? "))

        welcome_student(name)
        show_student_info(name, major, workshops)

        # This loop is inside the student loop.
        for workshop in range(1, workshops + 1):
            print(f"  Registered for Workshop {workshop}")

        total_workshops += workshops

        goodbye_student(name)

    print("\n--- EVENT CHECK-IN SUMMARY ---")
    print(f"Students Checked In: {students}")
    print(f"Total Workshop Registrations: {total_workshops}")


main()    