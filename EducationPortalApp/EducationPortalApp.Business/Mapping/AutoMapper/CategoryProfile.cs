using AutoMapper;
using EducationPortalApp.Dtos.CategoryDtos;
using EducationPortalApp.Entities;

namespace EducationPortalApp.Business.Mapping.AutoMapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();
        }
    }
}
