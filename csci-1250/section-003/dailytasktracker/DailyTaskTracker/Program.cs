using System;
using System.Collections.Generic;
using System.IO;

namespace DailyTaskTracker
{
    internal class Program
    {
        static List<TaskItem> tasks = new List<TaskItem>();
        static string filePath = "tasks.txt";

        static void Main(string[] args)
        {
            LoadTasks();

            bool running = true;

            while (running)
            {
                Console.WriteLine("\n=== Daily Task Tracker ===");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. View Tasks");
                Console.WriteLine("3. Mark Task as Complete");
                Console.WriteLine("4. Save and Exit");
                Console.Write("Choose an option (1-4): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        ViewTasks();
                        break;
                    case "3":
                        MarkTaskComplete();
                        break;
                    case "4":
                        SaveTasks();
                        running = false;
                        Console.WriteLine("Tasks saved. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void AddTask()
        {
            Console.Write("Enter task description: ");
            string desc = Console.ReadLine();
            TaskItem task = new TaskItem(desc);
            tasks.Add(task);
            Console.WriteLine("Task added!");
        }

        static void ViewTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
            }
            else
            {
                Console.WriteLine("\nYour Tasks:");
                for (int i = 0; i < tasks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {tasks[i]}");
                }
            }
        }

        static void MarkTaskComplete()
        {
            ViewTasks();
            if (tasks.Count == 0) return;

            Console.Write("Enter task number to mark as complete: ");
            if (int.TryParse(Console.ReadLine(), out int num) && num >= 1 && num <= tasks.Count)
            {
                tasks[num - 1].IsCompleted = true;
                Console.WriteLine("Task marked as complete!");
            }
            else
            {
                Console.WriteLine("Invalid number.");
            }
        }

        static void SaveTasks()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (TaskItem task in tasks)
                {
                    writer.WriteLine($"{task.IsCompleted}|{task.Description}");
                }
            }
        }

        static void LoadTasks()
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        TaskItem task = new TaskItem(parts[1]);
                        task.IsCompleted = bool.Parse(parts[0]);
                        tasks.Add(task);
                    }
                }
            }
        }
    }

    public class TaskItem
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public TaskItem(string description)
        {
            Description = description;
            IsCompleted = false;
        }

        public override string ToString()
        {
            return (IsCompleted ? "[✓] " : "[ ] ") + Description;
        }
    }
}
