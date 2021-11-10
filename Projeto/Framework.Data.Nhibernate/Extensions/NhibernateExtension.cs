using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Data.Nhibernate.Extensions
{
    public static class NhibernateExtension
    {
        public static IServiceCollection AddNhibernateSql(this IServiceCollection services, IConfiguration configuration, bool createSchema = false, bool activeProxy = true, bool executeSchemaUpdate = false)
        {
            if (NhibernateSessionFactory.Instance == null)
            {
                NhibernateSessionFactory.Initializar(configuration: configuration, createSchema: createSchema, activeProxy: activeProxy, executeSchemaUpdate: executeSchemaUpdate);
            }

            return services;
        }

        public static IServiceCollection AddNhibernateSqlLite(this IServiceCollection services, IConfiguration configuration, bool createSchema = false, bool activeProxy = true, bool executeSchemaUpdate = false)
        {
            if (NhibernateSessionFactory.Instance == null)
            {
                NhibernateSessionFactory.Initializar(configuration: configuration, createSchema: createSchema, dBType: DBType.SqlLite, activeProxy: activeProxy, executeSchemaUpdate: executeSchemaUpdate);
            }

            return services;
        }

        public static IServiceCollection AddNhibernateMySql(this IServiceCollection services, IConfiguration configuration, bool createSchema = false, bool activeProxy = true, bool executeSchemaUpdate = false)
        {
            if (NhibernateSessionFactory.Instance == null)
            {
                NhibernateSessionFactory.Initializar(configuration: configuration, createSchema: createSchema, dBType: DBType.MySql, activeProxy: activeProxy, executeSchemaUpdate: executeSchemaUpdate);
            }

            return services;
        }
    }
}