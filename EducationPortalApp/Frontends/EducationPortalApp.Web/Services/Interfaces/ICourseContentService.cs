using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Models.CourseContentModels;

namespace EducationPortalApp.Web.Services.Interfaces
{
    public interface ICourseContentService
    {
        Task<CustomResponse<IEnumerable<CourseContentVM>>> GetAllCourseContentByCourseIdAsync(int courseId);
        Task<CustomResponse<IEnumerable<CourseContentVM>>> GetAllCourseContentAsync();
        Task<CustomResponse<CourseContentVM>> GetCourseContentByIdAsync(int courseContentId);
        Task<CustomResponse<NoContent>> UpdateCourseContentStatusAsync(int courseContentId, bool isChecked);
        Task<CustomResponse<NoContent>> InsertCourseContentAsync(CourseContentCreateInput courseContentCreateInput);
        Task<CustomResponse<NoContent>> UpdateCourseContentAsync(CourseContentUpdateInput courseContentUpdateInput);
        Task<CustomResponse<NoContent>> RemoveCourseContentAsync(int courseContentId);
    }
}
