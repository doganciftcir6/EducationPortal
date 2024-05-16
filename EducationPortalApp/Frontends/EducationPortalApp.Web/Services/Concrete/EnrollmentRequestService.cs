using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Helpers.HttpHelpers;
using EducationPortalApp.Web.Models.EnrollmentRequestModels;
using EducationPortalApp.Web.Services.Interfaces;

namespace EducationPortalApp.Web.Services.Concrete
{
    public class EnrollmentRequestService : IEnrollmentRequestService
    {
        private readonly HttpService _httpService;
        public EnrollmentRequestService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<CustomResponse<IEnumerable<ComplexEnrollmentRequestVM>>> GetAllEnrollmentRequestAsync()
        {
            var enrollmentRequestResponse = await _httpService.HttpGet<CustomResponse<IEnumerable<ComplexEnrollmentRequestVM>>>("EnrollmentRequest/GetAllEnrollmentRequest");
            return HandleResponseHelper.HandleResponse(enrollmentRequestResponse);
        }

        public async Task<CustomResponse<IEnumerable<EnrollmentRequestVM>>> GetAllEnrollmentRequestByUserAsync()
        {
            var enrollmentRequestResponse = await _httpService.HttpGetWithToken<CustomResponse<IEnumerable<EnrollmentRequestVM>>>("EnrollmentRequest/GetAllEnrollmentRequestByUser");
            return HandleResponseHelper.HandleResponse(enrollmentRequestResponse);
        }

        public async Task<CustomResponse<NoContent>> InsertEnrollmentRequestAsync(EnrollmentRequestInput enrollmentRequestInput)
        {
            var result = await _httpService.HttpPostWithToken<CustomResponse<NoContent>>("EnrollmentRequest/InsertEnrollmentRequest", enrollmentRequestInput);
            return HandleResponseHelper.HandleResponse(result);
        }
    }
}
