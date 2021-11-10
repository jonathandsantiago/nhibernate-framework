using Framework.Data.Repository.Common;
using NHibernate;

namespace Framework.Data.Repository.Interfaces
{
    public interface IUnitOfWork : IDataContext
    {
        SessionWrapper Session { get; }
        IStatelessSession StatelessSession { get; }
    }
}
