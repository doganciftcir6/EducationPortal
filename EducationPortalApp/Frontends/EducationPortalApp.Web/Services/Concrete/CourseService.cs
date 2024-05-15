using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Helpers.HttpHelpers;
using EducationPortalApp.Web.Models.CourseModels;
using EducationPortalApp.Web.Services.Interfaces;

namespace EducationPortalApp.Web.Services.Concrete
{
    public class CourseService : ICourseService
    {
        private readonly HttpService _httpService;
        public CourseService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<CustomResponse<IEnumerable<CoursesVM>>> GetCoursesAsync()
        {
            var coursesResponse = await _httpService.HttpGet<CustomResponse<IEnumerable<CoursesVM>>>("Course/GetCourses");
            return HandleResponseHelper.HandleResponse(coursesResponse);
        }

        public async Task<CustomResponse<CourseVM>> GetCourseDetailAsync(int courseId)
        {
            var courseResponse = await _httpService.HttpGet<CustomResponse<CourseVM>>($"Course/GetCourse/{courseId}");
            return HandleResponseHelper.HandleResponse(courseResponse);
        }
    }
}
