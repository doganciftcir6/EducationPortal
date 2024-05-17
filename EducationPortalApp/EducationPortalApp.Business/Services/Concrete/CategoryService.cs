using AutoMapper;
using EducationPortalApp.Business.Helpers.Messages;
using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.DataAccess.UnitOfWork;
using EducationPortalApp.Dtos.CategoryDtos;
using EducationPortalApp.Entities;
using EducationPortalApp.Shared.Utilities.Response;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EducationPortalApp.Business.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryCreateDto> _categoryCreateDtoValidator;
        private readonly IValidator<CategoryUpdateDto> _categoryUpdateDtoValidator;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(IUow uow, IMapper mapper, IValidator<CategoryCreateDto> categoryCreateDtoValidator, IValidator<CategoryUpdateDto> categoryUpdateDtoValidator, ILogger<CategoryService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _categoryCreateDtoValidator = categoryCreateDtoValidator;
            _categoryUpdateDtoValidator = categoryUpdateDtoValidator;
            _logger = logger;
        }

        public async Task<CustomResponse<CategoryDto>> GetCategoryByIdAsync(int categoryId)
        {
            CategoryDto categoryDto = _mapper.Map<CategoryDto>(await _uow.GetRepository<Category>().AsNoTrackingGetByFilterAsync(x => x.Id == categoryId));
            if (categoryDto is not null)
            {
                return CustomResponse<CategoryDto>.Success(categoryDto, ResponseStatusCode.OK);

            }
            _logger.LogWarning("GetCategoryById: Category not found with categoryId: {categoryId}", categoryId);
            return CustomResponse<CategoryDto>.Fail(CategoryMessages.NOT_FOUND_CATEGORY, ResponseStatusCode.NOT_FOUND);
        }

        public async Task<CustomResponse<IEnumerable<CategoryDto>>> GetCategoriesAsync()
        {
            IEnumerable<CategoryDto> categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(await _uow.GetRepository<Category>().GetAllAsync());
            return CustomResponse<IEnumerable<CategoryDto>>.Success(categoryDtos, ResponseStatusCode.OK);
        }

        public async Task<CustomResponse<NoContent>> InsertCategoryAsync(CategoryCreateDto categoryCreateDto)
        {
            _logger.LogDebug("InsertCategoryInput: {@categoryCreateDto}", categoryCreateDto);
            var validationResult = _categoryCreateDtoValidator.Validate(categoryCreateDto);
            if (validationResult.IsValid)
            {
                Category category = _mapper.Map<Category>(categoryCreateDto);
                _logger.LogDebug("InsertCategoryEntity: {@category}", category);
                await _uow.GetRepository<Category>().InsertAsync(category);
                await _uow.SaveChangesAsync();
                _logger.LogInformation("InsertCategory: Category successfully inserted with Id: {categoryId}", category.Id);
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            _logger.LogError("Category creation failed due to validation errors: {errors}", validationResult.Errors);
            return CustomResponse<NoContent>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }

        public async Task<CustomResponse<NoContent>> RemoveCategoryAsync(int categoryId)
        {
            Category category = await _uow.GetRepository<Category>().GetByIdAsync(categoryId);
            if (category != null)
            {
                _uow.GetRepository<Category>().Delete(category);
                await _uow.SaveChangesAsync();
                _logger.LogInformation("RemoveCategory: Category with Id {categoryId} has been successfully deleted.", categoryId);
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            _logger.LogWarning("RemoveCategory: Category not found with categoryId: {categoryId}", categoryId);
            return CustomResponse<NoContent>.Fail(CategoryMessages.NOT_FOUND_CATEGORY, ResponseStatusCode.NOT_FOUND);
        }

        public async Task<CustomResponse<NoContent>> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto)
        {
            _logger.LogDebug("UpdateCategoryInput: {@categoryUpdateDto}", categoryUpdateDto);
            var validationResult = _categoryUpdateDtoValidator.Validate(categoryUpdateDto);
            if (validationResult.IsValid)
            {
                Category oldData = await _uow.GetRepository<Category>().AsNoTrackingGetByFilterAsync(x => x.Id == categoryUpdateDto.Id);
                if (oldData == null)
                {
                    _logger.LogWarning("RemoveCategory: Category not found with categoryId: {categoryId}", categoryUpdateDto.Id);
                    return CustomResponse<NoContent>.Fail(CategoryMessages.NOT_FOUND_CATEGORY, ResponseStatusCode.NOT_FOUND);
                }

                Category category = _mapper.Map<Category>(categoryUpdateDto);
                _logger.LogDebug("UpdateCategoryEntity: {@category}", category);

                _uow.GetRepository<Category>().Update(category);
                await _uow.SaveChangesAsync();
                _logger.LogInformation("UpdateCategory: Category successfully updated with Id: {categoryId}", category.Id);
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            _logger.LogError("Category update failed due to validation errors: {errors}", validationResult.Errors);
            return CustomResponse<NoContent>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }
    }
}
