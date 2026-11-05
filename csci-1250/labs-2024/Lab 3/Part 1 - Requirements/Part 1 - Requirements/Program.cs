namespace Part_1___Requirements
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string bestfriendname, fullname;

            Console.WriteLine("What is your best friend's name?");
            bestfriendname = Console.ReadLine();

            Console.WriteLine("what is your best friend's name starts with:" + bestfriendname[0]);

            Console.WriteLine("what is your first and last name");
            fullname = Console.ReadLine();

            string[] names = fullname.Split(' ');
            string firstname = names[0];
            string lastname = names[1];

            Console.WriteLine("Your first name is:"+ firstname);
            Console.WriteLine("your last name is:" + lastname);
        }
    }
}