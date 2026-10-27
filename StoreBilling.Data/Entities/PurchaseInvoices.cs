namespace StoreBilling.Data.Entities
{
    public class PurchaseInvoice : BaseEntity
    {
        public string InvoiceNo { get; set; }
        public int SupplierId { get; set; }
        public decimal GrandTotal { get; set; }
    }
}