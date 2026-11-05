using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Part_2___Requirements
{
    internal class Program
    {
        static void Main()
        {
            // Create a list of whole numbers
            List<int> numbers = new List<int> { 1, 10, 8, 2, 7 };

            // Display every item in the list
            Console.WriteLine("At first, the list looks like this");
            foreach (var number in numbers)
            {
                Console.Write($" {number} ");
            }

         
            // Sort the list
            numbers.Sort();

            // Display every item in the sorted list
            Console.WriteLine("\nSorting the list....");
            Console.WriteLine("This is how the list looks like in order");
            foreach (var number in numbers)
            {
                Console.Write($" {number} ");
            }

            // Calculate the average value of the list
            double average = numbers.Average();

            // Display the average with at most 2 decimal places
            Console.WriteLine($"\nAverage: {average}");

            // Display the smallest value
            Console.WriteLine($"Smallest value: {numbers.First()}");

            // Calculate the median value
            double median;
            if (numbers.Count % 2 == 0)
            {
                median = (numbers[numbers.Count / 2 - 1] + numbers[numbers.Count / 2]) / 2.0;
            }
            else
            {
                median = numbers[numbers.Count / 2];
            }

            // Display the median value
            Console.WriteLine($"Median value: {median}");

            // Display the largest value
            Console.WriteLine($"Largest value: {numbers.Last()}");

            // Add 3 more items to the list
            numbers.AddRange(new List<int> { -20, -3, -50 });

            Console.WriteLine($"Adding new numbers -20, -3, -50....");
            // Display the new list
            Console.WriteLine("This is what the updated like looks like");
            foreach (var number in numbers)
            {
                Console.Write($" {number} ");
            }
            // Sort the list again
            numbers.Sort();

            Console.WriteLine("\nHow the list looks sorted");
            foreach (var number in numbers)
            {
                Console.Write($" {number} ");
            }


            // Recalculate the average with at most 2 decimal places
            average = numbers.Average();

            // Display the updated average, smallest, median, and largest value
            Console.WriteLine("\nUpdated statistics after adding 3 more items:");
            Console.WriteLine($"Average: {average:F1}");
            Console.WriteLine($"Smallest value: {numbers.First()}");
            Console.WriteLine($"Median value: {median}");
            Console.WriteLine($"Largest value: {numbers.Last()}");
        }
    }
}
