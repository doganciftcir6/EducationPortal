using EducationPortalApp.Dtos.CourseDtos;

namespace EducationPortalApp.Dtos.CourseContentDtos
{
    public class CourseContentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public bool Status { get; set; }

        public CourseDto Course { get; set; }

        public string? CourseContentType { get; set; }
    }
}
