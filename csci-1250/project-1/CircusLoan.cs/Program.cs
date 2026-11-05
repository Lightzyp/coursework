/**
*--------------------------------------------------------------------
*File name: CircusLoan.CS
* Project name: CircusLoan.CS
* Solution name: CircusLoan.CS
* --------------------------------------------------------------------
*Author’s name and email: Daniel Macharia [REDACTED EMAIL]
* Course-Section: CSCI 1250
* Creation Date: Oct. 25, 2023
* Modified Date: Oct. 25, 2023
* -------------------------------------------------------------------
*/
using System;

class CircusLoan
{
    // Constants
    const decimal MinSalaryRequired = 20000.00M;
    const int MinCreditRatingRequired = 7;

    static void Qualify()
    {
        Console.WriteLine("Congratulations! You qualify for the circus loan.");
    }

    static void NoQualify()
    {
        Console.WriteLine("Sorry, you do not qualify for the circus loan.");
    }

    static void Main()
    {
        // Initialize variables
        decimal salary;
        int creditRating;

        while (true)
        {
            Console.Write("Enter your annual salary: ");
            if (!decimal.TryParse(Console.ReadLine(), out salary))
            {
                Console.WriteLine("Invalid input. Please enter a valid salary.");
                continue;
            }

            Console.Write("On a scale of 1 throught 10 , what is your credit rating? (10 = excellent, 1 = very bad)");
            if (!int.TryParse(Console.ReadLine(), out creditRating))
            {
                Console.WriteLine("Invalid input. Please enter a valid credit rating.");
                continue;
            }

            if (salary >= MinSalaryRequired && creditRating >= MinCreditRatingRequired)
            {
                Qualify();
            }
            else
            {
                NoQualify();
            }

            Console.Write("Do you want to enter another loan request? (Y/N): ");
            if (Console.ReadLine().Trim().ToUpper() != "Y")
            {
                break;
            }
        }
    }
}



