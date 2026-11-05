using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML_Diagram
{
    internal class Address
    {
        class Program
        {
            // Properties
            private string LotNumber { get; set; }
            private string Street { get; set; }
            private string City { get; set; }
            private string ZipCode { get; set; }
            private string State { get; set; }
            private string Country { get; set; }

            // Constructor
            public Program(string lotNumber, string street, string city, string zipCode, string state, string country)
            {
                LotNumber = lotNumber;
                Street = street;
                City = city;
                ZipCode = zipCode;
                State = state;
                Country = country;
            }

            // Methods
            public string GetAddress()
            {
                return $"{LotNumber}, {Street}, {City}, {ZipCode}, {State}, {Country}";
            }

            public void ChangeAddress()
            {
                // Changing something about the address
                Street = "New Street";
            }

            public void ChangeAddressNewObject()
            {
                // Setting the object equal to a NEW address object
                LotNumber = "456";
                Street = "Broadway";
                City = "Townsville";
                ZipCode = "54321";
                State = "New State";
                Country = "New Country";
            }

            static void Main(string[] args)
            {
                Program myAddress = new Program("123", "Main St", "Cityville", "12345", "State", "Country");

                Console.WriteLine("Original Address:");
                Console.WriteLine(myAddress.GetAddress());

                // Call the ChangeAddress method
                myAddress.ChangeAddress();

                Console.WriteLine("\nAddress after ChangeAddress method call:");
                Console.WriteLine(myAddress.GetAddress());

                // Call the ChangeAddress method again with a new address object
                myAddress.ChangeAddressNewObject();

                Console.WriteLine("\nAddress after ChangeAddressNewObject method call:");
                Console.WriteLine(myAddress.GetAddress());
            }
        }
    }
}
