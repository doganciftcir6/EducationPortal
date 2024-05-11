using EducationPortalApp.Entities.EnrollmentEntities;
using Microsoft.AspNetCore.Identity;

namespace EducationPortalApp.Entities.UserEntities
{
    public class AppUser : IdentityUser<int>
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }

        public int GenderId { get; set; }
        public Gender Gender { get; set; }

        public List<Enrollment> Enrollments { get; set; }
        public List<EnrollmentRequest> EnrollmentRequests { get; set; }
    }
}
