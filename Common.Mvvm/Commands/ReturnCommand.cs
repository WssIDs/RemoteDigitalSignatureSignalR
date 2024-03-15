using System.Windows.Input;
using Common.Mvvm.Helpers;

namespace Common.Mvvm.Commands
{
    /// <summary>
    /// Абстрактная команда
    /// </summary>
    public abstract class ReturnCommand<T> : IRaisedCommand
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

        public abstract T Execute(object parameter);

        public abstract bool CanExecute(object parameter);

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute(parameter);
        }
    }
}
