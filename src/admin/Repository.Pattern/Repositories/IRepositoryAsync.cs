using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Repository.Pattern.Infrastructure;
using TrackableEntities;

namespace Repository.Pattern.Repositories
{
    public interface IRepositoryAsync<TEntity> : IRepository<TEntity> where TEntity : class, ITrackable
    {
        Task<bool> ExistsAsync(params object[] keyValues);
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<bool> DeleteAsync(params object[] keyValues);
        Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<IEnumerable<TEntity>> SelectQueryAsync(string query, params object[] parameters);
        Task<IEnumerable<TEntity>> SelectQueryAsync(string query, CancellationToken cancellationToken, params object[] parameters);

        IRepositoryAsync<T> GetRepositoryAsync<T>() where T : class, ITrackable;
    }
}