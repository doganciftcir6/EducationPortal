namespace EducationPortalApp.Web.Models.CourseModels
{
    public class CoursesVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal CostPerDay { get; set; }
        public string? Picture { get; set; }
    }
}
