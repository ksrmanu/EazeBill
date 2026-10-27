namespace StoreBilling.Data.Entities
{
    public class PurchaseInvoiceItem : BaseEntity
    {
        public int PurchaseInvoiceId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Total { get; set; }
    }
}