using EducationPortalApp.Web.Models.CourseModels;

namespace EducationPortalApp.Web.Models.EnrollmentModels
{
    public class EnrollmentVM
    {
        public int Id { get; set; }

        public string? AppUser { get; set; }
        public CourseVM? Course { get; set; }
        public bool IsCompleted { get; set; }
    }
}
