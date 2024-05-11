using EducationPortalApp.DataAccess.Repositories.GenericRepositories;

namespace EducationPortalApp.DataAccess.UnitOfWork
{
    public interface IUow
    {
        IGenericRepository<T> GetRepository<T>() where T : class, new();
        Task SaveChangesAsync();
    }
}
