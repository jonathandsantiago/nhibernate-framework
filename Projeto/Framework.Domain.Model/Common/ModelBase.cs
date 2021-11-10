namespace Framework.Domain.Model.Common
{
    public class ModelBase<TId> : IModelBase<TId>
    {
        public TId Id { get; set; }
    }
}