using Framework.Domain.Base.Interfaces;
using Framework.Helper.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Domain.Base.Helper
{
    public static class EntityHelper
    {
        public static bool IsNullOrEmpty<T>(params IEntity<T>[] entidades)
        {
            foreach (IEntity<T> entidade in entidades)
            {
                if (IsNullOrEmpty(entidade))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsNullOrDefault<TEntity, TId>(TEntity entity)
            where TEntity : Entity<TId>
        {
            return entity == null || entity.Id == null || IsDefault(entity.Id);
        }

        public static bool IsDefault<TId>(TId id)
        {
            return id.Equals(default(TId));
        }

        public static bool IsNullOrEmpty<T>(IEntity<T> entidade)
        {
            return entidade == null || EqualityComparer<T>.Default.Equals(entidade.Id, default(T));
        }

        public static bool IsNull<T>(IEntity<T> entidade)
        {
            return entidade == null;
        }

        public static bool Equals<T>(IEntity<T> entity, IEntity<T> other)
        {
            if (EqualityComparer<T>.Default.Equals(entity.Id, default(T)))
            {
                return entity.Guid != other.Guid;
            }

            return !EqualityComparer<T>.Default.Equals(entity.Id, other.Id);
        }

        public static bool Equals(IEntity entity, IEntity other, bool validNullEntity = false)
        {
            if (validNullEntity && (entity == null || other == null))
            {
                return false;
            }

            if (IsDefaultId(entity))
            {
                return entity.Guid == other.Guid;
            }

            return EqualityComparer<object>.Default.Equals(entity.GetId(), other.GetId());
        }

        public static bool IsDefaultId(IEntity entity)
        {
            Type typeId = entity.GetId() != null ? entity.GetId().GetType() : typeof(object);
            return EqualityComparer<object>.Default.Equals(entity.GetId(), typeId.GetDefaultValue());
        }

        public static bool Equals<T>(IEntity<T> entity, IEntity<T> other, bool validNullEntity = false)
        {
            if (validNullEntity)
            {
                if (entity == null && other == null)
                {
                    return true;
                }

                if (entity == null || other == null)
                {
                    return false;
                }
            }

            if (EqualityComparer<T>.Default.Equals(entity.Id, default(T)))
            {
                return entity.Guid == other.Guid;
            }

            return EqualityComparer<T>.Default.Equals(entity.Id, other.Id);
        }

        public static bool IsNullable(IEntity entity, IEntity other)
        {
            if ((entity == null && other != null) ||
                (entity != null && other == null))
            {
                return false;
            }

            if (entity == null && other == null)
            {
                return true;
            }

            return Equals(entity, other);
        }

        public static bool ListIsNullOrEmpty<T>(IEnumerable<T> list)
        {
            return list == null || list.Count() == 0;
        }

        public static bool IsNew<T>(IEntity<T> entity)
        {
            return EqualityComparer<T>.Default.Equals(entity.Id, default(T));
        }
    }
}
