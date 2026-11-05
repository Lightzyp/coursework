/*       
 *--------------------------------------------------------------------
 * 	   File name: GuitarPlayer.cs
 * 	Project name: File_IO_Lab
 *--------------------------------------------------------------------
 * Author’s name and email:	   Daniel Macharia [REDACTED EMAIL] 			
 *          Course-Section: CSCI-1250-001 
 *           Creation Date:	 04/27/2024	
 * -------------------------------------------------------------------
 * This file contains the definition of the GuitarPlayer class.
 * The GuitarPlayer class represents a guitarist with properties for name, birthday, band,
 * whether they are deceased, and their favorite guitar.
 * It also provides methods to calculate the age of the guitarist and overrides the ToString
 * method for custom string representation.
 * -------------------------------------------------------------------
 */

using System;

namespace File_IO_Lab
{
    internal class GuitarPlayer
    {
        // Properties
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime? DeceasedOn { get; set; }
        public string Band { get; set; }
        public bool IsDeceased { get; private set; }
        public Guitar FavoriteGuitar { get; set; }

        // Constructor
        public GuitarPlayer(string name, DateTime birthday, DateTime? deceasedOn, string band, Guitar favoriteGuitar)
        {
            Name = name;
            Birthday = birthday;
            DeceasedOn = deceasedOn;
            Band = band;
            FavoriteGuitar = favoriteGuitar;

            // Check if player is deceased
            IsDeceased = DeceasedOn != null;
        }

        // Method to calculate the age of the guitarist
        public int GetAge()
        {
            DateTime now = DateTime.Now;
            int age = now.Year - Birthday.Year;
            if (now.Month < Birthday.Month || (now.Month == Birthday.Month && now.Day < Birthday.Day))
                age--;
            return age;
        }

        // Override ToString method for custom string representation
        public override string ToString()
        {
            string deceasedInfo = IsDeceased ? $", Deceased on: {DeceasedOn?.ToShortDateString()}" : "";
            return $"Name: {Name}, Birthday: {Birthday.ToShortDateString()}, Age: {GetAge()}, Band: {Band}, Favorite Guitar: {FavoriteGuitar}{deceasedInfo}";
        }
    }
}