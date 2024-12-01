using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connection = builder.Configuration.GetConnectionString("StudentEnrollmentDbConnection");
            builder.Services.AddDbContext<StudentEnrollmentDbContext>(options =>
            {
                options.UseSqlServer(connection);
            });

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/courses", async (StudentEnrollmentDbContext context) =>
            {
                return await context.Courses.ToListAsync();
            })
            .WithName("GetCourses")
            .WithOpenApi();

            app.MapGet("/courses/{id}", async (StudentEnrollmentDbContext context, Guid id) =>
            {
                return await context.Courses.FindAsync(id)
                    is Course course
                        ? Results.Ok(course)
                        : Results.NotFound();
            })
            .WithName("GetCourseById")
            .WithOpenApi();

            app.MapPost("/courses", async (StudentEnrollmentDbContext context, Course course) =>
            {
                course.CreatedAt = DateTime.Now;
                await context.Courses.AddAsync(course);
                await context.SaveChangesAsync();

                return Results.Created($"/courses/{course.Id}", course);
            })
            .WithName("CreateCourse")
            .WithOpenApi();

            app.MapPut("/courses/{id}", async (StudentEnrollmentDbContext context, Course course, Guid id) =>
            {
                var record = await context.Courses.FindAsync(id);

                if (record is null) return Results.NotFound();

                record.Title = course.Title;
                record.Credits = course.Credits;
                record.ModifiedBy = course.ModifiedBy;
                record.ModifiedAt = DateTime.UtcNow;

                if (context.ChangeTracker.HasChanges())
                    Console.WriteLine("Course has been modified");

                await context.SaveChangesAsync();

                return Results.NoContent();
            })
            .WithName("UpdateCourse")
            .WithOpenApi();

            app.MapDelete("/courses/{id}", async (StudentEnrollmentDbContext context, Guid id) =>
            {
                if (await context.Courses.FindAsync(id) is Course course)
                {
                    context.Courses.Remove(course);
                    await context.SaveChangesAsync();

                    return Results.NoContent();
                }

                return Results.NotFound();
            })
            .WithName("DeleteCourse")
            .WithOpenApi();

            app.Run();
        }
    }
}
