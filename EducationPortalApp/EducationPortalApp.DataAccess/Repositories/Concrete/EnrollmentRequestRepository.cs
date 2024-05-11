using EducationPortalApp.DataAccess.Contexts.EntityFramework;
using EducationPortalApp.DataAccess.Repositories.GenericRepositories;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.Entities.EnrollmentEntities;

namespace EducationPortalApp.DataAccess.Repositories.Concrete
{
    public class EnrollmentRequestRepository : GenericRepository<EnrollmentRequest>, IEnrollmentRequestRepository
    {
        public EnrollmentRequestRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
