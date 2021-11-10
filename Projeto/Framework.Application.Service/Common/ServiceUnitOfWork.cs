using Framework.Application.Service.Interfaces;
using Framework.Data.Repository.Common;
using Framework.Data.Repository.Interfaces;
using Framework.Validator.Validation;
using Framework.Validator.Validation.Interfaces;

namespace Framework.Application.Service.Common
{
    public class ServiceUnitOfWork : IServiceUnitOfWork
    {
        private readonly IDataContext _data;
        private int _counter;
        private IValidationResult _result;
        public string UserName { get; set; }
        public IValidationResult Result
        {
            get
            {
                if (_result == null)
                {
                    _result = new ValidationResult();
                }

                return _result;
            }
            set
            {
                _result = value;
            }
        }

        public IDataContext Data
        {
            get { return _data; }
        }

        public ServiceUnitOfWork()
        {
            _data = new UnitOfWork();
        }

        public ServiceUnitOfWork(string userName)
            : this()
        {
            UserName = userName;
        }

        public ServiceUnitOfWork(IDataContext data)
        {
            this._data = data;
        }

        public IServiceTransaction Start(TransactionParameter parameter)
        {
            return new ServiceTransaction(this, parameter);
        }

        public IServiceTransaction Start()
        {
            return new ServiceTransaction(this);
        }

        public void Clear()
        {
            Data.Clear();
            Result?.Clear();
        }

        public void Dispose()
        {
            Data.Dispose();
        }

        public int GetCounter()
        {
            return _counter;
        }

        public void IncreaseCounter()
        {
            _counter++;
        }

        public void DecreaseCounter()
        {
            _counter--;
        }

        public void StartCounter()
        {
            _counter = 0;
        }
    }
}