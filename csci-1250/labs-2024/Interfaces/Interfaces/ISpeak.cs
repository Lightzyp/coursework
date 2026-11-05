/*
*--------------------------------------------------------------------
* File name: Interface
* Project name: ISpeak
*--------------------------------------------------------------------
* Author’s name and email: Daniel Macharia  [REDACTED EMAIL]
* Course-Section:  CSCI-1250-001
* Creation Date: 4/19/2024
* -------------------------------------------------------------------
* This code defines an interface ISpeak with properties and methods for speaking
* -------------------------------------------------------------------
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    internal interface ISpeak
    {
        string Phrase { get; set; }
        void Speak();
        void Speak(string message);
    }
}
