using Framework.Helper.Extension;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Data.Nhibernate.Linq.Functions
{
    public class IsCurrentDateGenerator : BaseHqlGeneratorForMethod
    {
        public IsCurrentDateGenerator()
        {
            SupportedMethods = new[]
            {
                ReflectHelper.GetMethodDefinition<DateTime>(d => d.IsCurrentDate()),
            };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            return treeBuilder.BooleanMethodCall("IsCurrentDate", new List<HqlExpression>() { visitor.Visit(arguments[0]).AsExpression() });
        }
    }
}
