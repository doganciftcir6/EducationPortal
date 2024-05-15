using EducationPortalApp.DataAccess.Contexts.EntityFramework;
using EducationPortalApp.DataAccess.Repositories.GenericRepositories;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.Entities.CourseEntities;

namespace EducationPortalApp.DataAccess.Repositories.Concrete
{
    public class UserCourseContentStatusRepository : GenericRepository<UserCourseContentStatus>, IUserCourseContentStatusRepository
    {
        public UserCourseContentStatusRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
