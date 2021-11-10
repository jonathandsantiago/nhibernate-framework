using Framework.Helper.Extension;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.Util;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Data.Nhibernate.Linq.Functions
{
    public class IsNullGenerator : BaseHqlGeneratorForMethod
    {
        public IsNullGenerator()
        {
            SupportedMethods = new[]
            {
                ReflectHelper.GetMethodDefinition<decimal?>(d => d.IsNull(0))
            };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            return treeBuilder.BooleanMethodCall("IsNull", new List<HqlExpression>()
            {
                visitor.Visit(arguments[0]).AsExpression(),
                visitor.Visit(arguments[1]).AsExpression(),
            });
        }
    }
}
