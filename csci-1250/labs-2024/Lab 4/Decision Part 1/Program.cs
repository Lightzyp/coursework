
using System.ComponentModel;
using static System.Formats.Asn1.AsnWriter;
/**
*--------------------------------------------------------------------
* File name: Lab 3
* Project name:Decision Part 1
*--------------------------------------------------------------------
* Author’s name and email:[REDACTED EMAIL]
* Course-Section:CSCI-1250-001
* Creation Date:Feb 16 2024
* -------------------------------------------------------------------
* This program calculates the final grade based on the average lab
* grade, average quiz grade, midterm grade, and final exam grade
* provided by the user, along with the corresponding weights.
* -------------------------------------------------------------------
*/
namespace Decision_Part_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Declare variables to store grades and weights
            int AverageLabGrade, AverageQuizGrade, MidtermGrade, FinalExamGrade,
                FinalGrade;

            double Labgradeweight, Quizgradeweight, midtermgradeweight, finalexamgradeweight;

            Console.WriteLine("Please enter all of the grades Lab, Quiz, Midterm, FinalExam (seperate with a comma)");

            string[] gradeInputs = Console.ReadLine().Split(',');

            // Convert string inputs to integers and store in respective variables
            AverageLabGrade = Convert.ToInt32(gradeInputs[0]);
            AverageQuizGrade = Convert.ToInt32(gradeInputs[1]);
            MidtermGrade = Convert.ToInt32(gradeInputs[2]);
            FinalExamGrade = Convert.ToInt32(gradeInputs[3]);

            Console.WriteLine("Enter the weights for Lab Grade, Quiz Grade, Midterm Grade, and Final Exam Grade separated by commas (as decimal values between 0 and 1):");
            string[] weightInputs = Console.ReadLine().Split(',');

            // Convert strings to doubles and store in respective variables
            Labgradeweight = Convert.ToDouble(weightInputs[0]);
            Quizgradeweight = Convert.ToDouble(weightInputs[1]);
            midtermgradeweight = Convert.ToDouble(weightInputs[2]);
            finalexamgradeweight = Convert.ToDouble(weightInputs[3]);

            double totalWeight = Labgradeweight + Quizgradeweight + midtermgradeweight + finalexamgradeweight;
            if (Math.Abs(totalWeight - 1.0) > 0.001)
            {
                Console.WriteLine($"Warning: The sum of weights is not equal to 1. Total weight: {totalWeight}");
            }

            // Calculate the final grade
            FinalGrade = (int)((AverageLabGrade * Labgradeweight) +
                               (AverageQuizGrade * Quizgradeweight) +
                               (MidtermGrade * midtermgradeweight) +
                               (FinalExamGrade * finalexamgradeweight));

            // Display the final grade
            Console.WriteLine($"Final Grade: {FinalGrade}");

            // Determine and display the letter grade
            if (FinalGrade >= 90)
            {
                Console.WriteLine("Letter Grade: A");
            }
            else if (FinalGrade >= 80 || FinalGrade < 90)
            {
                Console.WriteLine("Letter Grade: B");
            }
            else if (FinalGrade >= 70 || FinalGrade < 80)
            {
                Console.WriteLine("Letter Grade: C");
            }
            else if (FinalGrade >= 60 || FinalGrade < 70)
            {
                Console.WriteLine("Letter Grade: D");
            }
            else
            {
                Console.WriteLine("Letter Grade: F");
            }
        }
    }
}