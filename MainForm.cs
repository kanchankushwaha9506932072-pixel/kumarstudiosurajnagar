using System;
using System.Drawing;
using System.Windows.Forms;

namespace KumarStudioBillingSoftware
{
    public class MainForm : Form
    {
        private Panel pnlSidebar = null!;
        private Panel pnlHeader = null!;
        private Panel pnlContent = null!;
        private Label lblHeaderTitle = null!;

        public MainForm()
        {
            InitializeComponent();
            ShowModule("Dashboard");
        }

        private void InitializeComponent()
        {
            this.Text = $"{DataManager.Db.Settings.StudioName} - Billing + Inventory Software Windows Application";
            this.Size = new Size(1280, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1024, 700);

            // Header Panel
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(127, 29, 29) // Primary Red
            };

            Label lblLogo = new Label
            {
                Text = "📷 KUMAR STUDIO",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(212, 175, 55),
                Left = 20,
                Top = 15,
                AutoSize = true
            };

            lblHeaderTitle = new Label
            {
                Text = "Dashboard",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Left = 260,
                Top = 18,
                AutoSize = true
            };

            Button btnExit = new Button
            {
                Text = "Exit App",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(180, 40, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 90,
                Height = 32,
                Top = 14,
                Left = this.Width - 120,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Click += (s, e) => Application.Exit();

            pnlHeader.Controls.Add(lblLogo);
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Controls.Add(btnExit);

            // Sidebar Panel
            pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = Color.FromArgb(40, 10, 10),
                AutoScroll = true
            };

            string[] modules = new string[]
            {
                "Dashboard",
                "Customers",
                "Events",
                "Billing",
                "Payments",
                "Inventory",
                "Purchase",
                "Suppliers",
                "Services",
                "Packages",
                "Reports",
                "Excel Export",
                "PDF Invoice",
                "Settings",
                "Backup / Restore"
            };

            int topPos = 10;
            foreach (var mod in modules)
            {
                Button btnMod = new Button
                {
                    Text = GetModuleIcon(mod) + " " + mod,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(40, 10, 10),
                    FlatStyle = FlatStyle.Flat,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(15, 0, 0, 0),
                    Width = 200,
                    Height = 40,
                    Left = 10,
                    Top = topPos,
                    Cursor = Cursors.Hand,
                    Tag = mod
                };
                btnMod.FlatAppearance.BorderSize = 0;
                btnMod.Click += (s, e) => {
                    string selectedMod = (s as Button)?.Tag?.ToString() ?? "Dashboard";
                    ShowModule(selectedMod);
                };

                pnlSidebar.Controls.Add(btnMod);
                topPos += 44;
            }

            // Content Panel
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 246, 242)
            };

            this.Controls.Add(pnlContent);
            this.Controls.Add(pnlSidebar);
            this.Controls.Add(pnlHeader);
        }

        private string GetModuleIcon(string moduleName)
        {
            return moduleName switch
            {
                "Dashboard" => "📊",
                "Customers" => "👥",
                "Events" => "📅",
                "Billing" => "🧾",
                "Payments" => "💳",
                "Inventory" => "📦",
                "Purchase" => "🛒",
                "Suppliers" => "🏭",
                "Services" => "🛠️",
                "Packages" => "🎁",
                "Reports" => "📈",
                "Excel Export" => "📊",
                "PDF Invoice" => "📑",
                "Settings" => "⚙️",
                "Backup / Restore" => "💾",
                _ => "📌"
            };
        }

        public void ShowModule(string moduleName)
        {
            lblHeaderTitle.Text = moduleName;
            pnlContent.Controls.Clear();

            // Highlight active button
            foreach (Control ctrl in pnlSidebar.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn.Tag?.ToString() == moduleName)
                    {
                        btn.BackColor = Color.FromArgb(127, 29, 29);
                        btn.ForeColor = Color.FromArgb(212, 175, 55);
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(40, 10, 10);
                        btn.ForeColor = Color.White;
                    }
                }
            }

            Control view = moduleName switch
            {
                "Dashboard" => new DashboardView(this),
                "Customers" => new CustomersView(),
                "Events" => new EventsView(),
                "Billing" => new BillingView(),
                "Payments" => new PaymentsView(),
                "Inventory" => new InventoryView(),
                "Purchase" => new PurchaseView(),
                "Suppliers" => new SuppliersView(),
                "Services" => new ServicesView(),
                "Packages" => new PackagesView(),
                "Reports" => new ReportsView(),
                "Excel Export" => new ExcelExportView(),
                "PDF Invoice" => new PdfInvoiceView(),
                "Settings" => new SettingsView(),
                "Backup / Restore" => new BackupRestoreView(),
                _ => new Label { Text = "Module Under Development: " + moduleName, Font = new Font("Segoe UI", 14), AutoSize = true, Left = 30, Top = 30 }
            };

            view.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(view);
        }
    }
}
