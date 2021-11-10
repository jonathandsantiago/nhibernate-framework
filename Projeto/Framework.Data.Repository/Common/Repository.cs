using Framework.Data.Repository.Interfaces;
using Framework.Domain.Base.Interfaces;
using System;
using System.Threading.Tasks;

namespace Framework.Data.Repository.Common
{
    public class Repository<TEntity, TId> : QueryRepository<TEntity, TId>, IRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
    {
        public Repository(IDataContext dataContext)
            : base(dataContext)
        { }

        public virtual TEntity Save(TEntity entity)
        {
            TEntity entitySubmerged = GetSession().Merge(entity);
            entity.Id = entitySubmerged.Id;
            return entitySubmerged;
        }

        public virtual void Add(TEntity entidade)
        {
            if (entidade == null)
            {
                throw new ArgumentNullException("item");
            }

            GetSession().Save(entidade);
        }

        public virtual void Delete(TEntity entity)
        {
            GetSession().Delete(entity);
        }

        public virtual void Delete(TId id)
        {
            TEntity entity = Get(id);
            Delete(entity);
        }

        public virtual void Detach(TEntity entity)
        {
            GetSession().Evict(entity);
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("item");
            }

            GetSession().Update(entity);
        }

        public virtual void Reload(TEntity entity)
        {
            GetSession().Refresh(entity);
        }

        public virtual void Save(TEntity entity, out TEntity entitySaved)
        {
            entitySaved = GetSession().Merge(entity);
        }

        #region Async

        public virtual async Task<TEntity> SaveAsync(TEntity entity)
        {
            TEntity entitySubmerged = await GetSession().MergeAsync(entity);
            entity.Id = entitySubmerged.Id;
            return entitySubmerged;
        }

        public virtual async Task AddAsync(TEntity entidade)
        {
            if (entidade == null)
            {
                throw new ArgumentNullException("item");
            }

            await GetSession().SaveAsync(entidade);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            await GetSession().DeleteAsync(entity);
        }

        public virtual async Task DeleteAsync(TId id)
        {
            TEntity entity = await GetAsync(id);
            await DeleteAsync(entity);
        }

        public virtual async Task DetachAsync(TEntity entity)
        {
            await GetSession().EvictAsync(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("item");
            }

            await GetSession().UpdateAsync(entity);
        }

        public virtual async Task ReloadAsync(TEntity entity)
        {
            await GetSession().RefreshAsync(entity);
        }

        #endregion
    }
}