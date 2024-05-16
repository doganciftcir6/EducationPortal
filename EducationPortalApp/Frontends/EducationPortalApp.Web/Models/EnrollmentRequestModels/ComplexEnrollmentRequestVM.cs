using EducationPortalApp.Web.Models.AppUserModels;
using EducationPortalApp.Web.Models.CourseModels;

namespace EducationPortalApp.Web.Models.EnrollmentRequestModels
{
    public class ComplexEnrollmentRequestVM
    {
        public int Id { get; set; }

        public AppUserVMForEnrollmentRQ? AppUser { get; set; }
        public CourseVMForEnrollmentRQ? Course { get; set; }
        public string? EnrollmentRequestStatus { get; set; }
    }
}
