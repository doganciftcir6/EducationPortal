namespace EducationPortalApp.Dtos.CourseDtos
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }
        public int Capacity { get; set; }
        public int MaxCapacity { get; set; }
        public decimal CostPerDay { get; set; }
        public int DurationInHours { get; set; }
        public string? Picture { get; set; }
        public DateTime CreateDate { get; set; }

        public string? Category { get; set; }
    }
}
