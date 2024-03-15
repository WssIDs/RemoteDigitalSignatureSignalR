using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace Common.Mvvm.Helpers
{
    public readonly struct DispatcherAwaiter : INotifyCompletion
    {
        private readonly Dispatcher _dispatcher;

        public DispatcherAwaiter(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public bool IsCompleted => _dispatcher.CheckAccess();

        public async void OnCompleted(Action continuation)
        {
            await _dispatcher.InvokeAsync(continuation);
        }

        public void GetResult()
        {
        }
    }
}
