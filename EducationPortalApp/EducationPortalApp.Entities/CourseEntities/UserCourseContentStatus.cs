using EducationPortalApp.Entities.UserEntities;
using System.ComponentModel.DataAnnotations;

namespace EducationPortalApp.Entities.CourseEntities
{
    public class UserCourseContentStatus
    {
        [Key]
        public int Id { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int CourseContentId { get; set; }
        public CourseContent CourseContent { get; set; }

        public bool IsCompleted { get; set; }
    }
}
