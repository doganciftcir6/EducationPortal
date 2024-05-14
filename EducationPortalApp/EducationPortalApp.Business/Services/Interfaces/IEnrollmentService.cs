using EducationPortalApp.Dtos.EnrollmentDtos;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<CustomResponse<IEnumerable<EnrollmentDto>>> GetAllEnrollmentAsync();
        Task<CustomResponse<IEnumerable<EnrollmentDto>>> GetAllEnrollmentByUserAsync();
        Task<CustomResponse<NoContent>> InsertEnrollmentAsync(EnrollmentCreateDto enrollmentCreateDto, int enrollmentRequestId);
        Task<CustomResponse<NoContent>> UpdateEnrollmentAsync(EnrollmentUpdateDto enrollmentUpdateDto);
        Task<CustomResponse<NoContent>> RemoveEnrollmentAsync(int enrollmentId);
    }
}
