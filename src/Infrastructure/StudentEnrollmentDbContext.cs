using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class StudentEnrollmentDbContext : IdentityDbContext
    {
        public StudentEnrollmentDbContext(DbContextOptions<StudentEnrollmentDbContext> options) : base(options)
        {

        }
    }
}
