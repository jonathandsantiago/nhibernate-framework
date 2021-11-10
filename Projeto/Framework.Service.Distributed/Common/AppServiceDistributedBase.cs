using AutoMapper;
using Framework.Application.Service.Interfaces;
using Framework.Domain.Base.Interfaces;
using Framework.Domain.Model.Common;
using Framework.Helper.Query;
using Framework.Service.Distributed.Interfaces;
using Framework.Validator.Validation;
using Framework.Validator.Validation.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Service.Distributed.Common
{
    public class AppServiceDistributedBase<TEntity, TModel, TId, TAppService> : IAppServiceDistributedBase<TEntity, TModel, TId>
       where TEntity : IEntity<TId>
       where TModel : IModelBase<TId>
       where TAppService : IAppServiceBase<TEntity, TId>
    {
        public string UserName
        {
            get { return UnitOfWork.UserName; }
            set { UnitOfWork.UserName = value; }
        }

        public IServiceUnitOfWork UnitOfWork
        {
            get { return _appService.UnitOfWork; }
        }

        protected virtual TAppService _appService { get; set; }

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

        protected virtual void AddValidator<TIValidator, TValidator>()
            where TIValidator : IValidation
            where TValidator : ValidationBase
        {
            if (_validators == null)
            {
                _validators = new Dictionary<TIValidator, TValidator>();
            }

            _validators.Add(typeof(TIValidator), Activator.CreateInstance(typeof(TValidator)));
        }

        public virtual TIValidator GetValidator<TIValidator>()
            where TIValidator : IValidation
        {
            return (TIValidator)_validators[typeof(TIValidator)];
        }

        public AppServiceDistributedBase(IServiceUnitOfWork unitOfWork)
        {
            _appService = (TAppService)Activator.CreateInstance(typeof(TAppService), args: unitOfWork);
        }

        public virtual bool Delete(TId id)
        {
            return _appService.Delete(_appService.Get(id));
        }

        public virtual bool Update(TModel model)
        {
            return _appService.Save(Mapper.Map(model, _appService.Get(model.Id)));
        }

        public virtual bool Save(TModel model)
        {
            TEntity entity = Mapper.Map<TEntity>(model);

            if (!_appService.Save(entity))
            {
                return false;
            }

            model.Id = entity.Id;
            return true;
        }

        public virtual TModel GetModel(TId id)
        {
            return Mapper.Map<TModel>(_appService.Get(id));
        }

        public virtual IList<TModel> GetModel()
        {
            return Mapper.Map<IList<TModel>>(_appService.GetAll());
        }

        public virtual IList<TModel> GetModelActive()
        {
            return Mapper.Map<IList<TModel>>(_appService.GetAllActive());
        }

        public virtual int Count(Expression<Func<TEntity, bool>> expressao = null)
        {
            return _appService.Count(expressao);
        }

        public virtual IList<TModel> GetAllPaged(int currentPage, int recordsPage)
        {
            return Mapper.Map<IList<TModel>>(_appService.GetAllPaged(currentPage, recordsPage));
        }

        public virtual IList<TModel> GetAllPaged(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order)
        {
            return Mapper.Map<IList<TModel>>(_appService.GetAllPaged(currentPage, recordsPage, filter, order));
        }

        public virtual IList<TModel> GetAllPagedComplete(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order)
        {
            return Mapper.Map<IList<TModel>>(_appService.GetAllPagedComplete(currentPage, recordsPage, filter, order));
        }

        #region Async

        public virtual async Task<bool> DeleteAsync(TId id)
        {
            return await _appService.DeleteAsync(_appService.Get(id));
        }

        public virtual async Task<bool> UpdateAsync(TModel model)
        {
            return await _appService.SaveAsync(Mapper.Map(model, _appService.Get(model.Id)));
        }

        public virtual async Task<bool> SaveAsync(TModel model)
        {
            TEntity entity = Mapper.Map<TEntity>(model);

            if (!await _appService.SaveAsync(entity))
            {
                return false;
            }

            model.Id = entity.Id;
            return true;
        }

        public virtual async Task<TModel> GetModelAsync(TId id)
        {
            return Mapper.Map<TModel>(await _appService.GetAsync(id));
        }

        public virtual async Task<IList<TModel>> GetModelAsync()
        {
            return Mapper.Map<IList<TModel>>(await _appService.GetAllAsync());
        }

        public virtual async Task<IList<TModel>> GetModelActiveAsync()
        {
            return Mapper.Map<IList<TModel>>(await _appService.GetAllActiveAsync());
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> expressao = null)
        {
            return await _appService.CountAsync(expressao);
        }

        public virtual async Task<IList<TModel>> GetAllPagedAsync(int currentPage, int recordsPage)
        {
            return Mapper.Map<IList<TModel>>(await _appService.GetAllPagedAsync(currentPage, recordsPage));
        }

        public virtual async Task<IList<TModel>> GetAllPagedAsync(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order)
        {
            return Mapper.Map<IList<TModel>>(await _appService.GetAllPagedAsync(currentPage, recordsPage, filter, order));
        }

        public virtual async Task<IList<TModel>> GetAllPagedCompleteAsync(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> order)
        {
            return Mapper.Map<IList<TModel>>(await _appService.GetAllPagedCompleteAsync(currentPage, recordsPage, filter, order));
        }

        #endregion
    }
}