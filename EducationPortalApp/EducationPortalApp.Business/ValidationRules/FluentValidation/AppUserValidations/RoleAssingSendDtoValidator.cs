using EducationPortalApp.Dtos.AppUserDtos;
using FluentValidation;

namespace EducationPortalApp.Business.ValidationRules.FluentValidation.AppUserValidations
{
    public class RoleAssingSendDtoValidator : AbstractValidator<RoleAssingSendDto>
    {
        public RoleAssingSendDtoValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("The role name field cannot be empty!");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("The user field cannot be empty!");
        }
    }
}
