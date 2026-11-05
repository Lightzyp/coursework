/**
 *--------------------------------------------------------------------
 * File name: Conversions.cs
 * Project name: Lab 6
 * Solution name: Conversions.cs
 * --------------------------------------------------------------------
 * Author’s name and email: Daniel Macharia [REDACTED EMAIL]
 * Course-Section: CSCI 1250
 * Creation Date: Oct. 25, 2023
 * Modified Date: Oct. 25, 2023
 * -------------------------------------------------------------------
 */

// Import the System namespace, which contains essential functions for console I/O
using System;

// Define a class named "Conversion"
class Conversion
{
    // Define a static method named "ShowKilometers" to convert and display meters to kilometers
    static void ShowKilometers(double meters)
    {
        double kilometers = meters * 0.001;
        Console.WriteLine($"Meters to Kilometers: {kilometers:F3} km");
    }

    // Define a static method named "ShowInches" to convert and display meters to inches
    static void ShowInches(double meters)
    {
        double inches = meters * 39.37;
        Console.WriteLine($"Meters to Inches: {inches:F3} inches");
    }

    // Define a static method named "ShowFeet" to convert and display meters to feet
    static void ShowFeet(double meters)
    {
        double feet = meters * 3.281;
        Console.WriteLine($"Meters to Feet: {feet:F3} feet");
    }

    // Define a static method named "Menu" to display the conversion options to the user
    static void Menu()
    {
        Console.WriteLine("1. Convert to kilometers");
        Console.WriteLine("2. Convert to inches");
        Console.WriteLine("3. Convert to feet");
        Console.WriteLine("0. Quit the program");
    }

    // Define the entry point of the program, the "Main" method
    static void Main()
    {
        double meters;  // Declare a variable to store the distance in meters
        int choice = -1; // Initialize choice to an invalid value

        do
        {
            // Prompt the user to enter a distance in meters
            Console.Write("Enter the distance in meters: ");

            // Read the user's input and validate if it's a non-negative number
            if (!double.TryParse(Console.ReadLine(), out meters) || meters < 0)
            {
                Console.WriteLine("Invalid input. Please enter a non-negative number.");
                continue; // Continue to the next iteration of the loop
            }

            // Display the conversion menu
            Menu();

            // Prompt the user to enter their choice
            Console.Write("Enter your choice: ");

            // Read the user's input and validate if it's a valid menu option
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid choice. Please enter a valid menu option.");
                continue; // Continue to the next iteration of the loop
            }

            // Use a switch statement to perform the selected conversion or quit the program
            switch (choice)
            {
                case 1:
                    ShowKilometers(meters);
                    break;
                case 2:
                    ShowInches(meters);
                    break;
                case 3:
                    ShowFeet(meters);
                    break;
                case 0:
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid menu option.");
                    break;
            }

        } while (choice != 0); // Repeat the loop until the user chooses to quit (choice = 0)
    }
}
