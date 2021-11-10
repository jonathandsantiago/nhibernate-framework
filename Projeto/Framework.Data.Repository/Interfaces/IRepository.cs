using Framework.Domain.Base.Interfaces;
using System.Threading.Tasks;

namespace Framework.Data.Repository.Interfaces
{
    public interface IRepository<TEntity, TId> : IQueryRepository<TEntity, TId>
         where TEntity : IEntity<TId>
    {
        TEntity Save(TEntity entity);
        void Add(TEntity entidade);
        void Delete(TEntity entity);
        void Delete(TId id);
        void Detach(TEntity entity);
        void Update(TEntity entity);
        void Reload(TEntity entity);
        void Save(TEntity entity, out TEntity entitySaved);

        #region Async

        Task<TEntity> SaveAsync(TEntity entity);

        Task AddAsync(TEntity entidade);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(TId id);

        Task DetachAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task ReloadAsync(TEntity entity);

        #endregion
    }
}