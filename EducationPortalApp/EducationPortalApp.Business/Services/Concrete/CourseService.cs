using AutoMapper;
using EducationPortalApp.Business.Helpers.Messages;
using EducationPortalApp.Business.Helpers.UploadHelpers.Course;
using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.DataAccess.UnitOfWork;
using EducationPortalApp.Dtos.CourseDtos;
using EducationPortalApp.Entities.CourseEntities;
using EducationPortalApp.Shared.Utilities.Response;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<CourseService> _logger;
        public CourseService(IUow uow, ICourseRepository courseRepository, IMapper mapper, IValidator<CourseCreateDto> courseCreateDtoValidator, IValidator<CourseUpdateDto> courseUpdateDtoValidator, IHostingEnvironment hostingEnvironment, IConfiguration configuration, ILogger<CourseService> logger)
        {
            _uow = uow;
            _courseRepository = courseRepository;
            _mapper = mapper;
            _courseCreateDtoValidator = courseCreateDtoValidator;
            _courseUpdateDtoValidator = courseUpdateDtoValidator;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<CustomResponse<CourseDto>> GetCourseByIdAsync(int courseId)
        {
            CourseDto courseDto = _mapper.Map<CourseDto>(await _courseRepository.GetByFilterAsync(x => x.Id == courseId));
            if (courseDto is not null)
            {
                return CustomResponse<CourseDto>.Success(courseDto, ResponseStatusCode.OK);

            }
            _logger.LogWarning("GetCourseById: Course not found with courseId: {courseId}", courseId);
            return CustomResponse<CourseDto>.Fail(CourseMessages.NOT_FOUND_COURSE, ResponseStatusCode.NOT_FOUND);
        }

        public async Task<CustomResponse<IEnumerable<CoursesDto>>> GetCoursesAsync()
        {
            IEnumerable<CoursesDto> coursesDtos = _mapper.Map<IEnumerable<CoursesDto>>(await _courseRepository.GetAllAsync());
            return CustomResponse<IEnumerable<CoursesDto>>.Success(coursesDtos, ResponseStatusCode.OK);
        }

        public async Task<CustomResponse<NoContent>> InsertCourseAsync(CourseCreateDto courseCreateDto, CancellationToken cancellationToken)
        {
            _logger.LogDebug("InsertCourseInput: {@courseCreateDto}", courseCreateDto);
            var validationResult = _courseCreateDtoValidator.Validate(courseCreateDto);
            if (validationResult.IsValid)
            {
                Course course = _mapper.Map<Course>(courseCreateDto);
                _logger.LogDebug("InsertCourseEntity: {@course}", course);
                if (courseCreateDto.Picture != null && courseCreateDto.Picture.Length > 0)
                {
                    string createdPictureName = await CoursePictureUploadHelper.Run(_hostingEnvironment, courseCreateDto.Picture, _configuration, cancellationToken);
                    course.Picture = createdPictureName;
                }
                course.CreateDate = DateTime.Now;
                await _uow.GetRepository<Course>().InsertAsync(course);
                await _uow.SaveChangesAsync();
                _logger.LogInformation("InsertCourse: Course successfully inserted with Id: {courseId}", course.Id);
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            _logger.LogError("Course creation failed due to validation errors: {errors}", validationResult.Errors);
            return CustomResponse<NoContent>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }

        public async Task<CustomResponse<NoContent>> RemoveCourseAsync(int courseId)
        {
            Course course = await _uow.GetRepository<Course>().GetByIdAsync(courseId);
            if (course != null)
            {
                if (course.Picture != null && course.Picture.Length != 0)
                {
                    CoursePictureDeleteHelper.Delete(_hostingEnvironment, course.Picture);
                    course.Picture = String.Empty;
                }

                _uow.GetRepository<Course>().Delete(course);
                await _uow.SaveChangesAsync();
                _logger.LogInformation("RemoveCourse: Course with Id {courseId} has been successfully deleted.", courseId);
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            _logger.LogWarning("RemoveCourse: Course not found with courseId: {courseId}", courseId);
            return CustomResponse<NoContent>.Fail(CourseMessages.NOT_FOUND_COURSE, ResponseStatusCode.NOT_FOUND);
        }

        public async Task<CustomResponse<NoContent>> UpdateCourseAsync(CourseUpdateDto courseUpdateDto, CancellationToken cancellationToken)
        {
            _logger.LogDebug("UpdateCourseInput: {@courseUpdateDto}", courseUpdateDto);
            var validationResult = _courseUpdateDtoValidator.Validate(courseUpdateDto);
            if (validationResult.IsValid)
            {
                Course oldData = await _uow.GetRepository<Course>().AsNoTrackingGetByFilterAsync(x => x.Id == courseUpdateDto.Id);
                if (oldData == null)
                {
                    _logger.LogWarning("UpdateCourse: Course not found with courseId: {courseId}", courseUpdateDto.Id);
                    return CustomResponse<NoContent>.Fail(CourseMessages.NOT_FOUND_COURSE, ResponseStatusCode.NOT_FOUND);
                }

                Course course = _mapper.Map<Course>(courseUpdateDto);
                _logger.LogDebug("UpdateCourseEntity: {@course}", course);
                if (courseUpdateDto.Picture != null && courseUpdateDto.Picture.Length > 0)
                {
                    string createdPictureName = await CoursePictureUploadHelper.Run(_hostingEnvironment, courseUpdateDto.Picture, _configuration, cancellationToken);
                    course.Picture = createdPictureName;
                }
                else
                {
                    course.Picture = oldData.Picture;
                }

                _uow.GetRepository<Course>().Update(course);
                await _uow.SaveChangesAsync();
                _logger.LogInformation("UpdateCourse: Course successfully updated with Id: {courseId}", course.Id);
                return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
            }
            _logger.LogError("Course update failed due to validation errors: {errors}", validationResult.Errors);
            return CustomResponse<NoContent>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), ResponseStatusCode.BAD_REQUEST);
        }

        public async Task<CustomResponse<NoContent>> DecreaseCourseCapacityAsync(int courseId)
        {
            Course course = await _uow.GetRepository<Course>().GetByIdAsync(courseId);
            if (course != null)
            {
                if (course.Capacity > 0)
                {
                    //Kontenjanı bir azalt
                    course.Capacity--;
                    _uow.GetRepository<Course>().Update(course);
                    await _uow.SaveChangesAsync();
                    _logger.LogInformation("DecreaseCourseCapacity: Capacity decreased successfully for courseId: {courseId}. New capacity: {capacity}", courseId, course.Capacity);
                    return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
                }
                else
                {
                    _logger.LogWarning("DecreaseCourseCapacity: Failed to decrease capacity for courseId: {courseId}. Capacity is already zero.", courseId);
                    return CustomResponse<NoContent>.Fail(CourseMessages.ZERO_CAPACITY, ResponseStatusCode.BAD_REQUEST);
                }
            }
            _logger.LogWarning("DecreaseCourseCapacity: Course not found with courseId: {courseId}", courseId);
            return CustomResponse<NoContent>.Fail(CourseMessages.NOT_FOUND_COURSE, ResponseStatusCode.NOT_FOUND);
        }

        public async Task<CustomResponse<NoContent>> IncreaseCourseCapacityAsync(int courseId)
        {
            Course course = await _uow.GetRepository<Course>().GetByIdAsync(courseId);
            if(course == null)
            {
                _logger.LogWarning("IncreaseCourseCapacity: Course not found with courseId: {courseId}", courseId);
                return CustomResponse<NoContent>.Fail(CourseMessages.NOT_FOUND_COURSE, ResponseStatusCode.NOT_FOUND);
            }

            if (course.Capacity >= course.MaxCapacity)
            {
                _logger.LogWarning("IncreaseCourseCapacity: Failed to increase capacity for courseId: {courseId}. Maximum capacity reached.", courseId);
                return CustomResponse<NoContent>.Fail(CourseMessages.MAX_CAPACITY_REACHED, ResponseStatusCode.BAD_REQUEST);
            }

            //Kapasite dolu değilse kapasiteyi arttır
            course.Capacity++;
            _uow.GetRepository<Course>().Update(course);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("IncreaseCourseCapacity: Capacity increased successfully for courseId: {courseId}. New capacity: {capacity}", courseId, course.Capacity);
            return CustomResponse<NoContent>.Success(ResponseStatusCode.OK);
        }
    }
}
