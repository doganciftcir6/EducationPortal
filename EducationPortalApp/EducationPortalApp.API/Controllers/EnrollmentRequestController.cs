using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Dtos.EnrollmentRequestDtos;
using EducationPortalApp.Shared.ControllerBases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentRequestController : CustomBaseController
    {
        private readonly IEnrollmentRequestService _enrollmentRequestService;
        public EnrollmentRequestController(IEnrollmentRequestService enrollmentRequestService)
        {
            _enrollmentRequestService = enrollmentRequestService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllEnrollmentRequest()
        {
            return CreateActionResultInstance(await _enrollmentRequestService.GetAllEnrollmentRequestAsync());
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllEnrollmentRequestByUser()
        {
            return CreateActionResultInstance(await _enrollmentRequestService.GetAllEnrollmentRequestByUserAsync());
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> InsertEnrollmentRequest(EnrollmentRequestCreateDto enrollmentRequestCreateDto)
        {
            return CreateActionResultInstance(await _enrollmentRequestService.InsertEnrollmentRequestAsync(enrollmentRequestCreateDto));
        }

        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> UpdateCourseContent(EnrollmentRequestUpdateDto enrollmentRequestUpdateDto)
        {
            return CreateActionResultInstance(await _enrollmentRequestService.UpdateEnrollmentRequestAsync(enrollmentRequestUpdateDto));
        }

        [HttpDelete("[action]/{enrollmentRequestId}")]
        public async Task<IActionResult> RemoveEnrollmentRequest(int enrollmentRequestId)
        {
            return CreateActionResultInstance(await _enrollmentRequestService.RemoveEnrollmentRequestAsync(enrollmentRequestId));
        }
    }
}
