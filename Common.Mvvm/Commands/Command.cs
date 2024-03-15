using System.Windows.Input;
using Common.Mvvm.Helpers;

namespace Common.Mvvm.Commands
{
    /// <summary>
    /// Абстрактная команда
    /// </summary>
    public abstract class Command : IRaisedCommand
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

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}
