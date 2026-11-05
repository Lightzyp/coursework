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

using static Shopping_cart.Program;
using static Shopping_cart.Shopping_Cart;

namespace Shopping_cart
{
    class program
    {
        static void Main(string[] args)
        {
            // Initialize some sample items
            var item1 = new Item("Shoes", 49.99m, "Comfortable sneakers");
            var item2 = new Item("T-shirt", 19.99m, "Cotton blend");
            var item3 = new Item("Backpack", 34.95m, "Water-resistant");
            // Create a shopping cart
            var cart = new ShoppingCart();
            cart.AddToCart(item1);
            cart.AddToCart(item2);
            cart.AddToCart(item3);

            // Display cart contents
            Console.WriteLine(cart);
            foreach (var item in cart.GetItems())
            {
                Console.WriteLine(item);
            }
        }
    }
}