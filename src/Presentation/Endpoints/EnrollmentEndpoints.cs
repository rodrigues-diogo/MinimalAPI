using AutoMapper;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs;

namespace Presentation.Endpoints;

public static class EnrollmentEndpoints
{
    public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("enrollment").WithTags(nameof(Enrollment));

        group.MapGet("/", async (StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var enrollments = await db.Enrollments.ToListAsync();
            return mapper.Map<List<EnrollmentDto>>(enrollments);
        })
        .WithName("GetAllEnrollments")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<EnrollmentDto>, NotFound>> (Guid id, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var enrollment = await db.Enrollments.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id);

            if (enrollment is null)
                return TypedResults.NotFound();

            return TypedResults.Ok(mapper.Map<EnrollmentDto>(enrollment));
        })
        .WithName("GetEnrollmentById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, Enrollment enrollment, StudentEnrollmentDbContext db) =>
        {
            var affected = await db.Enrollments
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.CourseId, enrollment.CourseId)
                    .SetProperty(m => m.StudentId, enrollment.StudentId)
                    .SetProperty(m => m.Id, enrollment.Id)
                    .SetProperty(m => m.ModifiedBy, "User")
                    .SetProperty(m => m.ModifiedAt, DateTime.UtcNow)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateEnrollment")
        .WithOpenApi();

        group.MapPost("/", async (EnrollmentDto enrollment, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var data = mapper.Map<Enrollment>(enrollment);
            data.CreatedBy = "User";
            data.CreatedAt = DateTime.UtcNow;

            db.Enrollments.Add(data);
            await db.SaveChangesAsync();

            return TypedResults.Created($"enrollment/{enrollment.Id}", enrollment);
        })
        .WithName("CreateEnrollment")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, StudentEnrollmentDbContext db) =>
        {
            var affected = await db.Enrollments
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteEnrollment")
        .WithOpenApi();
    }
}
