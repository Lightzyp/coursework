/**
*--------------------------------------------------------------------
* File name: Project 2
* Project name: Online Shopping System
* Solution name: Online Shopping System
* --------------------------------------------------------------------
* Author’s name and email: Daniel [REDACTED EMAIL]
* Course-Section: CSCI 1250
* Creation Date: Nov. 7, 2023
* Modified Date: Nov. 7, 2023
* -------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;

class Program
{
    // Lists to store product information and shopping cart items
    static List<Product> products = new List<Product>();
    static List<Product> shoppingCart = new List<Product>();

    // Variable to store the total price of items in the shopping cart
    static decimal totalPrice = 0;

    // Main method where the program execution begins
    static void Main(string[] args)
    {
        // Initializing the list of products
        InitializeProducts();

        // Variable to control the main shopping loop
        bool shopping = true;

        // Main shopping loop
        while (shopping)
        {
            // Displaying the main menu to the user
            DisplayMenu();

            // Getting the user's choice from the menu
            int choice = GetChoice();

            // Switch statement to handle user's choice
            switch (choice)
            {
                case 1:
                    // Display available products
                    DisplayProducts();
                    break;

                case 2:
                    // Add a selected product to the shopping cart
                    AddToCart();
                    break;

                case 3:
                    // View the contents of the shopping cart
                    ViewCart();
                    break;

                case 4:
                    // Complete the purchase, displaying the final cart and clearing it
                    CompletePurchase();
                    break;

                case 5:
                    // Exit the shopping application
                    shopping = false;
                    break;

                default:
                    // Display an error message for an invalid choice
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        }
    }

    // Method to initialize the list of products with some default items
    static void InitializeProducts()
    {
        products.Add(new Product("Laptop", 999.99m));
        products.Add(new Product("Smartphone", 499.99m));
        products.Add(new Product("Headphones", 49.99m));
        products.Add(new Product("Mouse", 19.99m));
    }

    // Method to display the main menu options
    static void DisplayMenu()
    {
        Console.WriteLine("Main Menu");
        Console.WriteLine("1. Browse Products");
        Console.WriteLine("2. Add to Cart");
        Console.WriteLine("3. View Cart");
        Console.WriteLine("4. Complete Purchase");
        Console.WriteLine("5. Exit");
    }

    // Method to get and validate the user's choice from the menu
    static int GetChoice()
    {
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5)
        {
            Console.WriteLine("Invalid input. Please enter a valid choice (1-5).");
        }
        return choice;
    }

    // Method to display the available products
    static void DisplayProducts()
    {
        Console.WriteLine("Available Products:");
        foreach (Product product in products)
        {
            Console.WriteLine($"{product.Name} - ${product.Price:F2}");
        }
    }

    // Method to add a selected product to the shopping cart
    static void AddToCart()
    {
        DisplayProducts();
        Console.Write("Enter the product ID to add to your cart: ");
        if (int.TryParse(Console.ReadLine(), out int productID) && productID > 0 && productID <= products.Count)
        {
            // Adding the selected product to the shopping cart and updating the total price
            Product selectedProduct = products[productID - 1];
            shoppingCart.Add(selectedProduct);
            totalPrice += selectedProduct.Price;
            Console.WriteLine($"{selectedProduct.Name} added to your cart.");
        }
        else
        {
            Console.WriteLine("Invalid product ID. Please enter a valid ID.");
        }
    }

    // Method to view the contents of the shopping cart
    static void ViewCart()
    {
        Console.WriteLine("Shopping Cart:");
        foreach (Product product in shoppingCart)
        {
            Console.WriteLine($"{product.Name} - ${product.Price:F2}");
        }
        Console.WriteLine($"Total Price: ${totalPrice:F2}");
    }

    // Method to complete the purchase, displaying the final cart and clearing it
    static void CompletePurchase()
    {
        ViewCart();
        Console.WriteLine("Purchase completed. Thank you!");
        shoppingCart.Clear();
        totalPrice = 0;
    }
}

// Product class to represent individual items with name and price
class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    // Constructor to initialize a product with a name and price
    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}
