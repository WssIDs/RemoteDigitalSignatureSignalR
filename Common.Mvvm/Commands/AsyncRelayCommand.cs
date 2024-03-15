namespace Common.Mvvm.Commands
{
    /// <summary>
    /// Универсальная команда для вызова
    /// </summary>
    public class AsyncRelayCommand : AsyncCommand
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Func<object, Task> _execute;

        /// <summary>
        /// 
        /// </summary>
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Конструктор команды
        /// </summary>
        /// <param name="execute">Вызов команды</param>
        /// <param name="canExecute">Проверка доступности команды</param>
        public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
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
        public override async Task RunAsync(object parameter) => await _execute.Invoke(parameter);
    }


    /// <summary>
    /// Универсальная команда для вызова
    /// </summary>
    public class AsyncRelayCommand<T> : AsyncCommand<T>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Func<T, Task> _execute;

        /// <summary>
        /// 
        /// </summary>
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// Конструктор команды
        /// </summary>
        /// <param name="execute">Вызов команды</param>
        /// <param name="canExecute">Проверка доступности команды</param>
        public AsyncRelayCommand(Func<T, Task> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;

        }

        /// <summary>
        /// Вызов команды
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanRun(T parameter) => _canExecute?.Invoke(parameter) ?? true;

        /// <summary>
        /// Проверка доступности команды
        /// </summary>
        /// <param name="parameter"></param>
        public override async Task RunAsync(T parameter) => await _execute.Invoke(parameter);
    }
}
