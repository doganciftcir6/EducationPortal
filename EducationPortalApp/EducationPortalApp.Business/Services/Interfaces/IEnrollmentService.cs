using EducationPortalApp.Dtos.EnrollmentDtos;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<CustomResponse<IEnumerable<EnrollmentDto>>> GetAllEnrollmentAsync();
        Task<CustomResponse<IEnumerable<EnrollmentDto>>> GetAllEnrollmentByUserAsync();
        Task<CustomResponse<EnrollmentDto>> GetEnrollmentByUserIdAndCourseIdAsync(int userId, int courseId);
        Task<CustomResponse<NoContent>> InsertEnrollmentAsync(EnrollmentCreateDto enrollmentCreateDto, int enrollmentRequestId);
        Task<CustomResponse<NoContent>> RemoveEnrollmentAsync(int enrollmentId, int enrollmentRequestId);
        Task<bool> UpdateEnrollmentCompletionStatusAsync(int courseContentId, int courseId);
    }
}
