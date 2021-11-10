using System.Data;

namespace Framework.Data.Repository.Common
{
    public class Transaction : IDbTransaction
    {
        private IDbTransaction _transaction;

        public bool ShouldCommit { get; private set; }
        public bool IsActive { get; private set; }

        public Transaction(bool shouldCommit, IDbTransaction transaction)
        {
            ShouldCommit = shouldCommit;
            IsActive = true;
            _transaction = transaction;
        }

        public void Commit()
        {
            if (ShouldCommit)
            {
                IsActive = false;
                _transaction.Commit();
            }
        }

        public IDbConnection Connection
        {
            get { return _transaction.Connection; }
        }

        public IsolationLevel IsolationLevel
        {
            get { return _transaction.IsolationLevel; }
        }

        public void Rollback()
        {
            if (ShouldCommit)
            {
                IsActive = false;
                _transaction.Rollback();
            }
        }

        public void Dispose()
        {
            if (ShouldCommit)
            {
                IsActive = false;
                _transaction.Dispose();
            }
        }

        public IDbTransaction GetDbTransaction()
        {
            return (_transaction is Transaction) ? (_transaction as Transaction).GetDbTransaction() : _transaction;
        }
    }
}
