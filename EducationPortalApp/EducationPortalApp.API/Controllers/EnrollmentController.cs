using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Dtos.EnrollmentDtos;
using EducationPortalApp.Shared.ControllerBases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : CustomBaseController
    {
        private readonly IEnrollmentService _enrollmentService;
        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllEnrollment()
        {
            return CreateActionResultInstance(await _enrollmentService.GetAllEnrollmentAsync());
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllEnrollmentByUser()
        {
            return CreateActionResultInstance(await _enrollmentService.GetAllEnrollmentByUserAsync());
        }

        [HttpPost("[action]/{enrollmentRequestId}")]
        public async Task<IActionResult> InsertEnrollment(EnrollmentCreateDto enrollmentCreateDto, int enrollmentRequestId)
        {
            return CreateActionResultInstance(await _enrollmentService.InsertEnrollmentAsync(enrollmentCreateDto, enrollmentRequestId));
        }

        [HttpDelete("[action]/{enrollmentId}/{enrollmentRequestId}")]
        public async Task<IActionResult> RemoveEnrollmentRequest(int enrollmentId, int enrollmentRequestId)
        {
            return CreateActionResultInstance(await _enrollmentService.RemoveEnrollmentAsync(enrollmentId, enrollmentRequestId));
        }
    }
}
