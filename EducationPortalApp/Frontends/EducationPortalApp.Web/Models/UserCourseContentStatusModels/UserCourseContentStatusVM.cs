namespace EducationPortalApp.Web.Models.UserCourseContentStatusModels
{
    public class UserCourseContentStatusVM
    {
        public int Id { get; set; }

        public int AppUserId { get; set; }

        public int CourseContentId { get; set; }

        public bool IsCompleted { get; set; }
    }
}
