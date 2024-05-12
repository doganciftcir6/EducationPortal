using EducationPortalApp.Dtos.EnrollmentRequestStatusDtos;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Interfaces
{
    public interface IEnrollmentRequestStatusService
    {
        Task<CustomResponse<IEnumerable<EnrollmentRequestStatusDto>>> GetEnrollmentRequestStatusesAsync();
    }
}
