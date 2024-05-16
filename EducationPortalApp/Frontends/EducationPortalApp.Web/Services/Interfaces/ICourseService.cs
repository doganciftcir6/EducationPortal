using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Models.CourseModels;

namespace EducationPortalApp.Web.Services.Interfaces
{
    public interface ICourseService
    {
        Task<CustomResponse<IEnumerable<CoursesVM>>> GetCoursesAsync();
        Task<CustomResponse<CourseVM>> GetCourseDetailAsync(int courseId);
        Task<CustomResponse<NoContent>> InsertCourseAsync(CourseCreateInput courseCreateInput);
        Task<CustomResponse<NoContent>> UpdateCourseAsync(CourseUpdateInput courseUpdateInput);
        Task<CustomResponse<NoContent>> RemoveCourseAsync(int courseId);
    }
}
