using System;
using System.Data;

namespace Framework.Data.Repository.Interfaces
{
    public interface IDataContext : IDisposable
    {
        Guid ContextId { get; }
        IDbConnection Connection { get; }
        IDbTransaction CurrentTransaction { get; }
        bool IsActiveTransaction { get; }
        void BeginTransaction(bool clearSession = false, bool BloqReader = false, bool disabilityCommit = false);
        void Commit();
        void Rollback();
        void Clear();
        void Flush();
    }
}
