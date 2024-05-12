using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenderController : CustomBaseController
    {
        private readonly IGenderService _genderService;
        public GenderController(IGenderService genderService)
        {
            _genderService = genderService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetGenders()
        {
            return CreateActionResultInstance(await _genderService.GetGendersAsync());
        }
    }
}
