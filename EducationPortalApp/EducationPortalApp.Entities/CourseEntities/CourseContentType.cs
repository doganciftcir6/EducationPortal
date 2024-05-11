using EducationPortalApp.Entities.UserEntities;
using System.ComponentModel.DataAnnotations;

namespace EducationPortalApp.Entities.CourseEntities
{
    public class CourseContentType
    {

        [Key]
        public int Id { get; set; }
        public string? Definition { get; set; }

        public List<CourseContent> CourseContents { get; set; }
    }
}
