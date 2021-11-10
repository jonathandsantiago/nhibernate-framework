using System;

namespace  Framework.Application.Service.Interfaces
{
    public interface IServiceTransaction : IDisposable
    {
        bool InProgress { get; }
        IServiceUnitOfWork UnitOfWork { get; }
        void Complete(bool force = false);
        void Cancel();
    }
}
