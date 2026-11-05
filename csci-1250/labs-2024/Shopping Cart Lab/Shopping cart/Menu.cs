using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shopping_cart.Program;
using static Shopping_cart.Shopping_Cart;

namespace Shopping_cart
{
    internal class Menu
    {
        private List<Item> availableItems;
        private ShoppingCart cart;

        public Menu(List<Item> items)
        {
            availableItems = items;
            cart = new ShoppingCart();
        }

        public void DisplayMenu()
        {
            Console.WriteLine("Welcome to the Shopping Cart App!");
            Console.WriteLine("Available Items:");
            int index = 1;
            foreach (var item in availableItems)
            {
                Console.WriteLine($"{index}. {item}");
                index++;
            }
            Console.WriteLine($"{index}. Exit");

            int choice = GetMenuChoice();
            ProcessChoice(choice);
        }

        private int GetMenuChoice()
        {
            int choice;
            do
            {
                Console.Write("Enter your choice: ");
            } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > availableItems.Count + 1);

            return choice;
        }

        private void ProcessChoice(int choice)
        {
            if (choice <= availableItems.Count)
            {
                var selectedItem = availableItems[choice - 1];
                cart.AddToCart(selectedItem);
                Console.WriteLine($"Added '{selectedItem.Name}' to the cart.");
                DisplayMenu(); // Show menu again after adding item to the cart
            }
            else
            {
                Console.WriteLine("Thank you for using the Shopping Cart App!");
                Console.WriteLine("Exiting...");
            }
        }
    }
}
