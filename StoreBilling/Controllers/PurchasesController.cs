using StoreBilling.Data;
using StoreBilling.Data.Entities;
using StoreBilling.Services;
using StoreBilling.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StoreBilling.Controllers
{
    [Authorize]
    public class PurchasesController : Controller
    {
        private readonly StoreDbContext _db = new StoreDbContext();

        public ActionResult Index() { return View(); }

        // 1. List of all purchases for the Register
        [HttpGet]
        public JsonResult GetList()
        {
            var data = _db.PurchaseInvoices
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.Id)
                .Select(p => new {
                    p.Id,
                    p.InvoiceNo,
                    Date = p.CreatedAt,
                    Supplier = _db.Suppliers.Where(s => s.Id == p.SupplierId).Select(s => s.Name).FirstOrDefault(),
                    p.GrandTotal
                }).ToList();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        // 2. Show the Purchase Entry Screen
        public ActionResult Create()
        {
            return View();
        }

        // 3. AJAX to get Suppliers for dropdown
        [HttpGet]
        public JsonResult GetSuppliers()
        {
            var suppliers = _db.Suppliers.Where(s => !s.IsDeleted && s.IsActive)
                .Select(s => new { s.Id, s.Name }).ToList();
            return Json(suppliers, JsonRequestBehavior.AllowGet);
        }

        // 4. AJAX to search products
        [HttpGet]
        public JsonResult SearchProduct(string term)
        {
            var products = _db.Products.Where(p => p.IsActive && !p.IsDeleted && (p.Name.Contains(term) || p.Barcode == term))
                .Select(p => new { p.Id, p.Name, p.PurchaseRate }).Take(10).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        // 5. Save the Purchase (Updates Stock)
        [HttpPost]
        public JsonResult SavePurchase(int supplierId, decimal grandTotal, List<PurchaseItemDto> items)
        {
            try
            {
                if (items == null || items.Count == 0) return Json(new { success = false, message = "Cart is empty." });

                // Create Invoice
                var invoice = new PurchaseInvoice
                {
                    InvoiceNo = "PUR-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    SupplierId = supplierId,
                    GrandTotal = grandTotal,
                    CreatedBy = User.Identity.Name
                };
                _db.PurchaseInvoices.Add(invoice);
                _db.SaveChanges(); // Save to get the Invoice ID

                var inventoryService = new InventoryService(_db);

                // Add Items & Update Inventory
                foreach (var item in items)
                {
                    _db.PurchaseInvoiceItems.Add(new PurchaseInvoiceItem
                    {
                        PurchaseInvoiceId = invoice.Id,
                        ProductId = item.Id,
                        Quantity = item.Qty,
                        Rate = item.Rate,
                        Total = item.Total
                    });

                    // INCREASE STOCK (+ QtyIn)
                    inventoryService.RecordIn(item.Id, 1, item.Qty, "PURCHASE", invoice.Id, "Purchase Entry");
                }
                _db.SaveChanges();

                return Json(new { success = true, message = "Purchase Saved! Invoice: " + invoice.InvoiceNo });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

    // DTO for JSON Deserialization
    public class PurchaseItemDto
    {
        public int Id { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal Total { get; set; }
    }
}