# Name: Daniel Macharia
# Course: CSCI 1200 -001

# Ask for product information.
Product_name = input("Enter the product name: ")

quantity = int(input("Enter the current quantity in stock: "))

reorder_point = int(input("Enter the reorder point: "))

unit_price = float(input("Enter the unit price: "))

high_priority = input(
    "Is this a high priority item? (yes/no): "
).strip().lower()

supplier_available = input(
    "Is the supplier available? (yes/no): "
).strip().lower()

# Calculate inventory value using the current quantity.
Inventory_value = quantity * unit_price

# Check and save the stock status.
if quantity > reorder_point:
    stock_status = "Stock Level OK"
elif quantity == reorder_point:
    stock_status = "Reorder Soon"
else:
    stock_status = "Low Stock - Reorder Needed"

# Decide the reorder priority.
reorder_needed = quantity < reorder_point

if reorder_needed and high_priority == "yes":
    reorder_priority = "URGENT REORDER"
elif reorder_needed:
    reorder_priority = "NORMAL REORDER"
else:
    reorder_priority = "NO REORDER NEEDED"

# Check whether an order can be placed.
if reorder_needed and supplier_available == "yes":
    order_status = "Reorder can be placed."
elif reorder_needed:
    order_status = "Reorder needed, but supplier is unavailable."
else:
    order_status = "No reorder required at this time."

# Display the inventory report.
print("\n====================================================")
print("INVENTORY STATUS REPORT")
print("====================================================")
print(f"Product: {Product_name}")
print(f"Current Stock: {quantity}")
print(f"Reorder Level: {reorder_point}")
print(f"Unit Price: ${unit_price:.2f}")
print(f"Inventory Value: ${Inventory_value:.2f}")
print(f"Stock Status: {stock_status}")
print(f"High Priority: {high_priority}")
print(f"Supplier Available: {supplier_available}")
print(f"Reorder Priority: {reorder_priority}")
print(f"Final Action: {order_status}")
print("====================================================")