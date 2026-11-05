/**
*--------------------------------------------------------------------
*File name: Project 1
* Project name: Interactive Quiz Application
* Solution name: Project 1
* --------------------------------------------------------------------
*Author’s name and email: Daniel [REDACTED EMAIL]
* Course-Section: CSCI 1250
* Creation Date: OCt. 18, 2023
* Modified Date: OCT. 18, 2023
* -------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Programming Quiz!");
        Console.WriteLine("You will be presented with 10 multiple-choice questions.");

        // Define questions and answer choices
        List<Question> questions = new List<Question>
        {
            new Question("What is the capital of France?", new string[] { "A) Berlin", "B) London", "C) Paris", "D) Madrid" }, "C"),
            new Question("Which programming language is used for web development?", new string[] { "A) Java", "B) Python", "C) HTML", "D) C#" }, "C"),
            new Question("How many blue stripes are there on the U.S. flag?", new string[] { "A) 6", "B) 7", "C) 13", "D) 0" }, "D"),
            new Question("Which one of these characters is not friends with Harry Potter?", new string[] { "A) Ron Weasley", "B) Neville Longbottom", "C) Draco Malfoy", "D) ME" }, "D"),
            new Question("What is the largest planet in our solar system?", new string[] { "A) Earth", "B) Venus", "C) Mars", "D) Jupiter" }, "D"),
            new Question("Which gas do plants absorb from the atmosphere?", new string[] { "A) Oxygen", "B) Nitrogen", "C) Carbon Dioxide", "D) Hydrogen" }, "C"),
            new Question("What is the boiling point of water in Fahrenheit?", new string[] { "A) 212°F", "B) 100°F", "C) 32°F", "D) 0°F" }, "A"),
            new Question("Who wrote the play 'Romeo and Juliet'?", new string[] { "A) William Shakespeare", "B) Charles Dickens", "C) Jane Austen", "D) Leo Tolstoy" }, "A"),
            new Question("Which planet is known as the Red Planet?", new string[] { "A) Mars", "B) Venus", "C) Jupiter", "D) Saturn" }, "A"),
            new Question("How many continents are there on Earth?", new string[] { "A) 4", "B) 5", "C) 6", "D) 7" }, "B"),

        };

        int score = 0;

        for (int i = 0; i < questions.Count; i++)
        {
            Console.Clear(); // Clear the console for a new question
            DisplayQuestion(questions[i]);
            string userAnswer = GetAnswerFromUser();

            if (userAnswer.ToUpper() == questions[i].CorrectAnswer)
            {
                Console.WriteLine("Correct!");
                score++;
            }
            else
            {
                Console.WriteLine($"Incorrect. The correct answer is: {questions[i].CorrectAnswer}");
            }

            Console.WriteLine($"Your current score: {score}/{i + 1}");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        Console.Clear();
        Console.WriteLine("Quiz Completed!");
        Console.WriteLine($"Your final score: {score}/{questions.Count}");

        // Offer the option to restart or exit
        Console.Write("Do you want to restart the quiz? (Y/N): ");
        string restartOption = Console.ReadLine();
        if (restartOption.ToUpper() == "Y")
        {
            Main(args); // Restart the quiz
        }
        else
        {
            Console.WriteLine("Thank you for playing. Goodbye!");
        }
    }

    static void DisplayQuestion(Question question)
    {
        Console.WriteLine(question.QuestionText);
        foreach (var choice in question.Choices)
        {
            Console.WriteLine(choice);
        }
    }

    static string GetAnswerFromUser()
    {
        Console.Write("Enter your answer (A/B/C/D): ");
        return Console.ReadLine().ToUpper();
    }
}

class Question
{
    public string QuestionText { get; }
    public string[] Choices { get; }
    public string CorrectAnswer { get; }

    public Question(string questionText, string[] choices, string correctAnswer)
    {
        QuestionText = questionText;
        Choices = choices;
        CorrectAnswer = correctAnswer.ToUpper();
    }
}
