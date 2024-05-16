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
    }
}
