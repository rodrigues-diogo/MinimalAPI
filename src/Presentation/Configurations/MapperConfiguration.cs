using AutoMapper;
using Infrastructure.Entities;
using Presentation.DTOs;

namespace Presentation.Configurations
{
    public class MapperConfiguration : Profile
    {
        public MapperConfiguration()
        {
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Course, CreateCourseDto>().ReverseMap();

            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Student, CreateStudentDto>().ReverseMap();

            CreateMap<Enrollment, EnrollmentDto>().ReverseMap();
            CreateMap<Enrollment, CreateEnrollmentDto>().ReverseMap();
        }
    }
}
