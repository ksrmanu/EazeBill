using System.Data.Entity;
using StoreBilling.Data.Entities;

namespace StoreBilling.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext() : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Store> Stores { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
                
        public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; }

        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().Property(p => p.PurchaseRate).HasPrecision(18, 2);
            modelBuilder.Entity<Product>().Property(p => p.SellingRate).HasPrecision(18, 2);
            modelBuilder.Entity<Product>().Property(p => p.MRP).HasPrecision(18, 2);
            modelBuilder.Entity<SalesInvoice>().Property(s => s.GrandTotal).HasPrecision(18, 2);
            modelBuilder.Entity<SalesInvoice>().Property(s => s.SubTotal).HasPrecision(18, 2);
            modelBuilder.Entity<SalesInvoice>().Property(s => s.TaxAmount).HasPrecision(18, 2);
        }
    }
}