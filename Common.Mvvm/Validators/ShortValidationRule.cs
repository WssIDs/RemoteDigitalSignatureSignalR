using System.Windows.Controls;

namespace Common.Mvvm.Validators
{
    public class ShortValidationRule : ValidationRule
    {
        public bool JustPositive { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var controlValue = short.Parse(value?.ToString() ?? string.Empty, cultureInfo);
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Необходимо ввести целое число");
            }
            catch (OverflowException)
            {
                if (JustPositive)
                {
                    return new ValidationResult(false,
                        $"Выход за пределы диапазона. Макс. = {short.MaxValue}");
                }

                return new ValidationResult(false, $"Выход за пределы диапазона. Мин. = {short.MinValue}, Макс. = {short.MaxValue}");
            }
            catch (ArgumentNullException)
            {
                return new ValidationResult(false, "Поле не может быть пустым");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, $"{ex.Message}");
            }
            return ValidationResult.ValidResult;
        }
    }
}
