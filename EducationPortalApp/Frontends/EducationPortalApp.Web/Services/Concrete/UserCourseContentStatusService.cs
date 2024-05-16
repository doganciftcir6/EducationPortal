using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Helpers.HttpHelpers;
using EducationPortalApp.Web.Models.UserCourseContentStatusModels;
using EducationPortalApp.Web.Services.Interfaces;

namespace EducationPortalApp.Web.Services.Concrete
{
    public class UserCourseContentStatusService : IUserCourseContentStatusService
    {
        private readonly HttpService _httpService;
        public UserCourseContentStatusService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<CustomResponse<IEnumerable<UserCourseContentStatusVM>>> GetContentStatusByUserAsync()
        {
            var userCourseContentStatuesResponse = await _httpService.HttpGetWithToken<CustomResponse<IEnumerable<UserCourseContentStatusVM>>>("UserCourseContentStatus/GetUserCourseContentStatus");
            return HandleResponseHelper.HandleResponse(userCourseContentStatuesResponse);
        }
    }
}
