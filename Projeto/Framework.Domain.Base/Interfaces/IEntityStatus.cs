namespace Framework.Domain.Base.Interfaces
{
    public interface IEntityStatus<TId> : IEntity<TId>
    {
        bool Active { get; set; }
    }
}