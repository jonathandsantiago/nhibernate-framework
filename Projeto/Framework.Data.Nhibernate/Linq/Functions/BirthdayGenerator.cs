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
    public class BirthdayGenerator : BaseHqlGeneratorForMethod
    {
        public BirthdayGenerator()
        {
            SupportedMethods = new[]
            {
                ReflectHelper.GetMethodDefinition<DateTime>(d => d.IsBirthday(DateTime.Now,DateTime.Now)),
            };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            List<HqlExpression> parameters = new List<HqlExpression>
            {
                visitor.Visit(arguments[0]).AsExpression(),
                visitor.Visit(arguments[1]).AsExpression(),
                visitor.Visit(arguments[2]).AsExpression()
            };

            return treeBuilder.BooleanMethodCall("Birthday", parameters);
        }
    }
}
