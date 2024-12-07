using AutoMapper;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs;

namespace Presentation.Endpoints;

public static class CourseEndpoints
{
    public static void MapCourseEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("course").WithTags(nameof(Course));

        group.MapGet("/", async (StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var courses = await db.Courses.ToListAsync();

            return mapper.Map<List<CourseDto>>(courses);
        })
        .WithName("GetAllCourses")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<CourseDto>, NotFound>> (Guid id, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var result = await db.Courses.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id);

            if (result == null)
                TypedResults.NotFound();

            return TypedResults.Ok(mapper.Map<CourseDto>(result));
        })
        .WithName("GetCourseById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, CourseDto course, StudentEnrollmentDbContext db) =>
        {
            var affected = await db.Courses
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Title, course.Title)
                    .SetProperty(m => m.Credits, course.Credits)
                    .SetProperty(m => m.ModifiedBy, "User")
                    .SetProperty(m => m.ModifiedAt, DateTime.UtcNow)
                    );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCourse")
        .WithOpenApi();

        group.MapPost("/", async (CourseDto course, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var data = mapper.Map<Course>(course);
            data.CreatedBy = "User";
            data.CreatedAt = DateTime.UtcNow;

            db.Courses.Add(data);
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
