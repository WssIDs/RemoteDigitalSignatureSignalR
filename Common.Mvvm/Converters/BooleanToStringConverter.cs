using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Common.Mvvm.Converters
{
    public class BooleanToStringConverter : MarkupExtension, IValueConverter
    {
        public BooleanToStringConverter()
        {
            TrueContent = "Да";
            FalseContent = "Нет";
            NullContent = "Нет данных";
        }

        public object TrueContent { get; set; }
        public object FalseContent { get; set; }
        public object NullContent { get; set; }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null)
                return NullContent;

            var boolValue = true;
            var isBool = true;

            try
            {
                boolValue = System.Convert.ToBoolean(value);
            }
            catch
            {
                isBool = false;
            }

            if (!isBool)
                return NullContent;

            return boolValue ? TrueContent : FalseContent;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value.Equals(TrueContent);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
