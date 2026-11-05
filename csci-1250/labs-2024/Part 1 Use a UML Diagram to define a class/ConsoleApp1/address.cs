using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp1.Program;

namespace ConsoleApp1
{
    internal class address
    {

        class Program
        {
            static void Main(string[] args)
            {
                Address myAddress = new Address("123", "Main St", "Cityville", "12345", "State", "Country");

                Console.WriteLine("Original Address:");
                Console.WriteLine(myAddress.GetAddress());

                // Call the ChangeAddress method
                ChangeAddress(myAddress);

                Console.WriteLine("\nAddress after ChangeAddress method call:");
                Console.WriteLine(myAddress.GetAddress());

                // Call the ChangeAddress method again with a new address object
                ChangeAddressNewObject(myAddress);

                Console.WriteLine("\nAddress after ChangeAddressNewObject method call:");
                Console.WriteLine(myAddress.GetAddress());
            }

            static void ChangeAddress(Address address)
            {
                // Changing something about the address
                address.SetStreet("New Street");
            }

            static void ChangeAddressNewObject(Address address)
            {
                // Setting the parameter equal to a NEW address object
                address = new Address("456", "Broadway", "Townsville", "54321", "New State", "New Country");
            }
        }
    }
}
