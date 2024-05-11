using System.Text.Json.Serialization;

namespace EducationPortalApp.Shared.Utilities.Response
{
    public class CustomResponse<T>
    {
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        [JsonIgnore]
        public int StatusCode { get; set; }

        public static CustomResponse<T> Success(T data, int statusCode)
        {
            return new CustomResponse<T> { Data = data, StatusCode = statusCode };
        }
        //başarılı ama data olmama durumu 
        public static CustomResponse<T> Success(int statusCode)
        {
            return new CustomResponse<T> { Data = default(T), StatusCode = statusCode };
        }
        //fail durumu birden çok hata
        public static CustomResponse<T> Fail(List<string> errors, int statusCode)
        {
            return new CustomResponse<T> { Errors = errors, StatusCode = statusCode };
        }
        //fail durumu tek hata
        public static CustomResponse<T> Fail(string error, int statusCode)
        {
            return new CustomResponse<T> { Errors = new List<string>() { error }, StatusCode = statusCode };
        }
    }
}
