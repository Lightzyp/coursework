#Pseudocode for Student Profile Program:
#1. ask the user for their first name, last name, age, major, school year, favorite hobby, and dream job
#2. store the user's input in variables so that they can be used later in the program
#3. concatenate the first name and last name to create a full name variable
#4. print out the student's profile information in a formatted way, including their full name, age, major, school year, favorite hobby, and dream job



first_name = input("Enter your first name: ")
last_name = input("Enter your last name: ")
age = input("Enter your age: ")
major = input("Enter your major: ")
school_year = input("Enter your school year: ")
favorite_hobby = input("Enter your favorite hobby: ")
dream_job = input("Enter your dream job: ")

full_name = first_name + " " + last_name

print("\n============================")
print("Student Profile")
print("============================")
print(f"Full Name: {full_name}")
print(f"Age: {age}")
print(f"Major: {major}")
print(f"School Year: {school_year}")
print(f"Favorite Hobby: {favorite_hobby}")
print(f"Dream Job: {dream_job}")
