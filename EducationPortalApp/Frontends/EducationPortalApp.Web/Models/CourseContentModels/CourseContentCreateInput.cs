using Microsoft.AspNetCore.Mvc.Rendering;

namespace EducationPortalApp.Web.Models.CourseContentModels
{
    public class CourseContentCreateInput
    {
        public string Name { get; set; }
        public IFormFile? File { get; set; }

        public int CourseId { get; set; }
        public int CourseContentTypeId { get; set; }
        public SelectList? CourseContentTypes { get; set; }
    }
}
