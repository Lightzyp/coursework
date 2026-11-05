namespace Review_Day_Lab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a number of seconds:");
            int totalSeconds = int.Parse(Console.ReadLine());


            int hours = totalSeconds / 3600;
            int remaningSeconds = totalSeconds % 3600;
            int minutes = totalSeconds / 60;
            int seconds = remaningSeconds % 60;

            Console.WriteLine($"Time: {hours} hours, {minutes} minutes, {seconds} seconds");

            Console.Write("Enter coffee size (Small, Medium, Large): ");
            string coffeeSize = Console.ReadLine().ToLower();

            Console.Write("Enter extras (Milk, Sugar, Creamer) or 'Nothing': ");
            string extras = Console.ReadLine().ToLower();

            double coffeePrice = (coffeeSize == "small") ? 2.00 :
                                 (coffeeSize == "medium") ? 3.49 :
                                 (coffeeSize == "large") ? 5.25 : 2.00;

            int extraCount = (extras == "nothing") ? 0 : extras.Split(",").Length;
            double extraPrice = extraCount * 0.50;
            double subtotal = coffeePrice + extraPrice;
            double tax = subtotal * 0.095;
            double total = subtotal + tax;


            Console.WriteLine($"\nYour order: {coffeeSize} coffee with {extras}");
            Console.WriteLine($"Subtotal: ${subtotal:F2}");
            Console.WriteLine($"Tax (9.5%): ${tax:F2}");
            Console.WriteLine($"Total: ${total:F2}");


            Console.WriteLine("Would you like to ask the Magic 8-Ball a yes/no question? (yes/no)");
            string responce = Console.ReadLine().ToLower();

            if (responce == "no")
            {
                Console.WriteLine("Have a great day!");
                return;
            }

            Console.WriteLine("Ask your yes/no question");
            Console.ReadLine();


            string[] responses =
            {

                "Yes, definitely!",
                "Ask again later.",
                "No way!",
                "It is uncertain.",
                "Absolutely!",
                "I wouldn't count on it.",
                "Signs point to yes.",
                "Better not tell you now"
                };

            Random rand = new Random();

            int index = rand.Next(responses.Length);
            string magicResponse = responses[index];
            Console.WriteLine($"Magic 8-Ball says: {magicResponse}");


            Console.Write("Enter a number: ");
            int number;
            while (!int.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("Invalid input. Please enter a valid number: ");
            }


            if (number < 0)
            {
                number = Math.Abs(number);
                Console.WriteLine($"Converted to positive: {number}");
            }


            if (IsPrime(number))
            {
                Console.WriteLine($"{number} is a prime number.");
            }
            else
            {
                Console.WriteLine($"{number} is NOT a prime number.");
            }



            static bool IsPrime(int num)
            {
                if (num < 2) return false;


                for (int i = 2; i <= Math.Sqrt(num); i++)
                {
                    if (num % i == 0) return false;
                }
                return true;
            }
        }
    }
}



    

