namespace StoreBilling.Data.Entities
{
    public class Setting : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
    }
}