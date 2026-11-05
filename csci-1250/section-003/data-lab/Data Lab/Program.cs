namespace Data_Lab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] numbers1 = { 1, 2, 3 };
            
            Console.Write("How many numbers would you like to enter? ");
            int count = int.Parse(Console.ReadLine());
            int[] userNums = new int[count];
            for (int i = 0; i < count; i++)
            {
                Console.Write($"Enter number {i + 1}: ");
                userNums[i] = int.Parse(Console.ReadLine());
            }
            Console.WriteLine($"Average: {GetAverage(userNums)}");


            Console.WriteLine("enter numbers separated by commas");
            int[] commaNums = GetIntArrayFromCommaInput();
            Console.WriteLine($"Average: {GetAverage(commaNums)}");

            
            Console.WriteLine("Reversed array:");
            Array.Reverse(commaNums);
            foreach (int num in commaNums)
                Console.WriteLine(num);

            Console.WriteLine("Min, Max, Median");
            Console.WriteLine("separated by commas");
            int[] sorted = GetIntArrayFromCommaInput();
            Array.Sort(sorted);
            Console.WriteLine($"Min: {sorted[0]}"); 
            Console.WriteLine($"Max: {sorted[sorted.Length - 1]}"); 
            double median = sorted.Length % 2 == 1
                ? sorted[sorted.Length / 2]
                : (sorted[sorted.Length / 2 - 1] + sorted[sorted.Length / 2]) / 2.0;
            Console.WriteLine($"Median: {median}"); 

            
            Console.Write("Enter exactly 5 numbers between 1 and 10 separated by commas:");
            int[] histValues = GetIntArrayFromCommaInput();
            foreach (int val in histValues)
            {
                Console.WriteLine(new string('*', val));
            }

            Console.WriteLine(" FileIO with names.txt");
            string filePath = "names.txt";
            List<string> names = new List<string>();
            if (File.Exists(filePath))
            {
                string[] fileLines = File.ReadAllLines(filePath);
                foreach (var line in fileLines)
                    Console.WriteLine(line);
                names = fileLines.ToList();
            }
            Console.Write("Add a name: ");
            names.Add(Console.ReadLine());

            Console.Write("Enter name to remove");
            string nameToRemove = Console.ReadLine();
            names.Remove(nameToRemove);

            File.WriteAllLines(filePath, names.ToArray());
            Console.WriteLine("File updated!");

            Console.WriteLine("Parsing guitars.csv");
            string guitarPath = "guitars.csv";
            if (File.Exists(guitarPath))
            {
                string[] lines = File.ReadAllLines(guitarPath);
                double totalPrice = 0;
                int lineCount = 0;
                foreach (string line in lines.Skip(1)) 
                {
                    string[] parts = line.Split('\t');
                    if (parts.Length == 3)
                    {
                        string brand = parts[0];
                        double price = double.Parse(parts[2]);
                        Console.WriteLine($"Brand: {brand}, Price: ${price}");
                        totalPrice += price;
                        lineCount++;
                    }
                }
                Console.WriteLine($"Average price of guitars: ${totalPrice / lineCount:F2}");
            }
            else
            {
                Console.WriteLine("guitars.csv not found!");
            }

            Console.WriteLine("All Done!");
        }

        static double GetAverage(int[] array)
        {
            int sum = 0;
            for (int i = 0; i < array.Length; i++)
                sum += array[i];
            return (double)sum / array.Length;
        }

        static int[] GetIntArrayFromCommaInput()
        {
            string input = Console.ReadLine();
            string[] parts = input.Split(',');
            int[] result = new int[parts.Length];
            for (int i = 0; i < parts.Length; i++)
                result[i] = int.Parse(parts[i].Trim());
            return result;
        }
    }
}
    
