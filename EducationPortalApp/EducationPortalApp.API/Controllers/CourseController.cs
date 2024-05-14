using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Dtos.CourseDtos;
using EducationPortalApp.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : CustomBaseController
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCourses()
        {
            return CreateActionResultInstance(await _courseService.GetCoursesAsync());
        }

        [HttpGet("[action]/{courseId}")]
        public async Task<IActionResult> GetCourse(int courseId)
        {
            return CreateActionResultInstance(await _courseService.GetCourseByIdAsync(courseId));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> InsertCourse([FromForm] CourseCreateDto courseCreateDto, CancellationToken cancellationToken)
        {
            return CreateActionResultInstance(await _courseService.InsertCourseAsync(courseCreateDto, cancellationToken));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCourse([FromForm] CourseUpdateDto courseUpdateDto, CancellationToken cancellationToken)
        {
            return CreateActionResultInstance(await _courseService.UpdateCourseAsync(courseUpdateDto, cancellationToken));
        }

        [HttpDelete("[action]/{courseId}")]
        public async Task<IActionResult> RemoveCourse(int courseId)
        {
            return CreateActionResultInstance(await _courseService.RemoveCourseAsync(courseId));
        }
    }
}
