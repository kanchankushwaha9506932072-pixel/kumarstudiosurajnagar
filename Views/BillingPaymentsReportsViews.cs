using System;
using System.Drawing;
using System.Windows.Forms;

namespace KumarStudioBillingSoftware
{
    public class BillingView : UserControl
    {
        private ComboBox cmbCustomer = null!;
        private ComboBox cmbPackage = null!;
        private TextBox txtTotal = null!;
        private TextBox txtAdvance = null!;
        private TextBox txtExtra = null!;
        private TextBox txtDiscount = null!;
        private Label lblDue = null!;
        private TextBox txtNotes = null!;
        private ComboBox cmbStatus = null!;
        private ComboBox cmbPayMode = null!;
        private DataGridView grid = null!;

        public BillingView()
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
                Text = "🧾 Customer Billing & Invoice Generator",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            GroupBox grpForm = new GroupBox
            {
                Text = "Generate / Edit Invoice",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 20,
                Top = 60,
                Width = 380,
                Height = 550,
                BackColor = Color.White
            };

            int topPos = 25;

            grpForm.Controls.Add(new Label { Text = "Select Customer *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            cmbCustomer = new ComboBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 20, Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };
            grpForm.Controls.Add(cmbCustomer);
            topPos += 55;

            grpForm.Controls.Add(new Label { Text = "Select Package (Optional)", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            cmbPackage = new ComboBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 20, Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPackage.SelectedIndexChanged += CmbPackage_SelectedIndexChanged;
            grpForm.Controls.Add(cmbPackage);
            topPos += 55;

            // Amounts grid inside form
            grpForm.Controls.Add(new Label { Text = "Base Amount (₹) *", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtTotal = new TextBox { Text = "0", Font = new Font("Segoe UI", 9.5f), Left = 15, Top = topPos + 18, Width = 160 };
            txtTotal.TextChanged += (s, e) => RecalculateDue();
            grpForm.Controls.Add(txtTotal);

            grpForm.Controls.Add(new Label { Text = "Advance Paid (₹)", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), Left = 195, Top = topPos, AutoSize = true });
            txtAdvance = new TextBox { Text = "0", Font = new Font("Segoe UI", 9.5f), Left = 195, Top = topPos + 18, Width = 160 };
            txtAdvance.TextChanged += (s, e) => RecalculateDue();
            grpForm.Controls.Add(txtAdvance);
            topPos += 50;

            grpForm.Controls.Add(new Label { Text = "Extra Charges (₹)", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtExtra = new TextBox { Text = "0", Font = new Font("Segoe UI", 9.5f), Left = 15, Top = topPos + 18, Width = 160 };
            txtExtra.TextChanged += (s, e) => RecalculateDue();
            grpForm.Controls.Add(txtExtra);

            grpForm.Controls.Add(new Label { Text = "Discount (₹)", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), Left = 195, Top = topPos, AutoSize = true });
            txtDiscount = new TextBox { Text = "0", Font = new Font("Segoe UI", 9.5f), Left = 195, Top = topPos + 18, Width = 160 };
            txtDiscount.TextChanged += (s, e) => RecalculateDue();
            grpForm.Controls.Add(txtDiscount);
            topPos += 50;

            // Due preview bar
            Panel pnlDue = new Panel { Left = 15, Top = topPos, Width = 340, Height = 36, BackColor = Color.FromArgb(254, 243, 199) };
            lblDue = new Label { Text = "Due Balance: ₹0", Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.FromArgb(146, 64, 14), Left = 10, Top = 8, AutoSize = true };
            pnlDue.Controls.Add(lblDue);
            grpForm.Controls.Add(pnlDue);
            topPos += 45;

            grpForm.Controls.Add(new Label { Text = "Package Notes / Inclusions Summary", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtNotes = new TextBox { Font = new Font("Segoe UI", 9), Left = 15, Top = topPos + 18, Width = 340, Multiline = true, Height = 45 };
            grpForm.Controls.Add(txtNotes);
            topPos += 70;

            grpForm.Controls.Add(new Label { Text = "Payment Status", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            cmbStatus = new ComboBox { Font = new Font("Segoe UI", 9), Left = 15, Top = topPos + 18, Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatus.Items.AddRange(new object[] { "Booked - Advance Pending", "Confirmed - Advance Paid", "Completed - Full Paid", "Due Balance" });
            cmbStatus.SelectedIndex = 1;
            grpForm.Controls.Add(cmbStatus);

            grpForm.Controls.Add(new Label { Text = "Payment Mode", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), Left = 195, Top = topPos, AutoSize = true });
            cmbPayMode = new ComboBox { Font = new Font("Segoe UI", 9), Left = 195, Top = topPos + 18, Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPayMode.Items.AddRange(new object[] { "Cash", "UPI", "Bank Transfer", "Card" });
            cmbPayMode.SelectedIndex = 1;
            grpForm.Controls.Add(cmbPayMode);
            topPos += 50;

            Button btnSave = new Button
            {
                Text = "💾 Save & Generate Invoice",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(127, 29, 29),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Left = 15,
                Top = topPos,
                Width = 340,
                Height = 38,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            grpForm.Controls.Add(btnSave);

            this.Controls.Add(grpForm);

            grid = new DataGridView
            {
                Left = 420,
                Top = 60,
                Width = 560,
                Height = 550,
                BackgroundColor = Color.White,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            grid.Columns.Add("InvoiceNo", "Invoice No");
            grid.Columns.Add("Customer", "Customer Name");
            grid.Columns.Add("Total", "Total (₹)");
            grid.Columns.Add("Advance", "Advance (₹)");
            grid.Columns.Add("Due", "Due (₹)");
            grid.Columns.Add("Status", "Status");

            this.Controls.Add(grid);
        }

        private void LoadData()
        {
            cmbCustomer.Items.Clear();
            foreach (var c in DataManager.Db.Customers)
            {
                cmbCustomer.Items.Add($"{c.Name} ({c.Mobile})");
            }
            if (cmbCustomer.Items.Count > 0) cmbCustomer.SelectedIndex = 0;

            cmbPackage.Items.Clear();
            cmbPackage.Items.Add("-- Select Package --");
            foreach (var p in DataManager.Db.Packages)
            {
                cmbPackage.Items.Add($"{p.PackageName} - ₹{p.Price}");
            }
            cmbPackage.SelectedIndex = 0;

            grid.Rows.Clear();
            foreach (var b in DataManager.Db.Bills)
            {
                grid.Rows.Add(b.InvoiceNumber, b.CustomerName, b.TotalAmount + b.ExtraCharges - b.Discount, b.AdvancePaid, b.DueBalance, b.Status);
            }
        }

        private void CmbPackage_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbPackage.SelectedIndex > 0)
            {
                var pkg = DataManager.Db.Packages[cmbPackage.SelectedIndex - 1];
                txtTotal.Text = pkg.Price.ToString();
                txtNotes.Text = pkg.Inclusions;
                RecalculateDue();
            }
        }

        private void RecalculateDue()
        {
            decimal.TryParse(txtTotal.Text, out decimal total);
            decimal.TryParse(txtAdvance.Text, out decimal adv);
            decimal.TryParse(txtExtra.Text, out decimal extra);
            decimal.TryParse(txtDiscount.Text, out decimal disc);

            decimal due = total + extra - adv - disc;
            lblDue.Text = $"Due Balance: ₹{due:N0}";
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (cmbCustomer.SelectedItem == null)
            {
                MessageBox.Show("Please select a customer.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string custStr = cmbCustomer.SelectedItem.ToString() ?? "";
            string custName = custStr.Split('(')[0].Trim();

            decimal.TryParse(txtTotal.Text, out decimal total);
            decimal.TryParse(txtAdvance.Text, out decimal adv);
            decimal.TryParse(txtExtra.Text, out decimal extra);
            decimal.TryParse(txtDiscount.Text, out decimal disc);

            decimal due = total + extra - adv - disc;

            Bill bill = new Bill
            {
                CustomerName = custName,
                TotalAmount = total,
                AdvancePaid = adv,
                ExtraCharges = extra,
                Discount = disc,
                DueBalance = due,
                PackageDetails = txtNotes.Text.Trim(),
                Status = cmbStatus.SelectedItem?.ToString() ?? "Confirmed - Advance Paid",
                PaymentMode = cmbPayMode.SelectedItem?.ToString() ?? "UPI"
            };

            DataManager.Db.Bills.Add(bill);

            if (adv > 0)
            {
                DataManager.Db.Payments.Add(new Payment
                {
                    InvoiceNumber = bill.InvoiceNumber,
                    CustomerName = custName,
                    AmountPaid = adv,
                    PaymentMode = bill.PaymentMode,
                    Notes = "Advance payment received"
                });
            }

            DataManager.SaveData();

            txtTotal.Text = "0";
            txtAdvance.Text = "0";
            txtExtra.Text = "0";
            txtDiscount.Text = "0";
            txtNotes.Clear();

            LoadData();
            MessageBox.Show($"Invoice {bill.InvoiceNumber} generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class PaymentsView : UserControl
    {
        private DataGridView grid = null!;

        public PaymentsView()
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
                Text = "💳 Payment Transactions & Ledger",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            grid = new DataGridView
            {
                Left = 20,
                Top = 60,
                Width = 960,
                Height = 550,
                BackgroundColor = Color.White,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            grid.Columns.Add("PayId", "Payment ID");
            grid.Columns.Add("InvoiceNo", "Invoice No");
            grid.Columns.Add("Customer", "Customer Name");
            grid.Columns.Add("Amount", "Amount Paid (₹)");
            grid.Columns.Add("Mode", "Payment Mode");
            grid.Columns.Add("Date", "Date & Time");
            grid.Columns.Add("Notes", "Notes");

            this.Controls.Add(grid);
        }

        private void LoadData()
        {
            grid.Rows.Clear();
            foreach (var p in DataManager.Db.Payments)
            {
                grid.Rows.Add(p.PaymentId, p.InvoiceNumber, p.CustomerName, p.AmountPaid, p.PaymentMode, p.PaymentDate, p.Notes);
            }
        }
    }

    public class ReportsView : UserControl
    {
        public ReportsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.AutoScroll = true;
            this.BackColor = Color.FromArgb(248, 246, 242);

            Label lblTitle = new Label
            {
                Text = "📈 Sales, Collection & Stock Business Reports",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            decimal totalSales = 0;
            decimal totalPaid = 0;
            decimal totalDue = 0;

            foreach (var b in DataManager.Db.Bills)
            {
                totalSales += b.TotalAmount + b.ExtraCharges - b.Discount;
                totalPaid += b.AdvancePaid;
                totalDue += b.DueBalance;
            }

            int itemTypes = DataManager.Db.Inventory.Count;
            int totalItemsCount = 0;
            foreach (var item in DataManager.Db.Inventory) totalItemsCount += item.Quantity;

            GroupBox grpSummary = new GroupBox
            {
                Text = "Financial Performance Summary",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Left = 20,
                Top = 60,
                Width = 960,
                Height = 220,
                BackColor = Color.White
            };

            grpSummary.Controls.Add(CreateReportCard("Total Sales Generated", $"₹{totalSales:N0}", Color.FromArgb(30, 58, 138), 20, 35));
            grpSummary.Controls.Add(CreateReportCard("Total Advance Collected", $"₹{totalPaid:N0}", Color.FromArgb(22, 101, 52), 250, 35));
            grpSummary.Controls.Add(CreateReportCard("Outstanding Dues", $"₹{totalDue:N0}", Color.FromArgb(153, 27, 27), 480, 35));
            grpSummary.Controls.Add(CreateReportCard("Total Stock Items", $"{totalItemsCount} Pcs ({itemTypes} Types)", Color.FromArgb(146, 64, 14), 710, 35));

            this.Controls.Add(grpSummary);
        }

        private Panel CreateReportCard(string title, string val, Color col, int left, int top)
        {
            Panel p = new Panel
            {
                Left = left,
                Top = top,
                Width = 210,
                Height = 150,
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label l1 = new Label { Text = title, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), ForeColor = col, Left = 10, Top = 15, Width = 190, Height = 40 };
            Label l2 = new Label { Text = val, Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = col, Left = 10, Top = 65, AutoSize = true };

            p.Controls.Add(l1);
            p.Controls.Add(l2);
            return p;
        }
    }
}
