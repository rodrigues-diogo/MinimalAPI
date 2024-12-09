namespace Presentation.DTOs
{
    public class EnrollmentDto
    {
        public Guid Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public virtual CourseDto Course { get; set; }
        public virtual StudentDto Student { get; set; }
    }
}
