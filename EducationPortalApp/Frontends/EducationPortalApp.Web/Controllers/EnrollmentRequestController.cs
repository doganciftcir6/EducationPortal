using EducationPortalApp.Web.Models.EnrollmentRequestModels;
using EducationPortalApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EducationPortalApp.Web.Controllers
{
    [Authorize]
    public class EnrollmentRequestController : Controller
    {
        private readonly IEnrollmentRequestService _enrollmentRequestService;
        private readonly ICourseService _courseService;
        private readonly IEnrollmentRequestStatusService _enrollmentRequestStatusService;
        public EnrollmentRequestController(IEnrollmentRequestService enrollmentRequestService, ICourseService courseService, IEnrollmentRequestStatusService enrollmentRequestStatusService)
        {
            _enrollmentRequestService = enrollmentRequestService;
            _courseService = courseService;
            _enrollmentRequestStatusService = enrollmentRequestStatusService;
        }

        [HttpPost]
        public async Task<IActionResult> InsertEnrollmentRequest(EnrollmentRequestInput enrollmentRequestInput)
        {
            var response = await _enrollmentRequestService.InsertEnrollmentRequestAsync(enrollmentRequestInput);
            if (response.Errors is not null)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                var courseDetailsModel = await _courseService.GetCourseDetailAsync(enrollmentRequestInput.CourseId);
                var enrollmentRequestStatusesData = await _enrollmentRequestStatusService.GetEnrollmentRequestStatusesAsync();
                var statuses = enrollmentRequestStatusesData.Data;
                ViewBag.EnrollmentRequestStatuses = new SelectList(statuses, "Id", "Definition");
                return View("~/Views/Home/CourseDetails.cshtml", courseDetailsModel.Data);
            }
            return RedirectToAction("EnrollmentRequestsByUser");
        }

        public async Task<IActionResult> EnrollmentRequestsByUser()
        {
            var response = await _enrollmentRequestService.GetAllEnrollmentRequestByUserAsync();
            return View(response.Data);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EnrollmentRequests()
        {
            var response = await _enrollmentRequestService.GetAllEnrollmentRequestAsync();
            return View(response.Data);
        }
    }
}
