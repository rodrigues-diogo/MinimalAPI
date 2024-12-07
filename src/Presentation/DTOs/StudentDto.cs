namespace Presentation.DTOs
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string IdNumber { get; set; }
        public string Picture { get; set; }
    }
}
