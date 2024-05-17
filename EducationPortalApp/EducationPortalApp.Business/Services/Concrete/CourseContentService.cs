﻿using AutoMapper;
using EducationPortalApp.Business.Helpers.Messages;
using EducationPortalApp.Business.Helpers.UploadHelpers.CourseContent;
using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.DataAccess.UnitOfWork;
using EducationPortalApp.Dtos.CourseContentDtos;
using EducationPortalApp.Entities.CourseEntities;
using EducationPortalApp.Shared.Services;
using EducationPortalApp.Shared.Utilities.Response;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EducationPortalApp.Business.Services.Concrete
{
    public class CourseContentService : ICourseContentService
    {
        private readonly IUow _uow;
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CourseContentCreateDto> _courseContentCreateDtoValidator;
        private readonly IValidator<CourseContentUpdateDto> _courseContentUpdateDtoValidator;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IEnrollmentService _enrollmentService;
        private readonly ISharedIdentityService _sharedIdentityService;
        public CourseContentService(IUow uow, ICourseContentRepository courseContentRepository, IMapper mapper, IValidator<CourseContentCreateDto> courseContentCreateDtoValidator, IValidator<CourseContentUpdateDto> courseContentUpdateDtoValidator, IHostingEnvironment hostingEnvironment, IConfiguration configuration, IEnrollmentService enrollmentService, ISharedIdentityService sharedIdentityService)
        {
            _uow = uow;
            _courseContentRepository = courseContentRepository;
            _mapper = mapper;
            _courseContentCreateDtoValidator = courseContentCreateDtoValidator;
            _courseContentUpdateDtoValidator = courseContentUpdateDtoValidator;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _enrollmentService = enrollmentService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<CustomResponse<IEnumerable<CourseContentDto>>> GetAllCourseContentAsync()
        {
            IEnumerable<CourseContentDto> courseContentDtos = _mapper.Map<IEnumerable<CourseContentDto>>(await _courseContentRepository.GetAllAsync());
            return CustomResponse<IEnumerable<CourseContentDto>>.Success(courseContentDtos, ResponseStatusCode.OK);
        }

        public async Task<CustomResponse<IEnumerable<CourseContentDto>>> GetAllCourseContentByCourseIdAsync(int courseId)
        {
            var enrollmentResult = await _enrollmentService.GetEnrollmentByUserIdAndCourseIdAsync((int)_sharedIdentityService.GetUserId, courseId);
            if (enrollmentResult.Data is null)
                return CustomResponse<IEnumerable<CourseContentDto>>.Fail(CourseContentMessages.NOT_ENROLLED_IN_COURSE, ResponseStatusCode.BAD_REQUEST);

            IEnumerable<CourseContentDto> courseContentDtos = _mapper.Map<IEnumerable<CourseContentDto>>(await _courseContentRepository.GetAllFilterAsync(x => x.CourseId == courseId));
            return CustomResponse<IEnumerable<CourseContentDto>>.Success(courseContentDtos, ResponseStatusCode.OK);
        }

        public async Task<CustomResponse<CourseContentDto>> GetByIdCourseContentAsync(int courseContentId)
        {
            CourseContentDto courseContentDto = _mapper.Map<CourseContentDto>(await _courseContentRepository.GetByFilterAsync(x => x.Id == courseContentId));
            if (courseContentDto is not null)
            {
                return CustomResponse<CourseContentDto>.Success(courseContentDto, ResponseStatusCode.OK);

            }
            return CustomResponse<CourseContentDto>.Fail(CourseContentMessages.NOT_FOUND_COURSE_CONTENT, ResponseStatusCode.NOT_FOUND);
        }

        public async Task<CustomResponse<NoContent>> InsertCourseContentAsync(CourseContentCreateDto courseContentCreateDto, CancellationToken cancellationToken)
        {
            var validationResult = _courseContentCreateDtoValidator.Validate(courseContentCreateDto);
            if (validationResult.IsValid)
            {
                CourseContent courseContent = _mapper.Map<CourseContent>(courseContentCreateDto);
                if (courseContentCreateDto.File != null && courseContentCreateDto.File.Length > 0)
                {
                    string createdFileName = await CourseContentFileUploadHelper.Run(_hostingEnvironment, courseContentCreateDto.File, _configuration, cancellationToken);
                    courseContent.FilePath = createdFileName;
                }

                await _uow.GetRepository<CourseContent>().InsertAsync(courseContent);
                await _uow.SaveChangesAsync();
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            return CustomResponse<NoContent>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }

        public async Task<CustomResponse<NoContent>> RemoveCourseContentAsync(int courseContentId)
        {
            CourseContent courseContent = await _uow.GetRepository<CourseContent>().GetByIdAsync(courseContentId);
            if (courseContent != null)
            {
                if (courseContent.FilePath != null && courseContent.FilePath.Length != 0)
                {
                    CourseContentFileDeleteHelper.Delete(_hostingEnvironment, courseContent.FilePath);
                    courseContent.FilePath = String.Empty;
                }

                _uow.GetRepository<CourseContent>().Delete(courseContent);
                await _uow.SaveChangesAsync();
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            return CustomResponse<NoContent>.Fail(CourseContentMessages.NOT_FOUND_COURSE_CONTENT, ResponseStatusCode.NOT_FOUND);
        }

        public async Task<CustomResponse<NoContent>> UpdateCourseContentAsync(CourseContentUpdateDto courseContentUpdateDto, CancellationToken cancellationToken)
        {
            var validationResult = _courseContentUpdateDtoValidator.Validate(courseContentUpdateDto);
            if (validationResult.IsValid)
            {
                CourseContent oldData = await _uow.GetRepository<CourseContent>().AsNoTrackingGetByFilterAsync(x => x.Id == courseContentUpdateDto.Id);
                if (oldData == null)
                    return CustomResponse<NoContent>.Fail(CourseContentMessages.NOT_FOUND_COURSE_CONTENT, ResponseStatusCode.NOT_FOUND);

                CourseContent courseContent = _mapper.Map<CourseContent>(courseContentUpdateDto);
                if (courseContentUpdateDto.File != null && courseContentUpdateDto.File.Length > 0)
                {
                    string createdFileName = await CourseContentFileUploadHelper.Run(_hostingEnvironment, courseContentUpdateDto.File, _configuration, cancellationToken);
                    courseContent.FilePath = createdFileName;
                }
                else
                {
                    courseContent.FilePath = oldData.FilePath;
                }

                _uow.GetRepository<CourseContent>().Update(courseContent);
                await _uow.SaveChangesAsync();
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            return CustomResponse<NoContent>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }

        public async Task<CustomResponse<NoContent>> UpdateCourseContentStatusAsync(int courseContentId, bool isChecked)
        {
            var userCourseContentStatus = await _uow.GetRepository<UserCourseContentStatus>()
            .GetByFilterAsync(x => x.AppUserId == _sharedIdentityService.GetUserId && x.CourseContentId == courseContentId);

            if (userCourseContentStatus == null)
            {
                //Kullanıcı için bu kurs içeriği durumu henüz oluşturulmamışsa yeni bir kayıt oluştur
                userCourseContentStatus = new UserCourseContentStatus
                {
                    AppUserId = (int)_sharedIdentityService.GetUserId,
                    CourseContentId = courseContentId,
                    IsCompleted = isChecked
                };
                await _uow.GetRepository<UserCourseContentStatus>().InsertAsync(userCourseContentStatus);
            }
            else
            {
                //Kullanıcı için bu kurs içeriği durumunu güncelle
                userCourseContentStatus.IsCompleted = isChecked;
                _uow.GetRepository<UserCourseContentStatus>().Update(userCourseContentStatus);
            }

            await _uow.SaveChangesAsync();
            return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
        }
    }
}
