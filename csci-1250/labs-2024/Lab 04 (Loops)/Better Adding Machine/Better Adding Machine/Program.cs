/*
*--------------------------------------------------------------------
* File name: BetterAddingMachine.cs
* Project name: Better Adding Machine
*--------------------------------------------------------------------
* Author’s name and email: Daniel Macharia, [REDACTED EMAIL]
* Course-Section: CSCI-1250-001 
* Creation Date: Feb 27, 2024
* -------------------------------------------------------------------
* This program allows the user to enter multiple prices, calculates
* the total, and displays it. The user can specify how many prices
* they want to enter and provide the prices accordingly.
* -------------------------------------------------------------------
*/

using System;

namespace Better_Adding_Machine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Prompt user to enter the number of prices
            Console.WriteLine("I will keep a running total. How many prices would you like to enter?");
            int numberOfPrices;

            // Validate user input for the number of prices
            while (!int.TryParse(Console.ReadLine(), out numberOfPrices) || numberOfPrices <= 0)
            {
                Console.WriteLine("Please enter a valid positive number.");
            }

            decimal total = 0;

            // Loop to input prices and calculate total
            for (int i = 1; i <= numberOfPrices; i++)
            {
                Console.WriteLine($"Enter price {i}:");
                decimal price;

                // Validate user input for price
                while (!decimal.TryParse(Console.ReadLine(), out price) || price < 0)
                {
                    Console.WriteLine("Please enter a valid positive number.");
                }

                total += price; // Add price to total
            }

            // Display the total amount
            Console.WriteLine($"The total amount is: ${total:F2}");
        }
    }
}
