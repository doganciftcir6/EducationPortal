using AutoMapper;
using EducationPortalApp.Business.Helpers.Messages;
using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Dtos.AppUserDtos;
using EducationPortalApp.Dtos.TokenDtos;
using EducationPortalApp.Entities.UserEntities;
using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Shared.Utilities.Security;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace EducationPortalApp.Business.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IValidator<AppUserRegisterDto> _AppUserRegisterDtoValidator;
        private readonly IValidator<AppUserLoginDto> _AppUserLoginDtoValidator;
        private readonly IMapper _mapper;
        public AuthService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IValidator<AppUserRegisterDto> appUserRegisterDtoValidator, IValidator<AppUserLoginDto> appUserLoginDtoValidator, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _AppUserRegisterDtoValidator = appUserRegisterDtoValidator;
            _AppUserLoginDtoValidator = appUserLoginDtoValidator;
            _mapper = mapper;
        }

        public async Task<CustomResponse<TokenResponseDto>> LoginAsync(AppUserLoginDto appUserLoginDto)
        {
            var validationResult = _AppUserLoginDtoValidator.Validate(appUserLoginDto);
            if (validationResult.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(appUserLoginDto.Username);
                if (user != null)
                {
                    var checkPassword = await _userManager.CheckPasswordAsync(user, appUserLoginDto.Password);
                    if (checkPassword)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        AppUserDto appUserDto = _mapper.Map<AppUserDto>(user);
                        //Login başarılı token üret ve tokeni dön.
                        var token = JwtTokenGenerator.GenerateToken(appUserDto, roles);
                        return CustomResponse<TokenResponseDto>.Success(token, ResponseStatusCode.OK);
                    }
                }
                return CustomResponse<TokenResponseDto>.Fail(
                    AppUserMessages.LOGİN_FAİLED, ResponseStatusCode.BAD_REQUEST);
            }
            return CustomResponse<TokenResponseDto>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }

        public async Task<CustomResponse<string>> RegisterWithRoleAsync(AppUserRegisterDto appUserRegisterDto)
        {
            var validationResult = _AppUserRegisterDtoValidator.Validate(appUserRegisterDto);
            if (validationResult.IsValid)
            {
                var appUser = _mapper.Map<AppUser>(appUserRegisterDto);

                var registerResult = await _userManager.CreateAsync(appUser, appUserRegisterDto.Password);
                if (registerResult.Succeeded)
                {
                    var memberRole = await _roleManager.FindByNameAsync("Member");
                    if (memberRole == null)
                    {
                        await _roleManager.CreateAsync(new()
                        {
                            Name = "Member",
                        });
                    }
                    await _userManager.AddToRoleAsync(appUser, "Member");
                    return CustomResponse<string>.Success(AppUserMessages.SUCCESS_REGİSTER, ResponseStatusCode.CREATED);
                }
                return CustomResponse<string>.Fail(registerResult.Errors.Select(x => x.Description).ToList(), ResponseStatusCode.BAD_REQUEST);
            }
            return CustomResponse<string>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }
    }
}
