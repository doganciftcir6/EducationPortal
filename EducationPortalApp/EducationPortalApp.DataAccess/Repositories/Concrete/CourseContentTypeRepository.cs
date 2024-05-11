using EducationPortalApp.DataAccess.Contexts.EntityFramework;
using EducationPortalApp.DataAccess.Repositories.GenericRepositories;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.Entities.CourseEntities;

namespace EducationPortalApp.DataAccess.Repositories.Concrete
{
    public class CourseContentTypeRepository : GenericRepository<CourseContentType>, ICourseContentTypeRepository
    {
        public CourseContentTypeRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
