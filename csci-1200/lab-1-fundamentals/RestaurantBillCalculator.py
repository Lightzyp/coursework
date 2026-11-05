#pseudocode for Restaurant Bill Calculator
#1. ask for the customer's name, the number of items ordered, and the price of each item
#2. convert the money and percentage entries to float data types so that they can be used in calculations
#3. convert the number of people to and interger
#4. calculate the tax amount and tip amount based on the subtotal and the tax and tip percentages
#5. Add the meal cost, tax amount, and tip amount to get the total bill
#6. divide the total bill by the number of people to get the amount each person owes
#7 display the customer's name, the subtotal, tax amount, tip amount, total bill, and the amount each person owes in a formatted way


customer_name = input("Enter the customer's name: ")
meal_cost = float(input("Enter the meal cost: "))
tax_percentage = float(input("Enter the tax percentage (as a decimal): "))
tip_percentage = float(input("Enter the tip percentage (as a decimal): "))
number_of_people = int(input("Enter the number of people: "))

tax_amount = meal_cost * tax_percentage / 100
tip_amount = meal_cost * tip_percentage / 100


total_bill = meal_cost + tax_amount + tip_amount
amount_per_person = total_bill / number_of_people

print("\n============================")
print("Restaurant Bill Summary")
print("============================")
print(f"Customer Name: {customer_name}")
print(f"Subtotal: ${meal_cost:.2f}")
print(f"Tax Amount: ${tax_amount:.2f}")
print(f"Tip Amount: ${tip_amount:.2f}")
print(f"Total Bill: ${total_bill:.2f}")
print(f"Amount Per Person: ${amount_per_person:.2f}")