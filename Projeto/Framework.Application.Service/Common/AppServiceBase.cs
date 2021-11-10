using Framework.Application.Service.Interfaces;
using Framework.Data.Repository.Interfaces;
using Framework.Domain.Base;
using Framework.Domain.Base.Interfaces;
using Framework.Helper.Query;
using Framework.Validator.Validation;
using Framework.Validator.Validation.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Application.Service.Common
{
    public abstract class AppServiceBase<TEntity, TId, TRepository, TIValidation> : IAppServiceBase<TEntity, TId>
          where TEntity : IEntity<TId>
          where TRepository : IRepository<TEntity, TId>
          where TIValidation : IValidation<TEntity>
    {
        protected virtual TRepository _repository { get; set; }
        public virtual IServiceUnitOfWork UnitOfWork { get; set; }
        protected virtual IDictionary _validators { get; private set; }
        public virtual IValidationResult Result
        {
            get
            {
                var result = new ValidationResult();

                if (_validators == null)
                {
                    return result;
                }

                foreach (var value in _validators.Values)
                {
                    if (value is ValidationBase validation)
                    {
                        result.Add(validation.Result.Message);
                    }
                }

                return result;
            }
        }

        public AppServiceBase()
        {
            UnitOfWork = CreateUnit();
        }

        public AppServiceBase(IServiceUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            Initialize();
        }

        protected virtual void AddValidator<TIValidator, TValidator>()
           where TIValidator : IValidation
           where TValidator : ValidationBase
        {
            if (_validators == null)
            {
                _validators = new Dictionary<Type, object>();
            }

            _validators.Add(typeof(TIValidator), (TIValidator)Activator.CreateInstance(typeof(TValidator)));
        }

        protected virtual void Initialize()
        { }

        protected virtual void Audit(TEntity entity)
        {
            Audit<TEntity, TId>(entity);
        }

        protected virtual void Audit<TAudit, TAuditId>(TAudit entity)
            where TAudit : IEntity<TAuditId>
        {
            if (!(entity is IEntityAudit<TId>))
            {
                return;
            }

            IEntityAudit<TId> entityAudit = entity as IEntityAudit<TId>;
            entityAudit.Revision += 1;

            if (entity.Id.Equals(default(TId)))
            {
                entityAudit.DateInsertion = DateTime.Now;
                entityAudit.UserInsertion = UnitOfWork.UserName ?? "System";
            }
            else
            {
                entityAudit.DateEdition = DateTime.Now;
                entityAudit.UserEdition = UnitOfWork.UserName ?? "System";
            }
        }

        protected virtual bool Validate(TEntity entity)
        {
            IValidation<TEntity> validator = GetValidator<TIValidation>();

            if (validator != null)
            {
                return validator.Validate(entity).IsValid;
            }

            return true;
        }

        public virtual TIValidator GetValidator<TIValidator>()
            where TIValidator : IValidation
        {
            return (TIValidator)_validators[typeof(TIValidator)];
        }

        protected virtual IServiceUnitOfWork CreateUnit()
        {
            return new ServiceUnitOfWork();
        }

        public virtual TEntity Get(TId id)
        {
            return _repository.Get(id);
        }

        public virtual IList<TEntity> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual IList<TEntity> GetAllActive()
        {
            return _repository.GetAllActive();
        }

        public virtual IList<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.GetAll(predicate);
        }

        public virtual IList<TEntity> GetAllPaged(int currentPage, int recordsPage)
        {
            return _repository.GetAllPaged(currentPage, recordsPage);
        }

        public virtual IList<TEntity> GetAllPaged(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order)
        {
            return _repository.GetAllPaged(currentPage, recordsPage, filter, order);
        }

        public virtual IList<TEntity> GetAllPagedComplete(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order)
        {
            return _repository.GetAllPagedComplete(currentPage, recordsPage, filter, order);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> expressao = null)
        {
            return _repository.Count(expressao);
        }

        public virtual bool Update(TEntity entity)
        {
            using (UnitOfWork.Start())
            {
                if (!Validate(entity))
                {
                    return false;
                }

                entity.Prepare();
                Audit<TEntity, TId>(entity);
                _repository.Update(entity);
                return true;
            }
        }

        public virtual bool Insert(TEntity entity)
        {
            using (UnitOfWork.Start())
            {
                if (!Validate(entity))
                {
                    return false;
                }

                entity.Prepare();
                Audit<TEntity, TId>(entity);
                _repository.Save(entity);
                return true;
            }
        }

        public virtual bool Save(TEntity entity)
        {
            using (UnitOfWork.Start())
            {
                if (!Validate(entity))
                {
                    return false;
                }

                entity.Prepare();
                Audit<TEntity, TId>(entity);
                _repository.Save(entity);
                return true;
            }
        }

        public virtual bool Delete(TEntity entity)
        {
            using (UnitOfWork.Start())
            {
                _repository.Delete(entity);
                return true;
            }
        }

        public virtual bool Delete(TId id)
        {
            using (UnitOfWork.Start())
            {
                _repository.Delete(_repository.Get(id));
                return true;
            }
        }

        public virtual bool Enable(TId id)
        {
            using (UnitOfWork.Start())
            {
                TEntity entity = _repository.Get(id);

                if (entity is EntityStatus<TId> entityStatus)
                {
                    entityStatus.Active = false;
                }

                Audit<TEntity, TId>(entity);
                return true;
            }
        }

        public virtual bool Disable(TId id)
        {
            using (UnitOfWork.Start())
            {
                TEntity entity = _repository.Get(id);

                if (entity is EntityStatus<TId> entityStatus)
                {
                    entityStatus.Active = false;
                }

                Audit<TEntity, TId>(entity);
                return true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnitOfWork.Dispose();
            }
        }

        #region Async

        public virtual async Task<TEntity> GetAsync(TId id)
        {
            return await _repository.GetAsync(id);
        }

        public virtual async Task<IList<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual async Task<IList<TEntity>> GetAllActiveAsync()
        {
            return await _repository.GetAllActiveAsync();
        }

        public virtual async Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repository.GetAllAsync(predicate);
        }

        public virtual async Task<IList<TEntity>> GetAllPagedAsync(int currentPage, int recordsPage)
        {
            return await _repository.GetAllPagedAsync(currentPage, recordsPage);
        }

        public virtual async Task<IList<TEntity>> GetAllPagedAsync(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order)
        {
            return await _repository.GetAllPagedAsync(currentPage, recordsPage, filter, order);
        }

        public virtual async Task<IList<TEntity>> GetAllPagedCompleteAsync(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order)
        {
            return await _repository.GetAllPagedCompleteAsync(currentPage, recordsPage, filter, order);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> expressao = null)
        {
            return await _repository.CountAsync(expressao);
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            using (UnitOfWork.Start())
            {
                if (!Validate(entity))
                {
                    return false;
                }

                entity.Prepare();
                Audit<TEntity, TId>(entity);
                await _repository.UpdateAsync(entity);
                return true;
            }
        }

        public virtual async Task<bool> InsertAsync(TEntity entity)
        {
            using (UnitOfWork.Start())
            {
                if (!Validate(entity))
                {
                    return false;
                }

                entity.Prepare();
                Audit<TEntity, TId>(entity);
                await _repository.SaveAsync(entity);
                return true;
            }
        }

        public virtual async Task<bool> SaveAsync(TEntity entity)
        {
            using (UnitOfWork.Start())
            {
                if (!Validate(entity))
                {
                    return false;
                }

                entity.Prepare();
                Audit<TEntity, TId>(entity);
                await _repository.SaveAsync(entity);
                return true;
            }
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            using (UnitOfWork.Start())
            {
                await _repository.DeleteAsync(entity);
                return true;
            }
        }

        public virtual async Task<bool> DeleteAsync(TId id)
        {
            using (UnitOfWork.Start())
            {
                await _repository.DeleteAsync(_repository.Get(id));
                return true;
            }
        }

        public virtual async Task<bool> EnableAsync(TId id)
        {
            using (UnitOfWork.Start())
            {
                TEntity entity = await _repository.GetAsync(id);

                if (entity is EntityStatus<TId> entityStatus)
                {
                    entityStatus.Active = false;
                }

                Audit<TEntity, TId>(entity);
                return true;
            }
        }

        public virtual async Task<bool> DisableAsync(TId id)
        {
            using (UnitOfWork.Start())
            {
                TEntity entity = await _repository.GetAsync(id);

                if (entity is EntityStatus<TId> entityStatus)
                {
                    entityStatus.Active = false;
                }

                Audit<TEntity, TId>(entity);
                return true;
            }
        }

        #endregion
    }
}