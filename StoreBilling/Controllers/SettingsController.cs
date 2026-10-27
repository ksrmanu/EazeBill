using System;
using System.Linq;
using System.Web.Mvc;
using StoreBilling.Data;
using StoreBilling.Data.Entities;
using StoreBilling.Models.ViewModels;

namespace StoreBilling.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly StoreDbContext _db = new StoreDbContext();

        // GET: /Settings
        public ActionResult Index()
        {
            // Load current settings from DB into the ViewModel
            var model = new SettingsViewModel
            {
                StoreName = GetSetting("StoreName", "SwiftBill Retail"),
                StoreAddress = GetSetting("StoreAddress", "123 Market Road, City"),
                StorePhone = GetSetting("StorePhone", "9876543210"),
                StoreGST = GetSetting("StoreGST", "29ABCDE1234F1Z5"),

                InvoicePrefix = GetSetting("InvoicePrefix", "INV-"),
                AllowNegativeStock = GetSetting("AllowNegativeStock", "false") == "true"
            };

            return View(model);
        }

        // POST: /Settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SettingsViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Save settings to DB
            SaveSetting("StoreName", model.StoreName, "Store");
            SaveSetting("StoreAddress", model.StoreAddress, "Store");
            SaveSetting("StorePhone", model.StorePhone, "Store");
            SaveSetting("StoreGST", model.StoreGST, "Store");

            SaveSetting("InvoicePrefix", model.InvoicePrefix, "Invoice");
            SaveSetting("AllowNegativeStock", model.AllowNegativeStock.ToString().ToLower(), "Inventory");

            TempData["Success"] = "Settings saved successfully!";
            return RedirectToAction("Index");
        }

        // --- Helper Methods ---
        private string GetSetting(string key, string defaultValue)
        {
            var setting = _db.Settings.FirstOrDefault(s => s.Key == key);
            return setting != null ? setting.Value : defaultValue;
        }

        private void SaveSetting(string key, string value, string group)
        {
            var setting = _db.Settings.FirstOrDefault(s => s.Key == key);
            if (setting != null)
            {
                setting.Value = value;
            }
            else
            {
                _db.Settings.Add(new Setting { Key = key, Value = value, Group = group });
            }
            _db.SaveChanges();
        }
    }
}