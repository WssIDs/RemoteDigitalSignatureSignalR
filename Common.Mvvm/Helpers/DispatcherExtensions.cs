using System.Windows.Threading;

namespace Common.Mvvm.Helpers
{
    public static class DispatcherExtensions
    {
        public static DispatcherAwaiter GetAwaiter(this Dispatcher dispatcher) => new(dispatcher);
    }
}