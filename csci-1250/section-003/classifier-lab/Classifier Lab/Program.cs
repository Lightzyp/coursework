using System;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            //Part 1: Biggest Number
            Console.WriteLine("Enter a number!");

            int A = int.Parse(Console.ReadLine());

            Console.WriteLine($"Your first number is {A}");

            Console.WriteLine("Enter your second number");

            int B = int.Parse(Console.ReadLine());

            Console.WriteLine($"Your second number is {B}");


            if (A > B)
            {
                Console.WriteLine("Player A wins");

            }

            if (B > A)
            {
                Console.WriteLine("Player B wins");
            }

            if (A == B)
            {
                Console.WriteLine("Its a Tie");
            }

            //Part 2: Is Multiple of 5

            Console.WriteLine("Enter a Number!");

            int multipleof5 = int.Parse(Console.ReadLine());

            if (multipleof5 % 5 == 0)
            {
                Console.WriteLine(multipleof5 + "is a multiple of 5");
            }

            else
            {
                Console.WriteLine(multipleof5 + "is not a multiple of 5");
            }

            //Part 3: Grade Classifier

            Console.WriteLine("Enter a grade");

            double Grade = int.Parse(Console.ReadLine());

            Console.WriteLine($"Your Grade is {Grade}");

            if (Grade >= 90.0)
            {
                Console.WriteLine("You have an A!!");
            }

            else if (Grade >= 80.0)
            {
                Console.WriteLine("You have a B!!");
            }

            else if (Grade >= 70.0)
            {
                Console.WriteLine("You Have a C!!");
            }

            else if (Grade >= 60.0)
            {
                Console.WriteLine("You have a D!!");
            }

            else
            {
                Console.WriteLine("You have a F!! :(");
            }

            //Part 4: Character Classifier

            Console.WriteLine("Enter a single character or letter");

            char inputChar = Console.ReadKey().KeyChar;

            Console.WriteLine(inputChar);

            if (char.IsUpper(inputChar))
            {
                Console.WriteLine("The character is an uppercase letter.");
            }
            else if (char.IsLower(inputChar))
            {
                Console.WriteLine("The character is a lowercase letter.");
            }
            else if (char.IsDigit(inputChar))
            {
                Console.WriteLine("The character is a numerical digit.");
            }
            else
            {
                Console.WriteLine("The character is a special character.");
            }

            //Part 5: Discount Calculator

            Console.Write("Enter the total cost of the item(s): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal totalCost))
            {
                Console.WriteLine("Invalid input. Please enter a valid decimal value.");
                return;
            }

            Console.Write("Enter your birth year: ");
            if (!int.TryParse(Console.ReadLine(), out int birthYear))
            {
                Console.WriteLine("Invalid input. Please enter a valid year.");
                return;
            }

            Console.Write("Are you a student? (Y/N): ");
            char studentResponse = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();
            bool isStudent = studentResponse == 'Y';


            Console.Write("Do you qualify for a military discount? (Y/N): ");
            char militaryResponse = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();
            bool hasMilitaryDiscount = militaryResponse == 'Y';


            int currentYear = DateTime.Now.Year;


            int age = currentYear - birthYear;


            decimal discount = 0m;
            if ((age < 18 || hasMilitaryDiscount) && !isStudent)
            {
                discount = 0.10m;
            }
            else if (isStudent && age < 25)
            {
                discount = 0.15m;
            }
            else if (isStudent && hasMilitaryDiscount)
            {
                discount = 0.30m;
            }


            decimal discountAmount = totalCost * discount;
            decimal finalCost = totalCost - discountAmount;


            Console.WriteLine($"\nOriginal cost: ${totalCost.ToString("0.00")}");
            Console.WriteLine($"Discount: {discount * 100}% (${discountAmount.ToString("0.00")})");
            Console.WriteLine($"Final cost: ${finalCost.ToString("0.00")}");

            //Part 6: Leap Year Classifier
            Console.Write("Enter a year: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("Invalid input. Please enter a valid year.");
                return;
            }

            // Get the current year
            int Year = DateTime.Now.Year;

            // Check if the year is a leap year
            bool isLeapYear = (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);

            // Determine if the year is in the past or future
            if (isLeapYear)
            {
                if (year < currentYear)
                    Console.WriteLine($"{year} was a leap year.");
                else
                    Console.WriteLine($"{year} will be a leap year.");
            }
            else
            {
                if (year < currentYear)
                    Console.WriteLine($"{year} was NOT a leap year.");
                else
                    Console.WriteLine($"{year} will NOT be a leap year.");
            }


        }



    }
}
