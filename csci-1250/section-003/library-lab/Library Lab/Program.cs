namespace Library_Lab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Library myLibrary = new Library();
            bool exit = false;

            Console.WriteLine("Welcome to the Library Management System!");

            while (!exit)
            {
                Console.WriteLine("!!MAIN MENU!!");
                Console.WriteLine("1. Add a Book");
                Console.WriteLine("2. Remove a Book");
                Console.WriteLine("3. Update a Book");
                Console.WriteLine("4. View All Books");
                Console.WriteLine("5. View Book Titles");
                Console.WriteLine("6. Find a Book by Title");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option (1-7): ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    myLibrary.AddBook();
                }
                else if (choice == "2")
                {
                    Console.Write("Enter the title of the book to remove: ");
                    string removeTitle = Console.ReadLine();
                    myLibrary.RemoveBook(removeTitle);
                }
                else if (choice == "3")
                {
                    Console.Write("Enter the title of the book to update: ");
                    string updateTitle = Console.ReadLine();
                    myLibrary.UpdateBook(updateTitle);
                }
                else if (choice == "4")
                {
                    myLibrary.PrintAllBooks();
                }
                else if (choice == "5")
                {
                    myLibrary.PrintAllTitles();
                }
                else if (choice == "6")
                {
                    Console.Write("Enter the title of the book to find: ");
                    string findTitle = Console.ReadLine();
                    Book foundBook = myLibrary.GetBookByTitle(findTitle);

                    if (foundBook != null)
                    {
                        Console.WriteLine($"Title: {foundBook.GetTitle()}, Author: {foundBook.GetAuthor()}, Year: {foundBook.GetPublicationYear()}, Checked Out: {foundBook.IsCheckedOut()}");
                    }
                    else
                    {
                        Console.WriteLine("Book not found.");
                    }
                }
                else if (choice == "7")
                {
                    exit = true;
                    Console.WriteLine("👋 Exiting the program. Goodbye!");
                }
                else
                {
                    Console.WriteLine("⚠️ Invalid choice. Please enter a number from 1 to 7.");
                }

            }
        }
    }
}
