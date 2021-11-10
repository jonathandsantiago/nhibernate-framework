namespace Framework.Validator.Validation.Interfaces
{
    public interface IValidation<in TEntity> : IValidation
    {
        IValidationResult Validate(TEntity entity);
    }

    public interface IValidation
    { }
}