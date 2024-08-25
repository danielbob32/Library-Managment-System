using System;

namespace test_gal_guy_arik
{
    public class Loan
    {
        public const int LoanPeriodDays = 14;
        public Book Book { get; set; }
        public User User { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public Loan(Book book, User user)
        {
            Book = book;
            User = user;
            LoanDate = DateTime.Now;
        }

        public bool IsOverdue => DateTime.Now > LoanDate.AddDays(LoanPeriodDays) && !ReturnDate.HasValue;
    }
}