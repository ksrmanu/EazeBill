namespace StoreBilling.Data.Entities
{
    public class Tax : BaseEntity // <-- Added 'public' and ': BaseEntity'
    {
        public string Name { get; set; }
        public string HSNCode { get; set; }
        public decimal Rate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}