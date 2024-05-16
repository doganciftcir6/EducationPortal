namespace EducationPortalApp.Web.Models.EnrollmentRequestModels
{
    public class EnrollmentRequestVM
    {
        public int Id { get; set; }

        public string? AppUser { get; set; }
        public string? Course { get; set; }
        public string? EnrollmentRequestStatus { get; set; }
    }
}
