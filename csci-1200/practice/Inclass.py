#creat a campis recreation access checker he program should--
#1. ask the user for their age 
#ask whether the person has a vaild campus ID (yes or no)
#ask whether the person is a student (yes or no)
#ask whether the person is a faculty member (yes or no)
#allow entry only of the person is 18 or older and has a valid campus ID, and if the person is a student or faculty member
#oterwise, deny entry and display a message indicating the reason for denial


age = int(input("Enter your age: "))
campus_id = input("Do you have a valid campus ID? (yes or no): ").lower()
student = input("Are you a student? (yes or no): ").lower()
faculty = input("Are you a faculty member? (yes or no): ").lower()

if age >= 18 and campus_id == "yes" and (student == "yes" or faculty == "yes"):
    print("Entry allowed.")
else:
    print("Entry denied.")
    