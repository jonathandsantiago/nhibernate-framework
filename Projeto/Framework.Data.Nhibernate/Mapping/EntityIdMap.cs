using FluentNHibernate.Mapping;
using Framework.Data.Nhibernate.Extensions;
using Framework.Domain.Base;
using Framework.Helper.Helpers;
using System;
using System.Linq.Expressions;

namespace Framework.Data.Nhibernate.Mapping
{
    public class EntityIdMap<TId, TEntity> : ClassMap<TEntity>
        where TEntity : Entity<TId>
    {
        public EntityIdMap()
        {
            OnMapId();
        }

        protected virtual void OnMapId()
        {
            if (typeof(TId) == typeof(Guid))
            {
                Id(c => c.Id).GeneratedBy.GuidComb();
            }
            else
            {
                Id(c => c.Id).GeneratedBy.Identity();
            }
        }

        public PropertyPart MapIndex(Expression<Func<TEntity, object>> memberExpression)
        {
            return Map(memberExpression).Index("Idx" + ExpressionHelper.GetPropertyName(memberExpression));
        }

        public ManyToOnePart<TOther> ReferenceIndex<TOther>(Expression<Func<TEntity, TOther>> memberExpression)
        {
            return NhibernateMapExtension.ReferenceIndex(this, memberExpression).ForeignKey($"Fk_{typeof(TEntity).Name}_{ExpressionHelper.GetPropertyName(memberExpression)}");
        }
    }
}