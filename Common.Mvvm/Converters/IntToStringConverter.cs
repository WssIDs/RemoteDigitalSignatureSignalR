using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Common.Mvvm.Converters
{
    public class IntToStringConverter : MarkupExtension, IValueConverter
    {
        public IntToStringConverter()
        {
            EqualContent = "Да";
            NotEqualContent = "Нет";
            NullContent = "Нет данных";
        }

        public int EqualValue { get; set; }
        public object EqualContent { get; set; }
        public object NotEqualContent { get; set; }
        public object NullContent { get; set; }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null)
                return NullContent;

            int? intValue;

            try
            {
                intValue = System.Convert.ToInt32(value);
            }
            catch
            {
                intValue = null;
            }

            if (intValue == null)
                return NullContent;

            return (intValue == EqualValue) ? EqualContent : NotEqualContent;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value.Equals(EqualContent);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
