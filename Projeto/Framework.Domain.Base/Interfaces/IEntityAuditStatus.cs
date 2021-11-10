namespace Framework.Domain.Base.Interfaces
{
    public interface IEntityAuditStatus<TId> : IEntityAudit<TId>, IEntityStatus<TId>
    { }
}