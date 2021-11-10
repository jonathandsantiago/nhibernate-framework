using Framework.Validator.Interfaces;
using Framework.Validator.Validation.Interfaces;
using System;
using System.Collections.Generic;

namespace Framework.Validator.Validation
{
    public class Validation<TEntity> : ValidationBase, IValidation<TEntity>
        where TEntity : IEntityValidation
    {
        private readonly Dictionary<string, IValidationRule<TEntity>> _validationsRules;

        public Validation()
        {
            Result = new ValidationResult();
            _validationsRules = new Dictionary<string, IValidationRule<TEntity>>();
        }

        protected virtual void AddRule(IValidationRule<TEntity> validationRule)
        {
            string ruleName = validationRule.GetType() + Guid.NewGuid().ToString("D");
            _validationsRules.Add(ruleName, validationRule);
        }

        protected virtual void RemoveRule(string ruleName)
        {
            _validationsRules.Remove(ruleName);
        }

        public virtual IValidationResult Validate(TEntity entity)
        {
            foreach (string key in _validationsRules.Keys)
            {
                IValidationRule<TEntity> rule = _validationsRules[key];

                if (!rule.Valid(entity))
                {
                    Result.Add(new ValidationError(rule.ErrorMessage, rule.PropertyName));
                }
            }

            return Result;
        }
    }

    public class ValidationBase : IValidation
    {
        public virtual IValidationResult Result { get; set; }
    }
}