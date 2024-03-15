namespace Common.Mvvm.Dialog
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFileDialogService
    {
        /// <summary>
        /// 
        /// </summary>
        string InitialDirectory { get; set; }

        /// <summary>
        /// Путь к выбранному файлу
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        /// Фильтр файлов
        /// </summary>
        string Filter { get; set; }

        /// <summary>
        /// Открытие файла
        /// </summary>
        /// <returns></returns>
        bool OpenFileDialog(string title);

        /// <summary>
        /// Сохранение файла
        /// </summary>
        /// <returns></returns>
        bool SaveFileDialog(string title);
    }
}