using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using test_gal_guy_arik;

namespace EnhancedLibrarySystem
{
    public partial class ReturnBookForm : Form
    {
        private LibrarySystem _librarySystem;
        private LibraryManagementForm _mainForm;
        private ComboBox isbnComboBox;

        public ReturnBookForm(LibrarySystem librarySystem, LibraryManagementForm mainForm)
        {
            _librarySystem = librarySystem;
            _mainForm = mainForm;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            FormStyler.StyleForm(this, "Return Book");

            var mainLayout = FormStyler.CreateMainLayout();
            var topPanel = FormStyler.CreateTopPanel(4, Color.FromArgb(52, 73, 94));

            var returnBookButton = FormStyler.CreateStyledButton("Return Book", Color.FromArgb(231, 76, 60), null);
            var addUserButton = FormStyler.CreateStyledButton("Add User", Color.FromArgb(52, 152, 219), (s, e) => NavigateToForm(new AddUserForm(_librarySystem, _mainForm)));
            var addBookButton = FormStyler.CreateStyledButton("Add Book", Color.FromArgb(46, 204, 113), (s, e) => NavigateToForm(new AddBookForm(_librarySystem, _mainForm)));
            var loanBookButton = FormStyler.CreateStyledButton("Loan Book", Color.FromArgb(155, 89, 182), (s, e) => NavigateToForm(new LoanBookForm(_librarySystem, _mainForm)));

            returnBookButton.Enabled = false;

            topPanel.Controls.Add(returnBookButton, 0, 0);
            topPanel.Controls.Add(addUserButton, 1, 0);
            topPanel.Controls.Add(addBookButton, 2, 0);
            topPanel.Controls.Add(loanBookButton, 3, 0);

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
                RowCount = 2,
                Padding = new Padding(20),
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 30F), new ColumnStyle(SizeType.Percent, 70F) }
            };

            inputPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            inputPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            inputPanel.Controls.Add(new Label { Text = "ISBN:", Dock = DockStyle.Right, TextAlign = ContentAlignment.MiddleRight, Margin = new Padding(0, 10, 10, 10) }, 0, 0);
            isbnComboBox = new ComboBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(10, 10, 10, 10) };
            PopulateISBNComboBox();
            inputPanel.Controls.Add(isbnComboBox, 1, 0);

            var buttonPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 50F), new ColumnStyle(SizeType.Percent, 50F) },
                Margin = new Padding(10, 10, 10, 10)
            };

            var returnButton = FormStyler.CreateStyledButton("Return", Color.FromArgb(231, 76, 60), ReturnBookButton_Click);
            var backButton = FormStyler.CreateStyledButton("Back", Color.FromArgb(231, 76, 60), (s, e) => { this.Close(); _mainForm.Show(); });

            buttonPanel.Controls.Add(returnButton, 0, 0);
            buttonPanel.Controls.Add(backButton, 1, 0);

            inputPanel.Controls.Add(buttonPanel, 1, 1);

            return inputPanel;
        }

        private void PopulateISBNComboBox()
        {
            var loanedBooks = _librarySystem.Loans.Select(l => l.Book.Serial).Distinct().ToList();
            isbnComboBox.Items.AddRange(loanedBooks.ToArray());
        }

        private void ReturnBookButton_Click(object sender, EventArgs e)
        {
            var selectedIsbn = isbnComboBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedIsbn))
            {
                MessageBox.Show("Please select an ISBN to return.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Find the loan record
                var loan = _librarySystem.Loans.FirstOrDefault(l => l.Book.Serial == selectedIsbn);

                if (loan != null)
                {
                    // Mark the book as available again
                    loan.Book.IsAvailable = true;

                    // Remove the loan record
                    _librarySystem.Loans.Remove(loan);

                    MessageBox.Show("Book returned successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the grids to reflect changes
                    _mainForm.RefreshGrids();

                    this.Close();
                    _mainForm.Show();
                }
                else
                {
                    MessageBox.Show("No loan record found for the selected book.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
