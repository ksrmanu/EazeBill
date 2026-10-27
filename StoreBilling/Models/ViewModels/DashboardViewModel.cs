using System.Collections.Generic;

namespace StoreBilling.Models.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TodaySales { get; set; }
        public decimal TodayPurchases { get; set; }
        public decimal TotalReceivables { get; set; }
        public int LowStockCount { get; set; }

        // Chart Data
        public List<string> ChartLabels { get; set; }
        public List<decimal> ChartValues { get; set; }

        // Recent Sales Table
        public List<RecentSaleDto> RecentSales { get; set; }
    }

    public class RecentSaleDto
    {
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }
}