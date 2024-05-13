using EducationPortalApp.Dtos.CourseDtos;

namespace EducationPortalApp.Dtos.CourseContentDtos
{
    public class CourseContentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }

        public int CourseId { get; set; }
        public CourseDto CourseDto { get; set; }

        public string? CourseContentType { get; set; }
    }
}
