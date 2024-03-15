using System.Windows.Input;
using Common.Mvvm.Helpers;

namespace Common.Mvvm.Commands
{
    /// <summary>
    /// Абстрактная команда
    /// </summary>
    public abstract class AsyncReturnCommand<T> : IRaisedCommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public async void RaiseCanExecuteChanged()
        {
            await DispatcherHelper.ReturnToUi();
            CommandManager.InvalidateRequerySuggested();
        }

        public abstract Task<T> RunAsync(object parameter);

        public abstract bool CanRun(object parameter);

        public async void Execute(object parameter)
        {
            await RunAsync(parameter);
        }

        public bool CanExecute(object parameter) => CanRun(parameter);
    }
}