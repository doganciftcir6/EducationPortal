using EducationPortalApp.Dtos.CourseContentDtos;
using FluentValidation;

namespace EducationPortalApp.Business.ValidationRules.FluentValidation.CourseContentValidations
{
    public class CourseContentUpdateDtoValidator : AbstractValidator<CourseContentUpdateDto>
    {
        public CourseContentUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("The id field cannot be empty!");
            RuleFor(x => x.Name).NotEmpty().WithMessage("The course content name field cannot be empty!");
            RuleFor(x => x.Name).MinimumLength(2).WithMessage("The course content name field must contain at least 2 characters!");
            RuleFor(x => x.Name).MaximumLength(200).WithMessage("The course content name field can contain a maximum of 200 characters!");
            RuleFor(x => x.CourseId).NotEmpty().WithMessage("The course field cannot be empty!");
            RuleFor(x => x.CourseContentTypeId).NotEmpty().WithMessage("The course content type field cannot be empty!");
        }
    }
}
