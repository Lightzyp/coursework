/**
*--------------------------------------------------------------------
*File name: CruiseList.cs
* Project name: CruiseList.cs
* Solution name: CruiseList.cs
* --------------------------------------------------------------------
*Author’s name and email: Daniel Macharia [REDACTED EMAIL]
* Course-Section: CSCI 1250
* Creation Date: Oct. 10, 2023
* Modified Date: Oct. 10, 2023
* -------------------------------------------------------------------
*/
using System;

namespace CruiseArray
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize variables
            int numberOfCruises;

            // Ask the user how many cruises to price
            Console.Write("Enter the number of cruises: ");
            if (!int.TryParse(Console.ReadLine(), out numberOfCruises) || numberOfCruises <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive integer.");
                return;
            }

            // Initialize arrays to store cruise locations and prices
            string[] cruiseLocations = new string[numberOfCruises];
            double[] cruisePrices = new double[numberOfCruises];

            // Input cruise locations and prices
            for (int i = 0; i < numberOfCruises; i++)
            {
                Console.Write($"Enter the location for Cruise {i + 1}: ");
                cruiseLocations[i] = Console.ReadLine();

                Console.Write($"Enter the price for Cruise {i + 1}: ");
                if (!double.TryParse(Console.ReadLine(), out cruisePrices[i]) || cruisePrices[i] <= 0)
                {
                    Console.WriteLine("Invalid input. Please enter a valid positive price.");
                    i--; // Retry the current cruise input
                }
            }

            // Calculate total prices, max price, min price, and average price
            double totalPrices = 0;
            double maxPrice = double.MinValue;
            double minPrice = double.MaxValue;
            string maxLocation = "";
            string minLocation = "";

            for (int i = 0; i < numberOfCruises; i++)
            {
                totalPrices += cruisePrices[i];
                if (cruisePrices[i] > maxPrice)
                {
                    maxPrice = cruisePrices[i];
                    maxLocation = cruiseLocations[i];
                }
                if (cruisePrices[i] < minPrice)
                {
                    minPrice = cruisePrices[i];
                    minLocation = cruiseLocations[i];
                }
            }

            double averagePrice = totalPrices / numberOfCruises;

            // Display results to the user
            Console.WriteLine($"Most expensive cruise place: {maxLocation}, Price: {maxPrice:C2}");
            Console.WriteLine($"Least expensive cruise place: {minLocation}, Price: {minPrice:C2}");
            Console.WriteLine($"Average cruise price: {averagePrice:C2}");
        }
    }
}
