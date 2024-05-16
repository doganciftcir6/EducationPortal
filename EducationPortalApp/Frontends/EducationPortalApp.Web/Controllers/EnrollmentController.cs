using EducationPortalApp.Web.Models.EnrollmentModels;
using EducationPortalApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace EducationPortalApp.Web.Controllers
{
    [Authorize]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IEnrollmentRequestService _enrollmentRequestService;
        public EnrollmentController(IEnrollmentService enrollmentService, IEnrollmentRequestService enrollmentRequestService)
        {
            _enrollmentService = enrollmentService;
            _enrollmentRequestService = enrollmentRequestService;
        }

        public async Task<IActionResult> MyCourses()
        {
            var result = await _enrollmentService.GetAllEnrollmentByUserAsync();
            return View(result.Data);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Enrollments(int page = 1)
        {
            var result = await _enrollmentService.GetAllEnrollmentAsync();
            var pagedData = result.Data.ToPagedList(page, 7);
            return View(pagedData);
        }

        [HttpPost]
        public async Task<IActionResult> InsertEnrollment(EnrollmentInput enrollmentInput, int enrollmentRequestId)
        {
            var result = await _enrollmentService.InsertEnrollmentAsync(enrollmentInput, enrollmentRequestId);
            if (result.Errors is not null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                var enrollmentRequestsModel = await _enrollmentRequestService.GetAllEnrollmentRequestAsync();
                return View("~/Views/EnrollmentRequest/EnrollmentRequests.cshtml", enrollmentRequestsModel.Data);
            }
            return RedirectToAction("Enrollments");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveEnrollment(int courseId, int appUserId, int enrollmentRequestId)
        {
            var result = await _enrollmentService.RemoveEnrollmentAsync(courseId, appUserId, enrollmentRequestId);
            if (result.Errors is not null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                var enrollmentRequestsModel = await _enrollmentRequestService.GetAllEnrollmentRequestAsync();
                return View("~/Views/EnrollmentRequest/EnrollmentRequests.cshtml", enrollmentRequestsModel.Data);
            }
            return RedirectToAction("Enrollments");
        }

    }
}
