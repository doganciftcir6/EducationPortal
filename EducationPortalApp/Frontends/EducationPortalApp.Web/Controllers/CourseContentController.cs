using EducationPortalApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.Web.Controllers
{
    [Authorize]
    public class CourseContentController : Controller
    {
        private readonly ICourseContentService _courseContentService;
        private readonly IUserCourseContentStatusService _userCourseContentStatusService;
        public CourseContentController(ICourseContentService courseContentService, IUserCourseContentStatusService userCourseContentStatusService)
        {
            _courseContentService = courseContentService;
            _userCourseContentStatusService = userCourseContentStatusService;
        }

        public async Task<IActionResult> Index(int courseId)
        {
            var result = await _courseContentService.GetAllCourseContentByCourseIdAsync(courseId);
            var userCourseContentStatues = await _userCourseContentStatusService.GetContentStatusByUserAsync();
            ViewBag.UserCourseContentStatues = userCourseContentStatues.Data;
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCourseContentStatus(int courseContentId, bool isChecked)
        {
            var result = await _courseContentService.UpdateCourseContentStatusAsync(courseContentId, isChecked);
            return Ok();
        }
    }
}
