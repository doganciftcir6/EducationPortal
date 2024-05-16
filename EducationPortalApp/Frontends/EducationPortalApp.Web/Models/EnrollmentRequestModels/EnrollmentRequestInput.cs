using Microsoft.AspNetCore.Mvc.Rendering;

namespace EducationPortalApp.Web.Models.EnrollmentRequestModels
{
    public class EnrollmentRequestInput
    {
        public int CourseId { get; set; }
        public int EnrollmentRequestStatusId { get; set; }
        public SelectList? EnrollmentRequestStatuses { get; set; }
    }
}
