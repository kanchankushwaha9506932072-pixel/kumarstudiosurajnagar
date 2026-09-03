using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace KumarStudioBillingSoftware
{
    public class ExcelExportView : UserControl
    {
        public ExcelExportView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.AutoScroll = true;
            this.BackColor = Color.FromArgb(248, 246, 242);

            Label lblTitle = new Label
            {
                Text = "📊 Excel / CSV Data Exporter Module",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            GroupBox grpBox = new GroupBox
            {
                Text = "Select Dataset to Export to Excel (CSV)",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Left = 20,
                Top = 60,
                Width = 600,
                Height = 350,
                BackColor = Color.White
            };

            Button btnExpCust = CreateExportButton("👥 Export Customer Directory to Excel", 20, 40, ExportCustomers);
            Button btnExpInvoices = CreateExportButton("🧾 Export Billing & Invoices to Excel", 20, 100, ExportInvoices);
            Button btnExpInventory = CreateExportButton("📦 Export Stock Inventory to Excel", 20, 160, ExportInventory);
            Button btnExpPayments = CreateExportButton("💳 Export Payment Ledger to Excel", 20, 220, ExportPayments);

            grpBox.Controls.Add(btnExpCust);
            grpBox.Controls.Add(btnExpInvoices);
            grpBox.Controls.Add(btnExpInventory);
            grpBox.Controls.Add(btnExpPayments);

            this.Controls.Add(grpBox);
        }

        private Button CreateExportButton(string text, int left, int top, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(127, 29, 29),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Left = left,
                Top = top,
                Width = 550,
                Height = 44,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;
            return btn;
        }

        private void ExportCustomers(object? sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CustomerId,Name,Mobile,Email,Address,Notes");
            foreach (var c in DataManager.Db.Customers)
            {
                sb.AppendLine($"\"{c.CustomerId}\",\"{c.Name}\",\"{c.Mobile}\",\"{c.Email}\",\"{c.Address}\",\"{c.Notes}\"");
            }
            SaveCsvFile("Customers_Export.csv", sb.ToString());
        }

        private void ExportInvoices(object? sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("InvoiceNumber,CustomerName,TotalAmount,AdvancePaid,ExtraCharges,Discount,DueBalance,Status");
            foreach (var b in DataManager.Db.Bills)
            {
                sb.AppendLine($"\"{b.InvoiceNumber}\",\"{b.CustomerName}\",{b.TotalAmount},{b.AdvancePaid},{b.ExtraCharges},{b.Discount},{b.DueBalance},\"{b.Status}\"");
            }
            SaveCsvFile("Invoices_Export.csv", sb.ToString());
        }

        private void ExportInventory(object? sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ItemId,ItemName,Category,Quantity,UnitPrice,MinimumStock");
            foreach (var i in DataManager.Db.Inventory)
            {
                sb.AppendLine($"\"{i.ItemId}\",\"{i.ItemName}\",\"{i.Category}\",{i.Quantity},{i.UnitPrice},{i.MinimumStock}");
            }
            SaveCsvFile("Inventory_Export.csv", sb.ToString());
        }

        private void ExportPayments(object? sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("PaymentId,InvoiceNumber,CustomerName,AmountPaid,PaymentMode,PaymentDate,Notes");
            foreach (var p in DataManager.Db.Payments)
            {
                sb.AppendLine($"\"{p.PaymentId}\",\"{p.InvoiceNumber}\",\"{p.CustomerName}\",{p.AmountPaid},\"{p.PaymentMode}\",\"{p.PaymentDate}\",\"{p.Notes}\"");
            }
            SaveCsvFile("Payments_Export.csv", sb.ToString());
        }

        private void SaveCsvFile(string defaultName, string content)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = defaultName;
                sfd.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, content, Encoding.UTF8);
                    MessageBox.Show("Data exported successfully to CSV/Excel format!", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }

    public class PdfInvoiceView : UserControl
    {
        private ComboBox cmbInvoices = null!;
        private WebBrowser webPreview = null!;

        public PdfInvoiceView()
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
                Text = "📑 PDF Invoice & Letterhead Printer",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            Label lblSelect = new Label
            {
                Text = "Select Invoice:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 20,
                Top = 60,
                AutoSize = true
            };
            this.Controls.Add(lblSelect);

            cmbInvoices = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Left = 130,
                Top = 56,
                Width = 280,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbInvoices.SelectedIndexChanged += CmbInvoices_SelectedIndexChanged;
            this.Controls.Add(cmbInvoices);

            Button btnPrint = new Button
            {
                Text = "🖨️ Print / Save PDF Invoice",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(127, 29, 29),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Left = 430,
                Top = 55,
                Width = 220,
                Height = 32,
                Cursor = Cursors.Hand
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += (s, e) => webPreview.ShowPrintDialog();
            this.Controls.Add(btnPrint);

            webPreview = new WebBrowser
            {
                Left = 20,
                Top = 100,
                Width = 960,
                Height = 520
            };
            this.Controls.Add(webPreview);
        }

        private void LoadData()
        {
            cmbInvoices.Items.Clear();
            foreach (var b in DataManager.Db.Bills)
            {
                cmbInvoices.Items.Add($"{b.InvoiceNumber} - {b.CustomerName}");
            }
            if (cmbInvoices.Items.Count > 0) cmbInvoices.SelectedIndex = 0;
        }

        private void CmbInvoices_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbInvoices.SelectedIndex >= 0 && cmbInvoices.SelectedIndex < DataManager.Db.Bills.Count)
            {
                var bill = DataManager.Db.Bills[cmbInvoices.SelectedIndex];
                var s = DataManager.Db.Settings;

                string html = $@"
                <html>
                <head>
                <style>
                    body {{ font-family: 'Segoe UI', Arial, sans-serif; margin: 30px; background-color: #fff; color: #333; }}
                    .header {{ border-bottom: 2px solid #7f1d1d; padding-bottom: 10px; margin-bottom: 20px; }}
                    .title {{ font-size: 24px; font-weight: bold; color: #7f1d1d; }}
                    .tagline {{ font-size: 12px; color: #666; }}
                    .inv-title {{ font-size: 20px; font-weight: bold; color: #7f1d1d; text-align: right; }}
                    .table {{ width: 100%; border-collapse: collapse; margin-top: 15px; }}
                    .table th, .table td {{ border: 1px solid #ddd; padding: 8px; font-size: 12px; }}
                    .table th {{ background-color: #7f1d1d; color: white; text-align: left; }}
                    .total-box {{ float: right; width: 300px; margin-top: 20px; }}
                    .footer {{ margin-top: 40px; text-align: center; font-size: 11px; color: #777; border-top: 1px solid #ddd; padding-top: 10px; }}
                </style>
                </head>
                <body>
                    <table style='width:100%'>
                    <tr>
                        <td class='header'>
                            <div class='title'>{s.StudioName}</div>
                            <div class='tagline'>{s.Tagline} | Phone: {s.Phone}</div>
                            <div class='tagline'>{s.Address}</div>
                        </td>
                        <td class='header' style='text-align:right'>
                            <div class='inv-title'>INVOICE</div>
                            <div><b>Invoice No:</b> {bill.InvoiceNumber}</div>
                            <div><b>Date:</b> {bill.BillDate}</div>
                        </td>
                    </tr>
                    </table>

                    <div style='margin-top:10px;'>
                        <b>Customer Name:</b> {bill.CustomerName}<br>
                        <b>Mobile:</b> {bill.CustomerMobile}<br>
                        <b>Payment Status:</b> {bill.Status}
                    </div>

                    <table class='table'>
                        <tr><th>Description / Services Included</th><th style='text-align:right'>Amount (₹)</th></tr>
                        <tr><td>{bill.PackageDetails}</td><td style='text-align:right'>₹{bill.TotalAmount}</td></tr>
                        <tr><td>Extra Charges / Equipment Rental</td><td style='text-align:right'>+ ₹{bill.ExtraCharges}</td></tr>
                        <tr><td>Discount Applied</td><td style='text-align:right'>- ₹{bill.Discount}</td></tr>
                    </table>

                    <div class='total-box'>
                        <table class='table'>
                            <tr><td><b>Total Payable:</b></td><td style='text-align:right'><b>₹{bill.TotalAmount + bill.ExtraCharges - bill.Discount}</b></td></tr>
                            <tr><td><b>Advance Paid:</b></td><td style='text-align:right; color:green'>- ₹{bill.AdvancePaid}</td></tr>
                            <tr style='background-color:#fffaf0;'><td><b style='color:#7f1d1d'>Due Balance:</b></td><td style='text-align:right'><b style='color:#7f1d1d'>₹{bill.DueBalance}</b></td></tr>
                        </table>
                    </div>

                    <div style='clear:both;'></div>
                    <div style='margin-top:30px; font-size:11px;'>
                        <b>UPI Payment ID:</b> {s.UpiId}<br>
                        <b>Terms & Conditions:</b><br>{s.TermsAndConditions.Replace("\n", "<br>")}
                    </div>

                    <div class='footer'>
                        Thank you for choosing {s.StudioName} - {s.Tagline}
                    </div>
                </body>
                </html>";

                webPreview.DocumentText = html;
            }
        }
    }

    public class SettingsView : UserControl
    {
        private TextBox txtStudioName = null!;
        private TextBox txtTagline = null!;
        private TextBox txtPhone = null!;
        private TextBox txtEmail = null!;
        private TextBox txtAddress = null!;
        private TextBox txtUpi = null!;
        private TextBox txtPassword = null!;

        public SettingsView()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            this.AutoScroll = true;
            this.BackColor = Color.FromArgb(248, 246, 242);

            Label lblTitle = new Label
            {
                Text = "⚙️ Studio Profile & System Settings",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            GroupBox grpBox = new GroupBox
            {
                Text = "Studio Configuration",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 20,
                Top = 60,
                Width = 500,
                Height = 480,
                BackColor = Color.White
            };

            int topPos = 30;

            grpBox.Controls.Add(new Label { Text = "Studio Name", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtStudioName = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 20, Width = 450 };
            grpBox.Controls.Add(txtStudioName);
            topPos += 55;

            grpBox.Controls.Add(new Label { Text = "Tagline / Subtitle", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtTagline = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 20, Width = 450 };
            grpBox.Controls.Add(txtTagline);
            topPos += 55;

            grpBox.Controls.Add(new Label { Text = "Phone / WhatsApp Number", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtPhone = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 20, Width = 450 };
            grpBox.Controls.Add(txtPhone);
            topPos += 55;

            grpBox.Controls.Add(new Label { Text = "Email Address", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtEmail = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 20, Width = 450 };
            grpBox.Controls.Add(txtEmail);
            topPos += 55;

            grpBox.Controls.Add(new Label { Text = "Studio Address", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtAddress = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 20, Width = 450 };
            grpBox.Controls.Add(txtAddress);
            topPos += 55;

            grpBox.Controls.Add(new Label { Text = "UPI Payment ID", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtUpi = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 20, Width = 450 };
            grpBox.Controls.Add(txtUpi);
            topPos += 55;

            grpBox.Controls.Add(new Label { Text = "Admin Login Password", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtPassword = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 20, Width = 450 };
            grpBox.Controls.Add(txtPassword);
            topPos += 60;

            Button btnSave = new Button
            {
                Text = "💾 Save Profile Settings",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(127, 29, 29),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Left = 15,
                Top = topPos,
                Width = 450,
                Height = 38,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            grpBox.Controls.Add(btnSave);

            this.Controls.Add(grpBox);
        }

        private void LoadSettings()
        {
            var s = DataManager.Db.Settings;
            txtStudioName.Text = s.StudioName;
            txtTagline.Text = s.Tagline;
            txtPhone.Text = s.Phone;
            txtEmail.Text = s.Email;
            txtAddress.Text = s.Address;
            txtUpi.Text = s.UpiId;
            txtPassword.Text = s.AdminPassword;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            var s = DataManager.Db.Settings;
            s.StudioName = txtStudioName.Text.Trim();
            s.Tagline = txtTagline.Text.Trim();
            s.Phone = txtPhone.Text.Trim();
            s.Email = txtEmail.Text.Trim();
            s.Address = txtAddress.Text.Trim();
            s.UpiId = txtUpi.Text.Trim();
            s.AdminPassword = txtPassword.Text.Trim();

            DataManager.SaveData();
            MessageBox.Show("Studio settings updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class BackupRestoreView : UserControl
    {
        public BackupRestoreView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.AutoScroll = true;
            this.BackColor = Color.FromArgb(248, 246, 242);

            Label lblTitle = new Label
            {
                Text = "💾 Database Backup & Restore",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            GroupBox grpBackup = new GroupBox
            {
                Text = "Database Data Protection",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Left = 20,
                Top = 60,
                Width = 550,
                Height = 220,
                BackColor = Color.White
            };

            Button btnBackup = new Button
            {
                Text = "💾 Create Full Database Backup File (.json)",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(22, 101, 52),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Left = 20,
                Top = 40,
                Width = 500,
                Height = 45,
                Cursor = Cursors.Hand
            };
            btnBackup.FlatAppearance.BorderSize = 0;
            btnBackup.Click += BtnBackup_Click;

            Button btnRestore = new Button
            {
                Text = "📂 Restore Database from Backup File",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(180, 40, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Left = 20,
                Top = 110,
                Width = 500,
                Height = 45,
                Cursor = Cursors.Hand
            };
            btnRestore.FlatAppearance.BorderSize = 0;
            btnRestore.Click += BtnRestore_Click;

            grpBackup.Controls.Add(btnBackup);
            grpBackup.Controls.Add(btnRestore);

            this.Controls.Add(grpBackup);
        }

        private void BtnBackup_Click(object? sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = $"KumarStudio_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                sfd.Filter = "JSON Backup (*.json)|*.json";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DataManager.BackupData(sfd.FileName);
                        MessageBox.Show("Backup created successfully!", "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error creating backup: {ex.Message}", "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnRestore_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON Backup (*.json)|*.json";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var res = MessageBox.Show("Restoring data will replace current records. Are you sure you want to proceed?", "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res == DialogResult.Yes)
                    {
                        try
                        {
                            if (DataManager.RestoreData(ofd.FileName))
                            {
                                MessageBox.Show("Database restored successfully!", "Restore Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error restoring backup: {ex.Message}", "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
