namespace Infrastructure.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedAt { get; set; }
    }
}
