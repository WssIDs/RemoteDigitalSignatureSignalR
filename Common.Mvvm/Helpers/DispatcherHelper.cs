using System.Windows;
using System.Windows.Threading;

namespace Common.Mvvm.Helpers
{
    /// <summary>
    /// Класс помощник синхронизации UI потока
    /// </summary>
    public static class DispatcherHelper
    {
        /// <summary>
        /// Вернуться в UI поток
        /// </summary>
        /// <returns></returns>
        public static Dispatcher ReturnToUi() => Application.Current.Dispatcher;
    }
}