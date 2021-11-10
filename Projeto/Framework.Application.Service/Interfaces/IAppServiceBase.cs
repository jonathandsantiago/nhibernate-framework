using Framework.Domain.Base.Interfaces;
using Framework.Helper.Query;
using Framework.Validator.Validation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Application.Service.Interfaces
{
    public interface IAppServiceBase<TEntity, TId> : IDisposable
        where TEntity : IEntity<TId>
    {
        IServiceUnitOfWork UnitOfWork { get; set; }
        IValidationResult Result { get; }
        TEntity Get(TId id);
        IList<TEntity> GetAll();
        IList<TEntity> GetAllActive();
        IList<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        IList<TEntity> GetAllPaged(int currentPage, int recordsPage);
        IList<TEntity> GetAllPaged(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order);
        IList<TEntity> GetAllPagedComplete(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order);
        int Count(Expression<Func<TEntity, bool>> expressao = null);
        bool Save(TEntity entity);
        bool Insert(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TEntity entity);
        bool Delete(TId id);
        bool Disable(TId id);
        bool Enable(TId id);

        #region Async

        Task<TEntity> GetAsync(TId id);
        Task<IList<TEntity>> GetAllAsync();
        Task<IList<TEntity>> GetAllActiveAsync();
        Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IList<TEntity>> GetAllPagedAsync(int currentPage, int recordsPage);
        Task<IList<TEntity>> GetAllPagedAsync(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order);
        Task<IList<TEntity>> GetAllPagedCompleteAsync(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> expressao = null);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> InsertAsync(TEntity entity);
        Task<bool> SaveAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task<bool> DeleteAsync(TId id);
        Task<bool> EnableAsync(TId id);
        Task<bool> DisableAsync(TId id);

        #endregion
    }
}