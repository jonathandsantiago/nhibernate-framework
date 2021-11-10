using System;
using System.Data;

namespace Framework.Data.Repository.Common
{
    public class Connection : IDbConnection
    {
        private readonly Func<IDbConnection> _factoryConnection;
        private IDbConnection _connection;
        private Transaction _transaction;

        public IDbTransaction Transaction
        {
            get { return _transaction; }
        }

        public string ConnectionString
        {
            get { return _connection.ConnectionString; }
            set { _connection.ConnectionString = value; }
        }

        public int ConnectionTimeout
        {
            get { return _connection.ConnectionTimeout; }
        }

        public ConnectionState State
        {
            get { return _connection.State; }
        }

        public Connection(Func<IDbConnection> factoryConnection)
        {
            _factoryConnection = factoryConnection;
            Initialize();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            bool shouldCommit = _transaction == null || !_transaction.IsActive;

            Transaction internalTransaction = new Transaction(shouldCommit, _transaction != null && _transaction.IsActive ? _transaction : _connection.BeginTransaction(il));

            if (shouldCommit)
            {
                _transaction = internalTransaction;
            }

            return internalTransaction;
        }

        public IDbTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void ChangeDatabase(string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            _connection.Close();
        }

        public IDbCommand CreateCommand()
        {
            IDbCommand command = _connection.CreateCommand();

            if (_transaction != null && _transaction.IsActive)
            {
                command.Transaction = _transaction.GetDbTransaction();
            }

            return command;
        }

        public string Database
        {
            get { return _connection.Database; }
        }

        public void Open()
        {
            try
            {
                _connection.Open();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Bad value near"))
                {
                    _connection.Open();
                }
            }
        }

        public void Dispose()
        {
            try
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    Close();
                }

                _connection.Dispose();
                GC.SuppressFinalize(_connection);
                GC.SuppressFinalize(this);
                GC.Collect();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao liberar connection", ex);
            }
        }

        public void Initialize()
        {
            if (NotShouldInitialize())
            {
                return;
            }

            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao iniciar conexão", ex);
            }
            finally
            {
                _connection = _factoryConnection();
                Open();
            }
        }

        private void Reset()
        {
            if (_connection == null)
            {
                return;
            }

            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
                _connection.Dispose();
            }

            _connection = null;
        }

        private bool NotShouldInitialize()
        {
            return _connection != null && !(_transaction == null || !_transaction.IsActive);
        }
    }
}
