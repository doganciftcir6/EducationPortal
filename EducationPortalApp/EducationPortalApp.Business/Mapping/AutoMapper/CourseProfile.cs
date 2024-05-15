using AutoMapper;
using EducationPortalApp.Dtos.CourseDtos;
using EducationPortalApp.Entities.CourseEntities;

namespace EducationPortalApp.Business.Mapping.AutoMapper
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>().ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null)).ReverseMap();
            CreateMap<Course, CoursesDto>().ReverseMap();
            CreateMap<Course, CourseCreateDto>().ReverseMap();
            CreateMap<Course, CourseUpdateDto>().ReverseMap();
            CreateMap<Course, CourseDtoForEnrollmentRQ>().ReverseMap();
        }
    }
}
