using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Helpers.HttpHelpers;
using EducationPortalApp.Web.Models.CourseModels;
using EducationPortalApp.Web.Services.Interfaces;

namespace EducationPortalApp.Web.Services.Concrete
{
    public class CourseService : ICourseService
    {
        private readonly HttpService _httpService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _contextAccessor;
        public CourseService(HttpService httpService, IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor)
        {
            _httpService = httpService;
            _httpClientFactory = httpClientFactory;
            _contextAccessor = contextAccessor;
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

        public async Task<CustomResponse<NoContent>> InsertCourseAsync(CourseCreateInput courseCreateInput)
        {
            var token = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Access token is not available.");
            }

            var _httpClient = _httpClientFactory.CreateClient("MyApiClient");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var multipartContent = new MultipartFormDataContent();
            if (courseCreateInput.Picture is not null)
            {
                using var ms = new MemoryStream();
                ms.Position = 0;
                await courseCreateInput.Picture.CopyToAsync(ms);
                multipartContent.Add(new ByteArrayContent(ms.ToArray()), "Picture", courseCreateInput.Picture.FileName);
            }
            multipartContent.Add(new StringContent(courseCreateInput.Name), "Name");
            multipartContent.Add(new StringContent(courseCreateInput.Description), "Description");
            multipartContent.Add(new StringContent(courseCreateInput.Instructor), "Instructor");
            multipartContent.Add(new StringContent(courseCreateInput.CostPerDay.ToString()), "CostPerDay");
            multipartContent.Add(new StringContent(courseCreateInput.MaxCapacity.ToString()), "MaxCapacity");
            multipartContent.Add(new StringContent(courseCreateInput.DurationInHours.ToString()), "DurationInHours");
            multipartContent.Add(new StringContent(courseCreateInput.CategoryId.ToString()), "CategoryId");

            var response = await _httpClient.PostAsync("Course/InsertCourse", multipartContent);
            CustomResponse<NoContent> insertCourseResponse = await response.Content.ReadFromJsonAsync<CustomResponse<NoContent>>();
            return HandleResponseHelper.HandleResponse(insertCourseResponse);
        }

        public async Task<CustomResponse<NoContent>> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var token = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Access token is not available.");
            }

            var _httpClient = _httpClientFactory.CreateClient("MyApiClient");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var multipartContent = new MultipartFormDataContent();
            if (courseUpdateInput.Picture is not null)
            {
                using var ms = new MemoryStream();
                ms.Position = 0;
                await courseUpdateInput.Picture.CopyToAsync(ms);
                multipartContent.Add(new ByteArrayContent(ms.ToArray()), "Picture", courseUpdateInput.Picture.FileName);
            }
            multipartContent.Add(new StringContent(courseUpdateInput.Id.ToString()), "Id");
            multipartContent.Add(new StringContent(courseUpdateInput.Name), "Name");
            multipartContent.Add(new StringContent(courseUpdateInput.Description), "Description");
            multipartContent.Add(new StringContent(courseUpdateInput.Instructor), "Instructor");
            multipartContent.Add(new StringContent(courseUpdateInput.CostPerDay.ToString()), "CostPerDay");
            multipartContent.Add(new StringContent(courseUpdateInput.Capacity.ToString()), "Capacity");
            multipartContent.Add(new StringContent(courseUpdateInput.MaxCapacity.ToString()), "MaxCapacity");
            multipartContent.Add(new StringContent(courseUpdateInput.DurationInHours.ToString()), "DurationInHours");
            multipartContent.Add(new StringContent(courseUpdateInput.CategoryId.ToString()), "CategoryId");

            var response = await _httpClient.PutAsync("Course/UpdateCourse", multipartContent);
            CustomResponse<NoContent> updateCourseResponse = await response.Content.ReadFromJsonAsync<CustomResponse<NoContent>>();
            return HandleResponseHelper.HandleResponse(updateCourseResponse);
        }

        public async Task<CustomResponse<NoContent>> RemoveCourseAsync(int courseId)
        {
            var response = await _httpService.HttpDeleteWithToken<CustomResponse<NoContent>>("Course/RemoveCourse", courseId);
            return HandleResponseHelper.HandleResponse(response);
        }
    }
}
