using EducationPortalApp.Shared.Utilities.Response;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.Shared.ControllerBases
{
    public class CustomBaseController : ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(CustomResponse<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode,
            };
        }
    }
}
