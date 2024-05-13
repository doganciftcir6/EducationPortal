using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Dtos.CategoryDtos;
using EducationPortalApp.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCategories()
        {
            return CreateActionResultInstance(await _categoryService.GetCategoriesAsync());
        }

        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            return CreateActionResultInstance(await _categoryService.GetCategoryByIdAsync(categoryId));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> InsertCategory(CategoryCreateDto categoryCreateDto)
        {
            return CreateActionResultInstance(await _categoryService.InsertCategoryAsync(categoryCreateDto));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateDto categoryUpdateDto)
        {
            return CreateActionResultInstance(await _categoryService.UpdateCategoryAsync(categoryUpdateDto));
        }

        [HttpDelete("[action]/{categoryId}")]
        public async Task<IActionResult> RemoveCategory(int categoryId)
        {
            return CreateActionResultInstance(await _categoryService.RemoveCategoryAsync(categoryId));
        }
    }
}
