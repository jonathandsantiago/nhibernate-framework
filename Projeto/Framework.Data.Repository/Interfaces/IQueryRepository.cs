using Framework.Domain.Base.Interfaces;
using Framework.Helper.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Data.Repository.Interfaces
{
    public interface IQueryRepository<TEntity, TId>
          where TEntity : IEntity<TId>
    {
        IDataContext DataContext { get; }
        TEntity Get(TId id);
        TEntity GetComplete(TId id);
        IList<TEntity> GetAllActive();
        IList<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, bool>> ordemExpression = null);
        bool Exist(TId id);
        bool Exist(Expression<Func<TEntity, bool>> expressao);
        int Count(Expression<Func<TEntity, bool>> expressao = null);
        IList<TEntity> GetAllPaged(int currentPage, int recordsPage);
        IList<TEntity> GetAllPaged(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order);
        IList<TEntity> GetAllPagedComplete(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order);

        #region Async

        Task<bool> ExistAync(Expression<Func<TEntity, bool>> expressao);

        Task<bool> ExistAync(TId id);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> expressao = null);

        Task<TEntity> GetAsync(TId id);

        Task<TEntity> GetCompleteAsync(TId id);

        Task<IList<TEntity>> GetAllActiveAsync();

        Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression = null, Expression<Func<TEntity, bool>> ordemExpression = null);
        Task<IList<TEntity>> GetAllPagedAsync(int currentPage, int recordsPage);

        Task<IList<TEntity>> GetAllPagedAsync(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> orderBy);

        Task<IList<TEntity>> GetAllPagedCompleteAsync(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> orderBy);

        #endregion
    }
}
