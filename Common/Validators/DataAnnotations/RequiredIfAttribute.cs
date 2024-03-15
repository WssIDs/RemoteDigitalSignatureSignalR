using System.ComponentModel.DataAnnotations;

namespace Common.Validators.DataAnnotations
{
    /// <summary>
    /// Атрибут валидации требования данного свойства при условии значения другого свойства
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class RequiredIfAttribute : ValidationAttribute
    {
        public string PropertyName { get; }
        public object Value { get; }
        public Type OperandType { get; private set; }

        public RequiredIfAttribute(Type type, string propertyName, object value)
        {
            PropertyName = propertyName;
            Value = value;
            OperandType = type;
        }

        public RequiredIfAttribute(string propertyName, object value)
        {
            PropertyName = propertyName;
            Value = value;
        }

        public RequiredIfAttribute(string propertyName, object value, string errorMessage = "")
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
            Value = value;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var propertyValue = GetPropertyValue(instance, PropertyName);

            bool compare;

            if (OperandType == typeof(decimal?))
            {
                compare =
                    (decimal.TryParse(Value?.ToString(), out var compare1) ? compare1 : default(decimal?)) ==
                    (decimal.TryParse(propertyValue?.ToString(), out var compare2) ? compare2 : default(decimal?));
            }
            else if (OperandType == typeof(decimal))
            {
                compare =
                    (decimal.TryParse(Value?.ToString(), out var compare1) ? compare1 : default) ==
                    (decimal.TryParse(propertyValue?.ToString(), out var compare2) ? compare2 : default);
            }
            else if (OperandType == typeof(short?))
            {
                compare =
                    (short.TryParse(Value?.ToString(), out var compare1) ? compare1 : default(short?)) ==
                    (short.TryParse(propertyValue?.ToString(), out var compare2) ? compare2 : default(short?));
            }
            else if (OperandType == typeof(short))
            {
                compare =
                    (short.TryParse(Value?.ToString(), out var compare1) ? compare1 : default) ==
                    (short.TryParse(propertyValue?.ToString(), out var compare2) ? compare2 : default);
            }
            else
            {
                compare = Equals(propertyValue, Value);
            }

            return (compare && (value == null || string.IsNullOrEmpty(value.ToString())))
                ? new ValidationResult(ErrorMessage, new List<string> { validationContext.DisplayName })
                : ValidationResult.Success;
        }

        private object GetPropertyValue(object src, string propName)
        {
            if (propName.Contains("."))//complex type nested
            {
                var temp = propName.Split(new[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            else
            {
                var prop = src?.GetType().GetProperty(propName);
                return prop?.GetValue(src, null);
            }
        }

        public override object TypeId => Guid.NewGuid();
    }

}
