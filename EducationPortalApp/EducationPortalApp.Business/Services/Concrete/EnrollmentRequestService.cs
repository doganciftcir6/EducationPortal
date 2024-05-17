using AutoMapper;
using EducationPortalApp.Business.Helpers.Messages;
using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.DataAccess.UnitOfWork;
using EducationPortalApp.Dtos.EnrollmentRequestDtos;
using EducationPortalApp.Entities.CourseEntities;
using EducationPortalApp.Entities.EnrollmentEntities;
using EducationPortalApp.Shared.Enums;
using EducationPortalApp.Shared.Services;
using EducationPortalApp.Shared.Utilities.Response;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EducationPortalApp.Business.Services.Concrete
{
    public class EnrollmentRequestService : IEnrollmentRequestService
    {
        private readonly IUow _uow;
        private readonly IEnrollmentRequestRepository _enrollmentRequestRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<EnrollmentRequestCreateDto> _enrollmentRequestCreateDtoValidator;
        private readonly IValidator<EnrollmentRequestUpdateDto> _enrollmentRequestUpdateDtoValidator;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly ILogger<EnrollmentRequestService> _logger;
        public EnrollmentRequestService(IUow uow, IEnrollmentRequestRepository enrollmentRequestRepository, IMapper mapper, IValidator<EnrollmentRequestCreateDto> enrollmentRequestCreateDtoValidator, IValidator<EnrollmentRequestUpdateDto> enrollmentRequestUpdateDtoValidator, ISharedIdentityService sharedIdentityService, ILogger<EnrollmentRequestService> logger)
        {
            _uow = uow;
            _enrollmentRequestRepository = enrollmentRequestRepository;
            _mapper = mapper;
            _enrollmentRequestCreateDtoValidator = enrollmentRequestCreateDtoValidator;
            _enrollmentRequestUpdateDtoValidator = enrollmentRequestUpdateDtoValidator;
            _sharedIdentityService = sharedIdentityService;
            _logger = logger;
        }

        public async Task<CustomResponse<IEnumerable<ComplexEnrollmentRequestDto>>> GetAllEnrollmentRequestAsync()
        {
            var query = _uow.GetRepository<EnrollmentRequest>().GetQuery();
            IEnumerable<EnrollmentRequest> enrollmentRequests = await query.Include(x => x.Course).Include(x => x.AppUser).Include(x => x.EnrollmentRequestStatus).ToListAsync();
            IEnumerable<ComplexEnrollmentRequestDto> enrollmentRequestDtos = _mapper.Map<IEnumerable<ComplexEnrollmentRequestDto>>(enrollmentRequests);
            return CustomResponse<IEnumerable<ComplexEnrollmentRequestDto>>.Success(enrollmentRequestDtos, ResponseStatusCode.OK);
        }

        public async Task<CustomResponse<IEnumerable<EnrollmentRequestDto>>> GetAllEnrollmentRequestByUserAsync()
        {
            IEnumerable<EnrollmentRequestDto> enrollmentRequestDtos = _mapper.Map<IEnumerable<EnrollmentRequestDto>>(await _enrollmentRequestRepository.GetAllFilterAsync(x => x.AppUserId == _sharedIdentityService.GetUserId));
            return CustomResponse<IEnumerable<EnrollmentRequestDto>>.Success(enrollmentRequestDtos, ResponseStatusCode.OK);
        }

        public async Task<CustomResponse<NoContent>> InsertEnrollmentRequestAsync(EnrollmentRequestCreateDto enrollmentRequestCreateDto)
        {
            _logger.LogDebug("InsertEnrollmentRequestInput: {@enrollmentRequestCreateDto}", enrollmentRequestCreateDto);

            //Kurs kapasite kontrolü
            var course = await _uow.GetRepository<Course>().GetByIdAsync(enrollmentRequestCreateDto.CourseId);
            if (course == null)
            {
                _logger.LogWarning("InsertEnrollmentRequest: Course not found with courseId: {courseId}", enrollmentRequestCreateDto.CourseId);
                return CustomResponse<NoContent>.Fail(CourseMessages.NOT_FOUND_COURSE, ResponseStatusCode.NOT_FOUND);
            }
            if (course.Capacity == course.MaxCapacity && enrollmentRequestCreateDto.EnrollmentRequestStatusId == (int)EnrollmentRequestStatusEnum.Participation)
            {
                _logger.LogWarning("InsertEnrollmentRequest: Course capacity reached for courseId: {courseId}", enrollmentRequestCreateDto.CourseId);
                return CustomResponse<NoContent>.Fail(EnrollmentRequestMessages.COURSE_CAPACİTY_ERROR, ResponseStatusCode.BAD_REQUEST);
            }

            //Aynı statustan kayıt daha önce yapılmış mı
            var existingRequests = await _uow.GetRepository<EnrollmentRequest>().GetAllFilterAsync(x => x.AppUserId == _sharedIdentityService.GetUserId && x.CourseId == enrollmentRequestCreateDto.CourseId && x.EnrollmentRequestStatusId == enrollmentRequestCreateDto.EnrollmentRequestStatusId);
            if (existingRequests.Any())
            {
                _logger.LogWarning("InsertEnrollmentRequest: Existing enrollment request found for courseId: {courseId} and status: {statusId}", enrollmentRequestCreateDto.CourseId, enrollmentRequestCreateDto.EnrollmentRequestStatusId);
                return CustomResponse<NoContent>.Fail(EnrollmentRequestMessages.EXİST_ENROLLMENTRQ_ERROR, ResponseStatusCode.BAD_REQUEST);
            }


            //Kullanıcının ilgili kursa kayıtlı olup olmadığını kontrol et
            var enrollmentExists = await _uow.GetRepository<Enrollment>()
                                             .GetAllFilterAsync(x => x.AppUserId == _sharedIdentityService.GetUserId && x.CourseId == enrollmentRequestCreateDto.CourseId);
            //Eğer ilgili kursa kayıt yoksa ve enrollmentRequestStatusu Cancellation ise hata döndür
            if (!enrollmentExists.Any() && enrollmentRequestCreateDto.EnrollmentRequestStatusId == (int)EnrollmentRequestStatusEnum.Cancellation)
            {
                _logger.LogWarning("InsertEnrollmentRequest: User is not enrolled to cancel for courseId: {courseId}", enrollmentRequestCreateDto.CourseId);
                return CustomResponse<NoContent>.Fail(EnrollmentRequestMessages.NOT_ENROLLED_TO_CANCEL, ResponseStatusCode.BAD_REQUEST);
            }

            //Eğer ilgili kursa kayıt varsa ve enrollmentRequestStatusu Participation ise hata döndür
            if (enrollmentExists.Any() && enrollmentRequestCreateDto.EnrollmentRequestStatusId == (int)EnrollmentRequestStatusEnum.Participation)
            {
                _logger.LogWarning("InsertEnrollmentRequest: User is already enrolled for courseId: {courseId}", enrollmentRequestCreateDto.CourseId);
                return CustomResponse<NoContent>.Fail(EnrollmentRequestMessages.ALREADY_ENROLLED, ResponseStatusCode.BAD_REQUEST);
            }


            var validationResult = _enrollmentRequestCreateDtoValidator.Validate(enrollmentRequestCreateDto);
            if (validationResult.IsValid)
            {
                EnrollmentRequest enrollmentRequest = _mapper.Map<EnrollmentRequest>(enrollmentRequestCreateDto);
                _logger.LogDebug("InsertEnrollmentRequestEntity: {@enrollmentRequest}", enrollmentRequest);
                enrollmentRequest.AppUserId = (int)_sharedIdentityService.GetUserId;
                await _uow.GetRepository<EnrollmentRequest>().InsertAsync(enrollmentRequest);
                await _uow.SaveChangesAsync();
                _logger.LogInformation("InsertEnrollmentRequest: EnrollmentRequest successfully inserted with Id: {EnrollmentRequestId}", enrollmentRequest.Id);
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            _logger.LogError("EnrollmentRequest creation failed due to validation errors: {errors}", validationResult.Errors);
            return CustomResponse<NoContent>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }

        public async Task<CustomResponse<NoContent>> RemoveEnrollmentRequestAsync(int enrollmentRequestId)
        {
            EnrollmentRequest enrollmentRequest = await _uow.GetRepository<EnrollmentRequest>().GetByIdAsync(enrollmentRequestId);
            if (enrollmentRequest != null)
            {
                _uow.GetRepository<EnrollmentRequest>().Delete(enrollmentRequest);
                await _uow.SaveChangesAsync();
                _logger.LogInformation("RemoveEnrollmentRequest: Enrollment request with Id {enrollmentRequestId} has been successfully deleted.", enrollmentRequestId);
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            _logger.LogWarning("RemoveEnrollmentRequest: EnrollmentRequest not found with enrollmentRequestId: {enrollmentRequestId}", enrollmentRequestId);
            return CustomResponse<NoContent>.Fail(EnrollmentRequestMessages.NOT_FOUND_ENROLLMENT_REQUEST, ResponseStatusCode.NOT_FOUND);
        }

        public async Task<CustomResponse<NoContent>> UpdateEnrollmentRequestAsync(EnrollmentRequestUpdateDto enrollmentRequestUpdateDto)
        {
            _logger.LogDebug("UpdateEnrollmentRequestInput: {@enrollmentRequestUpdateDto}", enrollmentRequestUpdateDto);
            var validationResult = _enrollmentRequestUpdateDtoValidator.Validate(enrollmentRequestUpdateDto);
            if (validationResult.IsValid)
            {
                EnrollmentRequest oldData = await _uow.GetRepository<EnrollmentRequest>().AsNoTrackingGetByFilterAsync(x => x.Id == enrollmentRequestUpdateDto.Id);
                if (oldData == null)
                {
                    _logger.LogWarning("UpdateEnrollmentRequest: EnrollmentRequest not found with enrollmentRequestId: {enrollmentRequestId}", enrollmentRequestUpdateDto.Id);
                    return CustomResponse<NoContent>.Fail(EnrollmentRequestMessages.NOT_FOUND_ENROLLMENT_REQUEST, ResponseStatusCode.NOT_FOUND);
                }

                EnrollmentRequest enrollmentRequest = _mapper.Map<EnrollmentRequest>(enrollmentRequestUpdateDto);
                _logger.LogDebug("UpdateEnrollmentRequestEntity: {@enrollmentRequest}", enrollmentRequest);
                enrollmentRequest.AppUserId = (int)_sharedIdentityService.GetUserId;
                _uow.GetRepository<EnrollmentRequest>().Update(enrollmentRequest);
                await _uow.SaveChangesAsync();
                _logger.LogInformation("UpdateEnrollmentRequest: EnrollmentRequest successfully updated with Id: {enrollmentRequestId}", enrollmentRequest.Id);
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            _logger.LogError("EnrollmentRequest update failed due to validation errors: {errors}", validationResult.Errors);
            return CustomResponse<NoContent>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }
    }
}
