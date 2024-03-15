namespace Common.Mvvm.Dialog
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDirectoryDialogService
    {
        /// <summary>
        /// 
        /// </summary>
        string InitialDirectory { get; set; }

        /// <summary>
        /// Путь к выбранному файлу
        /// </summary>
        string SelectedDirectory { get; set; }

        /// <summary>
        /// Фильтр файлов
        /// </summary>
        string Filter { get; set; }

        /// <summary>
        /// Открытие файла
        /// </summary>
        /// <returns></returns>
        bool Open(string title);
    }
}