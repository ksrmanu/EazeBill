using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StoreBilling.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [Display(Name = "Product Code")]
        public string Code { get; set; }

        [Display(Name = "Barcode")]
        public string Barcode { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        [Display(Name = "Brand")]
        public int? BrandId { get; set; }

        [Display(Name = "Unit")]
        public int? UnitId { get; set; }

        [Display(Name = "Tax")]
        public int? TaxId { get; set; }

        [Display(Name = "Purchase Rate")]
        public decimal PurchaseRate { get; set; }

        [Display(Name = "Selling Rate")]
        public decimal SellingRate { get; set; }

        [Display(Name = "MRP")]
        public decimal MRP { get; set; }

        [Display(Name = "Min Stock Alert")]
        public decimal MinStock { get; set; }

        [Display(Name = "Opening Stock")]
        public decimal OpeningStock { get; set; }

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; }

        // Dropdown lists for the Form
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Brands { get; set; }
        public IEnumerable<SelectListItem> Units { get; set; }
        public IEnumerable<SelectListItem> Taxes { get; set; }
    }
}