using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using test_gal_guy_arik;

namespace EnhancedLibrarySystem
{
    public partial class LibraryManagementForm : Form
    {
        private LibrarySystem _librarySystem;
        // tabs (tables) of the main page
        private DataGridView _booksGrid; 
        private DataGridView _usersGrid;
        private DataGridView _loansGrid;

        public LibraryManagementForm()
        {
            _librarySystem = new LibrarySystem();
            LoadTestData(); // load test data
            InitializeComponent(); // init components 
            InitializeUI(); // update UI with the data, check if needed
        }

        // addes all the buttons and the tables to the form
        private void InitializeComponent()
        {
            // use formstyler to style the form consistently for all of the panes
            // page title
            FormStyler.StyleForm(this, "Library Management System");

            // create the tables for the books, users and loans
            _booksGrid = FormStyler.CreateDataGrid();
            _usersGrid = FormStyler.CreateDataGrid();
            _loansGrid = FormStyler.CreateDataGrid();

            // create the main layout
            var mainLayout = FormStyler.CreateMainLayout();

            // create the top panel with the buttons
            var topPanel = FormStyler.CreateTopPanel(4, Color.FromArgb(52, 73, 94));

            // create the buttons, and set color and click event
            var addBookButton = FormStyler.CreateStyledButton("Add Book", Color.FromArgb(46, 204, 113), AddBookButton_Click);
            var addUserButton = FormStyler.CreateStyledButton("Add User", Color.FromArgb(52, 152, 219), AddUserButton_Click);
            var loanBookButton = FormStyler.CreateStyledButton("Loan Book", Color.FromArgb(155, 89, 182), LoanBookButton_Click);
            var returnBookButton = FormStyler.CreateStyledButton("Return Book", Color.FromArgb(231, 76, 60), ReturnBookButton_Click);

            topPanel.Controls.Add(addBookButton, 0, 0);
            topPanel.Controls.Add(addUserButton, 1, 0);
            topPanel.Controls.Add(loanBookButton, 2, 0);
            topPanel.Controls.Add(returnBookButton, 3, 0);

            var tabControl = FormStyler.CreateTabControl();
            FormStyler.AddTabsToTabControl(tabControl,
                ("Books", _booksGrid),
                ("Users", _usersGrid),
                ("Loans", _loansGrid)
            );

            mainLayout.Controls.Add(topPanel, 0, 0);
            mainLayout.Controls.Add(tabControl, 0, 1);

            this.Controls.Add(mainLayout);

            RefreshGrids();
        }

        // click event
        // for all the event: this.Hide() -> open the form -> this.Show() -> refresh the data
        // this.Hide() : hide the current form
        // open the form : open the form that the button is responsible for
        // this.Show() : show the current form
        // refresh the data : refresh the data in the tables
        private void AddBookButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var addBookForm = new AddBookForm(_librarySystem, this))
            {
                addBookForm.ShowDialog();
            }
            this.Show();
            RefreshGrids();
        }

        private void AddUserButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var addUserForm = new AddUserForm(_librarySystem, this))
            {
                addUserForm.ShowDialog();
            }
            this.Show();
            RefreshGrids();
        }

        private void LoanBookButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var loanBookForm = new LoanBookForm(_librarySystem, this))
            {
                loanBookForm.ShowDialog();
            }
            this.Show();
            RefreshGrids();
        }

        private void ReturnBookButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var returnBookForm = new ReturnBookForm(_librarySystem, this))
            {
                returnBookForm.ShowDialog();
            }
            this.Show();
            RefreshGrids();
        }

        // check if necessary to update the UI
        private void InitializeUI()
        {
            var defaultTab = 0;
            var tabControl = this.Controls.OfType<TabControl>().FirstOrDefault();
            if (tabControl != null && tabControl.TabPages.Count > 0)
            {
                tabControl.SelectedIndex = defaultTab;
            }

            if (_booksGrid != null && _booksGrid.Rows.Count > 0)
            {
                _booksGrid.Focus();
            }
        }

        // genereates generative data for testing
        private void LoadTestData()
        {
            _librarySystem.Books.AddRange(new List<Book>
            {
                new Book("The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", Genre.Fiction),
                new Book("A Brief History of Time", "Stephen Hawking", "9780553380163", Genre.Science),
                new Book("Pride and Prejudice", "Jane Austen", "9780141439518", Genre.Romance),
                new TextBook("Introduction to Algorithms", "Thomas H. Cormen", "9780262033848", Genre.Science, "Computer Science"),
                new Book ("The Catcher in the Rye", "J.D. Salinger", "9780316769488", Genre.Fiction),
                new Book ("To Kill a Mockingbird", "Harper Lee", "9780061120084", Genre.Fiction),
                new TextBook ("The Elements of Style", "William Strunk Jr.", "9780205309023", Genre.NonFiction, "Writing")
            });

            _librarySystem.Users.AddRange(new List<User>
            {
                new User("John Doe", "316544444"),
                new User("Jane Smith", "316544442"),
                new User("Bob Johnson", "316544441")
            });
        }

        public void RefreshGrids()
        {
            _booksGrid.DataSource = null;
            _booksGrid.DataSource = _librarySystem.Books.Select(b => new { b.Title, b.Author, b.Serial, b.Genre, b.IsAvailable }).ToList();

            _usersGrid.DataSource = null;
            _usersGrid.DataSource = _librarySystem.Users.Select(u => new { u.Name, u.UserId }).ToList();

            _loansGrid.DataSource = null;
            _loansGrid.DataSource = _librarySystem.Loans.Select(l => new { BookTitle = l.Book.Title, UserName = l.User.Name, l.LoanDate, l.ReturnDate, l.IsOverdue }).ToList();
        }

    }
}
