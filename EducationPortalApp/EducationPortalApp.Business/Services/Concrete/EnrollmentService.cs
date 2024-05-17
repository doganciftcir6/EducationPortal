using AutoMapper;
using EducationPortalApp.Business.Helpers.Messages;
using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.DataAccess.UnitOfWork;
using EducationPortalApp.Dtos.EnrollmentDtos;
using EducationPortalApp.Entities.CourseEntities;
using EducationPortalApp.Entities.EnrollmentEntities;
using EducationPortalApp.Shared.Services;
using EducationPortalApp.Shared.Utilities.Response;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EducationPortalApp.Business.Services.Concrete
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUow _uow;
        private readonly IEnrollmentRepository _enrollmenRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<EnrollmentCreateDto> _enrollmentCreateDtoValidator;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly ICourseService _courseService;
        private readonly IEnrollmentRequestService _enrollmentRequestService;
        private readonly ILogger<EnrollmentService> _logger;
        public EnrollmentService(IUow uow, IEnrollmentRepository enrollmenRepository, IMapper mapper, IValidator<EnrollmentCreateDto> enrollmentCreateDtoValidator, ISharedIdentityService sharedIdentityService, ICourseService courseService, IEnrollmentRequestService enrollmentRequestService, ILogger<EnrollmentService> logger)
        {
            _uow = uow;
            _enrollmenRepository = enrollmenRepository;
            _mapper = mapper;
            _enrollmentCreateDtoValidator = enrollmentCreateDtoValidator;
            _sharedIdentityService = sharedIdentityService;
            _courseService = courseService;
            _enrollmentRequestService = enrollmentRequestService;
            _logger = logger;
        }

        public async Task<CustomResponse<IEnumerable<EnrollmentDto>>> GetAllEnrollmentAsync()
        {
            IEnumerable<EnrollmentDto> enrollmentDtos = _mapper.Map<IEnumerable<EnrollmentDto>>(await _enrollmenRepository.GetAllAsync());
            return CustomResponse<IEnumerable<EnrollmentDto>>.Success(enrollmentDtos, ResponseStatusCode.OK);
        }

        public async Task<CustomResponse<IEnumerable<EnrollmentDto>>> GetAllEnrollmentByUserAsync()
        {
            IEnumerable<EnrollmentDto> enrollmentDtos = _mapper.Map<IEnumerable<EnrollmentDto>>(await _enrollmenRepository.GetAllFilterAsync(x => x.AppUserId == _sharedIdentityService.GetUserId));
            return CustomResponse<IEnumerable<EnrollmentDto>>.Success(enrollmentDtos, ResponseStatusCode.OK);
        }

        public async Task<CustomResponse<EnrollmentDto>> GetEnrollmentByUserIdAndCourseIdAsync(int userId, int courseId)
        {
            EnrollmentDto enrollmentDto = _mapper.Map<EnrollmentDto>(await _enrollmenRepository.GetByFilterAsync(x => x.AppUserId == userId && x.CourseId == courseId));
            return CustomResponse<EnrollmentDto>.Success(enrollmentDto, ResponseStatusCode.OK);
        }

        public async Task<CustomResponse<NoContent>> InsertEnrollmentAsync(EnrollmentCreateDto enrollmentCreateDto, int enrollmentRequestId)
        {
            _logger.LogDebug("InsertEnrollmentInput: {@enrollmentCreateDto} and enrollmentRequestId: {enrollmentRequestId}", enrollmentCreateDto, enrollmentRequestId);
            var enrollments = await _uow.GetRepository<Enrollment>().GetAllFilterAsync(x => x.AppUserId == enrollmentCreateDto.AppUserId && x.CourseId == enrollmentCreateDto.CourseId);
            if (enrollments.Any())
            {
                _logger.LogWarning("InsertEnrollment: User is already enrolled in the course with courseId: {courseId}", enrollmentCreateDto.CourseId);
                return CustomResponse<NoContent>.Fail(EnrollmentMessages.ALREADY_ENROLLED_ERROR, ResponseStatusCode.BAD_REQUEST);
            }

            var validationResult = _enrollmentCreateDtoValidator.Validate(enrollmentCreateDto);
            if (validationResult.IsValid)
            {
                Enrollment enrollment = _mapper.Map<Enrollment>(enrollmentCreateDto);
                _logger.LogDebug("InsertEnrollmentEntity: {@enrollment}", enrollment);
                await _uow.GetRepository<Enrollment>().InsertAsync(enrollment);
                //Kapasite arttır
                var increaseCourseCapacityResponse = await _courseService.IncreaseCourseCapacityAsync(enrollment.CourseId);
                if (increaseCourseCapacityResponse.Errors != null)
                {
                    _logger.LogError("InsertEnrollment: Failed to increase course capacity for courseId: {courseId}. Error: {errors}", enrollment.CourseId, increaseCourseCapacityResponse.Errors);
                    return CustomResponse<NoContent>.Fail(increaseCourseCapacityResponse.Errors, ResponseStatusCode.BAD_REQUEST);
                }

                //İlgili enrollment requesti sil artık sonuç geldi
                await _enrollmentRequestService.RemoveEnrollmentRequestAsync(enrollmentRequestId);
                await _uow.SaveChangesAsync();
                _logger.LogInformation("InsertEnrollment: Enrollment successfully inserted for user with userId: {userId} and courseId: {courseId}", enrollmentCreateDto.AppUserId, enrollmentCreateDto.CourseId);
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            _logger.LogError("Enrollment creation failed due to validation errors: {errors}", validationResult.Errors);
            return CustomResponse<NoContent>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }

        public async Task<CustomResponse<NoContent>> RemoveEnrollmentAsync(int courseId, int appUserId, int enrollmentRequestId)
        {
            Enrollment enrollment = await _uow.GetRepository<Enrollment>().GetByFilterAsync(x => x.CourseId == courseId && x.AppUserId == appUserId);
            if (enrollment != null)
            {
                _uow.GetRepository<Enrollment>().Delete(enrollment);

                //Kapasite azalt
                var decreaseCoruseCapacityResponse = await _courseService.DecreaseCourseCapacityAsync(enrollment.CourseId);
                if (decreaseCoruseCapacityResponse.Errors != null)
                {
                    _logger.LogError("RemoveEnrollment: Failed to decrease course capacity for courseId: {courseId}. Error: {errors}", enrollment.CourseId, decreaseCoruseCapacityResponse.Errors);
                    return CustomResponse<NoContent>.Fail(decreaseCoruseCapacityResponse.Errors, ResponseStatusCode.BAD_REQUEST);
                }
                //İlgili enrollment requesti sil artık sonuç geldi

                await _enrollmentRequestService.RemoveEnrollmentRequestAsync(enrollmentRequestId);
                await _uow.SaveChangesAsync();
                _logger.LogInformation("RemoveEnrollment: Enrollment successfully removed for user with userId: {userId} from course with courseId: {courseId}", appUserId, courseId);
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            _logger.LogWarning("RemoveEnrollment: User with userId: {userId} is not enrolled in course with courseId: {courseId}", appUserId, courseId);
            return CustomResponse<NoContent>.Fail(EnrollmentMessages.USER_NOT_ENROLLED_ERROR, ResponseStatusCode.NOT_FOUND);
        }

        public async Task<bool> UpdateEnrollmentCompletionStatusAsync(int courseContentId, int courseId)
        {
            var courseContentStatuses = await _uow.GetRepository<UserCourseContentStatus>().GetAllFilterAsync(x => x.CourseContentId == courseContentId);
            var enrollment = await _uow.GetRepository<Enrollment>().GetByFilterAsync(x => x.CourseId == courseId && x.AppUserId == _sharedIdentityService.GetUserId);

            if (courseContentStatuses == null || enrollment == null)
            {
                _logger.LogWarning("UpdateEnrollmentCompletionStatus: Unable to update completion status. Course content statuses or enrollment not found for courseContentId: {courseContentId} and courseId: {courseId}", courseContentId, courseId);
                return false;
            }

            //Tüm kurs içeriği işaretlenmiş mi kontrol et
            bool allChecked = courseContentStatuses.All(cc => cc.IsCompleted);

            //Eğer tüm kurs içeriği işaretlenmişse IsCompleted alanını true olarak güncelle
            if (allChecked)
            {
                enrollment.IsCompleted = true;
            }
            else
            {
                enrollment.IsCompleted = false;
            }
            await _uow.SaveChangesAsync();
            _logger.LogInformation("UpdateEnrollmentCompletionStatus: Enrollment completion status updated successfully for courseContentId: {courseContentId} and courseId: {courseId}. IsCompleted: {isCompleted}", courseContentId, courseId, enrollment.IsCompleted);

            return true;
        }
    }
}
