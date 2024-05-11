using EducationPortalApp.Entities.CourseEntities;
using System.ComponentModel.DataAnnotations;

namespace EducationPortalApp.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public List<Course> Courses { get; set; }
    }
}
