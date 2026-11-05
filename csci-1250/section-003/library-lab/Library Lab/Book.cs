using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Lab
{
    internal class Book
    {
        private string title;
        private string author;
        private int publicationYear;
        private bool checkedOut;

        public Book(string title, string author, int year)
        {
            this.title = title;
            this.author = author;
            this.publicationYear = year;
            this.checkedOut = false;
        }

        public string GetTitle()
        {
            return title;
        }

        public string GetAuthor()
        {
            return author;
        }

        public int GetPublicationYear()
        {
            return publicationYear;
        }

        public bool IsCheckedOut()
        {
            return checkedOut;
        }

        public void CheckOut()
        {
            checkedOut = true;
        }

        public void CheckIn()
        {
            checkedOut = false;
        }
    }
}
