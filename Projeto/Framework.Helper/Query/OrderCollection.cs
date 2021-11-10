using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Helper.Query
{
    public class OrderCollection<T> : Collection<Order<T>>
    {
        public OrderCollection()
        { }

        public OrderCollection(Expression<Func<T, object>> memberExpression, OrderType orderType = OrderType.Asc)
        {
            Items.Add(Order<T>.Create(memberExpression, orderType));
        }

        public void Asc(Expression<Func<T, object>> memberExpression)
        {
            Items.Add(Order<T>.Create(memberExpression, OrderType.Asc));
        }

        public void Asc(string member)
        {
            Items.Add(Order<T>.Create(member, OrderType.Asc));
        }

        public void Desc(Expression<Func<T, object>> memberExpression)
        {
            Items.Add(Order<T>.Create(memberExpression, OrderType.Desc));
        }

        public void Desc(string member)
        {
            Items.Add(Order<T>.Create(member, OrderType.Desc));
        }

        public IQueryable<T> OrderByQuery(IQueryable<T> query)
        {
            if (Items.Count == 0)
            {
                return query;
            }

            IOrderedQueryable<T> orderedQueryable = Items[0].Type == OrderType.Asc ?
                query.OrderBy(Items[0].MemberExpression) :
                query.OrderByDescending(Items[0].MemberExpression);

            for (int i = 1; i < Items.Count; i++)
            {
                orderedQueryable = Items[i].Type == OrderType.Asc ?
                    orderedQueryable.ThenBy(Items[i].MemberExpression) :
                    orderedQueryable.ThenByDescending(Items[i].MemberExpression);
            }

            return orderedQueryable;
        }

        public static OrderCollection<T> Create(params Expression<Func<T, object>>[] expressions)
        {
            OrderCollection<T> orders = new OrderCollection<T>();

            foreach (var expression in expressions)
            {
                orders.Asc(expression);
            }

            return orders;
        }

        public static OrderCollection<T> CreateDesc(params Expression<Func<T, object>>[] expressions)
        {
            OrderCollection<T> orders = new OrderCollection<T>();

            foreach (var expression in expressions)
            {
                orders.Desc(expression);
            }

            return orders;
        }

        public static OrderCollection<T> Create(params string[] members)
        {
            OrderCollection<T> orders = new OrderCollection<T>();

            foreach (var member in members)
            {
                orders.Asc(member);
            }

            return orders;
        }

        public static OrderCollection<T> CreateDesc(params string[] members)
        {
            OrderCollection<T> orders = new OrderCollection<T>();

            foreach (var member in members)
            {
                orders.Desc(member);
            }

            return orders;
        }
    }
}