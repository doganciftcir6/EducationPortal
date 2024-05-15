using EducationPortalApp.Shared.Utilities.Response;
using System.Net;

namespace EducationPortalApp.Web.Helpers.HttpHelpers
{
    public static class HandleResponseHelper
    {
        public static CustomResponse<T> HandleResponse<T>(CustomResponse<T> responseTask)
        {
            var response = responseTask;

            if (response is null)
                return CustomResponse<T>.Fail("No response from server.", ResponseStatusCode.INTERNAL_SERVER_ERROR);

            if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
                return CustomResponse<T>.Fail("Unauthorized access. Please log in again.", ResponseStatusCode.UNAUTHORIZED);

            if (response.StatusCode == (int)HttpStatusCode.Forbidden)
                return CustomResponse<T>.Fail("You don't have permission to access this resource.", ResponseStatusCode.FORBIDDEN);

            if (response.StatusCode == (int)HttpStatusCode.InternalServerError)
                return CustomResponse<T>.Fail("Server error. Please try again later.", ResponseStatusCode.INTERNAL_SERVER_ERROR);

            if (response.Errors is not null)
                return CustomResponse<T>.Fail(response.Errors, ResponseStatusCode.BAD_REQUEST);

            return response;
        }
    }
}

