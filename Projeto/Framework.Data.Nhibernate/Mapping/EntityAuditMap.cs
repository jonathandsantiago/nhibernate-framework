using Framework.Domain.Base;

namespace Framework.Data.Nhibernate.Mapping
{
    public class EntityAuditMap<TId, TEntity> : EntityIdMap<TId, TEntity>
        where TEntity : EntityAudit<TId>
    {
        public EntityAuditMap()
        {
            OnMapAudit();
        }

        protected virtual void OnMapAudit()
        {
            MapIndex(c => c.Revision).Default("0").Not.Nullable();
            MapIndex(c => c.UserInsertion).Length(50).Not.Nullable();
            MapIndex(c => c.UserEdition).Length(50).Nullable();
            MapIndex(c => c.DateInsertion).Not.Nullable();
            MapIndex(c => c.DateEdition).Nullable();
        }
    }
}