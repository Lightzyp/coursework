/**
*--------------------------------------------------------------------
* File name:  Use a UML Diagram to define a class
* Project name:  Use a UML Diagram to define a class
*--------------------------------------------------------------------
* Author’s name and email:Daniel Macharia, [REDACTED EMAIL]
* Course-Section: Computer Science 1250
* Creation Date: April, 1 2024
* -------------------------------------------------------------------
* This program defines a C# class named Address, which represents a
* physical address with various components such as lot number, street,
* city, zip code, state, and country
* -------------------------------------------------------------------
*/

using System;

namespace UML_Diagram
{
    internal class Program
    {
        public class Address
        {
            // Properties
            public string LotNumber { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string ZipCode { get; set; }
            public string State { get; set; }
            public string Country { get; set; }

            // Constructor
            public Address(string lotNumber, string street, string city, string zipCode, string state, string country)
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

            // Other getter and setter methods
            // ...

            // Implement setter methods for each property
            // For brevity, only one is shown here
            public void SetLotNumber(string lotNumber)
            {
                LotNumber = lotNumber;
            }
        }

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
            address.Street = "New Street";
        }

        static void ChangeAddressNewObject(Address address)
        {
            // Setting the parameter equal to a NEW address object
            address = new Address("456", "Broadway", "Townsville", "54321", "New State", "New Country");
        }
    }
}