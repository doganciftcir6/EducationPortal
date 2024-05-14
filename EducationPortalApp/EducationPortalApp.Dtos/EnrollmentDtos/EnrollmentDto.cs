using EducationPortalApp.Dtos.CourseDtos;

namespace EducationPortalApp.Dtos.EnrollmentDtos
{
    public class EnrollmentDto
    {
        public int Id { get; set; }

        public string? AppUser { get; set; }
        public CourseDto? Course { get; set; }
        public bool IsCompleted { get; set; }
    }
}
