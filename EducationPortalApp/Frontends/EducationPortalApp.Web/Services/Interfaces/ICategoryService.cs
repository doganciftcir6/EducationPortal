using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Models.CategoryModels;

namespace EducationPortalApp.Web.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CustomResponse<IEnumerable<CategoryVM>>> GetCategoriesAsync();
        Task<CustomResponse<CategoryVM>> GetCategoryAsync(int categoryId);
        Task<CustomResponse<NoContent>> InsertCategoryAsync(CategoryCreateInput categoryCreateInput);
        Task<CustomResponse<NoContent>> UpdateCategoryAsync(CategoryUpdateInput categoryUpdateInput);
        Task<CustomResponse<NoContent>> RemoveCategoryAsync(int categoryId);
    }
}
