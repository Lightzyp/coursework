/*
*--------------------------------------------------------------------
* File name: Interface
* Project name: SeinfeldCharacter
*--------------------------------------------------------------------
* Author’s name and email: Daniel Macharia  [REDACTED EMAIL]
* Course-Section:  CSCI-1250-001
* Creation Date: 4/19/2024
* -------------------------------------------------------------------
* 
This code defines a class SeinfeldCharacter that implements the ISpeak interface. It allows instances of SeinfeldCharacter to speak and change text color.
* -------------------------------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    internal class SeinfeldCharacter : ISpeak
    {
        public string Phrase { get; set; }

        public SeinfeldCharacter(string phrase)
        {
            Phrase = phrase;
        }

        public void Speak()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Phrase);
            Console.ResetColor();
            Console.Beep();
        }

        public void Speak(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.Beep();
        }
    }
}
        
