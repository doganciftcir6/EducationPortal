using EducationPortalApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace EducationPortalApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IEnrollmentRequestStatusService _enrollmentRequestStatusService;
        public HomeController(ICourseService courseService, IEnrollmentRequestStatusService enrollmentRequestStatusService)
        {
            _courseService = courseService;
            _enrollmentRequestStatusService = enrollmentRequestStatusService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await _courseService.GetCoursesAsync();
            var pagedData = result.Data.ToPagedList(page, 6);
            return View(pagedData);
        }

        public async Task<IActionResult> CourseDetails(int courseId)
        {
            var result = await _courseService.GetCourseDetailAsync(courseId);
            if (result.Errors is not null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            var enrollmentRequestStatusesData = await _enrollmentRequestStatusService.GetEnrollmentRequestStatusesAsync();
            var statuses = enrollmentRequestStatusesData.Data;
            ViewBag.EnrollmentRequestStatuses = new SelectList(statuses, "Id", "Definition");
            return View(result.Data);
        }
    }
}
