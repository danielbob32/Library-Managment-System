using System;
using System.Collections.Generic;
using System.Linq;

namespace test_gal_guy_arik
{
    public class LibrarySystem
    {
        public List<Book> Books { get; } = new List<Book>();
        public List<User> Users { get; } = new List<User>();
        public List<Loan> Loans { get; } = new List<Loan>();

        public void AddBook(Book book)
        {
            if (Books.Any(b => b.Serial == book.Serial))
            {
                throw new Exception("A book with this ISBN already exists.");
            }
            Books.Add(book);
        }

        public void AddUser(User user)
        {
            if (Users.Any(u => u.UserId == user.UserId))
            {
                throw new Exception("A user with this ID already exists.");
            }
            Users.Add(user);
        }

        public void LoanBook(string isbn, string userId)
        {
            var book = Books.FirstOrDefault(b => b.Serial == isbn);
            var user = Users.FirstOrDefault(u => u.UserId == userId);

            if (book == null)
            {
                throw new Exception("Book not found.");
            }
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            if (!book.IsAvailable)
            {
                throw new Exception("Book is not available for loan.");
            }

            book.IsAvailable = false;
            Loans.Add(new Loan(book, user));
        }

        public void ReturnBook(string isbn)
        {
            var loan = Loans.FirstOrDefault(l => l.Book.Serial == isbn && !l.ReturnDate.HasValue);
            if (loan == null)
            {
                throw new Exception("No active loan found for this book.");
            }
            loan.ReturnDate = DateTime.Now;
            loan.Book.IsAvailable = true;
        }
    }
}