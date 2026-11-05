using System;

class RunningTotal
{
    static void Main()
    {
        Console.Write("Enter the number of prices: ");
        int numberOfPrices = int.Parse(Console.ReadLine());

        decimal total = 0; // Initialize the running total accumulator

        for (int i = 1; i <= numberOfPrices; i++)
        {
            Console.Write("Enter the price for item #" + i + ": $");
            decimal price = decimal.Parse(Console.ReadLine());

            total += price; // Add the current price to the running total
        }

        // Display the total price in the specified format
        Console.WriteLine("Total price: $" + total.ToString("F2"));
    }
}
