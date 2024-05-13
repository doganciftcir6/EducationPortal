namespace EducationPortalApp.Dtos.CourseContentDtos
{
    public class CourseContentCreateDto
    {
        public string Name { get; set; }
        public string FilePath { get; set; }

        public int CourseId { get; set; }
        public int CourseContentTypeId { get; set; }
    }
}
