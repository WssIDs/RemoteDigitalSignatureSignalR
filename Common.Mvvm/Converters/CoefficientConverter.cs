using System.Globalization;
using System.Windows.Data;

namespace Common.Mvvm.Converters
{
    /// <summary> Перевод значений типа 99,95 в 0,9995 и обратно </summary>
    public class CoefficientConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            if (decimal.TryParse(value.ToString(), out var result))
            {
                return result * 100.0m;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            if (decimal.TryParse(value.ToString(), out var result))
            {
                return result / 100.0m;
            }

            return result;
        }
    }
}
