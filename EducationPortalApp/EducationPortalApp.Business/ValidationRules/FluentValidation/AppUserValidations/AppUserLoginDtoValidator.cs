using EducationPortalApp.Dtos.AppUserDtos;
using FluentValidation;

namespace EducationPortalApp.Business.ValidationRules.FluentValidation.AppUserValidations
{
    public class AppUserLoginDtoValidator : AbstractValidator<AppUserLoginDto>
    {
        public AppUserLoginDtoValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("The username field cannot be empty!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("The password field cannot be empty!");
        }
    }
}
