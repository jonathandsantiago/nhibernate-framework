using FluentNHibernate.Mapping;
using Framework.Data.Nhibernate.Extensions;
using Framework.Domain.Base.Interfaces;
using Framework.Helper.Helpers;
using System;
using System.Linq.Expressions;

namespace Framework.Data.Nhibernate.Mapping
{
    public class EntitySubclassMap<TEntity> : SubclassMap<TEntity>
        where TEntity : IEntity
    {
        public PropertyPart MapIndex(Expression<Func<TEntity, object>> memberExpression)
        {
            return Map(memberExpression).Index("Idx" + ExpressionHelper.GetPropertyName(memberExpression));
        }

        public ManyToOnePart<TOther> ReferenceIndex<TOther>(Expression<Func<TEntity, TOther>> memberExpression)
        {
            return NhibernateMapExtension.ReferenceIndex(this, memberExpression);
        }
    }
}