using EducationPortalApp.Dtos.CourseDtos;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Interfaces
{
    public interface ICourseService
    {
        Task<CustomResponse<IEnumerable<CoursesDto>>> GetCoursesAsync();
        Task<CustomResponse<CourseDto>> GetCourseByIdAsync(int courseId);
        Task<CustomResponse<NoContent>> InsertCourseAsync(CourseCreateDto courseCreateDto, CancellationToken cancellationToken);
        Task<CustomResponse<NoContent>> UpdateCourseAsync(CourseUpdateDto courseUpdateDto, CancellationToken cancellationToken);
        Task<CustomResponse<NoContent>> RemoveCourseAsync(int courseId);
        Task<CustomResponse<NoContent>> DecreaseCourseCapacityAsync(int courseId);
        Task<CustomResponse<NoContent>> IncreaseCourseCapacityAsync(int courseId);
    }
}
