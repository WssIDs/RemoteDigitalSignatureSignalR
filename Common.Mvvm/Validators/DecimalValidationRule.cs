using System.Globalization;
using System.Windows.Controls;

namespace Common.Mvvm.Validators
{
    public class DecimalValidator : ValidationRule
    {
        public override ValidationResult Validate
            (object value, CultureInfo cultureInfo)
        {
            try
            {
                var dummy = Convert.ToDecimal(value?.ToString(), CultureInfo.InvariantCulture);

                char[] separators = { ',', '.' };

                if (value != null)
                {
                    var splitting = value.ToString()?.Split(separators);

                    if (splitting is {Length: > 1})
                    {
                        var digits = splitting.First();
                        if (digits.Length > 14)
                        {
                            return new ValidationResult(false,
                                "Максимальное максимальная длина не должна превышать 14 символов до запятой");
                        }

                        var places = splitting.Last();
                        if (places.Length > 2)
                        {
                            return new ValidationResult(false,
                                "Максимальное допустимое количество знаков после запятой должно быть 2");
                        }
                    }
                    else if (splitting is {Length: 1})
                    {
                        var digits = splitting.First();
                        if (digits.Length > 14)
                        {
                            return new ValidationResult(false,
                                "Максимальное максимальная длина не должна превышать 14 символов до запятой");
                        }
                    }
                }
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Необходимо ввести корректное значение");
            }
            catch (OverflowException)
            {
                return new ValidationResult(false, $"Вывод за пределы диапазона. Значение должно быть в пределах от {decimal.MinValue} до {decimal.MaxValue}");
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Необходимо ввести корректное значение");
            }

            return new ValidationResult(true, null);
        }
    }
}
