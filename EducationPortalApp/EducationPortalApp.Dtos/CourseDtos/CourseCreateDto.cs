using Microsoft.AspNetCore.Http;

namespace EducationPortalApp.Dtos.CourseDtos
{
    public class CourseCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }
        public int Capacity { get; set; }
        public decimal CostPerDay { get; set; }
        public int DurationInHours { get; set; }
        public IFormFile? Picture { get; set; }

        public int CategoryId { get; set; }
    }
}
