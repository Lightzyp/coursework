/*
*--------------------------------------------------------------------
* File name: Program.cs
* Project name: ShoppingCartApp
*--------------------------------------------------------------------
* Author’s name and email: Daniel Macharia, [REDACTED EMAIL]
* Course-Section: CSCI-1250-001
* Creation Date: 4/3/2024
* -------------------------------------------------------------------
* This file contains the main program to interact with the shopping cart.
* It initializes the product list and drives the interaction with the menu class.
* -------------------------------------------------------------------
*/

namespace Shopping_cart
{
    internal partial class Program
    {
        public class Item
        {
            public int Id { get; private set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }

            // Constructor to initialize an item with name and price
            public Item(string name, decimal price)
            {
                Name = name;
                Price = price;
                // Generate a random ID (you can adjust the range as needed)
                Id = new Random().Next(1000, 9999);
            }

            // Constructor to initialize an item with name, price, and description
            public Item(string name, decimal price, string description)
                : this(name, price)
            {
                Description = description;
            }

            public override string ToString()
            {
                return $"{Name} (ID: {Id}) - ${Price}";
            }
        }
    }
}