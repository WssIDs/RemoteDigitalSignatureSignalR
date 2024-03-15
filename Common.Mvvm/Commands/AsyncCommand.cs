using System.Windows.Input;
using Common.Mvvm.Helpers;

namespace Common.Mvvm.Commands
{
    /// <summary>
    /// Абстрактная команда
    /// </summary>
    public abstract class AsyncCommand : IRaisedCommand
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

        public abstract Task RunAsync(object parameter);

        public abstract bool CanRun(object parameter);

        public async void Execute(object parameter)
        {
            await RunAsync(parameter);
        }

        public bool CanExecute(object parameter) => CanRun(parameter);
    }

    /// <summary>
    /// Абстрактная команда
    /// </summary>
    public abstract class AsyncCommand<T> : IRaisedCommand
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

        public abstract Task RunAsync(T parameter);

        public abstract bool CanRun(T parameter);

        public async void Execute(object parameter)
        {
            await RunAsync((T)Convert.ChangeType(parameter, typeof(T)));
        }

        public bool CanExecute(object parameter) => CanRun((T)Convert.ChangeType(parameter, typeof(T)));
    }
}
