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
        public EnrollmentService(IUow uow, IEnrollmentRepository enrollmenRepository, IMapper mapper, IValidator<EnrollmentCreateDto> enrollmentCreateDtoValidator, ISharedIdentityService sharedIdentityService, ICourseService courseService, IEnrollmentRequestService enrollmentRequestService)
        {
            _uow = uow;
            _enrollmenRepository = enrollmenRepository;
            _mapper = mapper;
            _enrollmentCreateDtoValidator = enrollmentCreateDtoValidator;
            _sharedIdentityService = sharedIdentityService;
            _courseService = courseService;
            _enrollmentRequestService = enrollmentRequestService;
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
            var validationResult = _enrollmentCreateDtoValidator.Validate(enrollmentCreateDto);
            if (validationResult.IsValid)
            {
                Enrollment enrollment = _mapper.Map<Enrollment>(enrollmentCreateDto);
                await _uow.GetRepository<Enrollment>().InsertAsync(enrollment);
                //Kapasite arttır
                var increaseCourseCapacityResponse = await _courseService.IncreaseCourseCapacityAsync(enrollment.CourseId);
                if (increaseCourseCapacityResponse.Errors != null)
                    return CustomResponse<NoContent>.Fail(increaseCourseCapacityResponse.Errors, ResponseStatusCode.BAD_REQUEST);

                //İlgili enrollment requesti sil artık sonuç geldi
                await _enrollmentRequestService.RemoveEnrollmentRequestAsync(enrollmentRequestId);
                await _uow.SaveChangesAsync();
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
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
                    return CustomResponse<NoContent>.Fail(decreaseCoruseCapacityResponse.Errors, ResponseStatusCode.BAD_REQUEST);
                //İlgili enrollment requesti sil artık sonuç geldi

                await _enrollmentRequestService.RemoveEnrollmentRequestAsync(enrollmentRequestId);
                await _uow.SaveChangesAsync();
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            return CustomResponse<NoContent>.Fail(EnrollmentMessages.NOT_FOUND_ENROLLMENT, ResponseStatusCode.NOT_FOUND);
        }

        public async Task<bool> UpdateEnrollmentCompletionStatusAsync(int courseContentId, int courseId)
        {
            var courseContentStatuses = await _uow.GetRepository<UserCourseContentStatus>().GetAllFilterAsync(x => x.CourseContentId == courseContentId);
            var enrollment = await _uow.GetRepository<Enrollment>().GetByFilterAsync(x => x.CourseId == courseId && x.AppUserId == _sharedIdentityService.GetUserId);

            if (courseContentStatuses == null || enrollment == null)
            {
                return false;
            }

            //Tüm kurs içeriği işaretlenmiş mi kontrol et
            bool allChecked = courseContentStatuses.All(cc => cc.IsCompleted);

            //Eğer tüm kurs içeriği işaretlenmişse, IsCompleted alanını true olarak güncelle
            if (allChecked)
            {
                enrollment.IsCompleted = true;
            }
            else
            {
                enrollment.IsCompleted = false;
            }
            await _uow.SaveChangesAsync();

            return true;
        }
    }
}
