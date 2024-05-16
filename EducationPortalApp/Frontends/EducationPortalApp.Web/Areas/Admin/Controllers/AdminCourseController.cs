using EducationPortalApp.Web.Models.CourseModels;
using EducationPortalApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using X.PagedList;

namespace EducationPortalApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/AdminCourse/{action}/{id?}")]
    [Authorize(Roles = "Admin")]
    public class AdminCourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        public AdminCourseController(ICourseService courseService, ICategoryService categoryService)
        {
            _courseService = courseService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var response = await _courseService.GetCoursesAsync();
            var pagedData = response.Data.ToPagedList(page, 7);
            return View(pagedData);
        }
        public async Task<IActionResult> InsertCourse()
        {
            var categoriesData = await _categoryService.GetCategoriesAsync();
            var model = new CourseCreateInput();
            model.Categories = new SelectList(categoriesData.Data, "Id", "CategoryName");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> InsertCourse(CourseCreateInput courseCreateInput)
        {
            if (!ModelState.IsValid)
            {
                var data = TempData["categories"]?.ToString();
                if (data != null)
                {
                    var categories = JsonSerializer.Deserialize<List<SelectListItem>>(data);
                    courseCreateInput.Categories = new SelectList(categories, "Value", "Text");
                }
                return View(courseCreateInput);
            }

            var response = await _courseService.InsertCourseAsync(courseCreateInput);
            if (response.Errors is not null)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                var data = TempData["categories"]?.ToString();
                if (data != null)
                {
                    var categories = JsonSerializer.Deserialize<List<SelectListItem>>(data);
                    courseCreateInput.Categories = new SelectList(categories, "Value", "Text");
                }
                return View(courseCreateInput);
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> UpdateCourse(int courseId)
        {
            var courseResponse = await _courseService.GetCourseDetailAsync(courseId);
            var categoriesData = await _categoryService.GetCategoriesAsync();
            var courseUpdateInput = new CourseUpdateInput()
            {
                Id = courseResponse.Data.Id,
                Name = courseResponse.Data.Name,
                Description = courseResponse.Data.Description,
                Instructor = courseResponse.Data.Instructor,
                Capacity = courseResponse.Data.Capacity,
                MaxCapacity = courseResponse.Data.MaxCapacity,
                CostPerDay = courseResponse.Data.CostPerDay,
                DurationInHours = courseResponse.Data.DurationInHours,
                Picturename = courseResponse.Data.Picture
            };

            var selectedCategory = categoriesData.Data.FirstOrDefault(x => x.CategoryName == courseResponse.Data.Category);
            if (selectedCategory is not null)
            {
                courseUpdateInput.CategoryId = selectedCategory.Id;
            }
            courseUpdateInput.Categories = new SelectList(categoriesData.Data, "Id", "CategoryName");
            return View(courseUpdateInput);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCourse(CourseUpdateInput courseUpdateInput)
        {
            if (!ModelState.IsValid)
            {
                var data = TempData["categories"]?.ToString();
                if (data != null)
                {
                    var categories = JsonSerializer.Deserialize<List<SelectListItem>>(data);
                    courseUpdateInput.Categories = new SelectList(categories, "Value", "Text");
                }
                return View(courseUpdateInput);
            }

            var response = await _courseService.UpdateCourseAsync(courseUpdateInput);
            if (response.Errors is not null)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                var data = TempData["categories"]?.ToString();
                if (data != null)
                {
                    var categories = JsonSerializer.Deserialize<List<SelectListItem>>(data);
                    courseUpdateInput.Categories = new SelectList(categories, "Value", "Text");
                }
                return View(courseUpdateInput);
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var response = await _courseService.RemoveCourseAsync(courseId);
            if (response.Errors != null)
                return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
