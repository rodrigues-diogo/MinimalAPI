﻿using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints;

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

            app.MapStudentEndpoints();
            app.MapCourseEndpoints();
            app.MapEnrollmentEndpoints();

            app.Run();
        }
    }
}
