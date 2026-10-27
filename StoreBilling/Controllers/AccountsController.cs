using System;
using System.Linq;
using System.Web.Mvc;
using StoreBilling.Data;
using StoreBilling.Data.Entities;
using StoreBilling.Models.ViewModels;

namespace StoreBilling.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly StoreDbContext _db = new StoreDbContext();

        public ActionResult Index()
        {
            // Get today's date
            var today = DateTime.Today;

            // Build the Accounts Dashboard ViewModel
            var model = new AccountsViewModel
            {
                TotalReceipts = _db.Receipts.Where(r => !r.IsDeleted).Sum(r => (decimal?)r.Amount) ?? 0,
                TotalPayments = _db.Payments.Where(p => !p.IsDeleted).Sum(p => (decimal?)p.Amount) ?? 0,
                TotalExpenses = _db.Expenses.Where(e => !e.IsDeleted).Sum(e => (decimal?)e.Amount) ?? 0,

                // Cash Book entries (combining all three chronologically)
                Transactions = _db.Receipts.Where(r => !r.IsDeleted).Select(r => new TransactionDto { Date = r.ReceiptDate, Description = "Receipt from " + r.CustomerName, Type = "Receipt", Amount = r.Amount })
                    .Concat(_db.Payments.Where(p => !p.IsDeleted).Select(p => new TransactionDto { Date = p.PaymentDate, Description = "Payment to " + p.SupplierName, Type = "Payment", Amount = -p.Amount }))
                    .Concat(_db.Expenses.Where(e => !e.IsDeleted).Select(e => new TransactionDto { Date = e.ExpenseDate, Description = e.Description, Type = "Expense", Amount = -e.Amount }))
                    .OrderByDescending(t => t.Date).Take(50).ToList()
            };

            return View(model);
        }

        // --- RECEIPTS ---
        [HttpPost]
        public JsonResult AddReceipt(Receipt model)
        {
            if (ModelState.IsValid)
            {
                model.ReceiptNo = "RCP-" + DateTime.Now.Ticks.ToString().Substring(10);
                model.CreatedBy = User.Identity.Name;
                _db.Receipts.Add(model);
                _db.SaveChanges();
                return Json(new { success = true, message = "Receipt saved successfully!" });
            }
            return Json(new { success = false, message = "Invalid data." });
        }

        // --- PAYMENTS ---
        [HttpPost]
        public JsonResult AddPayment(Payment model)
        {
            if (ModelState.IsValid)
            {
                model.PaymentNo = "PAY-" + DateTime.Now.Ticks.ToString().Substring(10);
                model.CreatedBy = User.Identity.Name;
                _db.Payments.Add(model);
                _db.SaveChanges();
                return Json(new { success = true, message = "Payment saved successfully!" });
            }
            return Json(new { success = false, message = "Invalid data." });
        }

        // --- EXPENSES ---
        [HttpPost]
        public JsonResult AddExpense(Expense model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = User.Identity.Name;
                _db.Expenses.Add(model);
                _db.SaveChanges();
                return Json(new { success = true, message = "Expense saved successfully!" });
            }
            return Json(new { success = false, message = "Invalid data." });
        }
    }
}