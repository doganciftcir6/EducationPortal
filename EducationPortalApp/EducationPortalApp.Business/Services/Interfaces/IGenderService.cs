using EducationPortalApp.Dtos.GenderDtos;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Interfaces
{
    public interface IGenderService
    {
        Task<CustomResponse<IEnumerable<GenderDto>>> GetGendersAsync();
    }
}
