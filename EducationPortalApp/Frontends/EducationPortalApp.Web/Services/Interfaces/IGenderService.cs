using EducationPortalApp.Shared.Utilities.Response;
using EducationPortalApp.Web.Models.GenderModels;

namespace EducationPortalApp.Web.Services.Interfaces
{
    public interface IGenderService
    {
        Task<CustomResponse<IEnumerable<GenderVM>>> GetGendersAsync();
    }
}
