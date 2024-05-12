using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseContentTypeController : CustomBaseController
    {
        private readonly ICourseContentTypeService _courseContentTypeService;
        public CourseContentTypeController(ICourseContentTypeService courseContentTypeService)
        {
            _courseContentTypeService = courseContentTypeService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCourseContentTypes()
        {
            return CreateActionResultInstance(await _courseContentTypeService.GetCourseContentTypesAsync());
        }
    }
}
