using System;
using System.Collections.Generic;

class SemesterDriver
{
    static void Main()
    {
        // Create a List collection to store Course objects
        List<Course> courses = new List<Course>();

        // Prompt the user to enter student name and semester
        Console.Write("Enter student name: ");
        string studentName = Console.ReadLine();
        Console.Write("Enter semester: ");
        string semester = Console.ReadLine();

        // Initialize a flag to add more courses and start a loop
        bool addMoreCourses = true;
        while (addMoreCourses)
        {
            // Call the AddCourse method to input course information
            Course newCourse = AddCourse();

            // Add the newly created Course object to the List of courses
            courses.Add(newCourse);

            // Prompt the user to add another course
            Console.Write("Add another course? (Y/N): ");
            addMoreCourses = (Console.ReadLine().ToUpper() == "Y");
        }

        // Display student and course information
        Console.WriteLine($"\nStudent: {studentName}");
        Console.WriteLine($"Semester: {semester}");
        Console.WriteLine("Course Information:");
        foreach (Course course in courses)
        {
            // Output each course's information using the ToString method
            Console.WriteLine(course);
        }
    }

    static Course AddCourse()
    {
        // Prompt the user to enter course department, number, and section
        Console.Write("Enter Department: ");
        string department = Console.ReadLine();
        Console.Write("Enter Course Number: ");
        int courseNum = int.Parse(Console.ReadLine());
        Console.Write("Enter Section: ");
        int section = int.Parse(Console.ReadLine());

        // Create a new Course object with the provided department, course number, and section
        Course course = new Course(department, courseNum, section);

        // Prompt the user to enter 5 grades for the course
        for (int i = 1; i <= 5; i++)
        {
            Console.Write($"Enter Grade {i}: ");
            int grade = int.Parse(Console.ReadLine());

            // Add each grade to the Course object using the AddGrade method
            course.AddGrade(grade);
        }

        // Return the Course object with its attributes and grades
        return course;
    }
}
