

budget = float(input("Enter your shopping budget: "))

items = ["Bread", "Milk", "Eggs", "Rice", "Apples"]
prices = [4.00, 3.50, 5.00, 10.00, 6.00]


display_prices = []
for price in prices:
    display_prices.append(f"${price:.2f}")

print("Original items:", items)
print("Original prices:", display_prices)
print("Number of items:", len(items))
print(f"Budget: ${budget:.2f}")


items.append("Chicken")
prices.append(12.00)


milk_index = items.index("Milk")
items.pop(milk_index)
prices.pop(milk_index)

display_prices = []
for price in prices:
    display_prices.append(f"${price:.2f}")

print("\nUpdated items:", items)
print("Updated prices:", display_prices)


search_item = input("\nEnter an item to search: ")
if search_item in items:
    print(search_item, "is on the list.")
else:
    print(search_item, "is not on the list.")

total = sum(prices)
average = total / len(prices)
print(f"\nTotal cost: ${total:.2f}")
print(f"Average item price: ${average:.2f}")
print(f"Highest price: ${max(prices):.2f}")
print(f"Lowest price: ${min(prices):.2f}")

t
balance = budget - total
if balance >= 0:
    budget_message = "You are within your budget!"
    balance_message = f"Remaining balance: ${balance:.2f}"
else:
    budget_message = "You are over your budget."
    balance_message = f"Overspending: ${-balance:.2f}"

print(budget_message)
print(balance_message)


store_info = ("Campus Grocery Store", "Johnson City")

print(f"\n===== {store_info[0].upper()} =====")
print("Location:", store_info[1])
for i in range(len(items)):
    print(f"{items[i]:<12} ${prices[i]:.2f}")
print("------------------------------")
print("Total items:", len(items))
print(f"Total cost: ${total:.2f}")
print(f"Budget: ${budget:.2f}")
print(balance_message)
print(budget_message)
