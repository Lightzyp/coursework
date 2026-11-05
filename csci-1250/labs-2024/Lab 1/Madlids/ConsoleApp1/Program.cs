using System;

namespace ConsoleApp1
{
    internal class Program
    {


        static void Main(string[] args)
        {
            // Declaring a variable to store
            string name;
            string city;
            string country;
            int age;
            string career;
            string animal;
            string pet;

            // Prompting the user to enter their name and storing the input in the 'name' variable
            Console.WriteLine("Enter your name");
            name = Console.ReadLine();
            // Prompting the user to enter their age and storing the input after converting it to an integer in the 'age' variable
            Console.WriteLine("What is your age");
            age = Convert.ToInt32(Console.ReadLine());
            // Prompting the user to enter the name of their city and storing the input in the 'city' variable
            Console.WriteLine("What is the name of your city");
            city = Console.ReadLine();
            // Prompting the user to enter the name of their country and storing the input in the 'country' variable
            Console.WriteLine("What is the name of your country");
            country = Console.ReadLine();
            // Prompting the user to enter their career and storing the input in the 'career' variable
            Console.WriteLine("What is your career");
            career= Console.ReadLine();
            // Prompting the user to enter favorite animal and storing the input in the 'animal' variable
            Console.WriteLine("Tell me your favorite animal");
            animal = Console.ReadLine();
            // Prompting the user to enter their pet's name and storing the input in the 'pet' variable
            Console.WriteLine(" What is your pet's name?");
            pet = Console.ReadLine();
            // Outputting a story using the collected information with strings and int.
            Console.WriteLine($"There once was a person {name} who lived in {city}. At the age of {age}, {name} decied to move to {country} and become a {career}. Then {name} adopted an {animal} name {pet}.They both lived happily ever after!!");

        }
    }
}