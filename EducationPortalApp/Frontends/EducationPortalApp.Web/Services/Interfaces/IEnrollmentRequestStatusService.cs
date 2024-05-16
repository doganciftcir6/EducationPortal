using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Models.EnrollmentRequestStatusModels;

namespace EducationPortalApp.Web.Services.Interfaces
{
    public interface IEnrollmentRequestStatusService
    {
        Task<CustomResponse<IEnumerable<EnrollmentRequestStatusVM>>> GetEnrollmentRequestStatusesAsync();
    }
}
