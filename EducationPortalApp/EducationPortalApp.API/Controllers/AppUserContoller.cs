using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Dtos.AppUserDtos;
using EducationPortalApp.Shared.ControllerBases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppUserContoller : CustomBaseController
    {
        private readonly IAppUserService _appUserService;
        public AppUserContoller(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProfile()
        {
            return CreateActionResultInstance(await _appUserService.GetProfileAsync());
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAppUsers()
        {
            return CreateActionResultInstance(await _appUserService.GetAppUsersAsync());
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetRoles(int userId)
        {
            return CreateActionResultInstance(await _appUserService.GetRolesAsync(userId.ToString()));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AssingRole(RoleAssingSendDto roleAssingSendDto)
        {
            return CreateActionResultInstance(await _appUserService.AssingRoleAsync(roleAssingSendDto));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ChangePassword(AppUserChangePasswordDto appUserChangePasswordDto)
        {
            return CreateActionResultInstance(await _appUserService.ChangePasswordAsync(appUserChangePasswordDto));
        }
    }
}
