using AutoMapper;
using EducationPortalApp.Dtos.EnrollmentRequestStatusDtos;
using EducationPortalApp.Entities.EnrollmentEntities;

namespace EducationPortalApp.Business.Mapping.AutoMapper
{
    public class EnrollmentRequestStatusProfile : Profile
    {
        public EnrollmentRequestStatusProfile()
        {
            CreateMap<EnrollmentRequestStatus, EnrollmentRequestStatusDto>().ReverseMap();
        }
    }
}
