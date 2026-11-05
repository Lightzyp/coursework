/*
*--------------------------------------------------------------------
* File name: Interface
* Project name: Animal
*--------------------------------------------------------------------
* Author’s name and email: Daniel Macharia  [REDACTED EMAIL]
* Course-Section:  CSCI-1250-001
* Creation Date: 4/19/2024
* -------------------------------------------------------------------
* The code prompts the user to input their favorite character, animal, and professor.
* -------------------------------------------------------------------
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    internal class Animal : ISpeak
    {
        public string Phrase { get; set; }

        public Animal(string phrase)
        {
            Phrase = phrase;
        }

        public void Speak()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Phrase);
            Console.ResetColor();
            Console.Beep();
        }

        public void Speak(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.Beep();
        }
    }
}
