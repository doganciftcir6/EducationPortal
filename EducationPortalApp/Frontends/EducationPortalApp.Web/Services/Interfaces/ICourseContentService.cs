using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Models.CourseContentModels;

namespace EducationPortalApp.Web.Services.Interfaces
{
    public interface ICourseContentService
    {
        Task<CustomResponse<IEnumerable<CourseContentVM>>> GetAllCourseContentByCourseIdAsync(int courseId);
        Task<CustomResponse<NoContent>> UpdateCourseContentStatusAsync(int courseContentId, bool isChecked);
    }
}
