﻿using System.ComponentModel.DataAnnotations;

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
        public decimal CostPerDay { get; set; }
        public int DurationInDays { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<CourseContent> CourseContents { get; set; }
        public List<EnrollmentRequest> EnrollmentRequests { get; set; }
    }
}
