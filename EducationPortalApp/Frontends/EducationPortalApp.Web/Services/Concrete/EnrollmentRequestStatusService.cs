using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Helpers.HttpHelpers;
using EducationPortalApp.Web.Models.EnrollmentRequestStatusModels;
using EducationPortalApp.Web.Services.Interfaces;

namespace EducationPortalApp.Web.Services.Concrete
{
    public class EnrollmentRequestStatusService : IEnrollmentRequestStatusService
    {
        private readonly HttpService _httpService;
        public EnrollmentRequestStatusService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<CustomResponse<IEnumerable<EnrollmentRequestStatusVM>>> GetEnrollmentRequestStatusesAsync()
        {
            var enrollmentRequestStatusResponse = await _httpService.HttpGet<CustomResponse<IEnumerable<EnrollmentRequestStatusVM>>>("EnrollmentRequestStatus/GetEnrollmentRequestStatuses");
            return HandleResponseHelper.HandleResponse(enrollmentRequestStatusResponse);
        }
    }
}
