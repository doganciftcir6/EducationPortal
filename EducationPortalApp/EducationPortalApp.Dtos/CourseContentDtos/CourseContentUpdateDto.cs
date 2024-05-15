using Microsoft.AspNetCore.Http;

namespace EducationPortalApp.Dtos.CourseContentDtos
{
    public class CourseContentUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile? File { get; set; }

        public int CourseId { get; set; }
        public int CourseContentTypeId { get; set; }
    }
}
