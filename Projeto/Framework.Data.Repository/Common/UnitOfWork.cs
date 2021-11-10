using Framework.Data.Nhibernate;
using Framework.Data.Repository.Interfaces;
using Framework.Helper.Helpers;
using NHibernate;
using System;
using System.Data;

namespace Framework.Data.Repository.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private IStatelessSession _statelessSession;
        private SessionWrapper _session;
        private Guid _contextId;

        public Guid ContextId
        {
            get
            {
                if (_contextId == Guid.Empty)
                {
                    _contextId = Guid.NewGuid();
                }

                return _contextId;
            }
        }
        public SessionWrapper Session
        {
            get
            {
                if (_session == null || !_session.IsOpen || !_session.IsConnected)
                {
                    _session = SessionWrapper.Create();
                }

                return _session;
            }
        }
        public IStatelessSession StatelessSession
        {
            get
            {
                if (_statelessSession == null || !_statelessSession.IsOpen || !_statelessSession.IsConnected)
                {
                    _statelessSession = NhibernateSessionFactory.Instance.OpenStatelessSession();
                }

                return _statelessSession;
            }
        }
        public IDbConnection Connection
        {
            get { return Session.Connection; }
        }
        public IDbTransaction CurrentTransaction
        {
            get { return IsActiveTransaction ? GetDbTransaction() : null; }
        }

        public UnitOfWork()
        {
            _session = SessionWrapper.Create();
        }

        public bool IsActiveTransaction
        {
            get { return Session.Transaction != null && Session.Transaction.IsActive; }
        }

        public void Dispose()
        {
            if (_session != null)
            {
                _session.Dispose();
                _session = null;
            }

            if (_statelessSession != null)
            {
                if (_statelessSession.IsOpen)
                {
                    _statelessSession.Close();
                }

                _statelessSession.Dispose();
                _statelessSession = null;
            }
        }

        public void Commit()
        {
            if (Session.Transaction == null || !Session.Transaction.IsActive)
            {
                throw new InvalidOperationException("Não é possível realizar o commit pois não existe nenhuma trasação ativa");
            }

            Session.Transaction.Commit();
        }

        public void Rollback()
        {
            if (!Session.Transaction.IsActive)
            {
                throw new InvalidOperationException("Nenhuma transação ativa para realizar o rollback.");
            }

            Session.Transaction.Rollback();
            CriarNovaSessao();
        }

        public void BeginTransaction(bool limparSessao = false, bool bloquearEscrita = false, bool desabilitarAutoCommit = false)
        {
            if (Session.Transaction != null && Session.Transaction.IsActive)
            {
                throw new InvalidOperationException("A transação já foi aberta.");
            }

            if (limparSessao)
            {
                Clear();
            }

            if (desabilitarAutoCommit)
            {
                Session.GetISession().FlushMode = FlushMode.Commit;
            }

#if DEBUG
            Session.BeginTransaction(IsolationLevel.ReadCommitted);
#else
            Session.BeginTransaction(bloquearEscrita ? IsolationLevel.RepeatableRead : IsolationLevel.ReadCommitted);
#endif
        }

        public void Clear()
        {
            Session.Clear();
        }

        private void CriarNovaSessao()
        {
            if (_session != null)
            {
                if (_session.IsOpen)
                {
                    _session.Close();
                }

                _session.Dispose();
            }

            _session = null;
        }

        public void Flush()
        {
            Session.Flush();
        }

        private IDbTransaction GetDbTransaction()
        {
            return TypeHelper.GetFieldValue(Session.Transaction, "trans") as IDbTransaction;
        }
    }
}
