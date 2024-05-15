using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Helpers.HttpHelpers;
using EducationPortalApp.Web.Models.GenderModels;
using EducationPortalApp.Web.Services.Interfaces;

namespace EducationPortalApp.Web.Services.Concrete
{
    public class GenderService : IGenderService
    {
        private readonly HttpService _httpService;
        public GenderService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<CustomResponse<IEnumerable<GenderVM>>> GetGendersAsync()
        {
            var genderResponse = await _httpService.HttpGet<CustomResponse<IEnumerable<GenderVM>>>("Gender/GetGenders");
            return HandleResponseHelper.HandleResponse(genderResponse);
        }
    }
}
