using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Helpers.HttpHelpers;
using EducationPortalApp.Web.Models.EnrollmentModels;
using EducationPortalApp.Web.Services.Interfaces;

namespace EducationPortalApp.Web.Services.Concrete
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly HttpService _httpService;
        public EnrollmentService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<CustomResponse<IEnumerable<EnrollmentVM>>> GetAllEnrollmentAsync()
        {
            var enrollmentResponse = await _httpService.HttpGet<CustomResponse<IEnumerable<EnrollmentVM>>>("Enrollment/GetAllEnrollment");
            return HandleResponseHelper.HandleResponse(enrollmentResponse);
        }

        public async Task<CustomResponse<IEnumerable<EnrollmentVM>>> GetAllEnrollmentByUserAsync()
        {
            var enrollmentResponse = await _httpService.HttpGetWithToken<CustomResponse<IEnumerable<EnrollmentVM>>>("Enrollment/GetAllEnrollmentByUser");
            return HandleResponseHelper.HandleResponse(enrollmentResponse);
        }

        public async Task<CustomResponse<NoContent>> InsertEnrollmentAsync(EnrollmentInput enrollmentInput, int enrollmentRequestId)
        {
            var insertEnrollmentResponse = await _httpService.HttpPost<CustomResponse<NoContent>>($"Enrollment/InsertEnrollment/{enrollmentRequestId}", enrollmentInput);
            return HandleResponseHelper.HandleResponse(insertEnrollmentResponse);
        }

        public async Task<CustomResponse<NoContent>> RemoveEnrollmentAsync(int courseId, int appUserId, int enrollmentRequestId)
        {
            var removeEnrollmentResponse = await _httpService.HttpDelete<CustomResponse<NoContent>>($"Enrollment/RemoveEnrollmentRequest/{courseId}/{appUserId}/{enrollmentRequestId}");
            return HandleResponseHelper.HandleResponse(removeEnrollmentResponse);
        }
    }
}
