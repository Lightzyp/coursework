/**
 * --------------------------------------------------------------------
 * File name: CircusLoan.cs
 * Project name: Lab 6
 * Solution name: CircusLoan.cs
 * --------------------------------------------------------------------
 * Author’s name and email: Daniel Macharia [REDACTED EMAIL]
 * Course-Section: CSCI 1250
 * Creation Date: Oct. 25, 2023
 * Modified Date: Oct. 25, 2023
 * -------------------------------------------------------------------
 */

using System;

// Define a class named "CircusLoan"
class CircusLoan
{
    // Constants to define the minimum salary and credit rating required for the loan
    const decimal MinSalaryRequired = 20000.00M;
    const int MinCreditRatingRequired = 7;

    // Define a static method "Qualify" to display a message when the applicant qualifies for the loan
    static void Qualify()
    {
        Console.WriteLine("Congratulations! You qualify for the circus loan.");
    }

    // Define a static method "NoQualify" to display a message when the applicant does not qualify for the loan
    static void NoQualify()
    {
        Console.WriteLine("Sorry, you do not qualify for the circus loan.");
    }

    // Define the entry point of the program, the "Main" method
    static void Main()
    {
        // Initialize variables to store the salary and credit rating of the applicant
        decimal salary;
        int creditRating;

        // Create an infinite loop that continues until the user chooses to exit
        while (true)
        {
            // Prompt the user to enter their annual salary
            Console.Write("Enter your annual salary: ");

            // Read the user's input and validate if it's a valid decimal (salary)
            if (!decimal.TryParse(Console.ReadLine(), out salary))
            {
                Console.WriteLine("Invalid input. Please enter a valid salary.");
                continue; // Continue to the next iteration of the loop
            }

            // Prompt the user to enter their credit rating on a scale of 1 to 10
            Console.Write("On a scale of 1 through 10, what is your credit rating? (10 = excellent, 1 = very bad): ");

            // Read the user's input and validate if it's a valid integer (creditRating)
            if (!int.TryParse(Console.ReadLine(), out creditRating))
            {
                Console.WriteLine("Invalid input. Please enter a valid credit rating.");
                continue; // Continue to the next iteration of the loop
            }

            // Check if the applicant's salary and credit rating meet the minimum requirements for the loan
            if (salary >= MinSalaryRequired && creditRating >= MinCreditRatingRequired)
            {
                Qualify(); // Display a message indicating qualification
            }
            else
            {
                NoQualify(); // Display a message indicating disqualification
            }

            // Ask the user if they want to enter another loan request
            Console.Write("Do you want to enter another loan request? (Y/N): ");

            // Read the user's input, trim any leading/trailing whitespace, and convert it to uppercase
            if (Console.ReadLine().Trim().ToUpper() != "Y")
            {
                break; // Exit the loop if the user's response is not "Y"
            }
        }
    }
}
