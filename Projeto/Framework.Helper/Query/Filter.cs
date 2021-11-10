using Framework.Helper.Extension;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Helper.Query
{
    public class Filter<T>
    {
        public Expression<Func<T, bool>> Expression { get; set; }

        public Filter()
        { }

        public Filter(Expression<Func<T, bool>> expressao)
        {
            Expression = expressao;
        }

        public Filter<T> And(Expression<Func<T, bool>> expression)
        {
            Expression = Expression != null ? Expression.And(expression) : expression;
            return this;
        }

        public Filter<T> And(Filter<T> Filter)
        {
            Expression = Expression != null ? Expression.And(Filter.Expression) : Filter.Expression;
            return this;
        }

        public Filter<T> Or(Filter<T> Filter)
        {
            Expression = Expression != null ? Expression.Or(Filter.Expression) : Filter.Expression;
            return this;
        }

        public Filter<T> Or(bool predicate, Expression<Func<T, bool>> trueExpression, Expression<Func<T, bool>> falseExpression)
        {
            Expression = Expression != null ?
                Expression.Or(predicate ? trueExpression : falseExpression) :
                (predicate ? trueExpression : falseExpression);
            return this;
        }

        public Filter<T> Or(Expression<Func<T, bool>> expression)
        {
            Expression = Expression != null ? Expression.Or(expression) : expression;
            return this;
        }

        public Filter<T> Set(Expression<Func<T, bool>> expression)
        {
            Expression = expression;
            return this;
        }

        public bool ExistsExpression()
        {
            return Expression != null;
        }

        public static Filter<T> Create(Expression<Func<T, bool>> expression)
        {
            return new Filter<T>()
            {
                Expression = expression
            };
        }

        public static bool IsValid(Filter<T> Filter)
        {
            return Filter != null && Filter.Expression != null;
        }

        public IQueryable<T> FilterQuery(IQueryable<T> query)
        {
            return Expression != null ? query.Where(Expression) : query;
        }
    }
}