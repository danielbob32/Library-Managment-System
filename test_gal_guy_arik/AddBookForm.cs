using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using test_gal_guy_arik;

namespace EnhancedLibrarySystem
{
    public partial class AddBookForm : Form
    {
        private LibrarySystem _librarySystem;
        private LibraryManagementForm _mainForm;
        private TextBox titleTextBox, authorTextBox, isbnTextBox;
        private ComboBox genreComboBox;

        public AddBookForm(LibrarySystem librarySystem, LibraryManagementForm mainForm)
        {
            _librarySystem = librarySystem;
            _mainForm = mainForm;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            FormStyler.StyleForm(this, "Add Book");

            var mainLayout = FormStyler.CreateMainLayout();
            var topPanel = FormStyler.CreateTopPanel(4, Color.FromArgb(52, 73, 94));

            var addBookButton = FormStyler.CreateStyledButton("Add Book", Color.FromArgb(46, 204, 113), null);
            var addUserButton = FormStyler.CreateStyledButton("Add User", Color.FromArgb(52, 152, 219), (s, e) => NavigateToForm(new AddUserForm(_librarySystem, _mainForm)));
            var loanBookButton = FormStyler.CreateStyledButton("Loan Book", Color.FromArgb(155, 89, 182), (s, e) => NavigateToForm(new LoanBookForm(_librarySystem, _mainForm)));
            var returnBookButton = FormStyler.CreateStyledButton("Return Book", Color.FromArgb(231, 76, 60), (s, e) => NavigateToForm(new ReturnBookForm(_librarySystem, _mainForm)));

            addBookButton.Enabled = false;

            topPanel.Controls.Add(addBookButton, 0, 0);
            topPanel.Controls.Add(addUserButton, 1, 0);
            topPanel.Controls.Add(loanBookButton, 2, 0);
            topPanel.Controls.Add(returnBookButton, 3, 0);

            var inputPanel = CreateInputPanel();

            mainLayout.Controls.Add(topPanel, 0, 0);
            mainLayout.Controls.Add(inputPanel, 0, 1);

            this.Controls.Add(mainLayout);
        }

        // navigates between different pages
        private void NavigateToForm(Form form)
        {
            this.Hide();
            form.ShowDialog();
            this.Close();
        }

        // creates the input panel for the user to enter the book details
        // its repeated in the code in various forms
        // this should be refactored to a single method or class
        private TableLayoutPanel CreateInputPanel()
        {
            var inputPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new Padding(20),
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 30F), new ColumnStyle(SizeType.Percent, 70F) }
            };

            for (int i = 0; i < 5; i++)
            {
                inputPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            }

            inputPanel.Controls.Add(new Label { Text = "Title:", Dock = DockStyle.Right, TextAlign = ContentAlignment.MiddleRight, Margin = new Padding(0, 10, 10, 10) }, 0, 0);
            titleTextBox = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F), Margin = new Padding(10, 10, 10, 10) };
            inputPanel.Controls.Add(titleTextBox, 1, 0);

            inputPanel.Controls.Add(new Label { Text = "Author:", Dock = DockStyle.Right, TextAlign = ContentAlignment.MiddleRight, Margin = new Padding(0, 10, 10, 10) }, 0, 1);
            authorTextBox = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F), Margin = new Padding(10, 10, 10, 10) };
            inputPanel.Controls.Add(authorTextBox, 1, 1);

            inputPanel.Controls.Add(new Label { Text = "Serial:", Dock = DockStyle.Right, TextAlign = ContentAlignment.MiddleRight, Margin = new Padding(0, 10, 10, 10) }, 0, 2);
            isbnTextBox = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F), Margin = new Padding(10, 10, 10, 10) };
            inputPanel.Controls.Add(isbnTextBox, 1, 2);

            inputPanel.Controls.Add(new Label { Text = "Genre:", Dock = DockStyle.Right, TextAlign = ContentAlignment.MiddleRight, Margin = new Padding(0, 10, 10, 10) }, 0, 3);
            genreComboBox = new ComboBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(10, 10, 10, 10) };
            genreComboBox.Items.AddRange(Enum.GetNames(typeof(Genre)));
            inputPanel.Controls.Add(genreComboBox, 1, 3);

            var buttonPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 50F), new ColumnStyle(SizeType.Percent, 50F) },
                Margin = new Padding(10, 10, 10, 10)
            };

            var addButton = FormStyler.CreateStyledButton("Add", Color.FromArgb(46, 204, 113), AddBookButton_Click);
            var backButton = FormStyler.CreateStyledButton("Back", Color.FromArgb(231, 76, 60), (s, e) => { this.Close(); _mainForm.Show(); });

            buttonPanel.Controls.Add(addButton, 0, 0);
            buttonPanel.Controls.Add(backButton, 1, 0);

            inputPanel.Controls.Add(buttonPanel, 1, 4);

            return inputPanel;
        }

        private void AddBookButton_Click(object sender, EventArgs e)
        {
            if (ValidateAddBookInput())
            {
                var isbn = isbnTextBox.Text.Trim();

                if (_librarySystem.Books.Any(b => b.Serial == isbn))
                {
                    MessageBox.Show("A book with the same serial number (ISBN) already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var genre = (Genre)Enum.Parse(typeof(Genre), genreComboBox.SelectedItem.ToString());
                _librarySystem.Books.Add(new Book(titleTextBox.Text, authorTextBox.Text, isbn, genre));

                MessageBox.Show("Book added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                _mainForm.Show();
                _mainForm.RefreshGrids();
            }
        }

        private bool ValidateAddBookInput()
        {
            if (string.IsNullOrWhiteSpace(titleTextBox.Text) ||
                string.IsNullOrWhiteSpace(authorTextBox.Text) ||
                string.IsNullOrWhiteSpace(isbnTextBox.Text) ||
                genreComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
