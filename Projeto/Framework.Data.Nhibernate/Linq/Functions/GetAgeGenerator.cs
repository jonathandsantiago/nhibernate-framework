using Framework.Helper.Extension;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.Util;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Data.Nhibernate.Linq.Functions
{
    public class GetAgeGenerator : BaseHqlGeneratorForMethod
    {
        public GetAgeGenerator()
        {
            SupportedMethods = new[]
            {
                ReflectHelper.GetMethodDefinition<DateTime?>(d => d.GetAge(DateTime.Now)),
                ReflectHelper.GetMethodDefinition<DateTime>(d => d.GetAge(DateTime.Now))
            };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            return treeBuilder.MethodCall("GetAge", visitor.Visit(arguments[0]).AsExpression());
        }
    }
}
