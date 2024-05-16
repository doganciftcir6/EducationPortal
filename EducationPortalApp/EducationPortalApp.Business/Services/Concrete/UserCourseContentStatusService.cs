using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.DataAccess.UnitOfWork;
using EducationPortalApp.Entities.CourseEntities;
using EducationPortalApp.Shared.Services;
using EducationPortalApp.Shared.Utilities.Response;

namespace EducationPortalApp.Business.Services.Concrete
{
    public class UserCourseContentStatusService : IUserCourseContentStatusService
    {
        private readonly IUow _uow;
        private readonly ISharedIdentityService _sharedIdentityService;
        public UserCourseContentStatusService(IUow uow, ISharedIdentityService sharedIdentityService)
        {
            _uow = uow;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<CustomResponse<IEnumerable<UserCourseContentStatus>>> GetContentStatusByUserAsync()
        {

            var userCourseContentStatues = await _uow.GetRepository<UserCourseContentStatus>().GetAllFilterAsync(x => x.AppUserId == _sharedIdentityService.GetUserId);
            return CustomResponse<IEnumerable<UserCourseContentStatus>>.Success(userCourseContentStatues, ResponseStatusCode.OK);
        }
    }
}
