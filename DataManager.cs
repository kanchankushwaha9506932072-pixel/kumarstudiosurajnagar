using System;
using System.IO;
using System.Text.Json;

namespace KumarStudioBillingSoftware
{
    public static class DataManager
    {
        private static readonly string DataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KumarStudioBillingSoftware");
        private static readonly string DbFilePath = Path.Combine(DataDirectory, "kumar_studio_db.json");

        public static DatabaseStore Db { get; private set; } = new DatabaseStore();

        static DataManager()
        {
            EnsureDirectoryExists();
            LoadData();
            SeedDefaultDataIfNeeded();
        }

        private static void EnsureDirectoryExists()
        {
            if (!Directory.Exists(DataDirectory))
            {
                Directory.CreateDirectory(DataDirectory);
            }
        }

        public static void LoadData()
        {
            try
            {
                if (File.Exists(DbFilePath))
                {
                    string json = File.ReadAllText(DbFilePath);
                    var store = JsonSerializer.Deserialize<DatabaseStore>(json);
                    if (store != null)
                    {
                        Db = store;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        public static void SaveData()
        {
            try
            {
                EnsureDirectoryExists();
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(Db, options);
                File.ReadAllText(DbFilePath); // dry check or overwrite
                File.WriteAllText(DbFilePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving data: {ex.Message}");
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string json = JsonSerializer.Serialize(Db, options);
                    File.WriteAllText(DbFilePath, json);
                }
                catch { }
            }
        }

        public static string BackupData(string destFilePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(Db, options);
                File.WriteAllText(destFilePath, json);
                return destFilePath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Backup failed: {ex.Message}");
            }
        }

        public static bool RestoreData(string srcFilePath)
        {
            try
            {
                if (!File.Exists(srcFilePath)) return false;
                string json = File.ReadAllText(srcFilePath);
                var store = JsonSerializer.Deserialize<DatabaseStore>(json);
                if (store != null)
                {
                    Db = store;
                    SaveData();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Restore failed: {ex.Message}");
            }
            return false;
        }

        private static void SeedDefaultDataIfNeeded()
        {
            if (Db.Customers.Count == 0)
            {
                Db.Customers.Add(new Customer { CustomerId = "CUST1001", Name = "Ramesh Kumar", Mobile = "9876543210", Address = "Padrauna Town, Kushinagar", Notes = "Wedding client" });
                Db.Customers.Add(new Customer { CustomerId = "CUST1002", Name = "Sunil Verma", Mobile = "9123456789", Address = "Surajnagar, Padrauna", Notes = "Tilak & Haldi shoot" });
            }

            if (Db.Packages.Count == 0)
            {
                Db.Packages.Add(new PackageModel { PackageName = "Silver Wedding Package", Price = 25000, Badge = "Popular", Inclusions = "1 DSLR Photo, 1 HD Video, 1 Photo Album 30 Pages, Pendrive" });
                Db.Packages.Add(new PackageModel { PackageName = "Gold Cinematic Package", Price = 45000, Badge = "Best Seller", Inclusions = "2 Photographer (DSLR + Candid), 1 Cinematic Video, 1 4K Drone, 1 Album, Teaser" });
                Db.Packages.Add(new PackageModel { PackageName = "Royal Grand Wedding", Price = 85000, Badge = "Premium", Inclusions = "3 Photographers, 2 Videographers, 4K Drone, Crane Camera, LED Wall, Digital Album" });
            }

            if (Db.Services.Count == 0)
            {
                Db.Services.Add(new ServiceModel { ServiceName = "Wedding Photography (DSLR)", Category = "Photography", BasePrice = 10000, Description = "Traditional Photography Per Day" });
                Db.Services.Add(new ServiceModel { ServiceName = "Candid Photography", Category = "Photography", BasePrice = 15000, Description = "High resolution candid captures" });
                Db.Services.Add(new ServiceModel { ServiceName = "4K Cinematic Videography", Category = "Videography", BasePrice = 20000, Description = "Cinematic video capture and highlight teaser" });
                Db.Services.Add(new ServiceModel { ServiceName = "4K Drone Aerial Shoot", Category = "Drone", BasePrice = 12000, Description = "Aerial drone coverage per event" });
                Db.Services.Add(new ServiceModel { ServiceName = "Crane Camera / Jib", Category = "Videography", BasePrice = 15000, Description = "Jib crane camera setup" });
            }

            if (Db.Inventory.Count == 0)
            {
                Db.Inventory.Add(new InventoryItem { ItemName = "Fujifilm X-H2S Camera Body", Category = "Camera", Unit = "Pcs", Quantity = 2, UnitPrice = 185000, MinimumStock = 1 });
                Db.Inventory.Add(new InventoryItem { ItemName = "Sony FX3 Cinema Line", Category = "Camera", Unit = "Pcs", Quantity = 1, UnitPrice = 320000, MinimumStock = 1 });
                Db.Inventory.Add(new InventoryItem { ItemName = "DJI Mini 4 Pro Drone", Category = "Drone", Unit = "Pcs", Quantity = 2, UnitPrice = 95000, MinimumStock = 1 });
                Db.Inventory.Add(new InventoryItem { ItemName = "Photographic Albums (Photobook 30P)", Category = "Album", Unit = "Pcs", Quantity = 15, UnitPrice = 3500, MinimumStock = 5 });
                Db.Inventory.Add(new InventoryItem { ItemName = "LED Video Light Panels", Category = "Lighting", Unit = "Pcs", Quantity = 6, UnitPrice = 4500, MinimumStock = 2 });
            }

            if (Db.Suppliers.Count == 0)
            {
                Db.Suppliers.Add(new Supplier { Name = "Gorakhpur Album Lab", ContactPerson = "Anand Sharma", Mobile = "9839000000", Address = "Cinema Road, Gorakhpur", GSTIN = "09ABCDE1234F1Z5" });
                Db.Suppliers.Add(new Supplier { Name = "Delhi Camera World", ContactPerson = "Rajesh Gupta", Mobile = "9811000000", Address = "Chandni Chowk, Delhi", GSTIN = "07XYZDE5678F1Z8" });
            }

            if (Db.Bills.Count == 0)
            {
                Db.Bills.Add(new Bill
                {
                    InvoiceNumber = "KS-1001",
                    BookingId = "KS-1001",
                    CustomerId = "CUST1001",
                    CustomerName = "Ramesh Kumar",
                    CustomerMobile = "9876543210",
                    CustomerAddress = "Padrauna Town, Kushinagar",
                    TotalAmount = 45000,
                    AdvancePaid = 10000,
                    ExtraCharges = 5000,
                    Discount = 2000,
                    DueBalance = 38000,
                    PackageDetails = "Gold Cinematic Package + Drone",
                    Status = "Confirmed - Advance Paid",
                    EventsSummary = "Engagement (2025-03-10) @ Hotel Grand, Shaadi (2025-03-12) @ Palace Hall",
                    EquipmentSummary = "Sony FX3, Candid Photo, 4K Drone"
                });
            }

            SaveData();
        }
    }
}
