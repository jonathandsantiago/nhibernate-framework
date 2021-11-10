using System;
using System.Linq.Expressions;

namespace Framework.Helper.Query
{
    public class Order<T>
    {
        public Expression<Func<T, object>> MemberExpression { get; set; }
        public OrderType Type { get; set; }

        public Order()
        { }

        public Order(Expression<Func<T, object>> membroExpression,
            OrderType type = OrderType.Asc)
        {
            MemberExpression = membroExpression;
            Type = type;
        }

        public static Order<T> Create(Expression<Func<T, object>> memberExpression,
            OrderType type = OrderType.Asc)
        {
            return new Order<T>()
            {
                MemberExpression = memberExpression,
                Type = type
            };
        }

        public static Order<T> Create(string member,
            OrderType tipo = OrderType.Asc)
        {
            var parameter = Expression.Parameter(typeof(T), "param");
            var property = typeof(T).GetProperty(member);
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            Expression conversion = Expression.Convert(propertyAccess, typeof(object));
            var expression = Expression.Lambda<Func<T, object>>(conversion, parameter);

            return new Order<T>()
            {
                MemberExpression = expression,
                Type = tipo
            };
        }
    }
}