using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Helper.Extension
{
    public static class ListExtension
    {
        public static bool Exists<T>(this IList<T> list, Predicate<T> predicate)
        {
            return list.Find<T>(predicate) != null;
        }

        public static bool ContainsUniqueItem<T>(this IList<T> list)
        {
            return list.Count == 1;
        }

        public static bool ContainsAll<T>(this IList<T> list, IList<T> comparerList, bool inverseComparation = false)
        {
            if (comparerList == null)
            {
                return !list.Any();
            }

            foreach (T item in list)
            {
                if (!comparerList.Contains(item))
                {
                    return false;
                }
            }

            if (inverseComparation)
            {
                foreach (T item in comparerList)
                {
                    if (!list.Contains(item))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool IsNotEmpty<T>(this IList<T> list)
        {
            return list.Count > 0;
        }

        public static bool IsEmpty<T>(this IList<T> list)
        {
            return list.Count == 0;
        }

        public static int IndexOf<T>(this IEnumerable<T> list, Predicate<T> predicate)
        {
            int i = -1;

            if (list == null)
            {
                return i;
            }

            return list.Any(x =>
            {
                i++;
                return predicate(x);
            }
            ) ? i : -1;
        }

        public static int IndexOf<T>(this IList lista, Predicate<T> predicate)
        {
            int i = -1;
            return lista.Cast<object>().Any(x => { i++; return predicate((T)x); }) ? i : -1;
        }

        public static string ExtractString(this IEnumerable<string> list, string key)
        {
            if (!key.EndsWith(":"))
            {
                key = key + ":";
            }

            string item = list.FirstOrDefault(c => c.StartsWith(key));

            return item != null ? item.Substring(key.Length, item.Length - key.Length) : null;
        }

        public static TResult Find<TResult>(this IList<TResult> list, Predicate<TResult> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    return list[i];
                }
            }

            return default(TResult);
        }

        public static TResult Find<TResult>(this IList<TResult> list, Predicate<TResult> predicate, Func<TResult, object> order = null)
        {
            return list.FindAll(predicate, order).FirstOrDefault();
        }

        public static TResult Next<TResult>(this IList<TResult> list, TResult currentItem)
        {
            int nextIndex = list.IndexOf(currentItem) + 1;

            return list.Count() == nextIndex ? default(TResult) : list[nextIndex];
        }

        public static TResult MaxWithDefault<T, TResult>(this IList<T> list, Func<T, TResult> selector)
        {
            return list.Count == 0 ? default(TResult) : list.Max(selector);
        }

        public static TResult MinWithDefault<T, TResult>(this IList<T> list, Func<T, TResult> selector)
        {
            return list.Count == 0 ? default(TResult) : list.Min(selector);
        }

        public static TResult SelectFirstOrDefault<T, TResult>(this IEnumerable<T> list, Func<T, TResult> selector)
        {
            return list.Select(selector).FirstOrDefault();
        }

        public static TResult[] ToArray<TResult>(this IEnumerable<TResult> list, Func<TResult, bool> predicate)
        {
            return list.Where(predicate).ToArray();
        }

        public static TResult[] SelectArray<T, TResult>(this IList<T> list, Func<T, TResult> selector)
        {
            return list.Select(selector).ToArray();
        }

        public static TResult[] SelectArray<T, TResult>(this IEnumerable<T> list, Func<T, TResult> selector)
        {
            return list.Select(selector).ToArray();
        }

        public static TResult[] SelectDistinctArray<T, TResult>(this IEnumerable<T> list, Func<T, TResult> selector)
        {
            return list.Select(selector).Distinct().ToArray();
        }

        public static TResult[] SelectManyArray<T, TResult>(this IEnumerable<T> list, Func<T, IEnumerable<TResult>> selector)
        {
            return list.SelectMany(selector).ToArray();
        }

        public static IList<TResult> AddMany<TResult>(this IList<TResult> list, IEnumerable<TResult> itens, bool clear = false)
        {
            if (clear)
            {
                list.Clear();
            }

            if (itens == null)
            {
                return list;
            }

            foreach (TResult item in itens)
            {
                list.Add(item);
            }

            return list;
        }

        public static IList<TResult> AddMany<TResult>(this IList<TResult> list, params TResult[] itens)
        {
            foreach (TResult item in itens)
            {
                list.Add(item);
            }

            return list;
        }

        public static IList<TResult> Clonar<TResult>(this IList<TResult> list, params TResult[] itensAdicionaisInicio)
        {
            List<TResult> listaClonada = new List<TResult>();

            if (itensAdicionaisInicio != null)
            {
                listaClonada.AddRange(itensAdicionaisInicio);
            }

            foreach (TResult item in list)
            {
                listaClonada.Add(item);
            }

            return listaClonada;
        }

        public static IList<TResult> DistinctToList<TResult>(this IList<TResult> list)
        {
            return list.Distinct().ToList();
        }

        public static IList<TResult> DistinctToList<TSource, TResult>(this IList<TSource> list, Func<TSource, TResult> selector)
        {
            return list.Select(selector).Distinct().ToList();
        }

        public static IList<TResult> FindAll<TResult>(this IList<TResult> list, Predicate<TResult> predicate, Func<TResult, object> order = null)
        {
            IList<TResult> items = new List<TResult>();

            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    items.Add(list[i]);
                }
            }

            return order != null ? items.OrderBy(order).ToList() : items;
        }

        public static IList<TResult> OrderByList<TResult, TKey>(this IEnumerable<TResult> source, Func<TResult, TKey> keySelector, IComparer<TKey> comparer)
        {
            return source.OrderBy(keySelector, comparer).ToList();
        }

        public static IList<TResult> ToList<TResult>(this IEnumerable<TResult> list, Func<TResult, bool> predicate)
        {
            return list.Where(predicate).ToList();
        }

        public static IList<TResult> SelectList<T, TResult>(this IEnumerable<T> list, Func<T, TResult> selector)
        {
            return list.Select(selector).ToList();
        }

        public static IList<TResult> SelectManyList<T, TResult>(this IEnumerable<T> list, Func<T, IEnumerable<TResult>> selector)
        {
            return list.SelectMany(selector).ToList();
        }

        public static IList<TResult> SelectDistinctList<T, TResult>(this IEnumerable<T> list, Func<T, TResult> selector)
        {
            return list.Select(selector).Distinct().ToList();
        }

        public static IEnumerable<TResult> ConcatItem<TResult>(this IEnumerable<TResult> list, TResult item)
        {
            return list.Concat(new List<TResult>() { item });
        }

        public static void AddIf<TResult>(this IList<TResult> list, TResult item, bool predicate)
        {
            if (predicate)
            {
                list.Add(item);
            }
        }

        public static void AddIfNotNull<T>(this IList<T> list, T item)
        {
            if (item == null || string.IsNullOrEmpty(item.ToString()))
            {
                return;
            }

            list.Add(item);
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                list.Add(item);
            }
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
            {
                action(item);
                list.Add(item);
            }
        }

        public static void AddUntil<T>(this IList<T> list, int until, T value)
        {
            for (int i = 0; i < until; i++)
            {
                list.Add(value);
            }
        }

        public static void AddUntil<T>(this IList<T> list, int until, Func<int, T> value)
        {
            for (int i = 0; i < until; i++)
            {
                list.Add(value(i));
            }
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T t in list)
            {
                action(t);
            }
        }

        public static void ForItem<T>(this IList<T> list, Predicate<T> predicado, Action<T> action)
        {
            T item = list.FirstOrDefault(c => predicado(c));

            if (item != null)
            {
                action(item);
            }
        }

        public static void ReplaceFirst<T>(this IList<T> list, Predicate<T> predicate, T item)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                    list.Insert(i, item);
                    return;
                }
            }
        }

        public static void ReplaceFirst<T>(this IList<T> list, Func<T, T, bool> predicate, IList<T> items)
        {
            foreach (T item in items)
            {
                list.ReplaceFirst((c) => predicate(c, item), item);
            }
        }

        public static void RemoveAll<T>(this IList<T> list)
        {
            while (list.Count > 0)
            {
                list.RemoveAt(0);
            }
        }

        public static void RemoveAll<T>(this IList<T> list, Predicate<T> predicate)
        {
            int i = list.IndexOf<T>(predicate);

            while (i >= 0)
            {
                list.RemoveAt(i);
                i = list.IndexOf(predicate);
            }
        }

        public static void RemoveAllOfType<T>(this IList list)
        {
            list.OfType<T>().ToList().ForEach(c => list.Remove(c));
        }
    }
}