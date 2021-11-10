using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Helper.Helpers
{
    public static class ExpressionHelper
    {
        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            return GetPropertyName<T, object>(expression);
        }

        public static string GetPropertyName<T, TMember>(Expression<Func<T, TMember>> expression)
        {
            MemberExpression body = expression.Body as MemberExpression;

            if (body == null)
            {
                body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        public static string GetNestledPropertyName<T, T2>(Expression<Func<T, T2>> expression)
        {
            NestedPropertyNameExpressionVisitor visitor = new NestedPropertyNameExpressionVisitor();
            visitor.Visit(expression.Body);
            return visitor.GetNomePropriedadeCompleto();
        }

        public static string[] GetPropertysNames<T>(params Expression<Func<T, object>>[] expression)
        {
            return GetPropertysNames<T, object>(expression);
        }

        public static string[] GetPropertysNames<T, TOther>(params Expression<Func<T, TOther>>[] expression)
        {
            return expression.Select(c => GetPropertyName(c)).ToArray();
        }

        public static object GetValue(LambdaExpression expression)
        {
            Delegate del = expression.Compile();
            return del.DynamicInvoke();
        }

        public static object GetValue(object source, LambdaExpression expression)
        {
            Delegate del = expression.Compile();
            return del.DynamicInvoke(source);
        }

        public static Expression CreateEqual(Type type, string property, object constatValue)
        {
            ParameterExpression parameter = Expression.Parameter(type, "p");
            Expression expression = ExpressionHelper.GetNestledPropertyExpression(parameter, property);
            ConstantExpression constantExpression = Expression.Constant(constatValue, constatValue.GetType());
            return Expression.Equal(expression, constantExpression);
        }

        public static Expression GetNestledPropertyExpression(ParameterExpression param, string propertyName)
        {
            Expression body = param;

            foreach (string member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return body;
        }

        public static Expression GetNestledPropertyExpression(Type type, string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(type, "p");
            return GetNestledPropertyExpression(parameter, propertyName);
        }

        public static Expression<Func<T, TResult>> Convert<TOrigin, T, TResult>(Expression<Func<TOrigin, TResult>> expr)
        {
            Dictionary<ParameterExpression, ParameterExpression> parametersMap = expr.Parameters
                .Where(pe => pe.Type == typeof(TOrigin))
                .ToDictionary(pe => pe, pe => Expression.Parameter(typeof(T)));

            DelegateConversionVisitor visitor = new DelegateConversionVisitor(parametersMap);
            Expression newBody = visitor.Visit(expr.Body);

            IEnumerable<ParameterExpression> parameters = expr.Parameters.Select(visitor.MapParameter);

            return Expression.Lambda<Func<T, TResult>>(newBody, parameters);
        }

        public static Expression<Func<T, TResult>> Convert<TOrigin, T, TResult>(Expression<Func<T, TOrigin>> member, Expression<Func<TOrigin, TResult>> expr)
        {
            Expression body = new ParameterExpressionReplacer { Source = expr.Parameters[0], Target = member.Body }.Visit(expr.Body);
            Expression<Func<T, TResult>> result = Expression.Lambda<Func<T, TResult>>(body, member.Parameters);
            return result;
        }

        public static Expression<Func<T, TResult>> CreateExpressionProperty<T, TResult>(ParameterExpression param, string propertyName)
        {
            Expression body = param;

            foreach (string member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return Expression.Lambda<Func<T, TResult>>(body, param);
        }

        public static Expression<Func<T, TResult>> ReplaceParameterByConstant<T, TConstant, TResult>(Expression<Func<T, TConstant, TResult>> predicate, object constant)
        {
            Expression body = predicate.Body;
            VariableSubstitutionVisitor substitutionVisitor = new VariableSubstitutionVisitor(predicate.Parameters[1], Expression.Constant(constant, typeof(TConstant)));
            Expression visitedBody = substitutionVisitor.Visit(body).Reduce();
            return Expression.Lambda<Func<T, TResult>>(visitedBody, predicate.Parameters[0]);
        }

        public static Expression<Func<T, TResult>> ReplaceParameterByConstant<T, TConstant1, TConstant2, TResult>(Expression<Func<T, TConstant1, TConstant2, bool>> predicate, TConstant1 constant1, TConstant2 constant2)
        {
            Expression body = predicate.Body;
            VariableSubstitutionVisitor substitutionVisitor = new VariableSubstitutionVisitor(predicate.Parameters[1], Expression.Constant(constant1, typeof(TConstant1)));
            Expression visitedBody = substitutionVisitor.Visit(body).Reduce();
            Expression<Func<T, TConstant2, TResult>> newLambda = Expression.Lambda<Func<T, TConstant2, TResult>>(visitedBody, predicate.Parameters[0], predicate.Parameters[2]);
            return ReplaceParameterByConstant<T, TConstant2, TResult>(newLambda, constant2);
        }

        public static Expression<Func<T, TResult>> ReplaceParameterByConstant<T, TResult>(Expression<Func<T, object, bool>> predicate, object constant)
        {
            Expression body = predicate.Body;
            VariableSubstitutionVisitor substitutionVisitor = new VariableSubstitutionVisitor(predicate.Parameters[1], Expression.Constant(constant, constant.GetType()));
            Expression visitedBody = substitutionVisitor.Visit(body).Reduce();
            return Expression.Lambda<Func<T, TResult>>(visitedBody, predicate.Parameters[0]);
        }

        public static Expression<Func<T, bool>> CombineWithAndAlso<T>(this Expression<Func<T, bool>> func1, Expression<Func<T, bool>> func2)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(func1.Body, new ExpressionParameterReplacer(func2.Parameters, func1.Parameters).Visit(func2.Body)), func1.Parameters);
        }

        public static Expression<Func<T, bool>> CombineWithOrElse<T>(this Expression<Func<T, bool>> func1, Expression<Func<T, bool>> func2)
        {
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    func1.Body, new ExpressionParameterReplacer(func2.Parameters, func1.Parameters).Visit(func2.Body)),
                func1.Parameters);
        }

        public static Func<T, TResult> ConvertFunc<TIn, T, TResult>(Func<TIn, TResult> func) where TIn : T
        {
            return p => func((TIn)p);
        }
    }

    public sealed class DelegateConversionVisitor : ExpressionVisitor
    {
        private IDictionary<ParameterExpression, ParameterExpression> parametersMap;
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(MapParameter(node));
        }
        public DelegateConversionVisitor(IDictionary<ParameterExpression, ParameterExpression> parametersMap)
        {
            this.parametersMap = parametersMap;
        }

        public ParameterExpression MapParameter(ParameterExpression source)
        {
            ParameterExpression target = source;
            parametersMap.TryGetValue(source, out target);
            return target;
        }
    }

    public class ExpressionParameterReplacer : ExpressionVisitor
    {
        private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; set; }
        protected override Expression VisitParameter(ParameterExpression node)
        {

            if (ParameterReplacements.TryGetValue(node, out ParameterExpression replacement))
            {
                node = replacement;
            }
            return base.VisitParameter(node);
        }

        public ExpressionParameterReplacer(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
        {
            ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
            for (int i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
            {
                ParameterReplacements.Add(fromParameters[i], toParameters[i]);
            }
        }
    }

    public class NestedPropertyNameExpressionVisitor : ExpressionVisitor
    {
        private string nomePropriedadeCompleta = string.Empty;
        protected override Expression VisitMember(MemberExpression node)
        {
            nomePropriedadeCompleta = node.Member.Name + "." + nomePropriedadeCompleta;
            return base.VisitMember(node);
        }

        public string GetNomePropriedadeCompleto()
        {
            return nomePropriedadeCompleta.Remove(nomePropriedadeCompleta.Length - 1);
        }
    }

    public class ParameterExpressionReplacer : ExpressionVisitor
    {
        public ParameterExpression Source { get; set; }
        public Expression Target { get; set; }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == Source ? Target : base.VisitParameter(node);
        }
    }

    public class VariableSubstitutionVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;
        private readonly ConstantExpression _constant;
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node == _parameter)
            {
                return _constant;
            }

            return node;
        }

        public VariableSubstitutionVisitor(ParameterExpression parameter, ConstantExpression constant)
        {
            _parameter = parameter;
            _constant = constant;
        }
    }
}