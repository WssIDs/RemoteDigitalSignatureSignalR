using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Common.Mvvm.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class IntToVisibilityConverter : IValueConverter
    {
        //private int? val;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;

            var val = System.Convert.ToInt32(value);
            //val = (int)value;

            return (val > 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InverseValueToVisibilityConverter : IValueConverter
    {
        //private int? val;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == null) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
