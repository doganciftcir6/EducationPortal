using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Dtos.AppUserDtos;
using EducationPortalApp.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register(AppUserRegisterDto appUserRegisterDto)
        {
            return CreateActionResultInstance(await _authService.RegisterWithRoleAsync(appUserRegisterDto));
            ;
        }
        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login(AppUserLoginDto appUserLoginDto)
        {
            return CreateActionResultInstance(await _authService.LoginAsync(appUserLoginDto));

        }
    }
}
