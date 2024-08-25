using System;
using System.Drawing;
using System.Windows.Forms;

namespace EnhancedLibrarySystem
{
    public static class FormStyler
    {
        public static void StyleForm(Form form, string title = "Library Management System", int width = 800, int height = 600)
        {
            form.Text = title;
            form.Size = new Size(width, height);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        public static TableLayoutPanel CreateMainLayout()
        {
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100F) },
                RowStyles = { new RowStyle(SizeType.Percent, 20F), new RowStyle(SizeType.Percent, 80F) }
            };
            return mainLayout;
        }

        public static TableLayoutPanel CreateTopPanel(int buttonCount, Color backgroundColor)
        {
            var topPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = buttonCount,
                RowCount = 1,
                BackColor = backgroundColor
            };

            for (int i = 0; i < buttonCount; i++)
            {
                topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / buttonCount));
            }

            return topPanel;
        }

        public static Button CreateStyledButton(string text, Color color, EventHandler clickHandler)
        {
            var button = new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(10),
                Padding = new Padding(5),
                Height = 40
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = Color.DarkGray;
            button.FlatAppearance.MouseOverBackColor = Color.LightGray;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.Gray;
            button.FlatAppearance.BorderSize = 1;

            // Adding shadow effect
            button.Paint += (sender, e) =>
            {
                var g = e.Graphics;
                var rectangle = new Rectangle(0, 0, button.Width, button.Height);
                var shadowColor = Color.FromArgb(128, 0, 0, 0); // Black with transparency
                for (int i = 0; i < 5; i++)
                {
                    using (var shadowPen = new Pen(shadowColor, i))
                    {
                        g.DrawRectangle(shadowPen, rectangle);
                        rectangle.Inflate(-1, -1);
                    }
                }
            };

            if (clickHandler != null)
                button.Click += clickHandler;

            return button;
        }

        public static TabControl CreateTabControl()
        {
            return new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0)
            };
        }

        public static DataGridView CreateDataGrid()
        {
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(224, 224, 224)
            };

            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(87, 166, 74);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grid.ColumnHeadersHeight = 40;
            grid.EnableHeadersVisualStyles = false;

            return grid;
        }

        public static void AddTabsToTabControl(TabControl tabControl, params (string title, Control content)[] tabs)
        {
            foreach (var tab in tabs)
            {
                var tabPage = new TabPage(tab.title) { BackColor = Color.White };
                tabPage.Controls.Add(tab.content);
                tabControl.TabPages.Add(tabPage);
            }
        }
    }
}
