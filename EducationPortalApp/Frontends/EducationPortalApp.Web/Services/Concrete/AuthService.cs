using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Helpers.HttpHelpers;
using EducationPortalApp.Web.Models.AppUserModels;
using EducationPortalApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EducationPortalApp.Web.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly HttpService _httpService;
        private readonly IHttpContextAccessor _contextAccessor;
        public AuthService(HttpService httpService, IHttpContextAccessor contextAccessor)
        {
            _httpService = httpService;
            _contextAccessor = contextAccessor;
        }

        public async Task<CustomResponse<AppUserLoginVM>> LoginAsync(AppUserLoginInput appUserLoginInput)
        {
            var loginResponse = await _httpService.HttpPost<CustomResponse<AppUserLoginVM>>("Auth/LoginUser", appUserLoginInput);
            //Login
            if (loginResponse != null && loginResponse.Errors == null)
            {
                var httpContext = _contextAccessor.HttpContext;
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    //kullanıcı zaten oturum açmış bu yüzden işlemi devam ettir
                    return loginResponse;
                }

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                //responstan gelen tokeni oku
                var token = handler.ReadJwtToken(loginResponse.Data.Token);
                var claims = token.Claims.ToList();
                if (loginResponse.Data.Token != null)
                    claims.Add(new Claim("accessToken", loginResponse.Data.Token));
                var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                var authProps = new AuthenticationProperties
                {
                    ExpiresUtc = loginResponse.Data.ExpireDate,
                    IsPersistent = true,
                };
                await httpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProps);
            }
            return HandleResponseHelper.HandleResponse(loginResponse);
        }


        public async Task Logout()
        {
            var httpContext = _contextAccessor.HttpContext;
            await httpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
        }

        public async Task<CustomResponse<string>> RegisterAsync(AppUserRegisterInput appUserRegisterInput)
        {
            var registerResponse = await _httpService.HttpPost<CustomResponse<string>>("Auth/RegisterUser", appUserRegisterInput);
            return HandleResponseHelper.HandleResponse(registerResponse);
        }
    }
}
