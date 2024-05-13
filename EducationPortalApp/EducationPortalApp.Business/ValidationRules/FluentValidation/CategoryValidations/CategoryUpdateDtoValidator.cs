using EducationPortalApp.Dtos.CategoryDtos;
using FluentValidation;

namespace EducationPortalApp.Business.ValidationRules.FluentValidation.CategoryValidations
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("The id field cannot be empty!");
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("The definition field cannot be empty!");
            RuleFor(x => x.CategoryName).MinimumLength(2).WithMessage("The definition field must contain at least 2 characters!");
            RuleFor(x => x.CategoryName).MaximumLength(200).WithMessage("The definition field can contain a maximum of 200 characters!");
        }
    }
}
