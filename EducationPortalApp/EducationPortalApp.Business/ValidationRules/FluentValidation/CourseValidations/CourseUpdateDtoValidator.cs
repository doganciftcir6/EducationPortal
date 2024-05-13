using EducationPortalApp.Dtos.CourseDtos;
using FluentValidation;

namespace EducationPortalApp.Business.ValidationRules.FluentValidation.CourseValidations
{
    public class CourseUpdateDtoValidator : AbstractValidator<CourseUpdateDto>
    {
        public CourseUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("The id field cannot be empty!");
            RuleFor(x => x.Name).NotEmpty().WithMessage("The course name field cannot be empty!");
            RuleFor(x => x.Name).MinimumLength(2).WithMessage("The course name field must contain at least 2 characters!");
            RuleFor(x => x.Name).MaximumLength(200).WithMessage("The course name field can contain a maximum of 200 characters!");
            RuleFor(x => x.Instructor).NotEmpty().WithMessage("The instructor field cannot be empty!");
            RuleFor(x => x.Instructor).MinimumLength(2).WithMessage("The instructor field must contain at least 2 characters!");
            RuleFor(x => x.Instructor).MaximumLength(150).WithMessage("The instructor field can contain a maximum of 150 characters!");
            RuleFor(x => x.Description).NotEmpty().WithMessage("The description field cannot be empty!");
            RuleFor(x => x.Description).MinimumLength(2).WithMessage("The description field must contain at least 2 characters!");
            RuleFor(x => x.Description).MaximumLength(500).WithMessage("The description field can contain a maximum of 500 characters!");
            RuleFor(x => x.CostPerDay).NotEmpty().WithMessage("The costperday field cannot be empty!");
            RuleFor(x => x.Capacity).NotEmpty().WithMessage("The capacity field cannot be empty!");
            RuleFor(x => x.DurationInHours).NotEmpty().WithMessage("The durationinhours field cannot be empty!");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("The category field cannot be empty!");
        }
    }
}
