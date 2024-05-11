using EducationPortalApp.Entities.CourseEntities;
using System.ComponentModel.DataAnnotations;

namespace EducationPortalApp.Entities.EnrollmentEntities
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public bool IsCompleted { get; set; }
    }
}
