using System.ComponentModel.DataAnnotations;

namespace EducationPortalApp.Entities.CourseEntities
{
    public class CourseContent
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int CourseContentTypeId { get; set; }
        public CourseContentType CourseContentType { get; set; }
    }
}
