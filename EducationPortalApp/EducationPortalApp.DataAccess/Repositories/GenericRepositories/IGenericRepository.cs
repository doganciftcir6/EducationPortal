using System.Linq.Expressions;

namespace EducationPortalApp.DataAccess.Repositories.GenericRepositories
{
    public interface IGenericRepository<T> where T : class, new()
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllFilterAsync(Expression<Func<T, bool>> filter);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter);
        Task<T> AsNoTrackingGetByFilterAsync(Expression<Func<T, bool>> filter);
        Task<T> InsertAsync(T entity);
        T Update(T entity);
        T Delete(T entity);
        IQueryable<T> GetQuery();
    }
}
