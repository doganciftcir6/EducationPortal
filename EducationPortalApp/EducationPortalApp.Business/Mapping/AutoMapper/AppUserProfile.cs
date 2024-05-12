using AutoMapper;
using EducationPortalApp.Dtos.AppUserDtos;
using EducationPortalApp.Entities.UserEntities;

namespace EducationPortalApp.Business.Mapping.AutoMapper
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            CreateMap<AppUser, AppUserRegisterDto>().ReverseMap();
            CreateMap<AppUser, AppUserLoginDto>().ReverseMap();
            CreateMap<AppUser, AppUserDto>().ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender != null ? src.Gender.Definition : null)).ReverseMap();
        }
    }
}
