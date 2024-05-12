using AutoMapper;
using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.DataAccess.UnitOfWork;
using EducationPortalApp.Dtos.GenderDtos;
using EducationPortalApp.Entities.UserEntities;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Concrete
{
    public class GenderService : IGenderService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        public GenderService(IUow uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<CustomResponse<IEnumerable<GenderDto>>> GetGendersAsync()
        {
            IEnumerable<GenderDto> genderDto = _mapper.Map<IEnumerable<GenderDto>>(await _uow.GetRepository<Gender>().GetAllAsync());
            return CustomResponse<IEnumerable<GenderDto>>.Success(genderDto, ResponseStatusCode.OK);
        }
    }
}
