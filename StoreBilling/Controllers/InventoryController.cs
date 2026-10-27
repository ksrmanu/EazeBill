using System.Linq;
using System.Web.Mvc;
using StoreBilling.Data;

namespace StoreBilling.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private readonly StoreDbContext _db = new StoreDbContext();

        // GET: /Inventory
        public ActionResult Index()
        {
            return View();
        }

        // AJAX Endpoint for the DataTable
        [HttpGet]
        public JsonResult GetStockList()
        {
            // Calculate current stock for each product using the InventoryTransactions table
            var query = _db.Products
                .Where(p => !p.IsDeleted && p.IsActive)
                .Select(p => new
                {
                    p.Id,
                    p.Code,
                    p.Name,
                    p.SellingRate,
                    p.MRP,
                    MinStock = p.MinStock,
                    CurrentStock = _db.InventoryTransactions
                        .Where(t => t.ProductId == p.Id)
                        .Sum(t => (decimal?)(t.QtyIn - t.QtyOut)) ?? 0m
                }).ToList(); // Execute query to memory so we can calculate Status below

            var data = query.Select(p => new
            {
                p.Id,
                p.Code,
                p.Name,
                p.SellingRate,
                p.MRP,
                p.CurrentStock,
                Status = p.CurrentStock <= 0 ? "Out of Stock" : (p.CurrentStock <= p.MinStock ? "Low Stock" : "In Stock")
            }).ToList();

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}