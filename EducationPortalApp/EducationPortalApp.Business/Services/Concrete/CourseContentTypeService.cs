using AutoMapper;
using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.DataAccess.UnitOfWork;
using EducationPortalApp.Dtos.CourseContentTypeDtos;
using EducationPortalApp.Entities.CourseEntities;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Concrete
{
    public class CourseContentTypeService : ICourseContentTypeService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        public CourseContentTypeService(IUow uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<CustomResponse<IEnumerable<CourseContentTypeDto>>> GetCourseContentTypesAsync()
        {
            IEnumerable<CourseContentTypeDto> courseContentTypeDtos = _mapper.Map<IEnumerable<CourseContentTypeDto>>(await _uow.GetRepository<CourseContentType>().GetAllAsync());
            return CustomResponse<IEnumerable<CourseContentTypeDto>>.Success(courseContentTypeDtos, ResponseStatusCode.OK);
        }
    }
}
