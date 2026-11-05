using System;
using System.Collections.Generic;

public class Farm
{
    private List<Turtle> turtles;

    // Constructor
    public Farm()
    {
        turtles = new List<Turtle>();
    }

    // Method to add a turtle to the list
    public void AddTurtle(Turtle turtle)
    {
        turtles.Add(turtle);
    }
}

public class Turtle
{
    // Turtle class properties and methods go here
}

class Program
{
    static void Main()
    {
        // Create a Farm
        Farm myFarm = new Farm();

        // Create some Turtle objects
        Turtle turtle1 = new Turtle();
        Turtle turtle2 = new Turtle();

        // Add Turtles to the Farm
        myFarm.AddTurtle(turtle1);
        myFarm.AddTurtle(turtle2);

        // Example: Print the number of turtles in the farm
        Console.WriteLine($"Number of turtles in the farm: {myFarm.GetNumberOfTurtles()}");
    }
}
