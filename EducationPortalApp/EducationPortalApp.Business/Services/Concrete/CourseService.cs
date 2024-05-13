using AutoMapper;
using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.DataAccess.UnitOfWork;
using EducationPortalApp.Dtos.CourseDtos;
using EducationPortalApp.Shared.Utilities.Response;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EducationPortalApp.Business.Services.Concrete
{
    public class CourseService : ICourseService
    {
        private readonly IUow _uow;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CourseCreateDto> _courseCreateDtoValidator;
        private readonly IValidator<CourseUpdateDto> _courseUpdateDtoValidator;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public CourseService(IUow uow, ICourseRepository courseRepository, IMapper mapper, IValidator<CourseCreateDto> courseCreateDtoValidator, IValidator<CourseUpdateDto> courseUpdateDtoValidator, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _uow = uow;
            _courseRepository = courseRepository;
            _mapper = mapper;
            _courseCreateDtoValidator = courseCreateDtoValidator;
            _courseUpdateDtoValidator = courseUpdateDtoValidator;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public Task<CustomResponse<CourseDto>> GetCourseByIdAsync(int courseId)
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponse<IEnumerable<CoursesDto>>> GetCoursesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponse<NoContent>> InsertCourseAsync(CourseCreateDto courseCreateDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponse<NoContent>> RemoveCourseAsync(int courseId)
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponse<NoContent>> UpdateCourseAsync(CourseUpdateDto courseUpdateDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
