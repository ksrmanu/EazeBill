using System;

namespace StoreBilling.Data.Entities
{
    public class SalesInvoice : BaseEntity
    {
        public string InvoiceNo { get; set; }
        public string CustomerName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; } = "Paid";
    }
}