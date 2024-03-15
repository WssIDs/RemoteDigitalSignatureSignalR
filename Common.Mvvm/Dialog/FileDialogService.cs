using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace Common.Mvvm.Dialog
{
    /// <summary>
    /// 
    /// </summary>
    public class FileDialogService : IFileDialogService
    {
        /// <summary>
        /// 
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        ///             
        /// </summary>
        public string InitialDirectory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool OpenFileDialog(string title)
        {
            var owner = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.IsActive || w.IsFocused);

            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory =
                    (!string.IsNullOrEmpty(FilePath) ? Path.GetDirectoryName(FilePath) : InitialDirectory) ??
                    string.Empty,
                FileName = (!string.IsNullOrEmpty(FilePath) ? Path.GetFileName(FilePath) : string.Empty)!,
                Title = title,
                Filter = Filter
            };

            var result = owner != null ? openFileDialog.ShowDialog(owner) : openFileDialog.ShowDialog();

            FilePath = openFileDialog.FileName;
            return result == true;
        }

        public bool SaveFileDialog(string title)
        {
            var owner = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.IsActive || w.IsFocused);

            var saveFileDialog = new SaveFileDialog
            {
                InitialDirectory =
                    (!string.IsNullOrEmpty(FilePath) ? InitialDirectory : Path.GetDirectoryName(FilePath)) ??
                    string.Empty,
                FileName = FilePath,
                Title = title,
                Filter = Filter
            };

            var result = owner != null ? saveFileDialog.ShowDialog(owner) : saveFileDialog.ShowDialog();

            FilePath = saveFileDialog.FileName;
            return result == true;
        }
    }
}