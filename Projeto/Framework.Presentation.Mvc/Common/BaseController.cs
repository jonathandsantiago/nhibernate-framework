using AutoMapper;
using Framework.Application.Service.Interfaces;
using Framework.Domain.Base.Interfaces;
using Framework.Domain.Model.Common;
using Framework.Service.Distributed.Interfaces;
using Framework.Validator.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Presentation.Mvc.Common
{
    public class BaseController<TEntity, TViewModel, TId, TAppService> : Controller
          where TEntity : IEntity<TId>
          where TViewModel : IModelBase<TId>
          where TAppService : IAppServiceBase<TEntity, TId>
    {
        protected virtual TAppService _appService { get; set; }

        public BaseController(TAppService appService)
        {
            _appService = appService;
        }

        public virtual async Task<IActionResult> Index()
        {
            return View(Mapper.Map<IList<TEntity>, IList<TViewModel>>(await _appService.GetAllAsync()));
        }

        public virtual async Task<IActionResult> Detail(TId id)
        {
            return View(Mapper.Map<TEntity, TViewModel>(await _appService.GetAsync(id)));
        }

        public virtual IActionResult Create()
        {
            return View();
        }

        public virtual async Task<IActionResult> Edit(TId id)
        {
            return View(Mapper.Map<TEntity, TViewModel>(await _appService.GetAsync(id)));
        }

        public virtual async Task<IActionResult> Delete(TId id)
        {
            return View(Mapper.Map<TEntity, TViewModel>(await _appService.GetAsync(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Create(TViewModel viewModel)
        {
            if (ModelState.IsValid && await ExecuteAction(ActionType.Insert, viewModel))
            {
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Edit(TViewModel viewModel)
        {
            if (ModelState.IsValid && await ExecuteAction(ActionType.Edit, viewModel))
            {
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> DeleteConfirmed(TId id)
        {
            TViewModel viewModel = Mapper.Map<TEntity, TViewModel>(_appService.Get(id));

            if (await ExecuteAction(ActionType.Delete, viewModel))
            {
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        protected virtual async Task<bool> ExecuteAction(ActionType acao, TViewModel viewModel)
        {
            bool resultado = false;

            switch (acao)
            {
                case ActionType.Insert:
                    resultado = await _appService.SaveAsync(Mapper.Map<TViewModel, TEntity>(viewModel));
                    break;
                case ActionType.Edit:
                    resultado = await _appService.UpdateAsync(Mapper.Map<TViewModel, TEntity>(viewModel));
                    break;
                case ActionType.Delete:
                    resultado = await _appService.DeleteAsync(viewModel.Id);
                    break;
                default:
                    break;
            }

            foreach (ValidationError error in _appService.Result?.Errors)
            {
                ModelState.AddModelError(error.PropertyName ?? string.Empty, error.Message);
            }

            return resultado;
        }
    }
}