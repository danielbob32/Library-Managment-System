using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using test_gal_guy_arik;

namespace EnhancedLibrarySystem
{
    public partial class LoanBookForm : Form
    {
        private LibrarySystem _librarySystem;
        private LibraryManagementForm _mainForm;
        private ComboBox isbnComboBox, userIdComboBox;

        public LoanBookForm(LibrarySystem librarySystem, LibraryManagementForm mainForm)
        {
            _librarySystem = librarySystem;
            _mainForm = mainForm;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            FormStyler.StyleForm(this, "Loan Book");

            var mainLayout = FormStyler.CreateMainLayout();
            var topPanel = FormStyler.CreateTopPanel(4, Color.FromArgb(52, 73, 94));

            var loanBookButton = FormStyler.CreateStyledButton("Loan Book", Color.FromArgb(155, 89, 182), null);
            var addUserButton = FormStyler.CreateStyledButton("Add User", Color.FromArgb(52, 152, 219), (s, e) => NavigateToForm(new AddUserForm(_librarySystem, _mainForm)));
            var addBookButton = FormStyler.CreateStyledButton("Add Book", Color.FromArgb(46, 204, 113), (s, e) => NavigateToForm(new AddBookForm(_librarySystem, _mainForm)));
            var returnBookButton = FormStyler.CreateStyledButton("Return Book", Color.FromArgb(231, 76, 60), (s, e) => NavigateToForm(new ReturnBookForm(_librarySystem, _mainForm)));

            loanBookButton.Enabled = false;

            topPanel.Controls.Add(loanBookButton, 0, 0);
            topPanel.Controls.Add(addUserButton, 1, 0);
            topPanel.Controls.Add(addBookButton, 2, 0);
            topPanel.Controls.Add(returnBookButton, 3, 0);

            var inputPanel = CreateInputPanel();

            mainLayout.Controls.Add(topPanel, 0, 0);
            mainLayout.Controls.Add(inputPanel, 0, 1);

            this.Controls.Add(mainLayout);
        }

        private void NavigateToForm(Form form)
        {
            this.Hide();
            form.ShowDialog();
            this.Close();
        }

        private TableLayoutPanel CreateInputPanel()
        {
            var inputPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(20),
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 30F), new ColumnStyle(SizeType.Percent, 70F) }
            };

            inputPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            inputPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            inputPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

            inputPanel.Controls.Add(new Label { Text = "ISBN:", Dock = DockStyle.Right, TextAlign = ContentAlignment.MiddleRight, Margin = new Padding(0, 10, 10, 10) }, 0, 0);
            isbnComboBox = new ComboBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(10, 10, 10, 10) };
            PopulateISBNComboBox();
            inputPanel.Controls.Add(isbnComboBox, 1, 0);

            inputPanel.Controls.Add(new Label { Text = "User ID:", Dock = DockStyle.Right, TextAlign = ContentAlignment.MiddleRight, Margin = new Padding(0, 10, 10, 10) }, 0, 1);
            userIdComboBox = new ComboBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(10, 10, 10, 10) };
            PopulateUserIdComboBox();
            inputPanel.Controls.Add(userIdComboBox, 1, 1);

            var buttonPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 50F), new ColumnStyle(SizeType.Percent, 50F) },
                Margin = new Padding(10, 10, 10, 10)
            };

            var loanButton = FormStyler.CreateStyledButton("Loan", Color.FromArgb(155, 89, 182), LoanBookButton_Click);
            var backButton = FormStyler.CreateStyledButton("Back", Color.FromArgb(231, 76, 60), (s, e) => { this.Close(); _mainForm.Show(); });

            buttonPanel.Controls.Add(loanButton, 0, 0);
            buttonPanel.Controls.Add(backButton, 1, 0);

            inputPanel.Controls.Add(buttonPanel, 1, 2);

            return inputPanel;
        }

        // addes the serials and the users to the combobox
        private void PopulateISBNComboBox()
        {
            var availableBooks = _librarySystem.Books.Where(b => b.IsAvailable).Select(b => b.Serial).ToList();
            isbnComboBox.Items.AddRange(availableBooks.ToArray());
        }

        private void PopulateUserIdComboBox()
        {
            var userIds = _librarySystem.Users.Select(u => u.UserId).ToList();
            userIdComboBox.Items.AddRange(userIds.ToArray());
        }

        private void LoanBookButton_Click(object sender, EventArgs e)
        {
            if (ValidateLoanBookInput())
            {
                var isbn = isbnComboBox.SelectedItem.ToString();
                var userId = userIdComboBox.SelectedItem.ToString();

                _librarySystem.LoanBook(isbn, userId);

                MessageBox.Show("Book loaned successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                _mainForm.Show();
                _mainForm.RefreshGrids();
            }
        }

        private bool ValidateLoanBookInput()
        {
            if (isbnComboBox.SelectedItem == null || userIdComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select both an ISBN and a User ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
