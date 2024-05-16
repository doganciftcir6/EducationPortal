using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Models.CategoryModels;

namespace EducationPortalApp.Web.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CustomResponse<IEnumerable<CategoryVM>>> GetCategoriesAsync();
    }
}
