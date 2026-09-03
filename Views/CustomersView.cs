using System;
using System.Drawing;
using System.Windows.Forms;

namespace KumarStudioBillingSoftware
{
    public class CustomersView : UserControl
    {
        private DataGridView grid = null!;
        private TextBox txtName = null!;
        private TextBox txtMobile = null!;
        private TextBox txtEmail = null!;
        private TextBox txtAddress = null!;
        private TextBox txtNotes = null!;

        public CustomersView()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.AutoScroll = true;
            this.BackColor = Color.FromArgb(248, 246, 242);

            Label lblTitle = new Label
            {
                Text = "👥 Customer Directory & Management",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Left Panel: Form
            GroupBox grpForm = new GroupBox
            {
                Text = "Add / Edit Customer",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 20,
                Top = 60,
                Width = 320,
                Height = 440,
                BackColor = Color.White
            };

            int topPos = 30;

            grpForm.Controls.Add(new Label { Text = "Customer Name *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtName = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtName);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Mobile Number *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtMobile = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtMobile);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Email Address", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtEmail = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtEmail);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Address / City", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtAddress = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtAddress);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Customer Notes", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtNotes = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280, Multiline = true, Height = 45 };
            grpForm.Controls.Add(txtNotes);
            topPos += 75;

            Button btnSave = new Button
            {
                Text = "💾 Save Customer Record",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(127, 29, 29),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Left = 15,
                Top = topPos,
                Width = 280,
                Height = 38,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            grpForm.Controls.Add(btnSave);

            this.Controls.Add(grpForm);

            // Right DataGrid
            grid = new DataGridView
            {
                Left = 360,
                Top = 60,
                Width = 620,
                Height = 550,
                BackgroundColor = Color.White,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            grid.Columns.Add("Id", "Customer ID");
            grid.Columns.Add("Name", "Name");
            grid.Columns.Add("Mobile", "Mobile");
            grid.Columns.Add("Email", "Email");
            grid.Columns.Add("Address", "Address");
            grid.Columns.Add("Notes", "Notes");

            this.Controls.Add(grid);
        }

        private void LoadData()
        {
            grid.Rows.Clear();
            foreach (var c in DataManager.Db.Customers)
            {
                grid.Rows.Add(c.CustomerId, c.Name, c.Mobile, c.Email, c.Address, c.Notes);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || txtMobile.Text.Trim().Length < 10)
            {
                MessageBox.Show("Please enter a valid Customer Name and 10-digit Mobile Number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Customer cust = new Customer
            {
                Name = txtName.Text.Trim(),
                Mobile = txtMobile.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Notes = txtNotes.Text.Trim()
            };

            DataManager.Db.Customers.Add(cust);
            DataManager.SaveData();

            txtName.Clear();
            txtMobile.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtNotes.Clear();

            LoadData();
            MessageBox.Show("Customer saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
