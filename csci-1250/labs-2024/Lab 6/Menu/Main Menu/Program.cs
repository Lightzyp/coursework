/*
*--------------------------------------------------------------------
* File name: Menu
* Project name: Lab 6 show Charater i named it worng
*--------------------------------------------------------------------
* Author’s name and email: Daniel Macharia [REDACTED EMAIL]
* Course-Section: CSCI-1250-001
* Creation Date: 3/1/2024
* -------------------------------------------------------------------
* This file contains the main program logic for executing various methods.
* It displays a menu to the user and executes the selected method.
* -------------------------------------------------------------------
*/

namespace Show_Character
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int choice;

            // Main loop for displaying menu and executing chosen method
            do
            {
                // Displaying menu options
                DisplayMenu();

                // Prompting user for choice
                Console.Write("Enter your choice: ");

                // Validating and parsing user input
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                // Switch case to execute the chosen method
                switch (choice)
                {
                    case 1:
                        RunShowCharacter(); // Executes Method 1: Show Character
                        break;
                    case 2:
                        RunCalculateRetail(); // Executes Method 2: Retail Price
                        break;
                    case 3:
                        DisplayTemperatureTable(); // Executes Method 3: Temperature Table
                        break;
                    case 4:
                        RunIsPrime(); // Executes Method 4: Prime Numbers
                        break;
                    case 5:
                        SimpleAddingMachine(); // Executes Method 5: Simple Adding Machine
                        break;
                    case 6:
                        RunPyramid(); // Executes Method 6: Pyramid
                        break;
                    case 9:
                        Console.WriteLine("Exiting program...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            } while (choice != 9);
        }

        // Section: Menu and User Input

        // Method to display menu options
        static void DisplayMenu()
        {
            Console.WriteLine("Method 0: Menu");
            Console.WriteLine("Method 1: Show Character");
            Console.WriteLine("Method 2: Retail Price");
            Console.WriteLine("Method 3: Temperature Table");
            Console.WriteLine("Method 4: Prime Numbers");
            Console.WriteLine("Method 5: Simple Adding Machine");
            Console.WriteLine("Method 6: Pyramid");
            Console.WriteLine("Enter 9 to exit.");
        }

        // Section: Method 1 - Show Character

        // Method to run ShowCharacter functionality
        static void RunShowCharacter()
        {
            Console.Write("Enter a number: ");
            string str = Console.ReadLine();
            Console.Write("Enter the position of character (starting from 1): ");
            int position = GetValidIntegerInput();
            ShowCharacter(str, position);
        }

        // Method to show character at given position
        static void ShowCharacter(string str, int position)
        {
            if (position >= 1 && position <= str.Length)
            {
                Console.WriteLine($"Character at position {position}: {str[position - 1]}");
            }
            else
            {
                Console.WriteLine("Invalid position.");
            }
        }

        // Section: Method 2 - Retail Price

        // Method to run CalculateRetail functionality
        static void RunCalculateRetail()
        {
            double cost = GetValidDoubleInput("Enter the wholesale cost: ");
            double markup = GetValidDoubleInput("Enter the markup percentage: ");
            Console.WriteLine($"Retail price: {CalculateRetail(cost, markup):0.00}");
        }

        // Method to calculate retail price
        static double CalculateRetail(double wholesaleCost, double markupPercentage)
        {
            return wholesaleCost * (1 + markupPercentage / 100);
        }

        // Section: Method 3 - Temperature Table

        // Method to display temperature table
        static void DisplayTemperatureTable()
        {
            Console.WriteLine("Fahrenheit   Celsius");
            for (int f = 80; f <= 100; f++)
            {
                double c = Celsius(f);
                Console.WriteLine($"{f,-12} {c,-8:0.00}");
            }
        }

        // Method to convert Fahrenheit to Celsius
        static double Celsius(double fahrenheit)
        {
            return 5.0 / 9.0 * (fahrenheit - 32);
        }

        // Section: Method 4 - Prime Numbers

        // Method to run IsPrime functionality
        static void RunIsPrime()
        {
            Console.Write("Enter a number to check if it's prime: ");
            int num = GetValidIntegerInput();
            Console.WriteLine($"Is prime: {IsPrime(num)}");
        }

        // Method to get valid integer input from user
        private static int GetValidIntegerInput()
        {
            int num;
            while (!int.TryParse(Console.ReadLine(), out num))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
                Console.Write("Enter a number: ");
            }
            return num;
        }

        // Method to check if a number is prime
        static bool IsPrime(int num)
        {
            if (num <= 1)
                return false;
            for (int i = 2; i <= Math.Sqrt(num); i++)
            {
                if (num % i == 0)
                    return false;
            }
            return true;
        }


        // Section: Method 5 - Simple Adding Machine

        // Method to run SimpleAddingMachine functionality
        static void SimpleAddingMachine()
        {
            do
            {
                int num1 = GetValidIntegerInput("Enter the first number: ");
                int num2 = GetValidIntegerInput("Enter the second number: ");
                Console.WriteLine($"Sum: {num1 + num2}");

                Console.Write("Do you want to perform the operation again? (yes/no): ");
            } while (Console.ReadLine().ToLower() == "yes");
        }

        // Section: Input Validation Methods

        // Method to get valid integer input
        static int GetValidIntegerInput(string v)
        {
            int num;
            while (!int.TryParse(Console.ReadLine(), out num))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }
            return num;
        }

        // Method to get valid double input with a prompt
        static double GetValidDoubleInput(string prompt)
        {
            double num;
            Console.Write(prompt);
            while (!double.TryParse(Console.ReadLine(), out num))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                Console.Write(prompt);
            }
            return num;
        }

        // Section: Method 6 - Pyramid

        // Method to run Pyramid functionality
        static void RunPyramid()
        {
            int height = GetValidPyramidHeight();
            DisplayPyramid(height);
        }

        // Method to display pyramid of given height
        static void DisplayPyramid(int height)
        {
            for (int i = 1; i <= height; i++)
            {
                for (int j = 1; j <= i; j++)
                {
                    Console.Write("* ");
                }
                Console.WriteLine();
            }
        }

        // Method to get valid pyramid height
        static int GetValidPyramidHeight()
        {
            int height;
            do
            {
                Console.Write("Enter the height of the pyramid (positive integer less than 50): ");
                while (!int.TryParse(Console.ReadLine(), out height) || height <= 0 || height >= 50)
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer less than 50.");
                }
            } while (height <= 0 || height >= 50);
            return height;
        }
    }
}