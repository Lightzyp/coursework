using System;

class Audiobooks
{
    static void Main()
    {
        char package;
        int numBooks;

        Console.WriteLine("Welcome to the Audiobooks Order System");

        do
        {
            // Input validation for package selection
            do
            {
                Console.Write("Enter package (A, B, or C): ");
                package = Char.ToUpper(Console.ReadKey().KeyChar);

                if (package != 'A' && package != 'B' && package != 'C')
                {
                    Console.WriteLine("\nError: Invalid package. Please enter A, B, or C.");
                }
            } while (package != 'A' && package != 'B' && package != 'C');

            // Input validation for number of books
            do
            {
                Console.Write("\nEnter the number of books: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out numBooks) || numBooks < 1)
                {
                    Console.WriteLine("Error: Invalid input. Please enter a positive integer.");
                }
                else
                {
                    break;
                }
            } while (true);

            // Calculate and display the order total
            double total = CalculateOrderTotal(package, numBooks);
            Console.WriteLine($"\nYour total cost is: ${total:F2}");

            // Ask if the user wants to repeat
            Console.Write("\nDo you want to place another order? (Y/N): ");
        } while (Char.ToUpper(Console.ReadKey().KeyChar) == 'Y');

        Console.WriteLine("\nThank you for using the Audiobooks Order System.");
    }

    static double CalculateOrderTotal(char package, int numBooks)
    {
        const double PackageA = 12.99;
        const double PackageB = 24.99;
        const double PackageC = 49.99;

        double total = 0.0;

        switch (package)
        {
            case 'A':
                total = PackageA * numBooks;
                break;
            case 'B':
                total = PackageB * numBooks;
                break;
            case 'C':
                total = PackageC * numBooks;
                break;
        }

        return total;
    }
}
