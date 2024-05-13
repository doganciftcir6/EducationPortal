using NSwag.Annotations;

namespace EducationPortalApp.Dtos.EnrollmentRequestDtos
{
    public class EnrollmentRequestUpdateDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int EnrollmentRequestStatusId { get; set; }
    }
}
