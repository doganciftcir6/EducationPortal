using EducationPortalApp.Dtos.EnrollmentRequestDtos;
using FluentValidation;

namespace EducationPortalApp.Business.ValidationRules.FluentValidation.EnrollmentRequestValidations
{
    public class EnrollmentRequestCreateDtoValidator : AbstractValidator<EnrollmentRequestCreateDto>
    {
        public EnrollmentRequestCreateDtoValidator()
        {
            RuleFor(x => x.CourseId).NotEmpty().WithMessage("The course field cannot be empty!");
            RuleFor(x => x.EnrollmentRequestStatusId).NotEmpty().WithMessage("The enrollment request status field cannot be empty!");
        }
    }
}
