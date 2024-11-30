namespace Infrastructure.Entities
{
    public class Student : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string IdNumber { get; set; }
        public string Picture { get; set; }
    }
}
