using StoreBilling.Data;
using StoreBilling.Data.Entities;
using StoreBilling.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StoreBilling.Controllers
{
    public class SalesItemDto
    {
        public int Id { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
    }
    public class POSController : Controller
    {
        private readonly StoreDbContext _db = new StoreDbContext();
        public ActionResult Index() { return View(); }
            public class SalesItemDto
    {
        public int Id { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
    }

        [HttpGet]
        public JsonResult SearchProduct(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);

            var products = _db.Products
                .Where(p => p.IsActive && !p.IsDeleted && (p.Name.Contains(term) || p.Barcode == term))
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.SellingRate,
                    p.Barcode,
                    Stock = _db.InventoryTransactions
                        .Where(t => t.ProductId == p.Id)
                        .Sum(t => (decimal?)(t.QtyIn - t.QtyOut)) ?? 0m
                })
                .Take(10) // Only return top 10 matches for performance
                .ToList();

            return Json(products, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveSale(string customerName, decimal subTotal, decimal taxAmount, decimal grandTotal, string paymentMethod, string cartJson)
        {
            try
            {
                // 1. Save Invoice Header
                var invoice = new SalesInvoice
                {
                    InvoiceNo = "INV-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    CustomerName = string.IsNullOrEmpty(customerName) ? "Walk-in" : customerName,
                    SubTotal = subTotal,
                    TaxAmount = taxAmount,
                    GrandTotal = grandTotal,
                    PaymentMethod = paymentMethod,
                    Status = "Paid",
                    CreatedBy = User.Identity.Name
                };
                _db.SalesInvoices.Add(invoice);
                _db.SaveChanges(); // Save to get the Invoice ID

                // 2. Save Receipt for Accounts
                var receipt = new Receipt
                {
                    ReceiptNo = "RCP-" + DateTime.Now.Ticks.ToString().Substring(10),
                    ReceiptDate = DateTime.Now,
                    CustomerName = string.IsNullOrEmpty(customerName) ? "Walk-in Customer" : customerName,
                    Amount = grandTotal,
                    PaymentMode = paymentMethod,
                    Remarks = "POS Sale: " + invoice.InvoiceNo,
                    CreatedBy = User.Identity.Name
                };
                _db.Receipts.Add(receipt);

                // 3. Save Invoice Items & Deduct Inventory
                var cartItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SalesItemDto>>(cartJson);
                var inventoryService = new InventoryService(_db);

                foreach (var item in cartItems)
                {
                    // Save item to database
                    _db.SalesInvoiceItems.Add(new SalesInvoiceItem
                    {
                        InvoiceId = invoice.Id,
                        ProductId = item.Id,
                        Quantity = item.Qty,
                        Rate = item.Price,
                        Total = item.Qty * item.Price,
                        CreatedBy = User.Identity.Name
                    });

                    // Deduct Stock from Inventory
                    inventoryService.RecordOut(item.Id, 1, item.Qty, "SALE", invoice.Id, "POS Sale: " + invoice.InvoiceNo);
                }

                _db.SaveChanges(); // Save everything

                return Json(new { success = true, invoiceNo = invoice.InvoiceNo });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMsg = ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null) errorMsg = ex.InnerException.InnerException.Message;
                }
                return Json(new { success = false, message = errorMsg });
            }
        }
    }
}