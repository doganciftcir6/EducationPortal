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
using Microsoft.Extensions.Logging;

namespace EducationPortalApp.Business.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IValidator<AppUserRegisterDto> _AppUserRegisterDtoValidator;
        private readonly IValidator<AppUserLoginDto> _AppUserLoginDtoValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        public AuthService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IValidator<AppUserRegisterDto> appUserRegisterDtoValidator, IValidator<AppUserLoginDto> appUserLoginDtoValidator, IMapper mapper, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _AppUserRegisterDtoValidator = appUserRegisterDtoValidator;
            _AppUserLoginDtoValidator = appUserLoginDtoValidator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CustomResponse<TokenResponseDto>> LoginAsync(AppUserLoginDto appUserLoginDto)
        {
            _logger.LogDebug("LoginInput: {@appUserLoginDto}", appUserLoginDto);
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
                        _logger.LogInformation("Login: User {username} logged in successfully.", appUserLoginDto.Username);
                        return CustomResponse<TokenResponseDto>.Success(token, ResponseStatusCode.OK);
                    }
                }
                _logger.LogWarning("Login: Login failed for user {username}. Invalid username or password.", appUserLoginDto.Username);
                return CustomResponse<TokenResponseDto>.Fail(
                    AppUserMessages.LOGİN_FAİLED, ResponseStatusCode.BAD_REQUEST);
            }
            _logger.LogError("Login: Login failed due to validation errors: {errors}", validationResult.Errors);
            return CustomResponse<TokenResponseDto>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }

        public async Task<CustomResponse<string>> RegisterWithRoleAsync(AppUserRegisterDto appUserRegisterDto)
        {
            _logger.LogDebug("RegisterWithRoleInput: {@appUserRegisterDto}", appUserRegisterDto);
            var validationResult = _AppUserRegisterDtoValidator.Validate(appUserRegisterDto);
            if (validationResult.IsValid)
            {
                var appUser = _mapper.Map<AppUser>(appUserRegisterDto);
                _logger.LogDebug("RegisterWithRoleEntity: {@appUser}", appUser);

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
                    _logger.LogInformation("RegisterWithRole: User {username} registered successfully.", appUser.UserName);
                    return CustomResponse<string>.Success(AppUserMessages.SUCCESS_REGİSTER, ResponseStatusCode.CREATED);
                }
                _logger.LogError("RegisterWithRole: Registration failed for user {username}. Errors: {errors}", appUser.UserName, registerResult.Errors.Select(x => x.Description));
                return CustomResponse<string>.Fail(registerResult.Errors.Select(x => x.Description).ToList(), ResponseStatusCode.BAD_REQUEST);
            }
            _logger.LogError("RegisterWithRole: Registration failed due to validation errors: {errors}", validationResult.Errors);
            return CustomResponse<string>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }
    }
}
