# Pseudocode for Campus Meal Discount Program
#1. ask for the customer's name, meal price, drink price, and student status (yes or no)
#2. convert the meal price and drink price to floats
#3. add the meail price and drinks price to get the total cost
#4. if the customer is a dn student, apply a 10% discount to the total cost other wise, do not apply any discount
#5. display the customer's name, total cost before discount, discount amount (if applicable), and total cost after discount in a formatted way
#7 Display yje complete bill summary including the customer's name, total cost before discount, discount amount (if applicable), and total cost after discount in a formatted way


customer_name = input("Enter the customer's name: ")
meal_price = float(input("Enter the meal price: "))
drink_price = float(input("Enter the drink price: "))
student_status = input("Are you a student? (yes or no): ")

subtotal = meal_price + drink_price

if student_status == "yes":
    discount_amount = subtotal * 0.10
    total_cost = subtotal - discount_amount
else:
    discount_amount = 0
    total_cost = subtotal

final_total = subtotal - discount_amount

if final_total >=20:
    dessert_message = "You are eligible for a free dessert!"
else:
    dessert_message = "You are not eligible for a free dessert."

print("\n============================")
print("Campus Meal Summary")
print("============================")
print(f"Customer Name: {customer_name}")    
print(f"Drink Price: ${drink_price:.2f} ")
print(f"Subtotal: ${subtotal:.2f} ")
print(f"Discount Amount: ${discount_amount:.2f} ")
print(f"Total Cost: ${total_cost:.2f} ")
print(f"Dessert Eligibility: {dessert_message} ")
print("\n============================") 
