﻿using Infrastructure;
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

        group.MapGet("/", async (StudentEnrollmentDbContext db) =>
        {
            List<CourseDto> data = [];
            var courses = await db.Courses.ToListAsync();

            foreach (var course in courses)
            {
                data.Add(
                    new CourseDto()
                    {
                        Id = course.Id,
                        Title = course.Title,
                        Credits = course.Credits,
                    }
                );
            };

            return data;
        })
        .WithName("GetAllCourses")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<CourseDto>, NotFound>> (Guid id, StudentEnrollmentDbContext db) =>
        {
            var result = await db.Courses.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id);

            if(result == null)
                TypedResults.NotFound();

            var data = new CourseDto()
            {
                Id = result.Id,
                Title = result.Title,
                Credits = result.Credits,
            };

            return TypedResults.Ok(data);
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
                    .SetProperty(m => m.Id, course.Id)
                    .SetProperty(m => m.ModifiedBy, "User")
                    .SetProperty(m => m.ModifiedAt, DateTime.UtcNow)
                    );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCourse")
        .WithOpenApi();

        group.MapPost("/", async (CourseDto course, StudentEnrollmentDbContext db) =>
        {
            var data = new Course()
            {
                Title = course.Title,
                Credits = course.Credits,
                CreatedBy = "User",
                CreatedAt = DateTime.UtcNow
            };

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
