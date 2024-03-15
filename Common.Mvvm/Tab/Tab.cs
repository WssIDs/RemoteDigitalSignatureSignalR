using Common.Mvvm.Commands;

namespace Common.Mvvm.Tab
{
    public abstract class Tab : ITab
    {
        protected Tab()
        {
            CloseCommand = new RelayCommand(p => CloseRequested?.Invoke(this, EventArgs.Empty));
        }

        /// <summary>
        /// Уникальное имя
        /// </summary>
        public string Action { get; set; } = null!;

        public string Name { get; set; } = null!;
        public IRaisedCommand CloseCommand { get; }
        public event EventHandler CloseRequested;
    }
}
