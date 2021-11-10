using Framework.Domain.Base.Interfaces;

namespace Framework.Domain.Base
{
    public class EntityStatus<TId>: Entity<TId>, IEntityStatus<TId>
    {
        public virtual bool Active { get; set; }
    }
}