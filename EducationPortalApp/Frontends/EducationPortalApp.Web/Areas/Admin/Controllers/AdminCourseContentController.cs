using EducationPortalApp.Web.Models.CourseContentModels;
using EducationPortalApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using X.PagedList;

namespace EducationPortalApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/AdminCourseContent/{action}/{id?}")]
    [Authorize(Roles = "Admin")]
    public class AdminCourseContentController : Controller
    {
        private readonly ICourseContentService _courseContentService;
        private readonly ICourseContentTypeService _courseContentTypeService;
        public AdminCourseContentController(ICourseContentService courseContentService, ICourseContentTypeService courseContentTypeService)
        {
            _courseContentService = courseContentService;
            _courseContentTypeService = courseContentTypeService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await _courseContentService.GetAllCourseContentAsync();
            var pagedData = result.Data.ToPagedList(page, 7);
            return View(pagedData);
        }

        public async Task<IActionResult> InsertCourseContent(int courseId)
        {
            var contentTypesData = await _courseContentTypeService.GetCourseContentTypesAsync();
            var model = new CourseContentCreateInput();
            model.CourseContentTypes = new SelectList(contentTypesData.Data, "Id", "Definition");
            if (courseId != null)
                ViewBag.CourseId = courseId;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> InsertCourseContent(CourseContentCreateInput courseContentCreateInput)
        {
            if (!ModelState.IsValid)
            {
                var data = TempData["contentTypes"]?.ToString();
                if (data != null)
                {
                    var contentTypes = JsonSerializer.Deserialize<List<SelectListItem>>(data);
                    courseContentCreateInput.CourseContentTypes = new SelectList(contentTypes, "Value", "Text");
                }
                return View(courseContentCreateInput);
            }

            var response = await _courseContentService.InsertCourseContentAsync(courseContentCreateInput);
            if (response.Errors is not null)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                var data = TempData["contentTypes"]?.ToString();
                if (data != null)
                {
                    var contentTypes = JsonSerializer.Deserialize<List<SelectListItem>>(data);
                    courseContentCreateInput.CourseContentTypes = new SelectList(contentTypes, "Value", "Text");
                }
                return View(courseContentCreateInput);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateCourseContent(int courseContentId)
        {
            var courseContentResponse = await _courseContentService.GetCourseContentByIdAsync(courseContentId);
            var contentTypesData = await _courseContentTypeService.GetCourseContentTypesAsync();
            var courseContentUpdateInput = new CourseContentUpdateInput()
            {
                Id = courseContentResponse.Data.Id,
                Name = courseContentResponse.Data.Name,
                CourseId = courseContentResponse.Data.Course.Id,
                Filename = courseContentResponse.Data.FilePath,
            };

            var selectedContentType = contentTypesData.Data.FirstOrDefault(x => x.Definition == courseContentResponse.Data.CourseContentType);
            if (selectedContentType is not null)
            {
                courseContentUpdateInput.CourseContentTypeId = selectedContentType.Id;
            }
            courseContentUpdateInput.CourseContentTypes = new SelectList(contentTypesData.Data, "Id", "Definition");
            return View(courseContentUpdateInput);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCourseContent(CourseContentUpdateInput courseContentUpdateInput)
        {
            if (!ModelState.IsValid)
            {
                var data = TempData["contentTypes"]?.ToString();
                if (data != null)
                {
                    var contentTypes = JsonSerializer.Deserialize<List<SelectListItem>>(data);
                    courseContentUpdateInput.CourseContentTypes = new SelectList(contentTypes, "Value", "Text");
                }
                return View(courseContentUpdateInput);
            }

            var response = await _courseContentService.UpdateCourseContentAsync(courseContentUpdateInput);
            if (response.Errors is not null)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                var data = TempData["contentTypes"]?.ToString();
                if (data != null)
                {
                    var contentTypes = JsonSerializer.Deserialize<List<SelectListItem>>(data);
                    courseContentUpdateInput.CourseContentTypes = new SelectList(contentTypes, "Value", "Text");
                }
                return View(courseContentUpdateInput);
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteCourseContent(int courseContentId)
        {
            var response = await _courseContentService.RemoveCourseContentAsync(courseContentId);
            if (response.Errors != null)
                return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
