using System;

namespace StoreBilling.Data.Entities
{
    public class Receipt : BaseEntity
    {
        public string ReceiptNo { get; set; }
        public DateTime ReceiptDate { get; set; } = DateTime.Now;
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; }
        public string Remarks { get; set; }
    }

    public class Payment : BaseEntity
    {
        public string PaymentNo { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public string SupplierName { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; }
        public string Remarks { get; set; }
    }

    public class Expense : BaseEntity
    {
        public DateTime ExpenseDate { get; set; } = DateTime.Now;
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; }
    }
}