/**
*--------------------------------------------------------------------
*File name: Madlibs.cs
* Project name: Madlibs.cs
* Solution name: CruiseArray.cs
* --------------------------------------------------------------------
*Author’s name and email: Daniel [REDACTED EMAIL]
* Course-Section: CSCI 1250
* Creation Date: Sept. 13, 2023
* Modified Date: Sept. 13, 2023
* -------------------------------------------------------------------
*/
using System;

class Madlibs
{
    static void Main()
    {
        // Prompt user for input
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();

        Console.Write("Enter your age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter a city: ");
        string city = Console.ReadLine();

        Console.Write("Enter a country: ");
        string country = Console.ReadLine();

        Console.Write("Enter a profession: ");
        string profession = Console.ReadLine();

        Console.Write("Enter a type of animal: ");
        string animal = Console.ReadLine();

        Console.Write("Enter a pet's name: ");
        string petName = Console.ReadLine();

        // Display the story using user input
        Console.WriteLine($"There once was a person named {name} who lived in {city}. " +
                          $"At the age of {age}, {name} decided to move to {country} " +
                          $"and become a {profession}. Then, {name} adopted a(n) {animal} " +
                          $"named {petName}. They both lived happily ever after!");
    }
}
