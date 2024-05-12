using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentRequestStatusController : CustomBaseController
    {
        private readonly IEnrollmentRequestStatusService _enrollmentRequestStatusService;
        public EnrollmentRequestStatusController(IEnrollmentRequestStatusService enrollmentRequestStatusService)
        {
            _enrollmentRequestStatusService = enrollmentRequestStatusService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEnrollmentRequestStatuses()
        {
            return CreateActionResultInstance(await _enrollmentRequestStatusService.GetEnrollmentRequestStatusesAsync());
        }
    }
}
