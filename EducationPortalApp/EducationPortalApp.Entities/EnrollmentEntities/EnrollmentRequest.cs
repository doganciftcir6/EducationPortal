using EducationPortalApp.Entities.CourseEntities;
using EducationPortalApp.Entities.UserEntities;
using System.ComponentModel.DataAnnotations;

namespace EducationPortalApp.Entities.EnrollmentEntities
{
    public class EnrollmentRequest
    {
        [Key]
        public int Id { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string Status { get; set; } 
    }
}
