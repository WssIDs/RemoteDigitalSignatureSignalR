using Common.Mvvm.Commands;

namespace Common.Mvvm.Tab
{
    public interface ITab
    {
        /// <summary>
        /// Уникальное имя
        /// </summary>
        string Action { get; set; }
        string Name { get; set; }
        IRaisedCommand CloseCommand { get; }
        event EventHandler CloseRequested;
    }
}
