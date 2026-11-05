/*       
 *--------------------------------------------------------------------
 * 	   File name: GuitarFileReader.cs
 * 	Project name: File_IO_Lab
 *--------------------------------------------------------------------
 * Author’s name and email:	   Daniel Macharia [REDACTED EMAIL] 			
 *          Course-Section: CSCI-1250-001 
 *           Creation Date:	 04/27/2024	
 * -------------------------------------------------------------------
 * This file contains the definition of the GuitarFileReader class.
 * The GuitarFileReader class is responsible for reading guitarist data from a file
 * and returning a list of GuitarPlayer objects.
 * -------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;

namespace File_IO_Lab
{
    internal class GuitarFileReader
    {
        private string filePath;

        public GuitarFileReader(string filePath)
        {
            this.filePath = filePath;
        }

        public List<GuitarPlayer> ReadFile()
        {
            // Implementation to read file and return list of GuitarPlayer objects
            return new List<GuitarPlayer>();
        }
    }
}