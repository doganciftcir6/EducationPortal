using EducationPortalApp.Dtos.EnrollmentDtos;

namespace EducationPortalApp.Dtos.AppUserDtos
{
    public class ProfileDto
    {
        public AppUserDto User { get; set; }
        public List<EnrollmentDto> Enrollments { get; set; }
    }
}
