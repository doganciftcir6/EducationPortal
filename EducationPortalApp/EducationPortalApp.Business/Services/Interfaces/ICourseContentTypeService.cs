using EducationPortalApp.Dtos.CourseContentTypeDtos;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Interfaces
{
    public interface ICourseContentTypeService
    {
        Task<CustomResponse<IEnumerable<CourseContentTypeDto>>> GetCourseContentTypesAsync();
    }
}
