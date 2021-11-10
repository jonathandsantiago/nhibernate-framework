using NHibernate;
using System;

namespace Framework.Data.Nhibernate
{
    public interface INhibernateSessionFactory : IDisposable
    {
        IStatelessSession OpenStatelessSession();
        ISession OpenSession();
        ISessionFactory GetFactory();
    }
}
