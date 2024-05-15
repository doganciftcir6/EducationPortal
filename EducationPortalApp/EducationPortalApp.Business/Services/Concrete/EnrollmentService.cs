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

        public async Task<CustomResponse<IEnumerable<EnrollmentDto>>> GetAllEnrollmentByUserIdAsync(int userId)
        {
            IEnumerable<EnrollmentDto> enrollmentDtos = _mapper.Map<IEnumerable<EnrollmentDto>>(await _enrollmenRepository.GetAllFilterAsync(x => x.AppUserId == userId));
            return CustomResponse<IEnumerable<EnrollmentDto>>.Success(enrollmentDtos, ResponseStatusCode.OK);
        }

        public async Task<CustomResponse<NoContent>> InsertEnrollmentAsync(EnrollmentCreateDto enrollmentCreateDto, int enrollmentRequestId)
        {
            var validationResult = _enrollmentCreateDtoValidator.Validate(enrollmentCreateDto);
            if (validationResult.IsValid)
            {
                Enrollment enrollment = _mapper.Map<Enrollment>(enrollmentCreateDto);
                await _uow.GetRepository<Enrollment>().InsertAsync(enrollment);
                //Kapasite düşür
                var decreaseCourseCapacityResponse = await _courseService.DecreaseCourseCapacityAsync(enrollment.CourseId);
                if (decreaseCourseCapacityResponse.Errors != null)
                    return CustomResponse<NoContent>.Fail(decreaseCourseCapacityResponse.Errors, ResponseStatusCode.BAD_REQUEST);

                //İlgili enrollment requesti sil artık sonuç geldi
                await _enrollmentRequestService.RemoveEnrollmentRequestAsync(enrollmentRequestId);
                await _uow.SaveChangesAsync();
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            return CustomResponse<NoContent>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }

        public async Task<CustomResponse<NoContent>> RemoveEnrollmentAsync(int enrollmentId, int enrollmentRequestId)
        {
            Enrollment enrollment = await _uow.GetRepository<Enrollment>().GetByIdAsync(enrollmentId);
            if (enrollment != null)
            {
                _uow.GetRepository<Enrollment>().Delete(enrollment);

                //Kapasite arttır
                var increaseCoruseCapacityResponse = await _courseService.IncreaseCourseCapacityAsync(enrollment.CourseId);
                if (increaseCoruseCapacityResponse.Errors != null)
                    return CustomResponse<NoContent>.Fail(increaseCoruseCapacityResponse.Errors, ResponseStatusCode.BAD_REQUEST);
                //İlgili enrollment requesti sil artık sonuç geldi

                await _enrollmentRequestService.RemoveEnrollmentRequestAsync(enrollmentRequestId);
                await _uow.SaveChangesAsync();
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            return CustomResponse<NoContent>.Fail(EnrollmentMessages.NOT_FOUND_ENROLLMENT, ResponseStatusCode.NOT_FOUND);
        }

        public async Task<bool> UpdateEnrollmentCompletionStatusAsync(int courseId)
        {
            var courseContents = await _uow.GetRepository<CourseContent>().GetAllFilterAsync(x => x.CourseId == courseId);
            var enrollment = await _uow.GetRepository<Enrollment>().GetByFilterAsync(x => x.CourseId == courseId && x.AppUserId == _sharedIdentityService.GetUserId);

            if (courseContents == null || enrollment == null)
            {
                return false;
            }

            //Tüm kurs içeriği işaretlenmiş mi kontrol et
            bool allChecked = courseContents.All(cc => cc.Status);

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
