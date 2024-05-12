using AutoMapper;
using EducationPortalApp.Dtos.GenderDtos;
using EducationPortalApp.Entities.UserEntities;

namespace EducationPortalApp.Business.Mapping.AutoMapper
{
    public class GenderProfile : Profile
    {
        public GenderProfile()
        {
            CreateMap<Gender, GenderDto>().ReverseMap();
        }
    }
}
