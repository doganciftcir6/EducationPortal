using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Models.CourseContentTypeModels;

namespace EducationPortalApp.Web.Services.Interfaces
{
    public interface ICourseContentTypeService
    {
        Task<CustomResponse<IEnumerable<CourseContentTypeVM>>> GetCourseContentTypesAsync();
    }
}
