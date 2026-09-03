using System;
using System.Drawing;
using System.Windows.Forms;

namespace KumarStudioBillingSoftware
{
    public class LoginForm : Form
    {
        private TextBox txtPassword = null!;
        private Button btnLogin = null!;
        private Label lblTitle = null!;
        private Label lblSubTitle = null!;
        private Label lblErr = null!;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Kumar Studio Billing Software - Login";
            this.Size = new Size(420, 360);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(255, 250, 240); // Soft gold/cream background

            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(127, 29, 29) // Primary Royal Red
            };

            lblTitle = new Label
            {
                Text = "KUMAR STUDIO",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(212, 175, 55), // Accent Gold
                AutoSize = true,
                Left = 20,
                Top = 15
            };

            lblSubTitle = new Label
            {
                Text = "Billing & Inventory Management System",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = true,
                Left = 20,
                Top = 45
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubTitle);

            Label lblPass = new Label
            {
                Text = "Enter Admin Password:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 20, 20),
                Left = 40,
                Top = 110,
                AutoSize = true
            };

            txtPassword = new TextBox
            {
                PasswordChar = '*',
                Font = new Font("Segoe UI", 12),
                Left = 40,
                Top = 135,
                Width = 320
            };
            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) PerformLogin(); };

            btnLogin = new Button
            {
                Text = "Login to Dashboard",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(127, 29, 29),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Left = 40,
                Top = 180,
                Width = 320,
                Height = 42,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += (s, e) => PerformLogin();

            lblErr = new Label
            {
                Text = "Incorrect password! Please try kumar123 or kumar@123",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = Color.Red,
                Left = 40,
                Top = 230,
                Width = 320,
                Height = 35,
                Visible = false
            };

            Label lblHint = new Label
            {
                Text = "Default Passwords: kumar123 / kumar@123",
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Gray,
                Left = 40,
                Top = 275,
                AutoSize = true
            };

            this.Controls.Add(pnlHeader);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(lblErr);
            this.Controls.Add(lblHint);
        }

        private void PerformLogin()
        {
            string pass = txtPassword.Text.Trim();
            string expectedPass = DataManager.Db.Settings.AdminPassword;

            if (pass == expectedPass || pass == "kumar123" || pass == "kumar@123")
            {
                lblErr.Visible = false;
                this.Hide();
                MainForm main = new MainForm();
                main.FormClosed += (s, args) => this.Close();
                main.Show();
            }
            else
            {
                lblErr.Visible = true;
            }
        }
    }
}
