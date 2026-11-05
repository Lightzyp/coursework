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
using System.Collections.Generic;

class CruiseList
{
    static void Main()
    {
        List<string> cruiseLocations = new List<string>();
        List<double> cruisePrices = new List<double>();

        while (true)
        {
            Console.Write("Enter the location for the cruise: ");
            string location = Console.ReadLine();

            Console.Write("Enter the price for the cruise: ");
            if (!double.TryParse(Console.ReadLine(), out double price) || price <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid positive price.");
                continue;
            }

            cruiseLocations.Add(location);
            cruisePrices.Add(price);

            Console.Write("Do you want to add another cruise? (yes/no): ");
            string anotherCruise = Console.ReadLine().ToLower();

            if (anotherCruise != "yes")
                break;
        }

        if (cruiseLocations.Count == 0)
        {
            Console.WriteLine("No cruises were added.");
            return;
        }

        double totalPrices = 0;
        double maxPrice = double.MinValue;
        double minPrice = double.MaxValue;
        string maxLocation = "";
        string minLocation = "";

        foreach (double price in cruisePrices)
        {
            totalPrices += price;
            if (price > maxPrice)
            {
                maxPrice = price;
                maxLocation = cruiseLocations[cruisePrices.IndexOf(price)];
            }
            if (price < minPrice)
            {
                minPrice = price;
                minLocation = cruiseLocations[cruisePrices.IndexOf(price)];
            }
        }

        double averagePrice = totalPrices / cruiseLocations.Count;

        Console.WriteLine($"Most expensive cruise place: {maxLocation}, Price: {maxPrice:C2}");
        Console.WriteLine($"Least expensive cruise place: {minLocation}, Price: {minPrice:C2}");
        Console.WriteLine($"Average cruise price: {averagePrice:C2}");
    }
}
