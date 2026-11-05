/**
*--------------------------------------------------------------------
* File name: SpeedOfSoundCalculator.cs
* Project name: Speed of Sound Calculator
*--------------------------------------------------------------------
* Author’s name and email:Daniel Macharia, [REDACTED EMAIL]
* Course-Section: Computer Science 1250
* Creation Date: Feb, 16 2024
* -------------------------------------------------------------------
* This program calculates the time it takes for sound to travel a
* certain distance in different mediums (air, water, steel) based on
* user input.
* -------------------------------------------------------------------
*/
namespace SpeedOfSoundCalculator
{
    internal class Program
    {
        static void Main(string[] args)

        {
           
            Console.WriteLine("enter the medium (air, water, or steel) ");
            string medium = Console.ReadLine().ToLower();

            Console.WriteLine("Enter the distance that a sound wave will travel in the medium (in feet):");
            double distance = Convert.ToDouble(Console.ReadLine());

            double time;
            // Calculate the time it takes for the sound wave to travel based on the selected medium
            switch (medium)
            {
                case "air":
                    time = distance / 1100; // Speed of sound in air is approximately 1100 feet per second
                    break;
                case "water":
                    time = distance / 4900; // Speed of sound in water is approximately 4900 feet per second
                    break;
                case "steel":
                    time = distance / 16400; // Speed of sound in steel is approximately 16400 feet per second
                    break;
                default:
                    // If the user enters an invalid medium, display an error message and exit the program
                    Console.WriteLine("Invalid medium.");
                    return;

                    Console.WriteLine($"It will take {time} seconds for the sound wave to travel {distance} feet in {medium}.");
        }
    }
}
    }
