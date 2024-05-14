using AutoMapper;
using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.DataAccess.UnitOfWork;
using EducationPortalApp.Dtos.EnrollmentDtos;
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
        private readonly IValidator<EnrollmentUpdateDto> _enrollmentUpdateDtoValidator;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly ICourseService _courseService;
        private readonly IEnrollmentRequestService _enrollmentRequestService;
        public EnrollmentService(IUow uow, IEnrollmentRepository enrollmenRepository, IMapper mapper, IValidator<EnrollmentCreateDto> enrollmentCreateDtoValidator, IValidator<EnrollmentUpdateDto> enrollmentUpdateDtoValidator, ISharedIdentityService sharedIdentityService, ICourseService courseService, IEnrollmentRequestService enrollmentRequestService)
        {
            _uow = uow;
            _enrollmenRepository = enrollmenRepository;
            _mapper = mapper;
            _enrollmentCreateDtoValidator = enrollmentCreateDtoValidator;
            _enrollmentUpdateDtoValidator = enrollmentUpdateDtoValidator;
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

        public Task<CustomResponse<NoContent>> RemoveEnrollmentAsync(int enrollmentId)
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponse<NoContent>> UpdateEnrollmentAsync(EnrollmentUpdateDto enrollmentUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
