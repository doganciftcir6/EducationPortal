using System.ComponentModel.DataAnnotations;

namespace EducationPortalApp.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public List<Category> Categories { get; set; }
    }
}
