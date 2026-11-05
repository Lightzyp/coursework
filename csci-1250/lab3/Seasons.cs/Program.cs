using System;

class Program
{
    static void Main(string[] args)
    {
        // Prompt the user to enter the name of a season
        Console.Write("Enter the name of a season, and I'll translate it to Spanish: ");
        string inputSeason = Console.ReadLine();

        // Convert the input to lowercase to accommodate all letter cases
        inputSeason = inputSeason.ToLower();

        // Initialize the message variable
        string message;

        // Use a switch statement to translate the season to Spanish
        switch (inputSeason)
        {
            case "spring":
                message = "la primavera";
                break;
            case "summer":
                message = "el Verano";
                break;
            case "autumn":
            case "fall":
                message = "el otono";
                break;
            case "winter":
                message = "el invierno";
                break;
            default:
                message = "Invalid input";
                break;
        }

        // Display the translated message or an error message
        Console.WriteLine(message);

        // Display a friendly closing message
        Console.WriteLine("Thank you for using the season translator!");
    }
}
