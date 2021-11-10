using Framework.Helper.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Helper.Extension
{
    public static class ObjectExtesion
    {
        public static bool In<T>(this T @value, params T[] values)
        {
            return values.Contains(@value);
        }

        public static bool In<T>(this T @value, IQueryable<T> values)
        {
            return values.Contains(@value);
        }

        public static bool NotIn<T>(this T @value, params T[] values)
        {
            return !values.Contains(@value);
        }

        public static bool NotIn<T>(this T @value, IQueryable<T> values)
        {
            return !values.Contains(@value);
        }

        public static TResult GetNestedPropertyValue<T, TResult>(this object obj, Expression<Func<T, object>> memberExpression)
        {
            return TypeHelper.GetNestedPropertyValue<T, TResult>(obj, memberExpression);
        }

        public static TResult GetPropertyValue<TResult>(this object obj, string propertyName)
        {
            return (TResult)TypeHelper.GetPropValue(obj, propertyName);
        }

        public static void SetNestedPropertyValue<T>(this object obj, Expression<Func<T, object>> memberExpression, object value)
        {
            TypeHelper.SetNestedPropertyValue<T>(obj, memberExpression, value);
        }
    }
}