using EducationPortalApp.Dtos.AppUserDtos;
using FluentValidation;

namespace EducationPortalApp.Business.ValidationRules.FluentValidation.AppUserValidations
{
    public class AppUserChangePasswordDtoValidator : AbstractValidator<AppUserChangePasswordDto>
    {
        public AppUserChangePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("The current password field cannot be empty!");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("The current password field cannot be empty!");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("The confirm password field cannot be empty!");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.NewPassword).WithMessage("Your passwords do not match!");
        }
    }
}
