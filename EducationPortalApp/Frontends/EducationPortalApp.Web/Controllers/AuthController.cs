using EducationPortalApp.Web.Models.AppUserModels;
using EducationPortalApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace EducationPortalApp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IGenderService _genderService;
        public AuthController(IAuthService authService, IGenderService genderService)
        {
            _authService = authService;
            _genderService = genderService;
        }

        public IActionResult Login()
        {
            return View(new AppUserLoginInput());
        }
        [HttpPost]
        public async Task<IActionResult> Login(AppUserLoginInput appUserLoginInput)
        {
            if (!ModelState.IsValid)
                return View(appUserLoginInput);

            var response = await _authService.LoginAsync(appUserLoginInput);
            if (response.Errors is not null)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(appUserLoginInput);
            }
            return RedirectToAction(nameof(Index), "Home");

        }

        public async Task<IActionResult> Register()
        {
            var gendersData = await _genderService.GetGendersAsync();
            var model = new AppUserRegisterInput();
            model.Genders = new SelectList(gendersData.Data, "Id", "Definition");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(AppUserRegisterInput appUserRegisterInput)
        {
            if (!ModelState.IsValid)
            {
                var data = TempData["genders"]?.ToString();
                if (data != null)
                {
                    var genders = JsonSerializer.Deserialize<List<SelectListItem>>(data);
                    appUserRegisterInput.Genders = new SelectList(genders, "Value", "Text");
                }
                return View(appUserRegisterInput);
            }

            var response = await _authService.RegisterAsync(appUserRegisterInput);
            if (response.Errors is not null)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                var data = TempData["genders"]?.ToString();
                if (data != null)
                {
                    var genders = JsonSerializer.Deserialize<List<SelectListItem>>(data);
                    appUserRegisterInput.Genders = new SelectList(genders, "Value", "Text");
                }
                return View(appUserRegisterInput);
            }
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
