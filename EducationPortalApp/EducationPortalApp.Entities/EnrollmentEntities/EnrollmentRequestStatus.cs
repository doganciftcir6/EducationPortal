using System.ComponentModel.DataAnnotations;

namespace EducationPortalApp.Entities.EnrollmentEntities
{
    public class EnrollmentRequestStatus
    {
        [Key]
        public int Id { get; set; }
        public string? Definition { get; set; }

        public List<EnrollmentRequest> EnrollmentRequests { get; set; }
    }
}
