using Framework.Domain.Base.Bases;
using Framework.Domain.Base.Helper;
using Framework.Domain.Base.Interfaces;
using Framework.Helper.Extension;
using Framework.Helper.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Domain.Base.Extension
{
    public static class EntidadeListExtensions
    {
        private static Dictionary<Type, IList<FieldInfo>> fieldsInfoCache = new Dictionary<Type, IList<FieldInfo>>();

        public static void Update<TEntity>(this IList<TEntity> list, int index, TEntity editItem, params Expression<Func<TEntity, object>>[] ignoredProperties)
        {

            Update(list as IList, index, editItem, new Dictionary<object, object>(), ExpressionHelper.GetPropertysNames(ignoredProperties));
        }

        public static bool Update<TEntity>(this IList<TEntity> list, Predicate<TEntity> predicate, TEntity editItem)
        {
            int index = list.IndexOf(predicate);

            if (index < 0)
            {
                return false;
            }

            (list as IList).Update(index, editItem, new Dictionary<object, object>());

            return true;
        }

        public static bool Remove<TEntity>(this IList<TEntity> list, Predicate<TEntity> predicate)
        {
            int index = list.IndexOf(predicate);

            if (index < 0)
            {
                return false;
            }

            list.RemoveAt(index);

            return true;
        }

        public static void RemoveAll<TEntity>(this IList<TEntity> list, Predicate<TEntity> predicate)
        {
            int index = list.IndexOf(predicate);

            while (index >= 0)
            {
                list.RemoveAt(index);
                index = list.IndexOf(predicate);
            }
        }

        public static bool RemoveAllById<TEntity>(this IList<TEntity> list, TEntity entity)
            where TEntity : IEntity
        {
            int index = list.IndexOf(c => EntityHelper.Equals(entity, c));

            if (index < 0)
            {
                return false;
            }

            list.RemoveAt(index);

            return true;
        }

        public static bool InsertOrEdit<TEntity>(this IList<TEntity> list, Predicate<TEntity> predicate, TEntity editItem, params Expression<Func<TEntity, object>>[] ignoredProperties)
        {
            int index = list.IndexOf(predicate);

            if (index < 0)
            {
                list.Add(editItem);
            }
            else
            {
                list.Update(index, editItem, ignoredProperties);
            }

            return true;
        }

        public static bool InsertOrEdit<TEntity>(this IList<TEntity> list, TEntity editItem)
            where TEntity : IEntity
        {
            int index = list.IndexOf(c => EntityHelper.Equals(c, editItem));

            if (index < 0)
            {
                list.Add(editItem);
            }
            else
            {
                list.Update(index, editItem);
            }

            return true;
        }

        public static bool InsertIfNoExist<TEntity>(this IList<TEntity> list, Predicate<TEntity> predicate, TEntity editItem)
        {
            int index = list.IndexOf(predicate);

            if (index < 0)
            {
                list.Add(editItem);
                return true;
            }

            return false;
        }

        public static TEntity InsertOrEditById<TEntity, TId>(this IList<TEntity> list, TEntity editItem)
            where TEntity : IEntity<TId>
        {
            int index = list.IndexOf(c => EntityHelper.Equals(editItem, c));

            if (index >= 0)
            {
                list.Update(index, editItem);
            }
            else
            {
                list.Add(editItem);
                index = list.Count - 1;
            }

            return list[index];
        }

        public static bool UpdateById<TEntity, TId>(this IList<TEntity> list, TEntity editItem)
            where TEntity : IEntity<TId>
        {
            int index = list.IndexOf(c => EntityHelper.Equals(editItem, (IEntity)c));

            if (index >= 0)
            {
                list.Update(index, editItem);
                return true;
            };

            return false;
        }

        public static bool Exist<T>(this IList<T> list, Predicate<T> predicate)
        {
            return list.IndexOf(predicate) >= 0;
        }

        public static bool Exist<TEntity>(this IList<TEntity> list, TEntity entity)
            where TEntity : IEntity
        {
            return list.IndexOf(c => EntityHelper.Equals(c, entity)) >= 0;
        }

        public static bool ExistById<TEntity>(this IList<TEntity> list, TEntity entity)
            where TEntity : IEntity
        {
            return list.IndexOf(c => EntityHelper.Equals(entity, c)) >= 0;
        }

        public static TEntity GetById<TEntity, TId>(this IList<TEntity> list, TEntity entity)
            where TEntity : IEntity<TId>
        {
            int idx = list.IndexOfById<TEntity, TId>(entity);
            return idx >= 0 ? list[idx] : default(TEntity);
        }

        public static int IndexOfById<TEntity, TId>(this IList<TEntity> list, TEntity entity)
            where TEntity : IEntity<TId>
        {
            return list.IndexOf(c => EntityHelper.Equals(entity, (IEntity)c));
        }

        public static bool ExistDifferentItems<TEntity>(this IList<TEntity> listA, IList<TEntity> listB)
            where TEntity : IEntity
        {
            if (listA.Any(c => !listB.Any(d => EntityHelper.Equals(c, d, true))))
            {
                return true;
            }

            if (listB.Any(c => !listA.Any(d => EntityHelper.Equals(c, d, true))))
            {
                return true;
            }

            return false;
        }

        private static bool EqualsAndNotDefaultId<TId>(TId idA, TId idB)
        {
            return EqualityComparer<TId>.Default.Equals(idA, idB) && !EqualityComparer<TId>.Default.Equals(idA, default(TId));
        }

        private static void Update(this IList list, int index, object editItem, Dictionary<object, object> circularReferences, params string[] ignoredProperties)
        {
            IList<FieldInfo> fields = GetFieldsType(editItem.GetType());

            circularReferences[editItem] = list[index];

            foreach (FieldInfo field in fields)
            {
                if (ignoredProperties.Any(c => string.Format("<{0}>k__BackingField", c) == field.Name))
                {
                    continue;
                }

                object value = field.GetValue(editItem);

                if (value != null && value.GetType().IsGenericType &&
                    value.GetType().InheritsOrImplements(typeof(IEnumerable)) &&
                    value.GetType().InheritsOrImplements(typeof(IEntity)))
                {
                    IEnumerable<IEntity> newCastedList = ((IEnumerable)value).Cast<IEntity>();
                    IList currentList = field.GetValue(list[index]) as IList;

                    IList<int> changedIndexes = new List<int>();

                    foreach (IEntity newItem in newCastedList)
                    {
                        changedIndexes.Add(currentList.InsertOrEditByGenericListId(newItem, circularReferences));
                    }

                    for (int j = currentList.Count - 1; j >= 0; j--)
                    {
                        if (!changedIndexes.Any(c => c == j))
                        {
                            currentList.RemoveAt(j);
                        }
                    }
                }
                else
                {
                    field.SetValue(list[index], value != null && circularReferences.ContainsKey(value) ? circularReferences[value] : value);
                }
            }
        }

        private static IList<FieldInfo> GetFieldsType(Type type)
        {
            fieldsInfoCache.TryGetValue(type, out IList<FieldInfo> fields);

            if (fields != null)
            {
                return fields;
            }

            fields = TypeHelper.GetAllFields(type);

            int i = fields.IndexOf(c => c.Name == "_guid" && c.DeclaringType.Name.Contains("Entidade"));

            if (i > 0)
            {
                fields.RemoveAt(i);
            }

            i = fields.IndexOf(c => c.Name.Contains("<Id>") && c.DeclaringType.Name.Contains("Entidade"));

            if (i > 0)
            {
                fields.RemoveAt(i);
            }

            foreach (FieldInfo item in fields)
            {

            }

            fieldsInfoCache.Add(type, fields);

            return fields;
        }

        public static IList<T> EntityDistinct<T>(this IEnumerable<T> list)
            where T : IEntity
        {
            return list.Distinct(new EntityDistinctComparer<T>()).ToList();
        }

        public static IList<TResult> SelectEntityDistinct<T, TResult>(this IEnumerable<T> list, Func<T, TResult> selector)
            where T : IEntity
            where TResult : IEntity
        {
            return list.Select(selector).Distinct(new EntityDistinctComparer<TResult>()).ToList();
        }

        public static void ReplaceEntityFisrt<TEntity>(this IList<TEntity> list, IList<TEntity> newList)
            where TEntity : IEntity
        {
            list.ReplaceFirst((c, d) => EntityHelper.Equals(c, d), newList);
        }

        public static IList<TEntity> Filter<TEntity>(this IList<TEntity> list, IList<TEntity> filterList, bool ignoreNullFilterList = true)
           where TEntity : IEntity
        {
            return ignoreNullFilterList && filterList == null ? list : list.ToList(c => filterList.Any(d => EntityHelper.Equals(c, d)));
        }

        public static IList<TEntity> Filter<TEntity>(this IList<TEntity> list, TEntity entity, bool ignoreNullFilterList = true)
           where TEntity : IEntity
        {
            return ignoreNullFilterList && entity == null ? list : list.ToList(c => EntityHelper.Equals(c, entity, ignoreNullFilterList));
        }

        public static TEntity FirstOrDefault<TEntity>(this IList<TEntity> list, TEntity entity, bool ignoreNullFilterList = true)
          where TEntity : IEntity
        {
            return list.Filter(entity, ignoreNullFilterList).FirstOrDefault();
        }

        private static int InsertOrEditByGenericListId<TEntity>(this IList list, TEntity editItem, Dictionary<object, object> circularReferences)
            where TEntity : IEntity
        {
            int index = list.IndexOf<IEntity>(c => EntityHelper.Equals(editItem, c));

            if (index >= 0)
            {
                list.Update(index, editItem, circularReferences);
                return index;
            }
            else
            {
                ReplaceCircularReferences(editItem, circularReferences);
                return list.Add(editItem);
            }
        }

        private static void ReplaceCircularReferences(object editItem, Dictionary<object, object> circularReferences)
        {
            IList<FieldInfo> fields = GetFieldsType(editItem.GetType());

            foreach (FieldInfo field in fields)
            {
                if (!TypeHelper.IsNotCoreType(field.FieldType))
                {
                    continue;
                }

                object value = field.GetValue(editItem);

                if (value != null && circularReferences.ContainsKey(value))
                {
                    field.SetValue(editItem, circularReferences[value]);
                }
            }
        }
    }
}