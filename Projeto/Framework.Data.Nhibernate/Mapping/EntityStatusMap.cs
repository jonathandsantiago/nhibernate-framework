using Framework.Domain.Base;

namespace Framework.Data.Nhibernate.Mapping
{
    public class EntityStatusMap<TId, TEntity> : EntityIdMap<TId, TEntity>
        where TEntity : EntityStatus<TId>
    {
        public EntityStatusMap()
        {
            OnMapearStatus();
        }

        protected virtual void OnMapearStatus()
        {
            MapIndex(c => c.Active).Default("1").Not.Nullable();
        }
    }
}