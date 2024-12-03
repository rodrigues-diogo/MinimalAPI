using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Endpoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("student").WithTags(nameof(Student));

        group.MapGet("/", async (StudentEnrollmentDbContext db) =>
        {
            return await db.Students.ToListAsync();
        })
        .WithName("GetAllStudents")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Student>, NotFound>> (Guid id, StudentEnrollmentDbContext db) =>
        {
            return await db.Students.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Student model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetStudentById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, Student student, StudentEnrollmentDbContext db) =>
        {
            var affected = await db.Students
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.FirstName, student.FirstName)
                    .SetProperty(m => m.LastName, student.LastName)
                    .SetProperty(m => m.BirthDate, student.BirthDate)
                    .SetProperty(m => m.IdNumber, student.IdNumber)
                    .SetProperty(m => m.Picture, student.Picture)
                    .SetProperty(m => m.Id, student.Id)
                    .SetProperty(m => m.CreatedBy, student.CreatedBy)
                    .SetProperty(m => m.CreatedAt, student.CreatedAt)
                    .SetProperty(m => m.ModifiedBy, student.ModifiedBy)
                    .SetProperty(m => m.ModifiedAt, student.ModifiedAt)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateStudent")
        .WithOpenApi();

        group.MapPost("/", async (Student student, StudentEnrollmentDbContext db) =>
        {
            db.Students.Add(student);
            await db.SaveChangesAsync();

            return TypedResults.Created($"student/{student.Id}", student);
        })
        .WithName("CreateStudent")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, StudentEnrollmentDbContext db) =>
        {
            var affected = await db.Students
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteStudent")
        .WithOpenApi();
    }
}
