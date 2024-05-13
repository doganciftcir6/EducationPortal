using EducationPortalApp.Dtos.EnrollmentRequestDtos;
using FluentValidation;

namespace EducationPortalApp.Business.ValidationRules.FluentValidation.EnrollmentRequestValidations
{
    public class EnrollmentRequestUpdateDtoValidator : AbstractValidator<EnrollmentRequestUpdateDto>
    {
        public EnrollmentRequestUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("The id field cannot be empty!");
            RuleFor(x => x.CourseId).NotEmpty().WithMessage("The course field cannot be empty!");
            RuleFor(x => x.EnrollmentRequestStatusId).NotEmpty().WithMessage("The enrollment request status field cannot be empty!");
        }
    }
}
