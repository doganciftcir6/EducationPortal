using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Helpers.HttpHelpers;
using EducationPortalApp.Web.Models.CourseContentModels;
using EducationPortalApp.Web.Services.Interfaces;

namespace EducationPortalApp.Web.Services.Concrete
{
    public class CourseContentService : ICourseContentService
    {
        private readonly HttpService _httpService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _contextAccessor;
        public CourseContentService(HttpService httpService, IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor)
        {
            _httpService = httpService;
            _httpClientFactory = httpClientFactory;
            _contextAccessor = contextAccessor;
        }

        public async Task<CustomResponse<IEnumerable<CourseContentVM>>> GetAllCourseContentAsync()
        {
            var courseContentsResponse = await _httpService.HttpGetWithToken<CustomResponse<IEnumerable<CourseContentVM>>>("CourseContent/GetAllCourseContent");
            return HandleResponseHelper.HandleResponse(courseContentsResponse);
        }

        public async Task<CustomResponse<IEnumerable<CourseContentVM>>> GetAllCourseContentByCourseIdAsync(int courseId)
        {
            var courseContentsResponse = await _httpService.HttpGetWithToken<CustomResponse<IEnumerable<CourseContentVM>>>($"CourseContent/GetAllCourseContentByCourseId/{courseId}");
            return HandleResponseHelper.HandleResponse(courseContentsResponse);
        }

        public async Task<CustomResponse<CourseContentVM>> GetCourseContentByIdAsync(int courseContentId)
        {
            var courseContentResponse = await _httpService.HttpGetWithToken<CustomResponse<CourseContentVM>>($"CourseContent/GetCourseContent/{courseContentId}");
            return HandleResponseHelper.HandleResponse(courseContentResponse);
        }

        public async Task<CustomResponse<NoContent>> InsertCourseContentAsync(CourseContentCreateInput courseContentCreateInput)
        {
            var token = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Access token is not available.");
            }

            var _httpClient = _httpClientFactory.CreateClient("MyApiClient");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var multipartContent = new MultipartFormDataContent();
            if (courseContentCreateInput.File is not null)
            {
                using var ms = new MemoryStream();
                ms.Position = 0;
                await courseContentCreateInput.File.CopyToAsync(ms);
                multipartContent.Add(new ByteArrayContent(ms.ToArray()), "File", courseContentCreateInput.File.FileName);
            }
            multipartContent.Add(new StringContent(courseContentCreateInput.Name), "Name");
            multipartContent.Add(new StringContent(courseContentCreateInput.CourseId.ToString()), "CourseId");
            multipartContent.Add(new StringContent(courseContentCreateInput.CourseContentTypeId.ToString()), "CourseContentTypeId");

            var response = await _httpClient.PostAsync("CourseContent/InsertCourseContent", multipartContent);
            CustomResponse<NoContent> insertCourseContentResponse = await response.Content.ReadFromJsonAsync<CustomResponse<NoContent>>();
            return HandleResponseHelper.HandleResponse(insertCourseContentResponse);
        }

        public async Task<CustomResponse<NoContent>> RemoveCourseContentAsync(int courseContentId)
        {
            var response = await _httpService.HttpDeleteWithToken<CustomResponse<NoContent>>("CourseContent/RemoveCourseContent", courseContentId);
            return HandleResponseHelper.HandleResponse(response);
        }

        public async Task<CustomResponse<NoContent>> UpdateCourseContentAsync(CourseContentUpdateInput courseContentUpdateInput)
        {
            var token = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Access token is not available.");
            }

            var _httpClient = _httpClientFactory.CreateClient("MyApiClient");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var multipartContent = new MultipartFormDataContent();
            if (courseContentUpdateInput.File is not null)
            {
                using var ms = new MemoryStream();
                ms.Position = 0;
                await courseContentUpdateInput.File.CopyToAsync(ms);
                multipartContent.Add(new ByteArrayContent(ms.ToArray()), "File", courseContentUpdateInput.File.FileName);
            }
            multipartContent.Add(new StringContent(courseContentUpdateInput.Id.ToString()), "Id");
            multipartContent.Add(new StringContent(courseContentUpdateInput.Name), "Name");
            multipartContent.Add(new StringContent(courseContentUpdateInput.CourseId.ToString()), "CourseId");
            multipartContent.Add(new StringContent(courseContentUpdateInput.CourseContentTypeId.ToString()), "CourseContentTypeId");

            var response = await _httpClient.PutAsync("CourseContent/UpdateCourseContent", multipartContent);
            CustomResponse<NoContent> updateCourseContentResponse = await response.Content.ReadFromJsonAsync<CustomResponse<NoContent>>();
            return HandleResponseHelper.HandleResponse(updateCourseContentResponse);
        }

        public async Task<CustomResponse<NoContent>> UpdateCourseContentStatusAsync(int courseContentId, bool isChecked)
        {
            var updateCourseContentStatusResponse = await _httpService.HttpPatchWithToken<CustomResponse<NoContent>>($"CourseContent/UpdateCourseContentStatus/{courseContentId}", isChecked);
            return HandleResponseHelper.HandleResponse(updateCourseContentStatusResponse);
        }
    }
}
