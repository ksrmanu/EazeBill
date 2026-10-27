using StoreBilling.Data;
using StoreBilling.Data.Entities;
using StoreBilling.Services;
using StoreBilling.Services.Implementations;
using System;
using System.Linq;
using System.Web.Mvc;

namespace StoreBilling.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StoreDbContext _db = new StoreDbContext();

        public ActionResult Index() { return View(); }

        [HttpGet]
        public JsonResult GetList(string search = "")
        {
            var q = _db.Products.Where(p => !p.IsDeleted);
            if (!string.IsNullOrEmpty(search)) q = q.Where(p => p.Name.Contains(search) || p.Code.Contains(search));
            var data = q.Select(p => new { p.Id, p.Code, p.Name, p.SellingRate, p.MRP, p.IsActive }).OrderByDescending(p => p.Id).ToList();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create() { return View("Form", new Product { IsActive = true }); }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Product model)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(model);
                _db.SaveChanges();
                if (model.OpeningStock > 0)
                {
                    new InventoryService(_db).RecordIn(model.Id, 1, model.OpeningStock, "OPENING", model.Id);
                    _db.SaveChanges();
                }
                TempData["Success"] = "Product saved!";
                return RedirectToAction("Index");
            }
            return View("Form", model);
        }

        public ActionResult Edit(int id)
        {
            var p = _db.Products.Find(id);
            if (p == null) return HttpNotFound();
            return View("Form", p);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                var p = _db.Products.Find(model.Id);
                p.Code = model.Code; p.Name = model.Name; p.SellingRate = model.SellingRate; p.MRP = model.MRP; p.IsActive = model.IsActive;
                _db.SaveChanges();
                TempData["Success"] = "Product updated!";
                return RedirectToAction("Index");
            }
            return View("Form", model);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var p = _db.Products.Find(id);
            if (p != null) { p.IsDeleted = true; _db.SaveChanges(); }
            return Json(new { success = true });
        }
    }
}