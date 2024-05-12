using EducationPortalApp.Dtos.AppUserDtos;
using EducationPortalApp.Dtos.TokenDtos;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<CustomResponse<string>> RegisterWithRoleAsync(AppUserRegisterDto appUserRegisterDto);
        Task<CustomResponse<TokenResponseDto>> LoginAsync(AppUserLoginDto appUserLoginDto);
    }
}
