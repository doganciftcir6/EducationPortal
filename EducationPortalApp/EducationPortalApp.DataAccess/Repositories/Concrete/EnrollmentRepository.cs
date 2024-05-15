using EducationPortalApp.DataAccess.Contexts.EntityFramework;
using EducationPortalApp.DataAccess.Repositories.GenericRepositories;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.Entities.EnrollmentEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EducationPortalApp.DataAccess.Repositories.Concrete
{
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public override async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            return await _appDbContext.Set<Enrollment>().Include(x => x.AppUser).Include(x => x.Course).ToListAsync();
        }

        public override async Task<IEnumerable<Enrollment>> GetAllFilterAsync(Expression<Func<Enrollment, bool>> filter)
        {
            return await _appDbContext.Set<Enrollment>().Include(x => x.AppUser).Include(x => x.Course).Where(filter).ToListAsync();
        }
    }
}
