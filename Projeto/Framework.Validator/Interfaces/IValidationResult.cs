using System.Collections.Generic;

namespace Framework.Validator.Validation.Interfaces
{
    public interface IValidationResult
    {
        IList<ValidationError> Errors { get; }
        string Message { get; }
        ValidationResult Add(string errorMessage, string attributeName = null);
        ValidationResult Add(ValidationError error);
        ValidationResult Add(params ValidationResult[] validationResults);
        ValidationResult Remove(ValidationError error);
        void Clear();
        bool IsValid { get; }
    }
}
