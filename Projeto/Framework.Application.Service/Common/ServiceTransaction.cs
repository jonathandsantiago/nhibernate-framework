using Framework.Application.Service.Interfaces;
using System;
using System.Runtime.InteropServices;

namespace Framework.Application.Service.Common
{
    public class ServiceTransaction : IServiceTransaction
    {
        private ServiceUnitOfWork _unitOfWork;
        public bool InProgress { get; private set; }

        public IServiceUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public ServiceTransaction(ServiceUnitOfWork unitOfWork)
        {
            Initialize(unitOfWork);
        }

        public ServiceTransaction(ServiceUnitOfWork unitOfWork, TransactionParameter parameter)
        {
            Initialize(unitOfWork, parameter);
        }

        private void Initialize(ServiceUnitOfWork unidade, TransactionParameter? parameter = null)
        {
            _unitOfWork = unidade;

            if (_unitOfWork.GetCounter() <= 0)
            {
                InProgress = true;

                if (parameter == null)
                {
                    _unitOfWork.Data.BeginTransaction();
                }
                else
                {
                    _unitOfWork.Data.BeginTransaction(
                        parameter.Value.HasFlag(TransactionParameter.ClearSession),
                        parameter.Value.HasFlag(TransactionParameter.BlockRead),
                        parameter.Value.HasFlag(TransactionParameter.DisableAutoCommit));
                }

                _unitOfWork.Result?.Clear();
                _unitOfWork.StartCounter();
            }

            _unitOfWork.IncreaseCounter();
        }

        public void Complete(bool forcar = false)
        {
            _unitOfWork.DecreaseCounter();

            if (_unitOfWork.GetCounter() == 0)
            {
                Conclude(forcar);
            }
        }

        private void Conclude(bool force = false)
        {
            if (!_unitOfWork.Result?.IsValid ?? false && !force)
            {
                throw new InvalidOperationException("Transação não concluida pois não possuia um resultado válido.");
            }

            if (!InProgress)
            {
                if (_unitOfWork.Data.IsActiveTransaction)
                {
                    throw new InvalidOperationException("Um transação de dados está aberta, porém a transação do serviço encontra-se fechada.");
                }

                return;
            }

            try
            {
                _unitOfWork.Data.Commit();
                InProgress = false;
            }
            catch (Exception)
            {
                if (_unitOfWork.Data.IsActiveTransaction)
                {
                    _unitOfWork.Data.Rollback();
                }

                InProgress = false;
                throw;
            }
        }

        public void Cancel()
        {
            _unitOfWork.DecreaseCounter();

            if (_unitOfWork.GetCounter() == 0)
            {
                _unitOfWork.Data.Rollback();
                InProgress = false;
            }
        }

        public void Dispose()
        {
            _unitOfWork.DecreaseCounter();

            if (_unitOfWork.GetCounter() != 0)
            {
                return;
            }

            if (!FromException() && _unitOfWork.Result.IsValid)
            {
                Conclude();
            }
            else
            {
                _unitOfWork.Data.Rollback();
            }
        }

        private bool FromException()
        {
            return Marshal.GetExceptionCode() != 0;
        }
    }
}
