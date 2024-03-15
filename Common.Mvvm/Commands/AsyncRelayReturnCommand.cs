namespace Common.Mvvm.Commands
{
    /// <summary>
    /// Универсальная команда для вызова
    /// </summary>
    public class AsyncRelayReturnCommand<T> : AsyncReturnCommand<T>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Func<object, Task<T>> _execute;

        /// <summary>
        /// 
        /// </summary>
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Конструктор команды
        /// </summary>
        /// <param name="execute">Вызов команды</param>
        /// <param name="canExecute">Проверка доступности команды</param>
        public AsyncRelayReturnCommand(Func<object, Task<T>> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;

        }

        /// <summary>
        /// Вызов команды
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanRun(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        /// <summary>
        /// Проверка доступности команды
        /// </summary>
        /// <param name="parameter"></param>
        public override async Task<T> RunAsync(object parameter) => await _execute(parameter);
    }
}
