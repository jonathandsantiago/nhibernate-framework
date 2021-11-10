using Framework.Helper.Helpers;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.Util;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Data.Nhibernate.Linq.Functions
{
    public class TryConvertIntGenerator : BaseHqlGeneratorForMethod
    {
        public TryConvertIntGenerator()
        {
            SupportedMethods = new[]
            {
                ReflectHelper.GetMethodDefinition(() => ConvertHelper.SafeToInt("0")),
            };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            return treeBuilder.MethodCall("TryConvert", visitor.Visit(arguments[0]).AsExpression());
        }
    }
}
