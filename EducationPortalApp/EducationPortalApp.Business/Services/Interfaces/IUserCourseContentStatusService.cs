using EducationPortalApp.Entities.CourseEntities;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Interfaces
{
    public interface IUserCourseContentStatusService
    {
        Task<CustomResponse<IEnumerable<UserCourseContentStatus>>> GetContentStatusByUserAsync();
    }
}
