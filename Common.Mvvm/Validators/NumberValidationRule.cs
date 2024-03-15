using System.Globalization;
using System.Windows.Controls;

namespace Common.Mvvm.Validators
{
    public enum NumberType
    {
        Int,
        Short,
        Double,
        Decimal,
        NullableInt
    }

    public class NumberValidator : ValidationRule
    {
        public NumberType NumberType { get; set; }

        public override ValidationResult Validate
            (object value, CultureInfo cultureInfo)
        {
            var minValue = string.Empty;
            var maxValue = string.Empty;
            var error = string.Empty;

            try
            {
                switch (NumberType)
                {
                    case NumberType.Int:
                        {
                            if (value == null)
                                return new ValidationResult(false, "Значение не может быть пустым");

                            minValue = int.MinValue.ToString();
                            maxValue = int.MaxValue.ToString();

                            error = "Поле может принимать только целые числа";

                            var dummy = Convert.ToInt32(value.ToString(), cultureInfo);
                            break;
                        }
                    case NumberType.Short:
                        {
                            if (value == null)
                                return new ValidationResult(false, "Значение не может быть пустым");

                            minValue = short.MinValue.ToString();
                            maxValue = short.MaxValue.ToString();

                            error = "Поле может принимать только целые числа";

                            var dummy = short.Parse(value.ToString() ?? throw new InvalidOperationException(), cultureInfo);
                            break;
                        }
                    case NumberType.Double:
                        {
                            if (value == null)
                                return new ValidationResult(false, "Значение не может быть пустым");
                            minValue = double.MinValue.ToString(CultureInfo.InvariantCulture);
                            maxValue = double.MaxValue.ToString(CultureInfo.InvariantCulture);

                            error = "Поле может принимать только целые и дробные числа";

                            var dummy = Convert.ToDouble(value.ToString(), cultureInfo);
                            break;
                        }
                    case NumberType.Decimal:
                        {
                            if (value == null)
                                return new ValidationResult(false, "Значение не может быть пустым");
                            minValue = decimal.MinValue.ToString(CultureInfo.InvariantCulture);
                            maxValue = decimal.MaxValue.ToString(CultureInfo.InvariantCulture);

                            error = "Поле может принимать только целые и дробные числа";

                            var dummy = Convert.ToDecimal(value.ToString(), cultureInfo);
                            break;
                        }

                    case NumberType.NullableInt:
                        {
                            minValue = int.MinValue.ToString();
                            maxValue = int.MaxValue.ToString();

                            error = "Поле может принимать только целые числа";

                            var flag = int.TryParse((string)value!, out var i);
                            if ((flag && i >= 0)) return new ValidationResult(true, null);
                            return new ValidationResult(false, "Необходимо ввести корректное значение");
                        }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(NumberType), @"Неверный тип");
                }
            }
            catch (FormatException)
            {
                return new ValidationResult(false, $"Необходимо ввести корректное значение. {error}");
            }
            catch (OverflowException)
            {
                return new ValidationResult(false, $"Выход за пределы диапазона значений. Поле может принимать значения в диапазоне от {minValue} до {maxValue}");
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Необходимо ввести корректное значение");
            }

            return new ValidationResult(true, null);
        }
    }
}
