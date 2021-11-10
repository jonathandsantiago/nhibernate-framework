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
    public class AddTimeSpanGenerator : BaseHqlGeneratorForMethod
    {
        public AddTimeSpanGenerator()
        {
            SupportedMethods = new[]
            {
                ReflectHelper.GetMethodDefinition<DateTime>(d => d.Add(TimeSpan.Zero)),
            };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            return treeBuilder.MethodCall("AddTimeSpan",
                                          visitor.Visit(targetObject).AsExpression(),
                                          visitor.Visit(arguments[0]).AsExpression());
        }
    }
}
