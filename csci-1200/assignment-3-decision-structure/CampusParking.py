#Campus Parking Fee and permit checker

Name = input("Enter name: ")
status = input("Enter status (student, employee, visitor): ")
hours = float(input("Enter number of hours parked: "))
permit = input(" Valid permit? (yes or no): ")

#Decide the parking fee

if status == "student" or status == "employee":
    if permit == "yes":
        fee = 0
    else:
        fee = hours * 2


elif status == "visitor":
    if hours <= 2:
        fee = 0
    else:
        fee = hours * 3
else:
    print("Invalid status, please enter student, employee, or visitor.")
    exit()

#show the parking summary.
print('\n== Campus Parking Summary ==')
print(f'Name: {Name}')
print(f'Status: {status.title()}')
print(f'Hours Parked: {hours}')
print(f'Permit: {permit.title()}')
print(f'Parking Fee: ${fee:.2f}')

if fee == 0:
    print("No payment required.")
else:
    print("Please proceed to payment.")

if hours > 8:
    print("Warning: You have exceeded the maximum parking time of 8 hours.")
