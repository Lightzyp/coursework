/*
*--------------------------------------------------------------------
* File name: SimpleAddingMachine.cs
* Project name: Simple Adding Machine
*--------------------------------------------------------------------
* Author’s name and email: Daniel Macharia, [REDACTED EMAIL]
* Course-Section: CSCI-1250-001 
* Creation Date: Feb 27, 2024
* -------------------------------------------------------------------
* This program allows the user to enter two numbers, adds them together,
* and displays the sum. The user can choose to perform the operation
* again or terminate the program.
* -------------------------------------------------------------------
*/

using System;

namespace Simple_Adding_Machine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("I will add two numbers together.");

            // Prompt the user to enter the first number
            Console.Write("Enter the first number: ");
            int num1 = Convert.ToInt32(Console.ReadLine());

            // ask's user to enter the second number
            Console.Write("Enter the second number: ");
            int num2 = Convert.ToInt32(Console.ReadLine());

            // Calculate the sum and display it
            Console.WriteLine($"The sum of {num1} and {num2} is {num1 + num2}.");

            // Asking if they want to perform the operation again
            Console.Write("Would you like to perform this operation again? (y/n): ");
            string again = Console.ReadLine();

            // Check user's response and decide whether to continue or exit
            if (again.ToLower() != "y")
            {
                Console.WriteLine("Goodbye!");

                

            }
        }
    }
}




