namespace StoreBilling.Data.Entities
{
    public class Product : BaseEntity
    {
        public string Code { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public decimal PurchaseRate { get; set; }
        public decimal SellingRate { get; set; }
        public decimal MRP { get; set; }
        public decimal MinStock { get; set; }
        public decimal OpeningStock { get; set; }

        public bool IsActive { get; set; } = true;
    }
}