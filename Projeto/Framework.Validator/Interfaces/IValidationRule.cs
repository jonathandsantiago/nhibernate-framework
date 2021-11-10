namespace Framework.Validator.Validation.Interfaces
{
    public interface IValidationRule<in TEntity>
    {
        string PropertyName { get; }
        string ErrorMessage { get; }
        bool Valid(TEntity entity);
    }
}