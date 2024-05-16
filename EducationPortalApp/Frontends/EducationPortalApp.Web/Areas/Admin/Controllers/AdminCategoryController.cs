using EducationPortalApp.Web.Models.CategoryModels;
using EducationPortalApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/AdminCategory/{action}/{id?}")]
    [Authorize(Roles = "Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public AdminCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _categoryService.GetCategoriesAsync();
            return View(response.Data);
        }

        public async Task<IActionResult> InsertCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> InsertCategory(CategoryCreateInput categoryCreateInput)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryCreateInput);
            }

            var response = await _categoryService.InsertCategoryAsync(categoryCreateInput);
            if (response.Errors is not null)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(categoryCreateInput);
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> UpdateCategory(int categoryId)
        {
            var response = await _categoryService.GetCategoryAsync(categoryId);
            if (response.Errors != null)
                return NotFound();
            return View(response.Data);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateInput categoryUpdateInput)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryUpdateInput);
            }

            var response = await _categoryService.UpdateCategoryAsync(categoryUpdateInput);
            if (response.Errors is not null)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(categoryUpdateInput);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var response = await _categoryService.RemoveCategoryAsync(categoryId);
            if (response.Errors != null)
                return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
