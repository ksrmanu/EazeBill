namespace StoreBilling.Models.ViewModels
{
    public class SettingsViewModel
    {
        // Store Profile
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StorePhone { get; set; }
        public string StoreGST { get; set; }

        // Invoice Settings
        public string InvoicePrefix { get; set; }
        public bool AllowNegativeStock { get; set; }
    }
}