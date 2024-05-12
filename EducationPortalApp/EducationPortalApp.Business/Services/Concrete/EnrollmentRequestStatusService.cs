using AutoMapper;
using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.DataAccess.UnitOfWork;
using EducationPortalApp.Dtos.EnrollmentRequestStatusDtos;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Concrete
{
    public class EnrollmentRequestStatusService : IEnrollmentRequestStatusService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        public EnrollmentRequestStatusService(IUow uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<CustomResponse<IEnumerable<EnrollmentRequestStatusDto>>> GetEnrollmentRequestStatusesAsync()
        {
            IEnumerable<EnrollmentRequestStatusDto> enrollmentRequestStatusDtos = _mapper.Map<IEnumerable<EnrollmentRequestStatusDto>>(await _uow.GetRepository<EnrollmentRequestStatusDto>().GetAllAsync());
            return CustomResponse<IEnumerable<EnrollmentRequestStatusDto>>.Success(enrollmentRequestStatusDtos, ResponseStatusCode.OK);
        }
    }
}
