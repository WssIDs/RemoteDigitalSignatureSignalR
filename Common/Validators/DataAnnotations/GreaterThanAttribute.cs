using System.ComponentModel.DataAnnotations;

namespace Common.Validators.DataAnnotations
{
    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly string _propertyToCompare;

        public GreaterThanAttribute(string propertyToCompare)
        {
            _propertyToCompare = propertyToCompare;
        }

        public GreaterThanAttribute(string propertyToCompare, string errorMessage) : this(propertyToCompare)
        {
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propInfo = validationContext.ObjectInstance.GetType().GetProperty(_propertyToCompare);
            if (propInfo != null)
            {
                var firstComparable = value as IComparable;
                var secondComparable = GetSecondComparable(validationContext);

                if (firstComparable != null)
                {
                    if (firstComparable.CompareTo(secondComparable) > 0)
                    {
                        if (validationContext.MemberName != null)
                            return new ValidationResult(ErrorMessage,
                                new List<string> {validationContext.MemberName});
                    }
                }
                else
                {
                    if (firstComparable != null)
                    {
                        var type = firstComparable.GetType();
                        if (firstComparable.CompareTo(Convert.ChangeType(0, type)) > 0)
                        {
                            if (validationContext.MemberName != null)
                                return new ValidationResult(ErrorMessage,
                                    new List<string> {validationContext.MemberName});
                        }
                    }
                }
            }
            else
                throw new NullReferenceException($"{_propertyToCompare} должно быть указано имя свойства для сравнения");

            return ValidationResult.Success;
        }

        protected IComparable GetSecondComparable(
        ValidationContext validationContext)
        {
            var propertyInfo = validationContext
                                  .ObjectType
                                  .GetProperty(_propertyToCompare);
            if (propertyInfo != null)
            {
                var secondValue = propertyInfo.GetValue(
                    validationContext.ObjectInstance, null);
                return secondValue as IComparable;
            }
            return null;
        }
    }
}
