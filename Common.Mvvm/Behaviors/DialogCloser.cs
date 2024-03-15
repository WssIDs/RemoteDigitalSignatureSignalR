using System.Windows;

namespace Common.Mvvm.Behaviors
{
    /// <summary>
    /// Класс поведения для добавления свойства зависимости для установки результата работы диалогового окна
    /// </summary>
    public static class DialogCloser
    {
        /// <summary>
        /// Свойство зависимости результата работы окна
        /// </summary>
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(DialogCloser),
                new PropertyMetadata(DialogResultChanged));

        /// <summary>
        /// Событие изменения свойства зависимости результата работы окна
        /// </summary>
        private static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is not Window window) return;
            window.DialogResult = e.NewValue as bool?;
            window.Close();

        }

        /// <summary>
        /// Установка значения результата работы диалогового окна
        /// </summary>
        /// <param name="target">Класс диалогового окна</param>
        /// <param name="value">Значение результата работы</param>
        public static void SetDialogResult(Window target, bool? value)
        {
            target?.SetValue(DialogResultProperty, value);
        }
    }
}