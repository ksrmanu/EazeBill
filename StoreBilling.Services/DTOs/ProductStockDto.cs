namespace StoreBilling.Services.DTOs
{
    public class ProductStockDto
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal MRP { get; set; }
        public decimal MinStock { get; set; }
        public decimal Stock { get; set; }
    }
}