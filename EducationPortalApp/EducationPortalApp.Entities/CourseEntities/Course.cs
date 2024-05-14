using EducationPortalApp.Entities.EnrollmentEntities;
using System.ComponentModel.DataAnnotations;

namespace EducationPortalApp.Entities.CourseEntities
{
    public class Course
    {
        [Key]
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


        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<CourseContent> CourseContents { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<EnrollmentRequest> EnrollmentRequests { get; set; }
    }
}
