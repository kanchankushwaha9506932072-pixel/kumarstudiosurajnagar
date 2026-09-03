using System;
using System.Drawing;
using System.Windows.Forms;

namespace KumarStudioBillingSoftware
{
    public class EventsView : UserControl
    {
        private DataGridView grid = null!;
        private ComboBox cmbCustomer = null!;
        private ComboBox cmbEventType = null!;
        private DateTimePicker dtpEventDate = null!;
        private TextBox txtEventTime = null!;
        private TextBox txtVenue = null!;

        public EventsView()
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
                Text = "📅 Multi-Event Booking & Schedule Tracker",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            GroupBox grpForm = new GroupBox
            {
                Text = "Add / Schedule Event",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 20,
                Top = 60,
                Width = 320,
                Height = 440,
                BackColor = Color.White
            };

            int topPos = 30;

            grpForm.Controls.Add(new Label { Text = "Select Customer *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            cmbCustomer = new ComboBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            grpForm.Controls.Add(cmbCustomer);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Event Type *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            cmbEventType = new ComboBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbEventType.Items.AddRange(new object[] { "Engagement", "Tilak", "Haldi", "Mehndi", "Sangeet", "Shaadi Wedding", "Reception", "Birthday", "Corporate / Other" });
            cmbEventType.SelectedIndex = 5;
            grpForm.Controls.Add(cmbEventType);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Event Date *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            dtpEventDate = new DateTimePicker { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280, Format = DateTimePickerFormat.Short };
            grpForm.Controls.Add(dtpEventDate);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Event Time", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtEventTime = new TextBox { Text = "10:00 AM", Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtEventTime);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Venue / Location", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtVenue = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtVenue);
            topPos += 65;

            Button btnSave = new Button
            {
                Text = "📅 Schedule Event",
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

            grid.Columns.Add("EventId", "Event ID");
            grid.Columns.Add("Customer", "Customer Name");
            grid.Columns.Add("Type", "Event Type");
            grid.Columns.Add("Date", "Date");
            grid.Columns.Add("Time", "Time");
            grid.Columns.Add("Venue", "Venue");
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

            grid.Rows.Clear();
            foreach (var ev in DataManager.Db.Events)
            {
                grid.Rows.Add(ev.EventId, ev.CustomerName, ev.EventType, ev.EventDate, ev.EventTime, ev.Venue, ev.Status);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (cmbCustomer.SelectedItem == null)
            {
                MessageBox.Show("Please select a customer.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string custStr = cmbCustomer.SelectedItem.ToString() ?? "";
            EventModel ev = new EventModel
            {
                CustomerName = custStr,
                EventType = cmbEventType.SelectedItem?.ToString() ?? "Wedding",
                EventDate = dtpEventDate.Value.ToString("yyyy-MM-dd"),
                EventTime = txtEventTime.Text.Trim(),
                Venue = txtVenue.Text.Trim(),
                Status = "Scheduled"
            };

            DataManager.Db.Events.Add(ev);
            DataManager.SaveData();

            LoadData();
            MessageBox.Show("Event scheduled successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class ServicesView : UserControl
    {
        private DataGridView grid = null!;
        private TextBox txtName = null!;
        private ComboBox cmbCat = null!;
        private TextBox txtPrice = null!;
        private TextBox txtDesc = null!;

        public ServicesView()
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
                Text = "🛠️ Studio Service Price List Manager",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            GroupBox grpForm = new GroupBox
            {
                Text = "Add / Edit Service",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 20,
                Top = 60,
                Width = 320,
                Height = 380,
                BackColor = Color.White
            };

            int topPos = 30;

            grpForm.Controls.Add(new Label { Text = "Service Name *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtName = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtName);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Category *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            cmbCat = new ComboBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCat.Items.AddRange(new object[] { "Photography", "Videography", "Drone", "Editing", "Album", "Equipment Rental" });
            cmbCat.SelectedIndex = 0;
            grpForm.Controls.Add(cmbCat);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Base Price (₹) *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtPrice = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtPrice);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Description", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtDesc = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtDesc);
            topPos += 65;

            Button btnSave = new Button
            {
                Text = "💾 Save Service",
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

            grid.Columns.Add("ServiceId", "Service ID");
            grid.Columns.Add("Name", "Service Name");
            grid.Columns.Add("Category", "Category");
            grid.Columns.Add("Price", "Price (₹)");
            grid.Columns.Add("Desc", "Description");

            this.Controls.Add(grid);
        }

        private void LoadData()
        {
            grid.Rows.Clear();
            foreach (var s in DataManager.Db.Services)
            {
                grid.Rows.Add(s.ServiceId, s.ServiceName, s.Category, s.BasePrice, s.Description);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || !decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Please enter a valid service name and numeric base price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ServiceModel srv = new ServiceModel
            {
                ServiceName = txtName.Text.Trim(),
                Category = cmbCat.SelectedItem?.ToString() ?? "Photography",
                BasePrice = price,
                Description = txtDesc.Text.Trim()
            };

            DataManager.Db.Services.Add(srv);
            DataManager.SaveData();

            txtName.Clear();
            txtPrice.Clear();
            txtDesc.Clear();

            LoadData();
            MessageBox.Show("Service added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class PackagesView : UserControl
    {
        private DataGridView grid = null!;
        private TextBox txtName = null!;
        private TextBox txtPrice = null!;
        private TextBox txtBadge = null!;
        private TextBox txtInclusions = null!;

        public PackagesView()
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
                Text = "🎁 Wedding & Event Package Builder",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            GroupBox grpForm = new GroupBox
            {
                Text = "Create / Edit Package",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 20,
                Top = 60,
                Width = 320,
                Height = 380,
                BackColor = Color.White
            };

            int topPos = 30;

            grpForm.Controls.Add(new Label { Text = "Package Title *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtName = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtName);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Package Price (₹) *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtPrice = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtPrice);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Badge (e.g. Popular, Best Seller)", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtBadge = new TextBox { Text = "Popular", Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtBadge);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Package Inclusions / Services", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtInclusions = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280, Multiline = true, Height = 45 };
            grpForm.Controls.Add(txtInclusions);
            topPos += 75;

            Button btnSave = new Button
            {
                Text = "🎁 Save Package",
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

            grid.Columns.Add("PkgId", "Package ID");
            grid.Columns.Add("Name", "Package Name");
            grid.Columns.Add("Price", "Price (₹)");
            grid.Columns.Add("Badge", "Badge");
            grid.Columns.Add("Inclusions", "Inclusions");

            this.Controls.Add(grid);
        }

        private void LoadData()
        {
            grid.Rows.Clear();
            foreach (var p in DataManager.Db.Packages)
            {
                grid.Rows.Add(p.PackageId, p.PackageName, p.Price, p.Badge, p.Inclusions);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || !decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Please enter valid package title and numeric price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PackageModel pkg = new PackageModel
            {
                PackageName = txtName.Text.Trim(),
                Price = price,
                Badge = txtBadge.Text.Trim(),
                Inclusions = txtInclusions.Text.Trim()
            };

            DataManager.Db.Packages.Add(pkg);
            DataManager.SaveData();

            txtName.Clear();
            txtPrice.Clear();
            txtInclusions.Clear();

            LoadData();
            MessageBox.Show("Package created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
