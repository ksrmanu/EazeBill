using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using StoreBilling.Data;

namespace StoreBilling.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private readonly StoreDbContext _db = new StoreDbContext();

        // GET: /Sales
        public ActionResult Index()
        {
            return View();
        }

        // AJAX Endpoint for DataTable
        [HttpGet]
        public JsonResult GetList(string startDate, string endDate)
        {
            DateTime start = string.IsNullOrEmpty(startDate) ? DateTime.Today.AddDays(-30) : Convert.ToDateTime(startDate);
            DateTime end = string.IsNullOrEmpty(endDate) ? DateTime.Today : Convert.ToDateTime(endDate).AddDays(1);

            var data = _db.SalesInvoices
                .Where(s => !s.IsDeleted && s.CreatedAt >= start && s.CreatedAt < end)
                .OrderByDescending(s => s.Id)
                .Select(s => new {
                    s.Id,
                    s.InvoiceNo,
                    Date = s.CreatedAt,
                    s.CustomerName,
                    s.PaymentMethod,
                    s.GrandTotal,
                    s.Status
                }).ToList();

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // GET: /Sales/Print/5 
        public ActionResult Print(int? id)
        {
            // If no ID is provided in the URL, redirect back to the list
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            var invoice = _db.SalesInvoices.FirstOrDefault(s => s.Id == id.Value && !s.IsDeleted);
            if (invoice == null) return HttpNotFound();

            // Fetch items for this invoice
            var items = _db.SalesInvoiceItems.Where(i => i.InvoiceId == id.Value).ToList();
            ViewBag.Items = items;

            return View(invoice);
        }
    }
}