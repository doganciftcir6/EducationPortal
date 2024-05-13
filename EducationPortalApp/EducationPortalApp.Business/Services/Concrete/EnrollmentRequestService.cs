using AutoMapper;
using EducationPortalApp.Business.Helpers.Messages;
using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.DataAccess.UnitOfWork;
using EducationPortalApp.Dtos.EnrollmentRequestDtos;
using EducationPortalApp.Entities.EnrollmentEntities;
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

        public async Task<CustomResponse<IEnumerable<EnrollmentRequestDto>>> GetAllEnrollmentRequestAsync()
        {
            var query = _uow.GetRepository<EnrollmentRequest>().GetQuery();
            IEnumerable<EnrollmentRequest> enrollmentRequests = await query.Include(x => x.Course).Include(x => x.AppUser).Include(x => x.EnrollmentRequestStatus).ToListAsync();
            IEnumerable<EnrollmentRequestDto> enrollmentRequestDtos = _mapper.Map<IEnumerable<EnrollmentRequestDto>>(enrollmentRequests);
            return CustomResponse<IEnumerable<EnrollmentRequestDto>>.Success(enrollmentRequestDtos, ResponseStatusCode.OK);
        }

        public async Task<CustomResponse<IEnumerable<EnrollmentRequestDto>>> GetAllEnrollmentRequestByUserAsync()
        {
            IEnumerable<EnrollmentRequestDto> enrollmentRequestDtos = _mapper.Map<IEnumerable<EnrollmentRequestDto>>(await _enrollmentRequestRepository.GetAllFilterAsync(x => x.AppUserId == _sharedIdentityService.GetUserId));
            return CustomResponse<IEnumerable<EnrollmentRequestDto>>.Success(enrollmentRequestDtos, ResponseStatusCode.OK);
        }

        public async Task<CustomResponse<NoContent>> InsertEnrollmentRequestAsync(EnrollmentRequestCreateDto enrollmentRequestCreateDto)
        {
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
