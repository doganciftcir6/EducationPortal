using EducationPortalApp.Web.Models.AppUserModels;
using EducationPortalApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.Web.Controllers
{
    public class AppUserController : Controller
    {
        private readonly IAppUserService _appUserService;
        public AppUserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(AppUserChangePasswordInput appUserChangePasswordInput)
        {
            var response = await _appUserService.ChangePasswordAsync(appUserChangePasswordInput);
            if (response.Errors is not null)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> ProfileDetails()
        {
            var response = await _appUserService.GetProfileAsync();
            if (response.Errors is not null)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View();
            }
            return View(response.Data);
        }
    }
}
