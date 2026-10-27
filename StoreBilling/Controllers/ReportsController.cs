using System;
using System.Linq;
using System.Web.Mvc;
using StoreBilling.Data;

namespace StoreBilling.Controllers
{
    public class ReportsController : Controller
    {
        private readonly StoreDbContext _db = new StoreDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetSalesReport(string startDate, string endDate)
        {
            DateTime start = string.IsNullOrEmpty(startDate) ? DateTime.Today.AddDays(-30) : Convert.ToDateTime(startDate);
            DateTime end = string.IsNullOrEmpty(endDate) ? DateTime.Today : Convert.ToDateTime(endDate).AddDays(1);

            var data = _db.SalesInvoices
                .Where(s => !s.IsDeleted && s.CreatedAt >= start && s.CreatedAt < end)
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => new {
                    s.Id,
                    s.InvoiceNo,
                    Date = s.CreatedAt,
                    s.CustomerName,
                    s.PaymentMethod,
                    s.SubTotal,
                    s.TaxAmount,
                    s.GrandTotal
                }).ToList();

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStockReport()
        {
            var data = _db.Products.Where(p => !p.IsDeleted && p.IsActive)
                .Select(p => new {
                    p.Code,
                    p.Name,
                    p.SellingRate,
                    p.MRP,
                    Stock = _db.InventoryTransactions.Where(t => t.ProductId == p.Id).Sum(t => (decimal?)(t.QtyIn - t.QtyOut)) ?? 0m
                }).ToList();

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}