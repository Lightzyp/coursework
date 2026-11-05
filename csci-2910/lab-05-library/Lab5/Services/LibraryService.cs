using Lab5.Models;

namespace Lab5.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly string booksFilePath;
        private readonly string usersFilePath;

        private List<Book> books = new();
        private List<User> users = new();
        private Dictionary<int, List<int>> borrowedBooks = new();

        public LibraryService(IWebHostEnvironment env)
        {
            string dataFolder = Path.Combine(env.ContentRootPath, "Data");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            booksFilePath = Path.Combine(dataFolder, "Books.csv");
            usersFilePath = Path.Combine(dataFolder, "Users.csv");

            if (!File.Exists(booksFilePath))
            {
                File.WriteAllText(booksFilePath, "Id,Title,Author,ISBN\n");
            }

            if (!File.Exists(usersFilePath))
            {
                File.WriteAllText(usersFilePath, "Id,Name,Email\n");
            }

            ReadBooks();
            ReadUsers();
        }

        public List<Book> GetBooks() => books;
        public List<User> GetUsers() => users;

        public void ReadBooks()
        {
            books = new List<Book>();

            var lines = File.ReadAllLines(booksFilePath).Skip(1);

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length >= 4)
                {
                    books.Add(new Book
                    {
                        Id = int.Parse(parts[0]),
                        Title = parts[1],
                        Author = parts[2],
                        ISBN = parts[3]
                    });
                }
            }
        }

        public void ReadUsers()
        {
            users = new List<User>();

            var lines = File.ReadAllLines(usersFilePath).Skip(1);

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length >= 3)
                {
                    users.Add(new User
                    {
                        Id = int.Parse(parts[0]),
                        Name = parts[1],
                        Email = parts[2]
                    });
                }
            }
        }

        public void AddBook(Book book)
        {
            book.Id = books.Count == 0 ? 1 : books.Max(b => b.Id) + 1;
            books.Add(book);
            WriteBooks();
        }

        public void EditBook(Book updatedBook)
        {
            var existing = books.FirstOrDefault(b => b.Id == updatedBook.Id);
            if (existing != null)
            {
                existing.Title = updatedBook.Title;
                existing.Author = updatedBook.Author;
                existing.ISBN = updatedBook.ISBN;
                WriteBooks();
            }
        }

        public void DeleteBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);

                foreach (var entry in borrowedBooks)
                {
                    entry.Value.Remove(id);
                }

                WriteBooks();
            }
        }

        public void AddUser(User user)
        {
            user.Id = users.Count == 0 ? 1 : users.Max(u => u.Id) + 1;
            users.Add(user);
            WriteUsers();
        }

        public void EditUser(User updatedUser)
        {
            var existing = users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (existing != null)
            {
                existing.Name = updatedUser.Name;
                existing.Email = updatedUser.Email;
                WriteUsers();
            }
        }

        public void DeleteUser(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                users.Remove(user);

                if (borrowedBooks.ContainsKey(id))
                {
                    borrowedBooks.Remove(id);
                }

                WriteUsers();
            }
        }

        public bool BorrowBook(int userId, int bookId)
        {
            if (IsBookBorrowed(bookId))
            {
                return false;
            }

            if (!borrowedBooks.ContainsKey(userId))
            {
                borrowedBooks[userId] = new List<int>();
            }

            borrowedBooks[userId].Add(bookId);
            return true;
        }

        public bool ReturnBook(int userId, int bookId)
        {
            if (borrowedBooks.ContainsKey(userId) && borrowedBooks[userId].Contains(bookId))
            {
                borrowedBooks[userId].Remove(bookId);
                return true;
            }

            return false;
        }

        public List<Book> GetBorrowedBooksByUser(int userId)
        {
            if (!borrowedBooks.ContainsKey(userId))
            {
                return new List<Book>();
            }

            var borrowedIds = borrowedBooks[userId];
            return books.Where(b => borrowedIds.Contains(b.Id)).ToList();
        }

        public bool IsBookBorrowed(int bookId)
        {
            return borrowedBooks.Values.Any(list => list.Contains(bookId));
        }

        private void WriteBooks()
        {
            var lines = new List<string> { "Id,Title,Author,ISBN" };

            foreach (var book in books)
            {
                lines.Add($"{book.Id},{book.Title},{book.Author},{book.ISBN}");
            }

            File.WriteAllLines(booksFilePath, lines);
        }

        private void WriteUsers()
        {
            var lines = new List<string> { "Id,Name,Email" };

            foreach (var user in users)
            {
                lines.Add($"{user.Id},{user.Name},{user.Email}");
            }

            File.WriteAllLines(usersFilePath, lines);
        }
    }
}