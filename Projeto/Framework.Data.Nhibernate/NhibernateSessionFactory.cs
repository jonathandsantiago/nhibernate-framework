using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Framework.Data.Nhibernate.Conventions;
using Framework.Data.Nhibernate.Dialects;
using Framework.Data.Nhibernate.Interceptors;
using Framework.Data.Nhibernate.Linq.Functions;
using Framework.Helper.Helpers;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Proxy;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace Framework.Data.Nhibernate
{
    public class NhibernateSessionFactory : IDisposable
    {
        private readonly object _objLock = new object();
        private readonly object _objNoLock = new object();
        private readonly IConfiguration _configuration;
        private readonly bool _createSchema;
        private readonly bool _executeSchemaUpdate;
        private readonly DBType _dBType;

        private static NhibernateSessionFactory _instance;
        private ISessionFactory _factory;

        public NhibernateSessionFactory()
        { }

        public NhibernateSessionFactory(bool createSchema, DBType dBType, bool executeSchemaUpdate)
            : this()
        {
            _createSchema = createSchema;
            _dBType = dBType;
            _executeSchemaUpdate = executeSchemaUpdate;
        }

        public NhibernateSessionFactory(IConfiguration configuration, bool createSchema = true, DBType dBType = DBType.SqlServer, bool executeSchemaUpdate = false)
            : this(createSchema, dBType, executeSchemaUpdate)
        {
            _configuration = configuration;
        }

        public virtual ISessionFactory SessionFactory
        {
            get { return _factory; }
        }

        public virtual IStatelessSession OpenStatelessSession()
        {
            return SessionFactory.OpenStatelessSession();
        }

        public virtual ISession OpenSession()
        {
            lock (_objLock)
            {
                ISession session = GetFactory().OpenSession();

                if (session == null)
                {
                    throw new InvalidOperationException("OpenSession() is null.");
                }

                return session;
            }
        }

        public ISession OpenNoLockSession()
        {
            lock (_objNoLock)
            {
                ISessionBuilder session = GetFactory().WithOptions();
                session.Interceptor(new NoLockInterceptor());

                if (session == null)
                {
                    throw new InvalidOperationException("NoLockInterceptor() is null.");
                }

                return session.OpenSession();
            }
        }

        public virtual ISessionFactory GetFactory()
        {
            if (_factory == null)
            {
                Assembly assembly = GetAssembly();
                FluentConfiguration configuration = Fluently.Configure();
                DbConnectionStringBuilder connStringBuilder = GetConnectionStringBuilder();

                switch (_dBType)
                {
                    case DBType.SqlServer:
                        configuration.Database(MsSqlConfiguration.MsSql2012.Dialect<SqlServer2012Dialect>().ConnectionString(connStringBuilder.ConnectionString));
                        break;
                    case DBType.SqlLite:
                        configuration.Database(SQLiteConfiguration.Standard.Dialect<SQLiteDialect>().ConnectionString(connStringBuilder.ConnectionString));
                        break;
                    case DBType.SqlCe:
                        configuration.Database(MsSqlCeConfiguration.MsSqlCe40.Dialect<MsSqlCeDialect>().ConnectionString(connStringBuilder.ConnectionString));
                        break;
                    case DBType.MySql:
                        configuration.Database(MySQLConfiguration.Standard.Dialect<MySQLDialect>().ConnectionString(connStringBuilder.ConnectionString));
                        break;
                    default:
                        throw new NotImplementedException();
                };

                configuration.ExposeConfiguration((c) =>
                {
                    c.LinqToHqlGeneratorsRegistry<ExtendedLinqtoHqlGeneratorsRegistry>();
                    c.SetProperty("command_timeout", "60");
                })
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssembly(assembly);
                    m.FluentMappings
                        .Conventions.Add<ForeignKeyNameConvention>()
                        .Conventions.Add<ComponentConvention>()
                        .Conventions.Add<ForeignKeyConstraintNameConvention>();
                });

                if (_createSchema)
                {
                    SchemaUpdate(connStringBuilder, configuration.BuildConfiguration());
                }

                if (_executeSchemaUpdate)
                {
                    new SchemaUpdate(configuration.BuildConfiguration()).Execute(true, true);
                }

                _factory = configuration.BuildSessionFactory();

                if (_factory == null)
                {
                    throw new InvalidOperationException("BuildSessionFactory is null.");
                }
            }

            return _factory;
        }

        public virtual void Dispose()
        {
            _factory.Close();
        }

        public static NhibernateSessionFactory Instance
        {
            get { return _instance; }
        }

        public static void Initializar(bool createSchema = true, DBType dBType = DBType.SqlServer, bool activeProxy = true, bool executeSchemaUpdate = false)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("Sessão já iniciada.");
            }

            _instance = new NhibernateSessionFactory(createSchema: createSchema, dBType: dBType, executeSchemaUpdate: executeSchemaUpdate);

            if (activeProxy)
            {
                InitializarProxy();
            }
        }

        public static void Initializar(IConfiguration configuration, bool createSchema = true, DBType dBType = DBType.SqlServer, bool activeProxy = true, bool executeSchemaUpdate = false)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("Sessão já iniciada.");
            }

            _instance = new NhibernateSessionFactory(configuration, createSchema, dBType, executeSchemaUpdate);

            if (activeProxy)
            {
                InitializarProxy();
            }
        }

        private static void InitializarProxy()
        {
            TypeHelper.AddRealTypeResolver((obj) =>
            {
                if (obj is INHibernateProxy)
                {
                    ILazyInitializer lazyInitialiser = ((INHibernateProxy)obj).HibernateLazyInitializer;
                    return lazyInitialiser.PersistentClass;
                }

                return null;
            });

            TypeHelper.AddCheckerPropProxies((obj, propName) =>
            {
                try
                {
                    return NHibernateUtil.IsInitialized(TypeHelper.GetPropValue(obj, propName));
                }
                catch
                {
                    return true;
                }
            });
        }

        private Assembly GetAssembly()
        {
            if (_configuration != null)
            {
                return Assembly.Load(_configuration.GetSection("AssemblyMapName").Value);
            }

            return Assembly.Load(ConfigurationManager.AppSettings["AssemblyMapName"]);
        }

        private string GetConnectionString()
        {
            if (_configuration != null)
            {
                return _configuration.GetConnectionString("ConnectionName");
            }

            return ConfigurationManager.ConnectionStrings["ConnectionName"].ConnectionString;
        }

        private void SchemaUpdate(DbConnectionStringBuilder connStringBuilder, NHibernate.Cfg.Configuration configuration)
        {
            bool commented = false;

            Action<string, string> salvarAction = (fileName, s) =>
            {
                using (FileStream file = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(file))
                    {
                        if (!commented)
                        {
                            string dataBaseName = string.Empty;

                            if (connStringBuilder is SqlConnectionStringBuilder)
                            {
                                dataBaseName = " / " + (connStringBuilder as SqlConnectionStringBuilder).InitialCatalog;
                            }

                            sw.WriteLine("-- Schema Update: " + DateTime.Now.ToString() + dataBaseName);
                            commented = true;
                        }

                        sw.WriteLine(s.Trim() + ";");
                        sw.Close();
                    }
                }
            };

            new SchemaUpdate(configuration).Execute((s) => salvarAction("schemaupdate." + Assembly.GetEntryAssembly().GetName().Version + ".sql", s), false);
        }

        private DbConnectionStringBuilder GetConnectionStringBuilder()
        {
            System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConnectionStringsSection configSection = configuration.GetSection("connectionStrings") as ConnectionStringsSection;

            if (!configSection.ElementInformation.IsLocked && !configSection.SectionInformation.IsLocked)
            {
                if (configSection.SectionInformation.IsProtected)
                {
                    configSection.SectionInformation.UnprotectSection();
                }
            }

            return new SqlConnectionStringBuilder(GetConnectionString());
        }
    }
}