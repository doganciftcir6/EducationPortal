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
        public EnrollmentRequestService(IUow uow, IEnrollmentRequestRepository enrollmentRequestRepository, IMapper mapper, IValidator<EnrollmentRequestCreateDto> enrollmentRequestCreateDtoValidator, IValidator<EnrollmentRequestUpdateDto> enrollmentRequestUpdateDtoValidator, ISharedIdentityService sharedIdentityService)
        {
            _uow = uow;
            _enrollmentRequestRepository = enrollmentRequestRepository;
            _mapper = mapper;
            _enrollmentRequestCreateDtoValidator = enrollmentRequestCreateDtoValidator;
            _enrollmentRequestUpdateDtoValidator = enrollmentRequestUpdateDtoValidator;
            _sharedIdentityService = sharedIdentityService;
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
            //Kurs kapasite kontrolü
            var course = await _uow.GetRepository<Course>().GetByIdAsync(enrollmentRequestCreateDto.CourseId);
            if (course == null)
            {
                return CustomResponse<NoContent>.Fail(CourseMessages.NOT_FOUND_COURSE, ResponseStatusCode.NOT_FOUND);
            }
            if (course.Capacity == course.MaxCapacity && enrollmentRequestCreateDto.EnrollmentRequestStatusId == (int)EnrollmentRequestStatusEnum.Participation)
            {
                return CustomResponse<NoContent>.Fail(EnrollmentRequestMessages.COURSE_CAPACİTY_ERROR, ResponseStatusCode.BAD_REQUEST);
            }

            //Aynı statustan kayıt daha önce yapılmış mı
            var existingRequests = await _uow.GetRepository<EnrollmentRequest>().GetAllFilterAsync(x => x.AppUserId == _sharedIdentityService.GetUserId && x.CourseId == enrollmentRequestCreateDto.CourseId && x.EnrollmentRequestStatusId == enrollmentRequestCreateDto.EnrollmentRequestStatusId);
            if (existingRequests.Any())
                return CustomResponse<NoContent>.Fail(EnrollmentRequestMessages.EXİST_ENROLLMENTRQ_ERROR, ResponseStatusCode.BAD_REQUEST);


            //Kullanıcının ilgili kursa kayıtlı olup olmadığını kontrol et
            var enrollmentExists = await _uow.GetRepository<Enrollment>()
                                             .GetAllFilterAsync(x => x.AppUserId == _sharedIdentityService.GetUserId && x.CourseId == enrollmentRequestCreateDto.CourseId);
            //Eğer ilgili kursa kayıt yoksa ve enrollmentRequestStatusu Cancellation ise hata döndür
            if (!enrollmentExists.Any() && enrollmentRequestCreateDto.EnrollmentRequestStatusId == (int)EnrollmentRequestStatusEnum.Cancellation)
            {
                return CustomResponse<NoContent>.Fail(EnrollmentRequestMessages.NOT_ENROLLED_TO_CANCEL, ResponseStatusCode.BAD_REQUEST);
            }

            //Eğer ilgili kursa kayıt varsa ve enrollmentRequestStatusu Participation ise hata döndür
            if (enrollmentExists.Any() && enrollmentRequestCreateDto.EnrollmentRequestStatusId == (int)EnrollmentRequestStatusEnum.Participation)
            {
                return CustomResponse<NoContent>.Fail(EnrollmentRequestMessages.ALREADY_ENROLLED, ResponseStatusCode.BAD_REQUEST);
            }


            var validationResult = _enrollmentRequestCreateDtoValidator.Validate(enrollmentRequestCreateDto);
            if (validationResult.IsValid)
            {
                EnrollmentRequest enrollmentRequest = _mapper.Map<EnrollmentRequest>(enrollmentRequestCreateDto);
                enrollmentRequest.AppUserId = (int)_sharedIdentityService.GetUserId;
                await _uow.GetRepository<EnrollmentRequest>().InsertAsync(enrollmentRequest);
                await _uow.SaveChangesAsync();
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            return CustomResponse<NoContent>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }

        public async Task<CustomResponse<NoContent>> RemoveEnrollmentRequestAsync(int enrollmentRequestId)
        {
            EnrollmentRequest enrollmentRequest = await _uow.GetRepository<EnrollmentRequest>().GetByIdAsync(enrollmentRequestId);
            if (enrollmentRequest != null)
            {
                _uow.GetRepository<EnrollmentRequest>().Delete(enrollmentRequest);
                await _uow.SaveChangesAsync();
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            return CustomResponse<NoContent>.Fail(EnrollmentRequestMessages.NOT_FOUND_ENROLLMENT_REQUEST, ResponseStatusCode.NOT_FOUND);
        }

        public async Task<CustomResponse<NoContent>> UpdateEnrollmentRequestAsync(EnrollmentRequestUpdateDto enrollmentRequestUpdateDto)
        {
            var validationResult = _enrollmentRequestUpdateDtoValidator.Validate(enrollmentRequestUpdateDto);
            if (validationResult.IsValid)
            {
                EnrollmentRequest oldData = await _uow.GetRepository<EnrollmentRequest>().AsNoTrackingGetByFilterAsync(x => x.Id == enrollmentRequestUpdateDto.Id);
                if (oldData == null)
                    return CustomResponse<NoContent>.Fail(EnrollmentRequestMessages.NOT_FOUND_ENROLLMENT_REQUEST, ResponseStatusCode.NOT_FOUND);

                EnrollmentRequest enrollmentRequest = _mapper.Map<EnrollmentRequest>(enrollmentRequestUpdateDto);
                enrollmentRequest.AppUserId = (int)_sharedIdentityService.GetUserId;
                _uow.GetRepository<EnrollmentRequest>().Update(enrollmentRequest);
                await _uow.SaveChangesAsync();
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            return CustomResponse<NoContent>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }
    }
}
