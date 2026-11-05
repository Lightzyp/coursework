
using System.Transactions;
/**
*--------------------------------------------------------------------
* File name: Desions Part 2
* Project name: Desions Part 2
*--------------------------------------------------------------------
* Author’s name and email: Daniel Macharia, [REDACTED EMAIL]
* Course-Section: Computer Science 1250
* Creation Date: February 16, 2024
* -------------------------------------------------------------------
* This program translates seasons between English and Spanish based
* on user input.
* -------------------------------------------------------------------
*/
namespace Desions_Part_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Prompt the user to choose the translation direction
            Console.WriteLine("Would you like to Translate from English to Spanish Y/N");
            string translateDirection = Console.ReadLine().ToUpper();
            string seasons;

            // Translate from English to Spanish
            if (translateDirection == "Y")
            {
                Console.WriteLine("What season would you like to be translate to Spanish");
                Console.WriteLine("Spring, Summer, Fall, Winter");
                seasons = Console.ReadLine()!;

                // Translate the season to Spanish
                switch (seasons)
                {
                    case "Spring":
                        Console.WriteLine("Primavera");
                        break;

                    case "Summer":
                        Console.WriteLine("Verano");
                        break;

                    case "Fall":
                        Console.WriteLine("Caer");
                        break;

                    case "Winter":
                        Console.WriteLine("Invierno");
                        break;
                }
            }
            // Translate from Spanish to English
            else if (translateDirection == "N")
            {
                // Prompt the user for the season in Spanish
                Console.WriteLine("Enter the seasons you want in Spanish:");
                string season = Console.ReadLine().ToLower();

                // Translate the season to English
                switch (season)
                {
                    case "primavera":
                        Console.WriteLine("Spring");
                        break;
                    case "verano":
                        Console.WriteLine("Summer");
                        break;
                    case "otoño":
                        Console.WriteLine("Autumn / Fall");
                        break;
                    case "invierno":
                        Console.WriteLine("Winter");
                        break;

                }
            }
            else
            {
                //Invalid input
                Console.WriteLine("Invalid input. Please enter Y or N.");
            }
        }
    }
}