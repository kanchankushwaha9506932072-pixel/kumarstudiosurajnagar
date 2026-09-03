using System;
using System.Drawing;
using System.Windows.Forms;

namespace KumarStudioBillingSoftware
{
    public class InventoryView : UserControl
    {
        private DataGridView grid = null!;
        private TextBox txtName = null!;
        private ComboBox cmbCategory = null!;
        private TextBox txtQty = null!;
        private TextBox txtPrice = null!;
        private TextBox txtMinStock = null!;

        public InventoryView()
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
                Text = "📦 Equipment & Stock Inventory Management",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            GroupBox grpForm = new GroupBox
            {
                Text = "Add / Update Stock Item",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 20,
                Top = 60,
                Width = 320,
                Height = 440,
                BackColor = Color.White
            };

            int topPos = 30;

            grpForm.Controls.Add(new Label { Text = "Item Name *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtName = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtName);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Category *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            cmbCategory = new ComboBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCategory.Items.AddRange(new object[] { "Camera", "Lens", "Drone", "Lighting", "Album", "Memory Card", "Accessory" });
            cmbCategory.SelectedIndex = 0;
            grpForm.Controls.Add(cmbCategory);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Quantity in Stock *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtQty = new TextBox { Text = "1", Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtQty);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Unit Price (₹)", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtPrice = new TextBox { Text = "0", Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtPrice);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Minimum Stock Level", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtMinStock = new TextBox { Text = "2", Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtMinStock);
            topPos += 65;

            Button btnSave = new Button
            {
                Text = "📦 Save Inventory Item",
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

            grid.Columns.Add("ItemId", "Item ID");
            grid.Columns.Add("Name", "Item Name");
            grid.Columns.Add("Category", "Category");
            grid.Columns.Add("Qty", "Stock Quantity");
            grid.Columns.Add("Price", "Unit Price (₹)");
            grid.Columns.Add("MinStock", "Min Stock");

            this.Controls.Add(grid);
        }

        private void LoadData()
        {
            grid.Rows.Clear();
            foreach (var item in DataManager.Db.Inventory)
            {
                grid.Rows.Add(item.ItemId, item.ItemName, item.Category, item.Quantity, item.UnitPrice, item.MinimumStock);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || !int.TryParse(txtQty.Text, out int qty))
            {
                MessageBox.Show("Please enter a valid item name and numeric quantity.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal.TryParse(txtPrice.Text, out decimal price);
            int.TryParse(txtMinStock.Text, out int minStock);

            InventoryItem item = new InventoryItem
            {
                ItemName = txtName.Text.Trim(),
                Category = cmbCategory.SelectedItem?.ToString() ?? "Camera",
                Quantity = qty,
                UnitPrice = price,
                MinimumStock = minStock
            };

            DataManager.Db.Inventory.Add(item);
            DataManager.SaveData();

            txtName.Clear();
            txtQty.Text = "1";

            LoadData();
            MessageBox.Show("Stock item saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class PurchaseView : UserControl
    {
        private DataGridView grid = null!;
        private ComboBox cmbSupplier = null!;
        private TextBox txtItem = null!;
        private TextBox txtQty = null!;
        private TextBox txtPrice = null!;

        public PurchaseView()
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
                Text = "🛒 Stock Purchase & Vendor Entry",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            GroupBox grpForm = new GroupBox
            {
                Text = "New Purchase Entry",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 20,
                Top = 60,
                Width = 320,
                Height = 380,
                BackColor = Color.White
            };

            int topPos = 30;

            grpForm.Controls.Add(new Label { Text = "Supplier / Vendor *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            cmbSupplier = new ComboBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            grpForm.Controls.Add(cmbSupplier);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Item Name *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtItem = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtItem);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Purchase Quantity *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtQty = new TextBox { Text = "1", Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtQty);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Unit Cost Price (₹) *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtPrice = new TextBox { Text = "0", Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtPrice);
            topPos += 65;

            Button btnSave = new Button
            {
                Text = "🛒 Save Purchase & Update Stock",
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

            grid.Columns.Add("PurchaseId", "Purchase ID");
            grid.Columns.Add("Supplier", "Supplier");
            grid.Columns.Add("Item", "Item Name");
            grid.Columns.Add("Qty", "Quantity");
            grid.Columns.Add("UnitPrice", "Unit Cost (₹)");
            grid.Columns.Add("Total", "Total Cost (₹)");

            this.Controls.Add(grid);
        }

        private void LoadData()
        {
            cmbSupplier.Items.Clear();
            foreach (var sup in DataManager.Db.Suppliers)
            {
                cmbSupplier.Items.Add(sup.Name);
            }
            if (cmbSupplier.Items.Count > 0) cmbSupplier.SelectedIndex = 0;

            grid.Rows.Clear();
            foreach (var p in DataManager.Db.Purchases)
            {
                grid.Rows.Add(p.PurchaseId, p.SupplierName, p.ItemName, p.Quantity, p.UnitPrice, p.TotalAmount);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (cmbSupplier.SelectedItem == null || string.IsNullOrWhiteSpace(txtItem.Text) || !int.TryParse(txtQty.Text, out int qty) || !decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Please fill all required purchase details.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Purchase pur = new Purchase
            {
                SupplierName = cmbSupplier.SelectedItem.ToString() ?? "",
                ItemName = txtItem.Text.Trim(),
                Quantity = qty,
                UnitPrice = price,
                TotalAmount = qty * price
            };

            DataManager.Db.Purchases.Add(pur);

            // Auto update stock
            var invItem = DataManager.Db.Inventory.Find(x => x.ItemName.Equals(pur.ItemName, StringComparison.OrdinalIgnoreCase));
            if (invItem != null)
            {
                invItem.Quantity += qty;
            }
            else
            {
                DataManager.Db.Inventory.Add(new InventoryItem { ItemName = pur.ItemName, Quantity = qty, UnitPrice = price });
            }

            DataManager.SaveData();

            txtItem.Clear();
            txtQty.Text = "1";
            txtPrice.Text = "0";

            LoadData();
            MessageBox.Show("Purchase recorded and inventory updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class SuppliersView : UserControl
    {
        private DataGridView grid = null!;
        private TextBox txtName = null!;
        private TextBox txtContact = null!;
        private TextBox txtMobile = null!;
        private TextBox txtAddress = null!;
        private TextBox txtGst = null!;

        public SuppliersView()
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
                Text = "🏭 Supplier & Vendor Directory",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 29, 29),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            GroupBox grpForm = new GroupBox
            {
                Text = "Add Supplier",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 20,
                Top = 60,
                Width = 320,
                Height = 440,
                BackColor = Color.White
            };

            int topPos = 30;

            grpForm.Controls.Add(new Label { Text = "Supplier Company Name *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtName = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtName);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Contact Person", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtContact = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtContact);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Mobile Number *", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtMobile = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtMobile);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "Address / City", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtAddress = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtAddress);
            topPos += 60;

            grpForm.Controls.Add(new Label { Text = "GSTIN Number", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 15, Top = topPos, AutoSize = true });
            txtGst = new TextBox { Font = new Font("Segoe UI", 10), Left = 15, Top = topPos + 22, Width = 280 };
            grpForm.Controls.Add(txtGst);
            topPos += 65;

            Button btnSave = new Button
            {
                Text = "🏭 Save Supplier Record",
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

            grid.Columns.Add("SupId", "Supplier ID");
            grid.Columns.Add("Name", "Company Name");
            grid.Columns.Add("Contact", "Contact Person");
            grid.Columns.Add("Mobile", "Mobile");
            grid.Columns.Add("Address", "Address");
            grid.Columns.Add("GST", "GSTIN");

            this.Controls.Add(grid);
        }

        private void LoadData()
        {
            grid.Rows.Clear();
            foreach (var sup in DataManager.Db.Suppliers)
            {
                grid.Rows.Add(sup.SupplierId, sup.Name, sup.ContactPerson, sup.Mobile, sup.Address, sup.GSTIN);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtMobile.Text))
            {
                MessageBox.Show("Please enter supplier name and mobile number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Supplier sup = new Supplier
            {
                Name = txtName.Text.Trim(),
                ContactPerson = txtContact.Text.Trim(),
                Mobile = txtMobile.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                GSTIN = txtGst.Text.Trim()
            };

            DataManager.Db.Suppliers.Add(sup);
            DataManager.SaveData();

            txtName.Clear();
            txtContact.Clear();
            txtMobile.Clear();
            txtAddress.Clear();
            txtGst.Clear();

            LoadData();
            MessageBox.Show("Supplier saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
