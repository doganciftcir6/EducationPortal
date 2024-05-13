using EducationPortalApp.Dtos.CategoryDtos;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CustomResponse<IEnumerable<CategoryDto>>> GetCategoriesAsync();
        Task<CustomResponse<CategoryDto>> GetCategoryByIdAsync(int categoryId);
        Task<CustomResponse<NoContent>> InsertCategoryAsync(CategoryCreateDto categoryCreateDto);
        Task<CustomResponse<NoContent>> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto);
        Task<CustomResponse<NoContent>> RemoveCategoryAsync(int productId);
    }
}
