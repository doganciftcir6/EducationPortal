using EducationPortalApp.Web.Models.CourseModels;

namespace EducationPortalApp.Web.Models.CourseContentModels
{
    public class CourseContentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }

        public CourseVM Course { get; set; }

        public string? CourseContentType { get; set; }
    }
}
