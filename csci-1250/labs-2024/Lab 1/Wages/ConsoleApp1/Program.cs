using System;

/// <summary>
/// This program calculates total wages including regular and overtime pay.
/// </summary>
class WagesCalculator
{
    static void Main(string[] args)
    {
        // Variable declarations
        string name;
        decimal regularPay, overtimePay, totalWages;
        double regularHours, overtimeHours;

        // Prompt user for input
        Console.WriteLine("Enter your name:");
        name = Console.ReadLine();

        Console.WriteLine("Enter your regular pay:");
        regularPay = decimal.Parse(Console.ReadLine());

        Console.WriteLine("Enter your overtime pay:");
        overtimePay = decimal.Parse(Console.ReadLine());

        Console.WriteLine("Enter the number of regular hours worked:");
        regularHours = double.Parse(Console.ReadLine());

        Console.WriteLine("Enter the number of overtime hours worked:");
        overtimeHours = double.Parse(Console.ReadLine());

        // Calculate total wages
        totalWages = regularPay * (decimal)regularHours + overtimePay * (decimal)overtimeHours;

        // Output total wages
        Console.WriteLine($"Hello, {name}! Your total wages are ${totalWages}");
    }
}
