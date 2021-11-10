using Framework.Helper.Extension;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.Util;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace Framework.Data.Nhibernate.Linq.Functions
{
    public class InGenerator : BaseHqlGeneratorForMethod
    {
        public InGenerator()
        {
            SupportedMethods = new[]
            {
                ReflectHelper.GetMethodDefinition(() => ObjectExtesion.In(null, (object[]) null)),
                ReflectHelper.GetMethodDefinition(() => ObjectExtesion.In<object>(null, (IQueryable<object>) null)),
                ReflectHelper.GetMethodDefinition(() => ObjectExtesion.NotIn<object>(null, (object[]) null)),
                ReflectHelper.GetMethodDefinition(() => ObjectExtesion.NotIn<object>(null, (IQueryable<object>) null))
            };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            HqlExpression value = visitor.Visit(arguments[0]).AsExpression();
            HqlTreeNode inClauseNode;

            if (arguments[1] is ConstantExpression)
            {
                inClauseNode = BuildFromArray((Array)((ConstantExpression)arguments[1]).Value, treeBuilder);
            }
            else
            {
                inClauseNode = BuildFromExpression(arguments[1], visitor);
            }

            HqlTreeNode inClause = treeBuilder.In(value, inClauseNode);

            if (method.Name == "NotIn")
            {
                inClause = treeBuilder.BooleanNot((HqlBooleanExpression)inClause);
            }

            return inClause;
        }

        private HqlTreeNode BuildFromExpression(Expression expression, IHqlExpressionVisitor visitor)
        {
            return visitor.Visit(expression).AsExpression();
        }

        private HqlTreeNode BuildFromArray(Array valueArray, HqlTreeBuilder treeBuilder)
        {
            Type elementType = valueArray.GetType().GetElementType();

            if (!elementType.IsValueType && elementType != typeof(string))
            {
                throw new ArgumentException("Only primitives and strings can be used");
            }

            Type enumUnderlyingType = elementType.IsEnum ? Enum.GetUnderlyingType(elementType) : null;
            HqlExpression[] variants = new HqlExpression[valueArray.Length];

            for (int index = 0; index < valueArray.Length; index++)
            {
                object variant = valueArray.GetValue(index);
                object val = variant;

                if (elementType.IsEnum)
                {
                    val = Convert.ChangeType(variant, enumUnderlyingType);
                }

                variants[index] = treeBuilder.Constant(val);
            }

            return treeBuilder.ExpressionSubTreeHolder(variants);
        }
    }
}
