using EducationPortalApp.Dtos.EnrollmentDtos;
using FluentValidation;

namespace EducationPortalApp.Business.ValidationRules.FluentValidation.EnrollmentValidations
{
    public class EnrollmentUpdateDtoValidator : AbstractValidator<EnrollmentUpdateDto>
    {
        public EnrollmentUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("The id field cannot be empty!");
            RuleFor(x => x.CourseId).NotEmpty().WithMessage("The course field cannot be empty!");
            RuleFor(x => x.AppUserId).NotEmpty().WithMessage("The appuser field cannot be empty!");
        }
    }
}
