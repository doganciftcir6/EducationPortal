using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Dtos.CourseContentDtos;
using EducationPortalApp.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseContentController : CustomBaseController
    {
        private readonly ICourseContentService _courseContentService;
        public CourseContentController(ICourseContentService courseContentService)
        {
            _courseContentService = courseContentService;
        }

        [HttpGet("[action]/{courseId}")]
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
    }
}
