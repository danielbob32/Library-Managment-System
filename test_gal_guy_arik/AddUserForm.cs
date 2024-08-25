using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using test_gal_guy_arik;

namespace EnhancedLibrarySystem
{
    public partial class AddUserForm : Form
    {
        private LibrarySystem _librarySystem;
        private LibraryManagementForm _mainForm;
        private TextBox nameTextBox, userIdTextBox;

        public AddUserForm(LibrarySystem librarySystem, LibraryManagementForm mainForm)
        {
            _librarySystem = librarySystem;
            _mainForm = mainForm;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            FormStyler.StyleForm(this, "Add User");

            var mainLayout = FormStyler.CreateMainLayout();
            var topPanel = FormStyler.CreateTopPanel(4, Color.FromArgb(52, 73, 94));

            var addUserButton = FormStyler.CreateStyledButton("Add User", Color.FromArgb(52, 152, 219), null);
            var addBookButton = FormStyler.CreateStyledButton("Add Book", Color.FromArgb(46, 204, 113), (s, e) => NavigateToForm(new AddBookForm(_librarySystem, _mainForm)));
            var loanBookButton = FormStyler.CreateStyledButton("Loan Book", Color.FromArgb(155, 89, 182), (s, e) => NavigateToForm(new LoanBookForm(_librarySystem, _mainForm)));
            var returnBookButton = FormStyler.CreateStyledButton("Return Book", Color.FromArgb(231, 76, 60), (s, e) => NavigateToForm(new ReturnBookForm(_librarySystem, _mainForm)));

            addUserButton.Enabled = false;

            topPanel.Controls.Add(addUserButton, 0, 0);
            topPanel.Controls.Add(addBookButton, 1, 0);
            topPanel.Controls.Add(loanBookButton, 2, 0);
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

            inputPanel.Controls.Add(new Label { Text = "Name:", Dock = DockStyle.Right, TextAlign = ContentAlignment.MiddleRight, Margin = new Padding(0, 10, 10, 10) }, 0, 0);
            nameTextBox = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F), Margin = new Padding(10, 10, 10, 10) };
            inputPanel.Controls.Add(nameTextBox, 1, 0);

            inputPanel.Controls.Add(new Label { Text = "User ID:", Dock = DockStyle.Right, TextAlign = ContentAlignment.MiddleRight, Margin = new Padding(0, 10, 10, 10) }, 0, 1);
            userIdTextBox = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F), Margin = new Padding(10, 10, 10, 10) };
            inputPanel.Controls.Add(userIdTextBox, 1, 1);

            var buttonPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 50F), new ColumnStyle(SizeType.Percent, 50F) },
                Margin = new Padding(10, 10, 10, 10)
            };

            var addButton = FormStyler.CreateStyledButton("Add", Color.FromArgb(52, 152, 219), AddUserButton_Click);
            var backButton = FormStyler.CreateStyledButton("Back", Color.FromArgb(231, 76, 60), (s, e) => { this.Close(); _mainForm.Show(); });

            buttonPanel.Controls.Add(addButton, 0, 0);
            buttonPanel.Controls.Add(backButton, 1, 0);

            inputPanel.Controls.Add(buttonPanel, 1, 2);

            return inputPanel;
        }

        private void AddUserButton_Click(object sender, EventArgs e)
        {
            if (ValidateAddUserInput())
            {
                var userId = userIdTextBox.Text.Trim();

                if (_librarySystem.Users.Any(u => u.UserId == userId))
                {
                    MessageBox.Show("A user with the same User ID already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _librarySystem.Users.Add(new User(nameTextBox.Text, userId));

                MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                _mainForm.Show();
                _mainForm.RefreshGrids();
            }
        }

        private bool ValidateAddUserInput()
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) || string.IsNullOrWhiteSpace(userIdTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
