using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Helpers.HttpHelpers;
using EducationPortalApp.Web.Models.CourseContentTypeModels;
using EducationPortalApp.Web.Services.Interfaces;

namespace EducationPortalApp.Web.Services.Concrete
{
    public class CourseContentTypeService : ICourseContentTypeService
    {
        private readonly HttpService _httpService;
        public CourseContentTypeService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<CustomResponse<IEnumerable<CourseContentTypeVM>>> GetCourseContentTypesAsync()
        {
            var courseContentTypeResponse = await _httpService.HttpGet<CustomResponse<IEnumerable<CourseContentTypeVM>>>("CourseContentType/GetCourseContentTypes");
            return HandleResponseHelper.HandleResponse(courseContentTypeResponse);
        }
    }
}
