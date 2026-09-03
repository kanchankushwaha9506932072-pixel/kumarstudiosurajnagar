using System;
using System.Drawing;
using System.Windows.Forms;

namespace KumarStudioBillingSoftware
{
    public class DashboardView : UserControl
    {
        private MainForm mainForm;
        public DashboardView(MainForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.AutoScroll = true;
            this.BackColor = Color.FromArgb(248, 246, 242);

            Label lblTitle = new Label
            {
                Text = "⚡ Kumar Studio Dashboard Summary",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };

            this.Controls.Add(lblTitle);

            // Calculate stats
            decimal totalRev = 0;
            decimal totalDue = 0;
            foreach (var b in DataManager.Db.Bills)
            {
                totalRev += b.TotalAmount + b.ExtraCharges - b.Discount;
                totalDue += b.DueBalance;
            }

            int custCount = DataManager.Db.Customers.Count;
            int lowStockCount = DataManager.Db.Inventory.FindAll(x => x.Quantity <= x.MinimumStock).Count;

            // Stats Cards
            int cardWidth = 220;
            int cardHeight = 100;

            Panel card1 = CreateStatCard("Total Revenue", $"₹{totalRev:N0}", "💳 Payments Collected", Color.FromArgb(220, 252, 231), Color.FromArgb(22, 101, 52), 20, 60, cardWidth, cardHeight);
            Panel card2 = CreateStatCard("Pending Dues", $"₹{totalDue:N0}", "⚠️ Payment Outstanding", Color.FromArgb(254, 242, 242), Color.FromArgb(153, 27, 27), 260, 60, cardWidth, cardHeight);
            Panel card3 = CreateStatCard("Total Customers", $"{custCount}", "👥 Registered Clients", Color.FromArgb(239, 246, 255), Color.FromArgb(30, 58, 138), 500, 60, cardWidth, cardHeight);
            Panel card4 = CreateStatCard("Low Stock Alerts", $"{lowStockCount}", "📦 Stock Items Need Reorder", Color.FromArgb(254, 243, 199), Color.FromArgb(146, 64, 14), 740, 60, cardWidth, cardHeight);

            this.Controls.Add(card1);
            this.Controls.Add(card2);
            this.Controls.Add(card3);
            this.Controls.Add(card4);

            // Quick Action Buttons
            Label lblActions = new Label
            {
                Text = "🚀 Quick Shortcuts & Actions",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 180,
                AutoSize = true
            };
            this.Controls.Add(lblActions);

            Button btnNewBill = CreateActionButton("➕ Create New Invoice", Color.FromArgb(127, 29, 29), 20, 215, (s, e) => mainForm.ShowModule("Billing"));
            Button btnAddCust = CreateActionButton("👤 Add Customer", Color.FromArgb(30, 58, 138), 220, 215, (s, e) => mainForm.ShowModule("Customers"));
            Button btnAddEvent = CreateActionButton("📅 Schedule Event", Color.FromArgb(146, 64, 14), 420, 215, (s, e) => mainForm.ShowModule("Events"));
            Button btnCheckStock = CreateActionButton("📦 Check Stock Inventory", Color.FromArgb(22, 101, 52), 620, 215, (s, e) => mainForm.ShowModule("Inventory"));

            this.Controls.Add(btnNewBill);
            this.Controls.Add(btnAddCust);
            this.Controls.Add(btnAddEvent);
            this.Controls.Add(btnCheckStock);

            // Recent Bookings DataGrid
            Label lblRecent = new Label
            {
                Text = "📋 Recent Invoices & Bookings",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 275,
                AutoSize = true
            };
            this.Controls.Add(lblRecent);

            DataGridView grid = new DataGridView
            {
                Left = 20,
                Top = 305,
                Width = 940,
                Height = 350,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            grid.Columns.Add("InvoiceNo", "Invoice No");
            grid.Columns.Add("Customer", "Customer Name");
            grid.Columns.Add("Mobile", "Mobile");
            grid.Columns.Add("Total", "Total (₹)");
            grid.Columns.Add("Advance", "Advance (₹)");
            grid.Columns.Add("Due", "Due Balance (₹)");
            grid.Columns.Add("Status", "Status");

            foreach (var b in DataManager.Db.Bills)
            {
                grid.Rows.Add(b.InvoiceNumber, b.CustomerName, b.CustomerMobile, b.TotalAmount + b.ExtraCharges - b.Discount, b.AdvancePaid, b.DueBalance, b.Status);
            }

            this.Controls.Add(grid);
        }

        private Panel CreateStatCard(string title, string val, string sub, Color bg, Color fg, int left, int top, int w, int h)
        {
            Panel p = new Panel
            {
                Left = left,
                Top = top,
                Width = w,
                Height = h,
                BackColor = bg,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label l1 = new Label { Text = title, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = fg, Left = 10, Top = 10, AutoSize = true };
            Label l2 = new Label { Text = val, Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = fg, Left = 10, Top = 30, AutoSize = true };
            Label l3 = new Label { Text = sub, Font = new Font("Segoe UI", 8, FontStyle.Italic), ForeColor = fg, Left = 10, Top = 68, AutoSize = true };

            p.Controls.Add(l1);
            p.Controls.Add(l2);
            p.Controls.Add(l3);
            return p;
        }

        private Button CreateActionButton(string text, Color bg, int left, int top, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Left = left,
                Top = top,
                Width = 180,
                Height = 40,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;
            return btn;
        }
    }
}
