using EducationPortalApp.Dtos.AppUserDtos;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Interfaces
{
    public interface IAppUserService
    {
        Task<CustomResponse<AppUserDto>> GetProfileAsync();
        Task<CustomResponse<IEnumerable<AppUserDto>>> GetAppUsersAsync();
        Task<CustomResponse<NoContent>> ChangePasswordAsync(AppUserChangePasswordDto appUserChangePassword);
        Task<CustomResponse<List<RoleDto>>> GetRolesAsync(string userId);
        Task<CustomResponse<NoContent>> AssingRoleAsync(RoleAssingSendDto roleAssingSendDto);
    }
}
