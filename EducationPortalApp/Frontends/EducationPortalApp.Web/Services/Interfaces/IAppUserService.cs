using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Models.AppUserModels;

namespace EducationPortalApp.Web.Services.Interfaces
{
    public interface IAppUserService
    {
        Task<CustomResponse<AppUserProfileVM>> GetProfileAsync();
        Task<CustomResponse<NoContent>> ChangePasswordAsync(AppUserChangePasswordInput appUserChangePasswordInput);
    }
}
