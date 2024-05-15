using EducationPortalApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace EducationPortalApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService;
        public HomeController(ICourseService courseService)
        {
            _courseService = courseService;
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
            return View(result.Data);
        }
    }
}
