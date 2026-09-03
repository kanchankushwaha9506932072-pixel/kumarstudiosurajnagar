using System;
using System.Collections.Generic;

namespace KumarStudioBillingSoftware
{
    public class Customer
    {
        public string CustomerId { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        public string Name { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class EventModel
    {
        public string EventId { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        public string BookingId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string EventType { get; set; } = "Wedding"; // Engagement, Tilak, Haldi, Mehndi, Sangeet, Shaadi, Reception, Birthday, Other
        public string EventDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        public string EventTime { get; set; } = "10:00 AM";
        public string Venue { get; set; } = string.Empty;
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled
    }

    public class Bill
    {
        public string BillId { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        public string InvoiceNumber { get; set; } = "KS-" + new Random().Next(1000, 9999);
        public string BookingId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerMobile { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal AdvancePaid { get; set; }
        public decimal ExtraCharges { get; set; }
        public decimal Discount { get; set; }
        public decimal DueBalance { get; set; }
        public string PackageDetails { get; set; } = string.Empty;
        public string Status { get; set; } = "Booked - Advance Pending"; // Booked - Advance Pending, Confirmed - Advance Paid, Completed - Full Paid, Due Balance
        public string PaymentMode { get; set; } = "Cash"; // Cash, UPI, Bank Transfer, Card
        public string BillDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        public string EventsSummary { get; set; } = string.Empty;
        public string EquipmentSummary { get; set; } = string.Empty;
    }

    public class Payment
    {
        public string PaymentId { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        public string InvoiceNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public string PaymentMode { get; set; } = "UPI"; // Cash, UPI, Bank Transfer, Card
        public string ReferenceNo { get; set; } = string.Empty;
        public string PaymentDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        public string Notes { get; set; } = string.Empty;
    }

    public class InventoryItem
    {
        public string ItemId { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        public string ItemName { get; set; } = string.Empty;
        public string Category { get; set; } = "Camera"; // Camera, Lens, Drone, Lighting, Album, Accessory, Other
        public string Unit { get; set; } = "Pcs";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int MinimumStock { get; set; } = 2;
        public string Location { get; set; } = "Main Studio";
        public string Notes { get; set; } = string.Empty;
    }

    public class Purchase
    {
        public string PurchaseId { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        public string InvoiceNo { get; set; } = string.Empty;
        public string SupplierId { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string PurchaseDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        public string PaymentStatus { get; set; } = "Paid";
    }

    public class Supplier
    {
        public string SupplierId { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        public string Name { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string GSTIN { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    public class ServiceModel
    {
        public string ServiceId { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        public string ServiceName { get; set; } = string.Empty;
        public string Category { get; set; } = "Photography"; // Photography, Videography, Drone, Editing, Album
        public decimal BasePrice { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
    }

    public class PackageModel
    {
        public string PackageId { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        public string PackageName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Badge { get; set; } = "Popular";
        public string Inclusions { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
    }

    public class SystemSettings
    {
        public string StudioName { get; set; } = "Kumar Studio";
        public string Tagline { get; set; } = "Surajnagar, Padrauna";
        public string Phone { get; set; } = "9506932072";
        public string Email { get; set; } = "kumarstudio@gmail.com";
        public string Address { get; set; } = "Surajnagar, Padrauna, Kushinagar - 274304";
        public string UpiId { get; set; } = "9506932072@upi";
        public string AdminPassword { get; set; } = "kumar123";
        public string CurrencySymbol { get; set; } = "₹";
        public string TermsAndConditions { get; set; } = "1. Advance payment is non-refundable.\n2. Balance payable before raw media handover.\n3. Photo selection to be completed within 15 days.";
    }

    public class DatabaseStore
    {
        public SystemSettings Settings { get; set; } = new SystemSettings();
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<EventModel> Events { get; set; } = new List<EventModel>();
        public List<Bill> Bills { get; set; } = new List<Bill>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();
        public List<Purchase> Purchases { get; set; } = new List<Purchase>();
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();
        public List<ServiceModel> Services { get; set; } = new List<ServiceModel>();
        public List<PackageModel> Packages { get; set; } = new List<PackageModel>();
    }
}
