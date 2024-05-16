using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Models.UserCourseContentStatusModels;

namespace EducationPortalApp.Web.Services.Interfaces
{
    public interface IUserCourseContentStatusService
    {
        Task<CustomResponse<IEnumerable<UserCourseContentStatusVM>>> GetContentStatusByUserAsync();
    }
}
