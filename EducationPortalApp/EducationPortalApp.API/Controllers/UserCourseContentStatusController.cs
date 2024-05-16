using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Shared.ControllerBases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserCourseContentStatusController : CustomBaseController
    {
        private readonly IUserCourseContentStatusService _userCourseContentStatusService;
        public UserCourseContentStatusController(IUserCourseContentStatusService userCourseContentStatusService)
        {
            _userCourseContentStatusService = userCourseContentStatusService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserCourseContentStatus()
        {
            return CreateActionResultInstance(await _userCourseContentStatusService.GetContentStatusByUserAsync());
        }
    }
}
