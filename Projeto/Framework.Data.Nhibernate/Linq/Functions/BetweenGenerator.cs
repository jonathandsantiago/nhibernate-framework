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
    public class BetweenGenerator : BaseHqlGeneratorForMethod
    {
        public BetweenGenerator()
        {
            SupportedMethods = new[]
            {
                ReflectHelper.GetMethodDefinition<DateTime>(d => d.Between(DateTime.Now, DateTime.Now)),
                ReflectHelper.GetMethodDefinition<TimeSpan>(d => d.Between(TimeSpan.Zero, TimeSpan.Zero))
            };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            return treeBuilder.BooleanMethodCall("Between", new List<HqlExpression>()
            {
                visitor.Visit(arguments[0]).AsExpression(),
                visitor.Visit(arguments[1]).AsExpression(),
                visitor.Visit(arguments[2]).AsExpression()
            });
        }
    }
}
