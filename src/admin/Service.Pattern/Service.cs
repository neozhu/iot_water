using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TrackableEntities;

namespace Service.Pattern
{
    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class, ITrackable
    {
        private readonly IRepositoryAsync<TEntity> _repository;

        protected Service(IRepositoryAsync<TEntity> repository) => this._repository = repository;
        public virtual async Task<bool> ExistsAsync(params object[] keyValues)
           => await this._repository.ExistsAsync(keyValues);

        public virtual TEntity Find(params object[] keyValues) => this._repository.Find(keyValues);

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters) => this._repository.SelectQuery(query, parameters).AsQueryable();

        public virtual void Insert(TEntity entity) => this._repository.Insert(entity);

        public virtual void ApplyChanges(ITrackable entity) => this._repository.ApplyChanges(entity);
        public virtual void ApplyChanges(IEnumerable<ITrackable> entities) => this._repository.ApplyChanges(entities);
        public virtual void InsertRange(IEnumerable<TEntity> entities) => this._repository.InsertRange(entities);

        public virtual void Update(TEntity entity) => this._repository.Update(entity);

        public virtual void Delete(params object[] id) => this._repository.Delete(id);

        public virtual void Delete(TEntity entity) => this._repository.Delete(entity);

        public IQueryFluent<TEntity> Query() => this._repository.Query();

        public virtual IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject) => this._repository.Query(queryObject);

        public virtual IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query) => this._repository.Query(query);

        public virtual async Task<TEntity> FindAsync(params object[] keyValues) => await this._repository.FindAsync(keyValues);

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues) => await this._repository.FindAsync(cancellationToken, keyValues);

        public virtual async Task<bool> DeleteAsync(params object[] keyValues) => await this.DeleteAsync(CancellationToken.None, keyValues);

        //IF 04/08/2014 - Before: return await DeleteAsync(cancellationToken, keyValues);
        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues) => await this._repository.DeleteAsync(cancellationToken, keyValues);

        public IQueryable<TEntity> Queryable() => this._repository.Queryable();
    }
}