using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Helpers.HttpHelpers;
using EducationPortalApp.Web.Models.AppUserModels;
using EducationPortalApp.Web.Services.Interfaces;

namespace EducationPortalApp.Web.Services.Concrete
{
    public class AppUserService : IAppUserService
    {
        private readonly HttpService _httpService;
        public AppUserService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<CustomResponse<NoContent>> ChangePasswordAsync(AppUserChangePasswordInput appUserChangePasswordInput)
        {
            var changePasswordResponse = await _httpService.HttpPutWithToken<CustomResponse<NoContent>>("AppUser/ChangePassword", appUserChangePasswordInput);
            return HandleResponseHelper.HandleResponse(changePasswordResponse);
        }

        public async Task<CustomResponse<AppUserProfileVM>> GetProfileAsync()
        {
            var profileResponse = await _httpService.HttpGetWithToken<CustomResponse<AppUserProfileVM>>("AppUser/GetProfile");
            return HandleResponseHelper.HandleResponse(profileResponse);
        }
    }
}
