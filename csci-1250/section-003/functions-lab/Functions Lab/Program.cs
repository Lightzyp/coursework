using System;

namespace Application
{
    class Program
    {
        
        static void Greeter()
        {
            Console.WriteLine("Hello, Mr. Gillenwater!");
            Console.WriteLine("My name is Daniel but I go by Dan.");
            Console.WriteLine("The class that I'm in is Intro to Computer Science.");
            Console.WriteLine("The section number that I'm in is CSCI-1250-003.");
        }

        
        static double ConvertToCelsius(double fahrenheit)
        {
            return (fahrenheit - 32) * 5 / 9;
        }

        static double GetNumber()
        {
            double number;
            while (true)
            {
                Console.Write("\nEnter a number: ");
                string userInput = Console.ReadLine();

                if (double.TryParse(userInput, out number))
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }
        }

        
        static int Sum(int n)
        {
            if (n == 1)
            {
                return 1;
            }
            else
            {
                return Sum(n - 1) + n;
            }
        }

        
        static void Main(string[] args)
        {
            
            Greeter();

            
            Console.Write("Give me a temperature in Fahrenheit: ");
            double fahrenheit = GetNumber();
            double celsius = ConvertToCelsius(fahrenheit);
            Console.WriteLine($"The temperature in Celsius is {celsius}°C");

            
            Console.Write("Enter a positive number: ");
            int n = GetPositiveNumber();
            int result = Sum(n);
            Console.WriteLine($"The sum of the first number is {n} and the result is:{result}");
        }

        
        static int GetPositiveNumber()
        {
            int number;
            while (true)
            {
                Console.Write("");
                string userInput = Console.ReadLine();
                if (int.TryParse(userInput, out number) && number > 0)
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("Please enter a valid positive number.");
                }
            }
        }
    }
}
