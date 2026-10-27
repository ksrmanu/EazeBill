namespace StoreBilling.Data.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}