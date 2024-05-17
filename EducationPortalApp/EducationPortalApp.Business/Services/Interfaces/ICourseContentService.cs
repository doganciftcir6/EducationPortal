using EducationPortalApp.Dtos.CourseContentDtos;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Interfaces
{
    public interface ICourseContentService
    {
        Task<CustomResponse<IEnumerable<CourseContentDto>>> GetAllCourseContentByCourseIdAsync(int courseId);
        Task<CustomResponse<IEnumerable<CourseContentDto>>> GetAllCourseContentAsync();
        Task<CustomResponse<CourseContentDto>> GetByIdCourseContentAsync(int courseContentId);
        Task<CustomResponse<NoContent>> InsertCourseContentAsync(CourseContentCreateDto courseContentCreateDto, CancellationToken cancellationToken);
        Task<CustomResponse<NoContent>> UpdateCourseContentAsync(CourseContentUpdateDto courseContentUpdateDto, CancellationToken cancellationToken);
        Task<CustomResponse<NoContent>> RemoveCourseContentAsync(int courseContentId);
        Task<CustomResponse<NoContent>> UpdateCourseContentStatusAsync(int courseContentId, bool isChecked);
    }
}
