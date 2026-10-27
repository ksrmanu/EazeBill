using System.Linq;
using System.Web.Mvc;
using StoreBilling.Data;
using StoreBilling.Data.Entities;

namespace StoreBilling.Controllers
{
    public class CustomersController : Controller
    {
        private readonly StoreDbContext _db = new StoreDbContext();
        public ActionResult Index() { return View(); }

        [HttpGet]
        public JsonResult GetList()
        {
            var data = _db.Customers.Where(c => !c.IsDeleted).Select(c => new { c.Id, c.Code, c.Name, c.Mobile }).ToList();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create() { return View("Form", new Customer()); }
        [HttpPost]
        public ActionResult Create(Customer model)
        {
            if (ModelState.IsValid) { _db.Customers.Add(model); _db.SaveChanges(); return RedirectToAction("Index"); }
            return View("Form", model);
        }
    }
}