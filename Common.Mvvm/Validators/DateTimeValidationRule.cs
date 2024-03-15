using System.Globalization;
using System.Windows.Controls;

namespace Common.Mvvm.Validators
{
    public class DateTimeValidationRule : ValidationRule
    {
        public bool AllowNull { get; set; }

        public override ValidationResult Validate
            (object value, CultureInfo cultureInfo)
        {
            var error = string.Empty;

            try
            {
                if (!AllowNull)
                {
                    if (value == null) return new ValidationResult(false, "Значение не может быть пустым");
                    var dateTime = DateTime.Parse(value.ToString() ?? throw new InvalidOperationException(), cultureInfo);
                }
            }
            catch (FormatException)
            {
                return new ValidationResult(false, $"Необходимо ввести корректное значение. {error}");
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Необходимо ввести корректное значение");
            }

            return new ValidationResult(true, null);
        }
    }
}
