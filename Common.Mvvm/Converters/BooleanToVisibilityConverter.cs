using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Common.Mvvm.Converters
{
    /// <summary>
    /// Конвертер значения типа bool в Visibility (false = Visibility.Collapsed, true = Visibility.Visible)
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Visible;
            var boolValue = (bool)value;
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
