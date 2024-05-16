using Microsoft.AspNetCore.Mvc.Rendering;

namespace EducationPortalApp.Web.Models.CourseModels
{
    public class CourseUpdateInput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }
        public int Capacity { get; set; }
        public int MaxCapacity { get; set; }
        public decimal CostPerDay { get; set; }
        public int DurationInHours { get; set; }
        public IFormFile? Picture { get; set; }
        public string? Picturename { get; set; }

        public int CategoryId { get; set; }
        public SelectList? Categories { get; set; }
    }
}
