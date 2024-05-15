using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Dtos.CourseContentDtos;
using EducationPortalApp.Shared.ControllerBases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseContentController : CustomBaseController
    {
        private readonly ICourseContentService _courseContentService;
        private readonly IEnrollmentService _enrollmentService;
        public CourseContentController(ICourseContentService courseContentService, IEnrollmentService enrollmentService)
        {
            _courseContentService = courseContentService;
            _enrollmentService = enrollmentService;
        }

        [HttpGet("[action]/{courseId}")]
        [Authorize]
        public async Task<IActionResult> GetAllCourseContent(int courseId)
        {
            return CreateActionResultInstance(await _courseContentService.GetAllCourseContentByCourseIdAsync(courseId));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> InsertCourseContent([FromForm] CourseContentCreateDto courseContentCreateDto, CancellationToken cancellationToken)
        {
            return CreateActionResultInstance(await _courseContentService.InsertCourseContentAsync(courseContentCreateDto, cancellationToken));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCourseContent([FromForm] CourseContentUpdateDto courseContentUpdateDto, CancellationToken cancellationToken)
        {
            return CreateActionResultInstance(await _courseContentService.UpdateCourseContentAsync(courseContentUpdateDto, cancellationToken));
        }

        [HttpDelete("[action]/{courseContentId}")]
        public async Task<IActionResult> RemoveCourseContent(int courseContentId)
        {
            return CreateActionResultInstance(await _courseContentService.RemoveCourseContentAsync(courseContentId));
        }

        [HttpPatch("[action]/{courseContentId}")]
        [Authorize]
        public async Task<IActionResult> UpdateCourseContentStatus(int courseContentId, [FromBody] bool isChecked)
        {
            var result = await _courseContentService.UpdateCourseContentStatusAsync(courseContentId, isChecked);
            if (result.Errors != null)
                return BadRequest(result.Errors);

            var courseContentResult = await _courseContentService.GetByIdCourseContentAsync(courseContentId);
            if (courseContentResult.Errors != null)
                return BadRequest(courseContentResult.Errors);

            var enrollmentUpdated = await _enrollmentService.UpdateEnrollmentCompletionStatusAsync(courseContentId, courseContentResult.Data.Course.Id);
            if (!enrollmentUpdated)
            {
                return BadRequest("Failed to update enrollment completion status.");
            }
            return Ok();
        }
    }
}
