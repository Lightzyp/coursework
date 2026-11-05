using System;

class Pyramid
{
    static void Main()
    {
        Console.Write("Enter the height of the pyramid: ");
        int height = int.Parse(Console.ReadLine());

        for (int i = 1; i <= height; i++)
        {
            // Print spaces before each '*'
            for (int j = 1; j <= height - i; j++)
            {
                Console.Write(" ");
            }

            // Print '*' for the current row
            for (int k = 1; k <= i; k++)
            {
                Console.Write("* ");
            }

            Console.WriteLine(); // Move to the next line
        }
    }
}
