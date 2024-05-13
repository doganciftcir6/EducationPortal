using AutoMapper;
using EducationPortalApp.Dtos.CourseContentDtos;
using EducationPortalApp.Entities.CourseEntities;

namespace EducationPortalApp.Business.Mapping.AutoMapper
{
    public class CourseContentProfile : Profile
    {
        public CourseContentProfile()
        {
            CreateMap<CourseContent, CourseContentDto>().ForMember(dest => dest.CourseContentType, opt => opt.MapFrom(src => src.CourseContentType != null ? src.CourseContentType.Definition : null)).ReverseMap();
            CreateMap<CourseContent, CourseContentCreateDto>().ReverseMap();
            CreateMap<CourseContent, CourseContentUpdateDto>().ReverseMap();
        }
    }
}
