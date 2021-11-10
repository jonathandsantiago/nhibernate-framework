using Framework.Helper.Extension;
using Framework.Validator.Validation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Validator.Validation
{
    public class ValidationResult : IValidationResult
    {
        public IList<ValidationError> Errors { get; private set; }
        public string Message { get { return string.Join(Environment.NewLine, Errors.Select(c => c.Message)); } }
        public bool IsValid { get { return !Errors.Any(); } }

        public ValidationResult()
        {
            Errors = new List<ValidationError>();
        }

        public ValidationResult Add(string errorMessage, string attributeName = null)
        {
            Errors.Add(new ValidationError(errorMessage, attributeName));
            return this;
        }

        public ValidationResult Add(ValidationError error)
        {
            Errors.Add(error);
            return this;
        }

        public ValidationResult Add(params ValidationResult[] validationResults)
        {
            if (validationResults == null)
            {
                return this;
            }

            foreach (ValidationResult result in validationResults.Where(r => r != null))
            {
                Errors.AddRange(result.Errors);
            }

            return this;
        }

        public ValidationResult Remove(ValidationError error)
        {
            if (Errors.Contains(error))
            {
                Errors.Remove(error);
            }

            return this;
        }

        public void Clear()
        {
            Errors.Clear();
        }
    }
}