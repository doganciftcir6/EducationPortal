using EducationPortalApp.DataAccess.Contexts.EntityFramework;
using EducationPortalApp.DataAccess.Repositories.GenericRepositories;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.Entities.CourseEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EducationPortalApp.DataAccess.Repositories.Concrete
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public override async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _appDbContext.Set<Course>().Include(x => x.Category).ToListAsync();
        }

        public override async Task<Course> GetByFilterAsync(Expression<Func<Course, bool>> filter)
        {
            return await _appDbContext.Set<Course>().Include(x => x.Category).Where(filter).SingleOrDefaultAsync();
        }
    }
}
