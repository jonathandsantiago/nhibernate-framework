using Framework.Data.Repository.Interfaces;
using NHibernate;

namespace Framework.Data.Repository.Common
{
    public class NhibernateRepositoryBase : RepositoryBase
    {
        protected NhibernateRepositoryBase(IDataContext dataContext)
            : base(dataContext)
        { }

        protected IUnitOfWork GetUnidade()
        {
            return _dataContext as IUnitOfWork;
        }

        protected SessionWrapper GetSession()
        {
            return GetUnidade().Session;
        }

        protected ISession GetNoLockSession()
        {
            return GetUnidade().Session.OpenNoLockSession();
        }

        protected IStatelessSession GetStatelessSession()
        {
            return GetUnidade().StatelessSession;
        }
    }

    public class NhibernateRepositoryBase<T> : NhibernateRepositoryBase
    {
        protected NhibernateRepositoryBase(IDataContext dataContext)
            : base(dataContext)
        { }
    }
}
