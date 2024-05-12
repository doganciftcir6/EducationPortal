using AutoMapper;
using EducationPortalApp.Dtos.CourseContentTypeDtos;
using EducationPortalApp.Entities.CourseEntities;

namespace EducationPortalApp.Business.Mapping.AutoMapper
{
    public class CourseContentTypeProfile : Profile
    {
        public CourseContentTypeProfile()
        {
            CreateMap<CourseContentType, CourseContentTypeDto>().ReverseMap();
        }
    }
}
