/*       
 *--------------------------------------------------------------------
 * 	   File name: Program.cs
 * 	Project name: File_IO_Lab
 *--------------------------------------------------------------------
 * Author’s name and email:	   Daniel Macharia [REDACTED EMAIL] 			
 *          Course-Section: CSCI-1250-001 
 *           Creation Date:	 04/27/2024	
 * -------------------------------------------------------------------
 * This file contains the main program logic for a Guitarist management system.
 * It demonstrates the use of classes like Guitar, GuitarPlayer, and GuitarFileReader
 * to read and process guitarist data from an XML file, display guitarists, and
 * calculate statistics such as the average age of guitarists and the most common
 * guitar brand.
 * -------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace File_IO_Lab
{
    internal class Program
    {
        static string filePath = "Guitarist.xml"; // Path to the XML file containing guitarist data

        // Entry point of the program
        static void Main(string[] args)
        {
            // Part 2: Implement Guitar class
            Guitar guitar1 = new Guitar("Fender", "Stratocaster"); // Create a new Guitar object
            Console.WriteLine(guitar1); // Print guitar details

            // Part 3: Implement GuitarPlayer class
            DateTime birthday = new DateTime(1942, 11, 27); // Jimi Hendrix's birthday
            GuitarPlayer jimiHendrix = new GuitarPlayer("Jimi Hendrix", birthday, new DateTime(1970, 9, 18), "The Jimi Hendrix Experience", guitar1); // Create a new GuitarPlayer object
            Console.WriteLine(jimiHendrix); // Print guitarist details

            // Part 4: Implement GuitarFileReader class
            GuitarFileReader fileReader = new GuitarFileReader(filePath); // Create a GuitarFileReader object with the file path
            List<GuitarPlayer> guitarPlayers = fileReader.ReadFile(); // Read guitarist data from the XML file

            // Part 5: Display Guitarists and Statistics
            DisplayGuitarists(guitarPlayers); // Display all guitarists
            DisplayStatistics(guitarPlayers); // Display statistics about guitarists
        }

        // Display all guitarists
        static void DisplayGuitarists(List<GuitarPlayer> guitarPlayers)
        {
            Console.WriteLine("\nAll Guitarists:");
            foreach (var player in guitarPlayers)
            {
                Console.WriteLine(player);
            }
        }

        // Display statistics about guitarists
        static void DisplayStatistics(List<GuitarPlayer> guitarPlayers)
        {
            int totalGuitarists = guitarPlayers.Count; // Total number of guitarists

            if (totalGuitarists > 0) // Check if there are guitarists
            {
                int deceasedCount = guitarPlayers.Count(p => p.IsDeceased); // Count of deceased guitarists
                double deceasedPercentage = (double)deceasedCount / totalGuitarists * 100; // Percentage of deceased guitarists
                double averageAge = guitarPlayers.Select(p => p.GetAge()).DefaultIfEmpty(0).Average(); // Average age of guitarists

                // Calculate other statistics such as most common brand

                // Display statistics
                Console.WriteLine("\nStatistics:");
                Console.WriteLine($"Total Guitarists: {totalGuitarists}");
                Console.WriteLine($"Percentage of Deceased Guitarists: {deceasedPercentage:F2}%");
                Console.WriteLine($"Average Age of Guitarists: {averageAge:F2}");
            }
            else
            {
                Console.WriteLine("No guitar players found to display statistics.");
            }
        }

        // Get the most common item from a dictionary
        static string GetMostCommonItem(Dictionary<string, int> dictionary)
        {
            if (dictionary.Count == 0)
                return "N/A";

            KeyValuePair<string, int> maxItem = dictionary.Aggregate((x, y) => x.Value > y.Value ? x : y);
            return maxItem.Key;
        }
    }
}