using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Entities.Configurations
{
    internal class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasData(
                new Course()
                {
                    Id = new Guid("4fb28f72-0e7b-4cc7-80d7-33e525d35f9b"),
                    Title = "Title",
                    Credits = 1
                },
                new Course()
                {
                    Id = new Guid("24e60f17-901d-4dab-b152-ea3ecaa4a0ef"),
                    Title = "Title",
                    Credits = 2
                }
            );
        }
    }
}
