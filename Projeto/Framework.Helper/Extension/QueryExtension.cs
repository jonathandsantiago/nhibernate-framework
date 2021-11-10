using Framework.Helper.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Helper.Extension
{
    public static class QueryExtension
    {
        public static IQueryable<TSource> Order<TSource>(this IQueryable<TSource> source, OrderCollection<TSource> order)
        {
            if (order == null || order.Count == 0)
            {
                return source;
            }

            IOrderedQueryable<TSource> orderedQueryable = order[0].Type == OrderType.Asc ?
                source.OrderBy(order[0].MemberExpression) :
                source.OrderByDescending(order[0].MemberExpression);

            for (int i = 1; i < order.Count; i++)
            {
                orderedQueryable = order[i].Type == OrderType.Asc ?
                    orderedQueryable.ThenBy(order[i].MemberExpression) :
                    orderedQueryable.ThenByDescending(order[i].MemberExpression);
            }

            return orderedQueryable;
        }

        public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> source, Filter<TSource> filter)
        {
            if (!Query.Filter<TSource>.IsValid(filter))
            {
                return source;
            }

            return filter.FilterQuery(source);
        }

        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int currentPage, int recordsPerPage)
        {
            return source
                .Skip((currentPage - 1) * recordsPerPage)
                .Take(recordsPerPage);
        }

        public static IEnumerable<TResult> Distinct<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Select(selector).Distinct();
        }

        public static int CountDistinct<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Select(selector).Distinct().Count();
        }
    }
}