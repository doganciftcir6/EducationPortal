namespace EducationPortalApp.Dtos.CourseDtos
{
    public class CourseDtoForEnrollmentRQ
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Capacity { get; set; }
        public int MaxCapacity { get; set; }
    }
}
