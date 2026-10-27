using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using StoreBilling.Data;
using StoreBilling.Models.ViewModels;

namespace StoreBilling.Controllers
{
    public class HomeController : Controller
    {
        private readonly StoreDbContext _db = new StoreDbContext();

        public ActionResult Index()
        {
            var today = DateTime.Today;
            var vm = new DashboardViewModel();

            // 1. Calculate KPIs
            vm.TodaySales = _db.SalesInvoices
                .Where(s => !s.IsDeleted && DbFunctions.TruncateTime(s.CreatedAt) == today)
                .Select(s => (decimal?)s.GrandTotal).Sum() ?? 0m;

            vm.TodayPurchases = _db.PurchaseInvoices
                .Where(p => !p.IsDeleted && DbFunctions.TruncateTime(p.CreatedAt) == today)
                .Select(p => (decimal?)p.GrandTotal).Sum() ?? 0m;

            // Receivables (Mocked for now, as we haven't added partial payments yet)
            vm.TotalReceivables = 8900m;

            // Calculate Low Stock (Where Current Stock <= MinStock)
            var products = _db.Products.Where(p => !p.IsDeleted && p.IsActive).ToList();
            int lowStockCount = 0;
            foreach (var p in products)
            {
                var stock = _db.InventoryTransactions.Where(t => t.ProductId == p.Id).Sum(t => (decimal?)(t.QtyIn - t.QtyOut)) ?? 0m;
                if (stock <= p.MinStock) lowStockCount++;
            }
            vm.LowStockCount = lowStockCount;

            // 2. Generate Chart Data (Last 7 Days)
            vm.ChartLabels = new List<string>();
            vm.ChartValues = new List<decimal>();
            for (int i = 6; i >= 0; i--)
            {
                var date = today.AddDays(-i);
                vm.ChartLabels.Add(date.ToString("ddd"));

                var daySales = _db.SalesInvoices
                    .Where(s => !s.IsDeleted && DbFunctions.TruncateTime(s.CreatedAt) == date)
                    .Select(s => (decimal?)s.GrandTotal).Sum() ?? 0m;
                vm.ChartValues.Add(daySales);
            }

            // 3. Get Recent Sales
            vm.RecentSales = _db.SalesInvoices
                .Where(s => !s.IsDeleted)
                .OrderByDescending(s => s.Id)
                .Take(5)
                .Select(s => new RecentSaleDto
                {
                    InvoiceNo = s.InvoiceNo,
                    Amount = s.GrandTotal,
                    Status = s.Status
                }).ToList();

            return View(vm);
        }
    }
}