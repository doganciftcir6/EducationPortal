using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Models.EnrollmentModels;

namespace EducationPortalApp.Web.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<CustomResponse<IEnumerable<EnrollmentVM>>> GetAllEnrollmentAsync();
        Task<CustomResponse<IEnumerable<EnrollmentVM>>> GetAllEnrollmentByUserAsync();
        Task<CustomResponse<NoContent>> InsertEnrollmentAsync(EnrollmentInput enrollmentInput, int enrollmentRequestId);
        Task<CustomResponse<NoContent>> RemoveEnrollmentAsync(int courseId, int appUserId, int enrollmentRequestId);
    }
}
