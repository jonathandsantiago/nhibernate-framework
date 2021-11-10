using Framework.Helper.Helpers;
using Framework.Validator.Validation.Interfaces;
using System;
using System.Linq.Expressions;

namespace Framework.Validator.Validation
{
    public class ValidationRule<TEntity> : IValidationRule<TEntity>
    {
        private readonly Func<TEntity, bool> _specificationRule;
        public string ErrorMessage { get; private set; }
        public string PropertyName { get; private set; }

        public ValidationRule(Func<TEntity, bool> specificationRule, string errorMessage, Expression<Func<TEntity, object>> expression = null)
            : this(specificationRule, errorMessage)
        {
            PropertyName = expression == null ? string.Empty : ExpressionHelper.GetPropertyName(expression);
        }

        public ValidationRule(Func<TEntity, bool> specificationRule, string errorMessage, string propertyName)
             : this(specificationRule, errorMessage)
        {
            PropertyName = propertyName;
        }

        public ValidationRule(Func<TEntity, bool> specificationRule, string errorMessage)
        {
            _specificationRule = specificationRule;
            ErrorMessage = errorMessage;
        }

        public bool Valid(TEntity entity)
        {
            return !_specificationRule(entity);
        }
    }
}