using Framework.Data.Nhibernate;
using Framework.Domain.Base.Interfaces;
using Framework.Helper.Query;
using NHibernate;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Data.Repository.Common
{
    public class SessionWrapper : IDisposable
    {
        private ISession _session;

        public bool IsOpen
        {
            get { return _session.IsOpen; }
        }

        public bool IsConnected
        {
            get { return _session.IsConnected; }
        }

        public ITransaction Transaction
        {
            get { return _session.Transaction; }
        }

        public IDbConnection Connection
        {
            get { return _session.Connection; }
        }

        private SessionWrapper(ISession session)
        {
            this._session = session;
        }

        public T Merge<T>(T entidade)
            where T : class
        {
            CheckTransaction();
            return _session.Merge<T>(entidade);
        }

        public T MergeEntity<T, TId>(ref T entidade)
            where T : class, IEntity<TId>
        {
            T entidadeMergiada = Merge<T>(entidade as T);
            (entidade as IEntity<TId>).Id = (entidadeMergiada as IEntity<TId>).Id;
            return entidadeMergiada;
        }

        public object MergeObject(object obj)
        {
            CheckTransaction();
            return _session.Merge(obj);
        }

        public void Delete<T>(T entidade)
            where T : class
        {
            CheckTransaction();

            _session.Delete(entidade);
        }

        public void Update(object obj)
        {
            CheckTransaction();
            _session.Update(obj);
        }

        public void Save(object obj)
        {
            _session.Save(obj);
        }

        public IQueryable<T> Query<T>()
        {
            return _session.Query<T>();
        }

        public IQueryable<T> Query<T>(Filter<T> filter)
        {
            var query = _session.Query<T>();

            if (filter != null)
            {
                query = filter.FilterQuery(query);
            }

            return query;
        }

        public bool Exists<T, TId>(Expression<Func<T, bool>> predicate)
            where T : IEntity<TId>
        {
            return _session.Query<T>()
                .Where(predicate)
                .Select(c => new { c.Id })
                .FirstOrDefault() != null;
        }

        public IQueryable<T> QueryWithPredicate<T>(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _session.Query<T>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query;
        }

        public T Load<T>(object id)
        {
            return _session.Load<T>(id);
        }

        public IQueryOver<T, T> QueryOver<T>()
            where T : class
        {
            return _session.QueryOver<T>();
        }

        public void Refresh(object obj)
        {
            _session.Refresh(obj);
        }

        public void Evict(object obj)
        {
            _session.Evict(obj);
        }

        public T Get<T>(object id)
        {
            return _session.Get<T>(id);
        }

        public void SaveOrUpdate(object configuracao)
        {
            CheckTransaction();
            _session.SaveOrUpdate(configuracao);
        }

        #region Async

        public async Task<T> MergeAsync<T>(T entidade)
            where T : class
        {
            CheckTransaction();
            return await _session.MergeAsync<T>(entidade);
        }

        public async Task<object> MergeObjectAsync(object obj)
        {
            CheckTransaction();
            return await _session.MergeAsync(obj);
        }

        public async Task DeleteAsync<T>(T entidade)
            where T : class
        {
            CheckTransaction();

            await _session.DeleteAsync(entidade);
        }

        public async Task UpdateAsync(object obj)
        {
            CheckTransaction();
            await _session.UpdateAsync(obj);
        }

        public async Task SaveAsync(object obj)
        {
          await  _session.SaveAsync(obj);
        }

        public async Task<T> LoadAsync<T>(object id)
        {
            return await _session.LoadAsync<T>(id);
        }

        public async Task RefreshAsync(object obj)
        {
            await _session.RefreshAsync(obj);
        }

        public async Task EvictAsync(object obj)
        {
            await _session.EvictAsync(obj);
        }

        public async Task<T> GetAsync<T>(object id)
        {
            return await _session.GetAsync<T>(id);
        }

        public async Task SaveOrUpdateAsync(object configuracao)
        {
            CheckTransaction();
            await _session.SaveOrUpdateAsync(configuracao);
        }

        #endregion

        private void CheckTransaction()
        {
            if (_session.Transaction == null || !_session.Transaction.IsActive)
            {
                throw new InvalidOperationException("Nenhuma trasação foi iniciada.");
            }
        }

        public void Close()
        {
            _session.Close();
        }

        public void Dispose()
        {
            _session.Dispose();
        }

        public void Clear()
        {
            _session.Clear();
        }

        public void Flush()
        {
            _session.Flush();
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return _session.BeginTransaction(isolationLevel);
        }

        public ISession GetISession()
        {
            return _session;
        }

        public ISQLQuery CreateSQLQuery(string queryString)
        {
            return _session.CreateSQLQuery(queryString);
        }

        public IQuery CreateQuery(string queryString)
        {
            return _session.CreateQuery(queryString);
        }

        public void ExposeISession(Action<ISession> action)
        {
            action(_session);
        }

        public void DisableAutoFlush()
        {
            _session.FlushMode = FlushMode.Commit;
        }

        public ISession OpenNoLockSession()
        {
            return NhibernateSessionFactory.Instance.OpenNoLockSession();
        }

        public static SessionWrapper Create()
        {
            return new SessionWrapper(NhibernateSessionFactory.Instance.OpenSession());
        }
    }
}