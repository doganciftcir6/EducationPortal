using AutoMapper;
using EducationPortalApp.Dtos.EnrollmentDtos;
using EducationPortalApp.Dtos.EnrollmentRequestDtos;
using EducationPortalApp.Entities.EnrollmentEntities;

namespace EducationPortalApp.Business.Mapping.AutoMapper
{
    public class EnrollmentRequestProfile : Profile
    {
        public EnrollmentRequestProfile()
        {
            CreateMap<EnrollmentRequest, EnrollmentRequestDto>().ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course != null ? src.Course.Name : null)).ForMember(dest => dest.EnrollmentRequestStatus, opt => opt.MapFrom(src => src.EnrollmentRequestStatus != null ? src.EnrollmentRequestStatus.Definition : null)).ForMember(dest => dest.AppUser, opt => opt.MapFrom(src => src.AppUser != null ? $"{src.AppUser.Firstname} {src.AppUser.Lastname}".Trim() : null)).ReverseMap();
            CreateMap<EnrollmentRequest, ComplexEnrollmentRequestDto>().ForMember(dest => dest.EnrollmentRequestStatus, opt => opt.MapFrom(src => src.EnrollmentRequestStatus != null ? src.EnrollmentRequestStatus.Definition : null)).ReverseMap();
            CreateMap<EnrollmentRequest, EnrollmentRequestCreateDto>().ReverseMap();
            CreateMap<EnrollmentRequest, EnrollmentRequestUpdateDto>().ReverseMap();
        }
    }
}
