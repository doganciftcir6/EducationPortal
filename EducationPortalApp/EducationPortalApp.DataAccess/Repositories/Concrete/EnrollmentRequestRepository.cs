using EducationPortalApp.DataAccess.Contexts.EntityFramework;
using EducationPortalApp.DataAccess.Repositories.GenericRepositories;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.Entities.EnrollmentEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EducationPortalApp.DataAccess.Repositories.Concrete
{
    public class EnrollmentRequestRepository : GenericRepository<EnrollmentRequest>, IEnrollmentRequestRepository
    {
        public EnrollmentRequestRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public override async Task<IEnumerable<EnrollmentRequest>> GetAllFilterAsync(Expression<Func<EnrollmentRequest, bool>> filter)
        {
            return await _appDbContext.Set<EnrollmentRequest>().Include(x => x.AppUser).Include(x => x.Course).Include(x => x.EnrollmentRequestStatus).Where(filter).ToListAsync();
        }
    }
}
