namespace StoreBilling.Data.Entities
{
    public class Store : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string GSTNumber { get; set; }
        public bool IsActive { get; set; } = true;
    }
}