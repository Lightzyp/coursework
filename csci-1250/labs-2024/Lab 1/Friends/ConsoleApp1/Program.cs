namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Variables
            string name, freindName;
            int age, friendage;
          
            Console.WriteLine("What is your name?");
            name = Console.ReadLine();

            Console.WriteLine("What is your age?");
            age = Convert.ToInt32 (Console.ReadLine());

            Console.WriteLine($"Your {name} and you are {age} years old.");

            Console.WriteLine("What is your freinds name?");
            freindName =Console.ReadLine();

            Console.WriteLine("What is your freinds age?");
            friendage = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"Your freind's name is  {freindName} and is  {friendage} years old.");

            int sum = age + friendage;
            Console.WriteLine("The sum of the two ages is: " + sum);
            
            age = Math.Abs(age - friendage);
            Console.WriteLine($"There is a {age} year diffrence between the two of you" );
        }
    }
}