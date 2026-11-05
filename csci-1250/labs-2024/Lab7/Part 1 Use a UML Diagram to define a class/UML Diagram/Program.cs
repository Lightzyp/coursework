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
namespace UML_Diagram
{
    internal class Program
    {
        public class Address
        {
            // Fields to store address information
            private string lotNumber;
            private string street;
            private string city;
            private string zipCode;
            private string state;
            private string country;

            // Constructor to initialize address fields
            public Address(string lotNumber, string street, string city, string zipCode, string state, string country)
            {
                // Initializing fields with provided values
                this.lotNumber = lotNumber;
                this.street = street;
                this.city = city;
                this.zipCode = zipCode;
                this.state = state;
                this.country = country;
            }

            // Method to get the full address
            public string GetAddress()
            {
                // Concatenating address components into a single string
                return $"Lot Number: {lotNumber}, Street: {street}, City: {city}, State: {state}, Zip Code: {zipCode}, Country: {country}";
            }

            // Methods to get address components individually
            public string GetLotNumber()
            {
                return lotNumber;
            }

            public string GetStreet()
            {
                return street;
            }

            public string GetCity()
            {
                return city;
            }

            public string GetZipCode()
            {
                return zipCode;
            }

            public string GetState()
            {
                return state;
            }

            public string GetCountry()
            {
                return country;
            }

            // Methods to set address components
            public void SetLotNumber(string lotNumber)
            {
                // Setting the lot number field with a new value
                this.lotNumber = lotNumber;
            }

            public void SetStreet(string street)
            {
                // Setting the street field with a new value
                this.street = street;
            }

            public void SetCity(string city)
            {
                // Setting the city field with a new value
                this.city = city;
            }

            public void SetZipCode(string zipCode)
            {
                // Setting the zip code field with a new value
                this.zipCode = zipCode;
            }

            public void SetState(string state)
            {
                // Setting the state field with a new value
                this.state = state;
            }

            public void SetCountry(string country)
            {
                // Setting the country field with a new value
                this.country = country;
            }
        }
    }
}