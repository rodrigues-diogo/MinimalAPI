using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Endpoints;

public static class CourseEndpoints
{
    public static void MapCourseEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("course").WithTags(nameof(Course));

        group.MapGet("/", async (StudentEnrollmentDbContext db) =>
        {
            return await db.Courses.ToListAsync();
        })
        .WithName("GetAllCourses")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Course>, NotFound>> (Guid id, StudentEnrollmentDbContext db) =>
        {
            return await db.Courses.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Course model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCourseById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, Course course, StudentEnrollmentDbContext db) =>
        {
            var affected = await db.Courses
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Title, course.Title)
                    .SetProperty(m => m.Credits, course.Credits)
                    .SetProperty(m => m.Id, course.Id)
                    .SetProperty(m => m.CreatedBy, course.CreatedBy)
                    .SetProperty(m => m.CreatedAt, course.CreatedAt)
                    .SetProperty(m => m.ModifiedBy, course.ModifiedBy)
                    .SetProperty(m => m.ModifiedAt, course.ModifiedAt)
                    );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCourse")
        .WithOpenApi();

        group.MapPost("/", async (Course course, StudentEnrollmentDbContext db) =>
        {
            db.Courses.Add(course);
            await db.SaveChangesAsync();

            return TypedResults.Created($"course/{course.Id}", course);
        })
        .WithName("CreateCourse")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, StudentEnrollmentDbContext db) =>
        {
            var affected = await db.Courses
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCourse")
        .WithOpenApi();
    }
}
