using EducationPortalApp.Web.Models.EnrollmentModels;

namespace EducationPortalApp.Web.Models.AppUserModels
{
    public class AppUserProfileVM
    {
        public AppUserVM User { get; set; }
        public List<EnrollmentVM> Enrollments { get; set; }
    }
}
