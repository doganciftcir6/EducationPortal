using System.ComponentModel.DataAnnotations;

namespace EducationPortalApp.Entities.UserEntities
{
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        public string? Definition { get; set; }

        public List<AppUser> AppUsers { get; set; }
    }
}
