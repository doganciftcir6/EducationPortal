using EducationPortalApp.Dtos.EnrollmentDtos;
using EducationPortalApp.Dtos.EnrollmentRequestDtos;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Interfaces
{
    public interface IEnrollmentRequestService
    {
        Task<CustomResponse<IEnumerable<ComplexEnrollmentRequestDto>>> GetAllEnrollmentRequestAsync();
        Task<CustomResponse<IEnumerable<EnrollmentRequestDto>>> GetAllEnrollmentRequestByUserAsync();
        Task<CustomResponse<NoContent>> InsertEnrollmentRequestAsync(EnrollmentRequestCreateDto enrollmentRequestCreateDto);
        Task<CustomResponse<NoContent>> UpdateEnrollmentRequestAsync(EnrollmentRequestUpdateDto enrollmentRequestUpdateDto);
        Task<CustomResponse<NoContent>> RemoveEnrollmentRequestAsync(int enrollmentRequestId);
    }
}
