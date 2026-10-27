using System;

namespace StoreBilling.Data.Entities
{
    public class InventoryTransaction : BaseEntity
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int StoreId { get; set; }
        public virtual Store Store { get; set; }

        public decimal QtyIn { get; set; }
        public decimal QtyOut { get; set; }

        public string RefType { get; set; }
        public int? RefId { get; set; }
        public DateTime TxnDate { get; set; } = DateTime.Now;
        public string Remarks { get; set; }
    }
}