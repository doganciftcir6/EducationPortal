using System.Text;
using System.Text.Json;

namespace EducationPortalApp.Web.Helpers.HttpHelpers
{
    public class HttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _contextAccessor;
        public HttpService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _contextAccessor = contextAccessor;
        }

        public async Task<T> HttpGet<T>(string uri)
        where T : class
        {
            var _httpClient = _httpClientFactory.CreateClient("MyApiClient");
            var result = await _httpClient.GetAsync(uri);

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> HttpGetWithToken<T>(string uri)
      where T : class
        {
            var token = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Access token is not available.");
            }

            var _httpClient = _httpClientFactory.CreateClient("MyApiClient");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.GetAsync(uri);

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> HttpDelete<T>(string uri, int id)
            where T : class
        {
            var _httpClient = _httpClientFactory.CreateClient("MyApiClient");
            var result = await _httpClient.DeleteAsync($"{uri}/{id}");
            //if (!result.IsSuccessStatusCode)
            //{
            //    return null;
            //}

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> HttpDelete<T>(string uri)
         where T : class
        {
            var _httpClient = _httpClientFactory.CreateClient("MyApiClient");
            var result = await _httpClient.DeleteAsync($"{uri}");
            //if (!result.IsSuccessStatusCode)
            //{
            //    return null;
            //}

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> HttpDeleteWithToken<T>(string uri)
         where T : class
        {
            var token = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Access token is not available.");
            }

            var _httpClient = _httpClientFactory.CreateClient("MyApiClient");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await _httpClient.DeleteAsync(uri);
            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"HTTP request failed with status code {result.StatusCode}");
            }

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> HttpDeleteWithToken<T>(string uri, int id)
        where T : class
        {
            var token = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Access token is not available.");
            }

            var _httpClient = _httpClientFactory.CreateClient("MyApiClient");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await _httpClient.DeleteAsync($"{uri}/{id}");
            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"HTTP request failed with status code {result.StatusCode}");
            }

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> HttpPost<T>(string uri, object dataToSend)
            where T : class
        {
            var _httpClient = _httpClientFactory.CreateClient("MyApiClient");
            var content = ToJson(dataToSend);

            var result = await _httpClient.PostAsync(uri, content);

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> HttpPostWithToken<T>(string uri, object dataToSend)
          where T : class
        {
            var token = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Access token is not available.");
            }

            var _httpClient = _httpClientFactory.CreateClient("MyApiClient");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var content = ToJson(dataToSend);

            var result = await _httpClient.PostAsync(uri, content);

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> HttpPutWithToken<T>(string uri, object dataToSend)
            where T : class
        {
            var token = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Access token is not available.");
            }

            var _httpClient = _httpClientFactory.CreateClient("MyApiClient");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var content = ToJson(dataToSend);

            var result = await _httpClient.PutAsync(uri, content);

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> HttpPut<T>(string uri, object dataToSend)
         where T : class
        {
            var _httpClient = _httpClientFactory.CreateClient("MyApiClient");
            var content = ToJson(dataToSend);

            var result = await _httpClient.PutAsync(uri, content);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            return await FromHttpResponseMessage<T>(result);
        }

        private StringContent ToJson(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }

        private async Task<T> FromHttpResponseMessage<T>(HttpResponseMessage result)
        {
            return JsonSerializer.Deserialize<T>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
