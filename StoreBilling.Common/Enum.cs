using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBilling.Common
{
    internal class Enum
    {
  
        public static class StockRefType
        {
            public const string Opening = "OPENING";
            public const string Sale = "SALE";
            public const string Purchase = "PURCHASE";
            public const string SalesReturn = "SALES_RETURN";
            public const string PurchaseReturn = "PURCHASE_RETURN";
            public const string Adjustment = "ADJUSTMENT";
            public const string Transfer = "TRANSFER";
        }

        public static class PaymentStatus
        {
            public const string Paid = "Paid";
            public const string Partial = "Partial";
            public const string Unpaid = "Unpaid";
        }
    }
}

