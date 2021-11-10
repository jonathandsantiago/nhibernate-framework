namespace Framework.Validator.Validation
{
    public class ValidationError
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }

        public ValidationError(string message, string attributeName)
        {
            Message = message;
            PropertyName = attributeName;
        }
    }
}