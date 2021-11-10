namespace Framework.Domain.Model.Common
{
    public interface IModelBase<TId>
    {
        TId Id { get; set; }
    }
}
