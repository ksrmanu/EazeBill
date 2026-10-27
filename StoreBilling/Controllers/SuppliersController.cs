using System.Linq;
using System.Web.Mvc;
using StoreBilling.Data;
using StoreBilling.Data.Entities;

namespace StoreBilling.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly StoreDbContext _db = new StoreDbContext();
        public ActionResult Index() { return View(); }

        [HttpGet]
        public JsonResult GetList()
        {
            var data = _db.Suppliers.Where(s => !s.IsDeleted).Select(s => new { s.Id, s.Code, s.Name, s.ContactPerson, s.Mobile }).ToList();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create() { return View("Form", new Supplier()); }
        [HttpPost]
        public ActionResult Create(Supplier model)
        {
            if (ModelState.IsValid) { _db.Suppliers.Add(model); _db.SaveChanges(); return RedirectToAction("Index"); }
            return View("Form", model);
        }
    }
}