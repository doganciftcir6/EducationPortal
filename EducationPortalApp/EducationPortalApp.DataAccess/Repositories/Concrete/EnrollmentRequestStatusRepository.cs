using EducationPortalApp.DataAccess.Contexts.EntityFramework;
using EducationPortalApp.DataAccess.Repositories.GenericRepositories;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.Entities.EnrollmentEntities;

namespace EducationPortalApp.DataAccess.Repositories.Concrete
{
    public class EnrollmentRequestStatusRepository : GenericRepository<EnrollmentRequestStatus>, IEnrollmentRequestStatusRepository
    {
        public EnrollmentRequestStatusRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
