﻿using AutoMapper;
using EducationPortalApp.Dtos.EnrollmentDtos;
using EducationPortalApp.Entities.EnrollmentEntities;

namespace EducationPortalApp.Business.Mapping.AutoMapper
{
    public class EnrollmentProfile : Profile
    {
        public EnrollmentProfile()
        {
            CreateMap<Enrollment, EnrollmentDto>().ForMember(dest => dest.AppUser, opt => opt.MapFrom(src => src.AppUser != null ? $"{src.AppUser.Firstname} {src.AppUser.Lastname}".Trim() : null)).ReverseMap();
            CreateMap<Enrollment, EnrollmentCreateDto>().ReverseMap();
            CreateMap<Enrollment, EnrollmentUpdateDto>().ReverseMap();
        }
    }
}
