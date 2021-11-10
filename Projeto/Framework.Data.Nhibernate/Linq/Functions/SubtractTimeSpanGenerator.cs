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
    public class SubtractTimeSpanGenerator : BaseHqlGeneratorForMethod
    {
        public SubtractTimeSpanGenerator()
        {
            SupportedMethods = new[]
            {
                ReflectHelper.GetMethodDefinition<DateTime>(d => d.Subtract(TimeSpan.Zero)),
            };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            return treeBuilder.MethodCall("SubtractTimeSpan",
                                          visitor.Visit(targetObject).AsExpression(),
                                          visitor.Visit(arguments[0]).AsExpression());
        }
    }
}
