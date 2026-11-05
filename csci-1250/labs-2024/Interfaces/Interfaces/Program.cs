/*
*--------------------------------------------------------------------
* File name: Interface
* Project name: Interface
*--------------------------------------------------------------------
* Author’s name and email: Daniel Macharia  [REDACTED EMAIL]
* Course-Section:  CSCI-1250-001
* Creation Date: 4/19/2024
* -------------------------------------------------------------------
* The code prompts the user to input their favorite character, animal, and professor.
* -------------------------------------------------------------------
*/




namespace Interfaces
{
    internal class Program
    {
        static void Main(string[] args)
        {
           

                string character;
                string animal;
                string professor;

                // Create objects and store them into a list of ISpeak

                Console.WriteLine("whats your fav character");
                character = Console.ReadLine();
                new SeinfeldCharacter(character);

                Console.WriteLine(" whats your fav Animal");
                animal= Console.ReadLine();
                new Animal("Woof woof!");

                Console.WriteLine("whos your fav professor");
                professor= Console.ReadLine();  
                new FavoriteProfessor("Welcome to my class!");

                // Iterate through the list and make everyone speak
                //foreach (var speaker in speakers)
                //{
                //    speaker.Speak();
                //    speaker.Speak("Additional message!");
                //}
            
        }
    }
}
    