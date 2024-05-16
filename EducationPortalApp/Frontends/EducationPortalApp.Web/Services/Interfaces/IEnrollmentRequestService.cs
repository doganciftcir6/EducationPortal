using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Models.EnrollmentRequestModels;

namespace EducationPortalApp.Web.Services.Interfaces
{
    public interface IEnrollmentRequestService
    {
        Task<CustomResponse<NoContent>> InsertEnrollmentRequestAsync(EnrollmentRequestInput enrollmentRequestInput);
        Task<CustomResponse<IEnumerable<ComplexEnrollmentRequestVM>>> GetAllEnrollmentRequestAsync();
        Task<CustomResponse<IEnumerable<EnrollmentRequestVM>>> GetAllEnrollmentRequestByUserAsync();
    }
}
