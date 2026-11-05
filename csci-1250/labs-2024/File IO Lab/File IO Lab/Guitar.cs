/*       
 *--------------------------------------------------------------------
 * 	   File name: Guitar.cs
 * 	Project name: File_IO_Lab
 *--------------------------------------------------------------------
 * Author’s name and email:	   Daniel Macharia [REDACTED EMAIL] 			
 *          Course-Section: CSCI-1250-001 
 *           Creation Date:	 04/27/2024	
 * -------------------------------------------------------------------
 * This file contains the definition of the Guitar class.
 * The Guitar class represents a guitar with properties for brand and model.
 * It also overrides the ToString method for custom string representation.
 * -------------------------------------------------------------------
 */

namespace File_IO_Lab
{
    internal class Guitar
    {
        // Properties
        public string Brand { get; set; } // Stores the brand of the guitar
        public string Model { get; set; } // Stores the model of the guitar

        // Constructor
        public Guitar(string brand, string model)
        {
            Brand = brand; // Initialize the brand property
            Model = model; // Initialize the model property
        }

        // Override ToString method for custom string representation
        public override string ToString()
        {
            return $"Guitar: {Brand} {Model}"; // Returns a string representation of the guitar
        }
    }
}