using EducationPortalApp.Dtos.AppUserDtos;
using FluentValidation;

namespace EducationPortalApp.Business.ValidationRules.FluentValidation.AppUserValidations
{
    public class AppUserRegisterDtoValidator : AbstractValidator<AppUserRegisterDto>
    {
        public AppUserRegisterDtoValidator()
        {
            RuleFor(x => x.Firstname).NotEmpty().WithMessage("The name field cannot be empty!");
            RuleFor(x => x.Firstname).MaximumLength(30).WithMessage("The name field can contain a maximum of 30 characters!");
            RuleFor(x => x.Firstname).MinimumLength(2).WithMessage("The name field must contain at least 2 characters!");
            RuleFor(x => x.Lastname).NotEmpty().WithMessage("The lastname field cannot be empty!");
            RuleFor(x => x.Username).NotEmpty().WithMessage("The username field cannot be empty!");
            RuleFor(x => x.Email).NotEmpty().WithMessage("The email field cannot be empty!");
            RuleFor(x => x.Email).EmailAddress().WithMessage("You did not enter a valid email address!");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("The phone number field cannot be empty!");
            RuleFor(x => x.PhoneNumber).MinimumLength(11).WithMessage("The phone number field must contain at least 11 characters!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("The password field cannot be empty!");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("The confirm password field cannot be empty!");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Your passwords do not match!");
            RuleFor(x => x.GenderId).NotEmpty().WithMessage("The gender field cannot be empty!");
        }
    }
}
