using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Helpers.HttpHelpers;
using EducationPortalApp.Web.Models.CategoryModels;
using EducationPortalApp.Web.Services.Interfaces;

namespace EducationPortalApp.Web.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpService _httpService;
        public CategoryService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<CustomResponse<IEnumerable<CategoryVM>>> GetCategoriesAsync()
        {
            var categoriesRespones = await _httpService.HttpGet<CustomResponse<IEnumerable<CategoryVM>>>("Category/GetCategories");
            return HandleResponseHelper.HandleResponse(categoriesRespones);
        }

        public async Task<CustomResponse<CategoryVM>> GetCategoryAsync(int categoryId)
        {
            var categoryResponse = await _httpService.HttpGetWithToken<CustomResponse<CategoryVM>>($"Category/GetCategory/{categoryId}");
            return HandleResponseHelper.HandleResponse(categoryResponse);
        }

        public async Task<CustomResponse<NoContent>> InsertCategoryAsync(CategoryCreateInput categoryCreateInput)
        {
            var categoryInsertResponse = await _httpService.HttpPostWithToken<CustomResponse<NoContent>>("Category/InsertCategory", categoryCreateInput);
            return HandleResponseHelper.HandleResponse(categoryInsertResponse);
        }

        public async Task<CustomResponse<NoContent>> RemoveCategoryAsync(int categoryId)
        {
            var categoryRemoveResponse = await _httpService.HttpDeleteWithToken<CustomResponse<NoContent>>("Category/RemoveCategory", categoryId);
            return HandleResponseHelper.HandleResponse(categoryRemoveResponse);
        }

        public async Task<CustomResponse<NoContent>> UpdateCategoryAsync(CategoryUpdateInput categoryUpdateInput)
        {
            var categoryUpdateResponse = await _httpService.HttpPut<CustomResponse<NoContent>>("Category/UpdateCategory", categoryUpdateInput);
            return HandleResponseHelper.HandleResponse(categoryUpdateResponse);
        }
    }
}
