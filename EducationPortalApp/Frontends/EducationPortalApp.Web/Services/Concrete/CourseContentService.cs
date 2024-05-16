using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Helpers.HttpHelpers;
using EducationPortalApp.Web.Models.CourseContentModels;
using EducationPortalApp.Web.Services.Interfaces;

namespace EducationPortalApp.Web.Services.Concrete
{
    public class CourseContentService : ICourseContentService
    {
        private readonly HttpService _httpService;
        public CourseContentService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<CustomResponse<IEnumerable<CourseContentVM>>> GetAllCourseContentByCourseIdAsync(int courseId)
        {
            var courseContentsResponse = await _httpService.HttpGetWithToken<CustomResponse<IEnumerable<CourseContentVM>>>($"CourseContent/GetAllCourseContent/{courseId}");
            return HandleResponseHelper.HandleResponse(courseContentsResponse);
        }

        public async Task<CustomResponse<NoContent>> UpdateCourseContentStatusAsync(int courseContentId, bool isChecked)
        {
            var updateCourseContentStatusResponse = await _httpService.HttpPatchWithToken<CustomResponse<NoContent>>($"CourseContent/UpdateCourseContentStatus/{courseContentId}", isChecked);
            return HandleResponseHelper.HandleResponse(updateCourseContentStatusResponse);
        }
    }
}
