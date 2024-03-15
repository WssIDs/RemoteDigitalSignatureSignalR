using System.ComponentModel.DataAnnotations;

namespace Common.Validators.DataAnnotations
{
    public sealed class EqualsValidationAttribute : ValidationAttribute
    {
        private readonly string _propertyToCompare;

        public EqualsValidationAttribute(string propertyToCompare)
        {
            _propertyToCompare = propertyToCompare;
        }

        public EqualsValidationAttribute(string propertyToCompare, string errorMessage) : this(propertyToCompare)
        {
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propInfo = validationContext.ObjectInstance.GetType().GetProperty(_propertyToCompare);
            if (propInfo != null)
            {
                var propValue = propInfo.GetValue(validationContext.ObjectInstance);
                if (value != null && propValue != null && !string.IsNullOrEmpty(value.ToString()) &&
                    !string.IsNullOrEmpty(propValue.ToString()) //if either one is empty don't Validate
                    && (value.ToString() != propValue.ToString()))
                    if (validationContext.MemberName != null)
                        return new ValidationResult(ErrorMessage,
                            new List<string> {validationContext.MemberName, propInfo.Name});
            }
            else
                throw new NullReferenceException($"{_propertyToCompare} must be the name of property to compare");

            return ValidationResult.Success;
        }
    }
}
