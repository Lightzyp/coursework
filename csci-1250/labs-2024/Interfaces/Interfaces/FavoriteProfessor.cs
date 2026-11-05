/*
*--------------------------------------------------------------------
* File name: Interface
* Project name: Favprofessor
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
    internal class FavoriteProfessor : ISpeak
    {
        public string Phrase { get; set; }

        public FavoriteProfessor(string phrase)
        {
            Phrase = phrase;
        }

        public void Speak()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Phrase);
            Console.ResetColor();
            Console.Beep();
        }

        public void Speak(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.Beep();
        }
    }
}
