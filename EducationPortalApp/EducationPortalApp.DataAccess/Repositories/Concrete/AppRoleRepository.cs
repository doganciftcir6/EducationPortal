using EducationPortalApp.DataAccess.Contexts.EntityFramework;
using EducationPortalApp.DataAccess.Repositories.GenericRepositories;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.Entities.UserEntities;

namespace EducationPortalApp.DataAccess.Repositories.Concrete
{
    public class AppRoleRepository : GenericRepository<AppRole>, IAppRoleRepository
    {
        public AppRoleRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
