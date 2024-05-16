namespace EducationPortalApp.Web.Models.CourseModels
{
    public class CourseVMForEnrollmentRQ
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Capacity { get; set; }
        public int MaxCapacity { get; set; }
    }
}
