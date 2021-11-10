using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Reflection;

namespace Framework.Data.Nhibernate.Helpers
{
    public static class NhibernateReflectionHelper
    {
        public static readonly MethodInfo ToFutureMethod = typeof(LinqExtensionMethods).GetMethod("ToFuture");
        public static readonly MethodInfo ToFutureValueMethod = typeof(LinqExtensionMethods).GetMethods(BindingFlags.Static | BindingFlags.Public).LastOrDefault(c => c.Name == "ToFutureValue");
        public static readonly MethodInfo QueryMethod = typeof(LinqExtensionMethods).GetMethod("Query", new Type[] { typeof(ISession) });
        public static readonly MethodInfo WhereMethod = typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(c => c.Name == "Where");
        public static readonly MethodInfo CountMethod = typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(c => c.Name == "Count");
        public static readonly MethodInfo FirstOrDefaultMethod = typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public).LastOrDefault(c => c.Name == "FirstOrDefault");

        public static class Enumerable
        {
            public static readonly MethodInfo FirstOrDefaultWithouParameterMethod = typeof(System.Linq.Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(c => c.Name == "FirstOrDefault");
        }
    }
}
