using System;
using System.Collections.Generic;

public class Course
{
    // Declare a private List collection to store grades for the course
    private List<int> grades;

    // Define public properties for the course attributes
    public string Department { get; set; }
    public int CourseNum { get; set; }
    public int Section { get; set; }

    // Default constructor
    public Course()
    {
        // Initialize the private grades list as a new empty list
        grades = new List<int>();
    }

    // Fully parameterized constructor
    public Course(string department, int courseNum, int section)
    {
        // Set the course attributes and initialize the grades list as an empty list
        Department = department;
        CourseNum = courseNum;
        Section = section;
        grades = new List<int>();
    }

    // Copy constructor
    public Course(Course otherCourse)
    {
        // Copy the course attributes from another Course object
        Department = otherCourse.Department;
        CourseNum = otherCourse.CourseNum;
        Section = otherCourse.Section;

        // Create a new list of grades and copy the grades from the other Course object
        grades = new List<int>(otherCourse.grades);
    }

    // Method to add a grade to the grades list
    public void AddGrade(int grade)
    {
        // Add the provided grade to the grades list
        grades.Add(grade);
    }

    // Private method to calculate the average grade
    private double CalcAverage()
    {
        // Check if the grades list is empty
        if (grades.Count == 0)
            return 0.0;

        // Calculate the sum of all grades in the list
        int sum = 0;
        foreach (int grade in grades)
        {
            sum += grade;
        }

        // Calculate and return the average as a double
        return (double)sum / grades.Count;
    }

    // Override the ToString method to provide a custom string representation of the Course object
    public override string ToString()
    {
        // Calculate the average grade using the CalcAverage method
        double average = CalcAverage();

        // Create a string that lists all the grades separated by commas
        string gradeList = string.Join(", ", grades);

        // Return a formatted string with course details, grades, and average grade
        return $"Course: {Department} {CourseNum}-{Section}\nGrades: {gradeList}\nAverage: {average:F1}%";
    }
}
