using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Models.AppUserModels;

namespace EducationPortalApp.Web.Services.Interfaces
{
    public interface IAuthService
    {
        Task<CustomResponse<AppUserLoginVM>> LoginAsync(AppUserLoginInput appUserLoginInput);
        Task<CustomResponse<string>> RegisterAsync(AppUserRegisterInput appUserRegisterInput);
        Task Logout();
    }
}
