using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Library_Lab
{
    internal class Library
    {
        private List<Book> books;

        public Library()
        {
            books = new List<Book>();
        }

        public void AddBook()
        {
            Console.Write("Enter book title: ");
            string title = Console.ReadLine();

            Console.Write("Enter author: ");
            string author = Console.ReadLine();

            Console.Write("Enter publication year: ");
            bool isValidYear = int.TryParse(Console.ReadLine(), out int year);

            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(author) && isValidYear)
            {
                Book newBook = new Book(title, author, year);
                books.Add(newBook);
                Console.WriteLine("Book added successfully!");
            }
            else
            {
                Console.WriteLine("Invalid book details. Book not added.");
            }
        }

        public Book GetBookByTitle(string title)
        {
            foreach (Book book in books)
            {
                if (book != null && book.GetTitle() != null && book.GetTitle().Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    return book;
                }
            }
            return null; 
        }

        public void UpdateBook(string oldTitle)
        {
            Book book = GetBookByTitle(oldTitle);
            if (book != null)
            {
                Console.WriteLine("Enter new title:");
                string newTitle = Console.ReadLine();

                Console.WriteLine("Enter new author:");
                string newAuthor = Console.ReadLine();

                Console.WriteLine("Enter new publication year:");
                bool isValidYear = int.TryParse(Console.ReadLine(), out int newYear);

                if (!string.IsNullOrWhiteSpace(newTitle) && !string.IsNullOrWhiteSpace(newAuthor) && isValidYear)
                {
                    books.Remove(book);
                    Book updatedBook = new Book(newTitle, newAuthor, newYear);
                    books.Add(updatedBook);
                    Console.WriteLine("Book updated successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid details. Book not updated.");
                }
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }

        public void RemoveBook(string title)
        {
            Book book = GetBookByTitle(title);
            if (book != null)
            {
                books.Remove(book);
                Console.WriteLine($"Book '{title}' removed.");
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }

        public List<Book> GetAllBooks()
        {
            return books;
        }

        public void PrintAllBooks()
        {
            Console.WriteLine("Library Inventory:");
            foreach (Book book in books)
            {
                if (book != null)
                {
                    Console.WriteLine($"Title: {book.GetTitle()}, Author: {book.GetAuthor()}, Year: {book.GetPublicationYear()}, Checked Out: {book.IsCheckedOut()}");
                }
            }
        }

        public void PrintAllTitles()
        {
            Console.WriteLine("Book Titles:");
            foreach (Book book in books)
            {
                if (book != null)
                {
                    if (book.IsCheckedOut())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(book.GetTitle() + " (Checked Out)");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(book.GetTitle());
                    }
                }
            }
        }
    }
}
