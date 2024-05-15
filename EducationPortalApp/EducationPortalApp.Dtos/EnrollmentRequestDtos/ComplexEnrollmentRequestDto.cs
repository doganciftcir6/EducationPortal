using EducationPortalApp.Dtos.AppUserDtos;
using EducationPortalApp.Dtos.CourseDtos;

namespace EducationPortalApp.Dtos.EnrollmentRequestDtos
{
    public class ComplexEnrollmentRequestDto
    {
        public int Id { get; set; }

        public AppUserDtoForEnrollmentRQ? AppUser { get; set; }
        public CourseDtoForEnrollmentRQ? Course { get; set; }
        public string? EnrollmentRequestStatus { get; set; }
    }
}
