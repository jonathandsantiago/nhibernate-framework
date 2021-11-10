using Framework.Application.Service.Common;
using Framework.Data.Repository.Interfaces;
using Framework.Validator.Validation.Interfaces;
using System;

namespace Framework.Application.Service.Interfaces
{
    public interface IServiceUnitOfWork : IDisposable
    {
        IDataContext Data { get; }
        IServiceTransaction Start();
        IServiceTransaction Start(TransactionParameter parametros);
        string UserName { get; set; }
        void Clear();
    }
}
