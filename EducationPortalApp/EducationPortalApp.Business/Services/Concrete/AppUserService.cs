using AutoMapper;
using EducationPortalApp.Business.Helpers.Messages;
using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Dtos.AppUserDtos;
using EducationPortalApp.Dtos.EnrollmentDtos;
using EducationPortalApp.Entities.UserEntities;
using EducationPortalApp.Shared.Services;
using EducationPortalApp.Shared.Utilities.Response;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EducationPortalApp.Business.Services.Concrete
{
    public class AppUserService : IAppUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMapper _mapper;
        private readonly IValidator<AppUserChangePasswordDto> _changePasswordValidator;
        private readonly IValidator<RoleAssingSendDto> _roleAssingSendDto;
        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger<AppUserService> _logger;
        public AppUserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ISharedIdentityService sharedIdentityService, IMapper mapper, IValidator<AppUserChangePasswordDto> changePasswordValidator, IValidator<RoleAssingSendDto> roleAssingSendDto, IEnrollmentService enrollmentService, ILogger<AppUserService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _sharedIdentityService = sharedIdentityService;
            _mapper = mapper;
            _changePasswordValidator = changePasswordValidator;
            _roleAssingSendDto = roleAssingSendDto;
            _enrollmentService = enrollmentService;
            _logger = logger;
        }

        public async Task<CustomResponse<NoContent>> AssingRoleAsync(RoleAssingSendDto roleAssingSendDto)
        {
            var validationResult = _roleAssingSendDto.Validate(roleAssingSendDto);
            if (validationResult.IsValid)
            {
                var currentUser = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == roleAssingSendDto.UserId);
                if (currentUser != null)
                {
                    var currentRole = await _roleManager.FindByNameAsync(roleAssingSendDto.RoleName);
                    if (currentRole == null)
                    {
                        _logger.LogError("AssingRole: Role {roleName} not found.", roleAssingSendDto.RoleName);
                        return CustomResponse<NoContent>.Fail(AppUserMessages.NOT_FOUND_ROLE, ResponseStatusCode.NOT_FOUND);
                    }

                    var userRoles = await _userManager.GetRolesAsync(currentUser);
                    if (userRoles.Count != 0)
                    {
                        await _userManager.RemoveFromRolesAsync(currentUser, userRoles);
                        await _userManager.AddToRoleAsync(currentUser, roleAssingSendDto.RoleName);
                    }
                    await _userManager.AddToRoleAsync(currentUser, roleAssingSendDto.RoleName);

                    _logger.LogInformation("AssingRole: Role {roleName} assigned to user {userId}.", roleAssingSendDto.RoleName, roleAssingSendDto.UserId);
                    return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
                }
                _logger.LogError("AssingRole: User with ID {userId} not found.", roleAssingSendDto.UserId);
                return CustomResponse<NoContent>.Fail(AppUserMessages.NOT_FOUND_USER, ResponseStatusCode.NOT_FOUND);
            }
            _logger.LogError("AssingRole: Role assignment failed due to validation errors: {errors}", validationResult.Errors);
            return CustomResponse<NoContent>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }

        public async Task<CustomResponse<NoContent>> ChangePasswordAsync(AppUserChangePasswordDto appUserChangePassword)
        {
            var validationResult = _changePasswordValidator.Validate(appUserChangePassword);
            if (validationResult.IsValid)
            {
                var currentUser = await _userManager.FindByIdAsync(_sharedIdentityService.GetUserId.ToString());
                if (currentUser != null)
                {
                    var checkPassword = await _userManager.CheckPasswordAsync(currentUser, appUserChangePassword.CurrentPassword);
                    if (checkPassword)
                    {
                        await _userManager.ChangePasswordAsync(currentUser, appUserChangePassword.CurrentPassword, appUserChangePassword.NewPassword);
                        _logger.LogInformation("ChangePassword: Password changed successfully for user {userId}.", _sharedIdentityService.GetUserId);
                        return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
                    }
                    _logger.LogError("ChangePassword: Current password is incorrect for user {userId}.", _sharedIdentityService.GetUserId);
                    return CustomResponse<NoContent>.Fail(AppUserMessages.WRONG_CURRENT_PASSWORD, ResponseStatusCode.BAD_REQUEST);
                }
                _logger.LogError("ChangePassword: User not found.");
                return CustomResponse<NoContent>.Fail(AppUserMessages.NOT_FOUND_USER, ResponseStatusCode.NOT_FOUND);
            }
            _logger.LogError("ChangePassword: Password change failed due to validation errors: {errors}", validationResult.Errors);
            return CustomResponse<NoContent>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }

        public async Task<CustomResponse<IEnumerable<AppUserDto>>> GetAppUsersAsync()
        {
            IEnumerable<AppUserDto> appUserDto = _mapper.Map<IEnumerable<AppUserDto>>(await _userManager.Users.ToListAsync());
            return CustomResponse<IEnumerable<AppUserDto>>.Success(appUserDto, ResponseStatusCode.OK);
        }

        public async Task<CustomResponse<ProfileDto>> GetProfileAsync()
        {
            var userId = _sharedIdentityService.GetUserId;

            //Kullanıcı bilgilerini al
            var userInfo = await _userManager.Users.Include(u => u.Gender)
                                                   .FirstOrDefaultAsync(u => u.Id == userId);
            if (userInfo == null)
            {
                _logger.LogError("GetProfile: User not found.");
                return CustomResponse<ProfileDto>.Fail(AppUserMessages.NOT_FOUND_USER, ResponseStatusCode.NOT_FOUND);
            }

            //Kullanıcının eğitimlerini al
            var enrollments = await _enrollmentService.GetAllEnrollmentByUserAsync();
            var profileDto = new ProfileDto
            {
                User = _mapper.Map<AppUserDto>(userInfo),
                Enrollments = _mapper.Map<List<EnrollmentDto>>(enrollments.Data)
            };

            return CustomResponse<ProfileDto>.Success(profileDto, ResponseStatusCode.OK);
        }

        public async Task<CustomResponse<List<RoleDto>>> GetRolesAsync(string userId)
        {
            var currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser != null)
            {
                var roleAssingDtos = new List<RoleDto>();
                var userRoles = await _userManager.GetRolesAsync(currentUser);

                foreach (var roleName in userRoles)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);
                    if (role != null)
                    {
                        roleAssingDtos.Add(new()
                        {
                            RoleId = role.Id,
                            Name = role.Name,
                        });
                    }
                }
                return CustomResponse<List<RoleDto>>.Success(roleAssingDtos, ResponseStatusCode.OK);
            }
            _logger.LogError("GetRoles: User not found with ID {userId}.", userId);
            return CustomResponse<List<RoleDto>>.Fail(AppUserMessages.NOT_FOUND_USER, ResponseStatusCode.NOT_FOUND);
        }
    }
}
