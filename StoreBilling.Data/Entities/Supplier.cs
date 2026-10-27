namespace StoreBilling.Data.Entities
{
    public class Supplier : BaseEntity // <-- Added 'public' and ': BaseEntity'
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
    }
}