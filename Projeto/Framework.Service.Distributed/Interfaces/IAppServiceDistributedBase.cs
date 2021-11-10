using Framework.Application.Service.Interfaces;
using Framework.Domain.Base.Interfaces;
using Framework.Domain.Model.Common;
using Framework.Helper.Query;
using Framework.Validator.Validation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Service.Distributed.Interfaces
{
    public interface IAppServiceDistributedBase<TEntity, TModel, TId>
         where TEntity : IEntity<TId>
         where TModel : IModelBase<TId>
    {
        string UserName { get; set; }
        IValidationResult Result { get; }
        IServiceUnitOfWork UnitOfWork { get; }
        bool Delete(TId entidade);
        bool Update(TModel entidade);
        bool Save(TModel entidade);
        TModel GetModel(TId id);
        IList<TModel> GetModel();
        IList<TModel> GetModelActive();
        int Count(Expression<Func<TEntity, bool>> expressao = null);
        IList<TModel> GetAllPaged(int currentPage, int recordsPage);
        IList<TModel> GetAllPaged(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order);
        IList<TModel> GetAllPagedComplete(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order);

        #region Async

        Task<bool> DeleteAsync(TId entidade);
        Task<bool> UpdateAsync(TModel entidade);
        Task<bool> SaveAsync(TModel entidade);
        Task<TModel> GetModelAsync(TId id);
        Task<IList<TModel>> GetModelAsync();
        Task<IList<TModel>> GetModelActiveAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> expressao = null);
        Task<IList<TModel>> GetAllPagedAsync(int currentPage, int recordsPage);
        Task<IList<TModel>> GetAllPagedAsync(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order);
        Task<IList<TModel>> GetAllPagedCompleteAsync(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order);

        #endregion
    }
}
