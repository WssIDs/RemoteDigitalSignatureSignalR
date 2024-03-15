using System.Diagnostics;

namespace Common.Logging
{
    /// <summary>
    /// Класс логирования
    /// </summary>
    public static class Logger
    {
        private enum Verbosity
        {
            Log,
            Warning,
            Error,
            Critical
        }

        private static string LogFileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private static bool IsWriteToFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private static bool IsWriteToConsole { get; set; }

        /// <summary>
        /// Инициализация логгера
        /// </summary>
        /// <param name="fileName">имя файла логирования</param>
        /// <param name="writeToConsole"></param>
        /// <param name="deleteAfterCount">Количество файлов логирования до удаления крайнего</param>
        /// <param name="writeToFile"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Init(string fileName, bool writeToFile = true, bool writeToConsole = true, int deleteAfterCount = 5)
        {
            IsWriteToFile = writeToFile;
            IsWriteToConsole = writeToConsole; 

            LogFileName = fileName;

            if (string.IsNullOrEmpty(LogFileName)) throw new ArgumentNullException(nameof(LogFileName));

            var path = Path.GetDirectoryName(fileName);

            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path))
            {
                HelpLink = null,
                HResult = 0,
                Source = null
            };

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                var di = new DirectoryInfo(path);

                var ext = Path.GetExtension(LogFileName);

                var files = di.GetFiles(ext).OrderByDescending(file => file.CreationTime).ToList();
                var oldFiles = files.Skip(deleteAfterCount).ToList();

                oldFiles.ForEach(file => file.Delete());
            }

            if (!File.Exists(LogFileName))
            {
                using var stream = File.CreateText(LogFileName);
            }
        }

        /// <summary>
        /// Запись сообщения в лог
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            InternalLog(Verbosity.Log, message);
        }

        /// <summary>
        /// Запись сообщения в лог
        /// </summary>
        /// <param name="category"></param>
        /// <param name="message"></param>
        public static void Log(string category, string message)
        {
            InternalLog(category, message);
        }

        /// <summary>
        /// Запись сообщения с ошибкой в лог
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(string message)
        {
            InternalLog(Verbosity.Error, message);
        }

        /// <summary>
        /// Запись сообщения с критической ошибкой в лог
        /// </summary>
        /// <param name="message"></param>
        public static void LogCriticalError(string message)
        {
            InternalLog(Verbosity.Critical, message);
        }

        /// <summary>
        /// Запись сообщения с предупреждением в лог
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(string message)
        {
            InternalLog(Verbosity.Warning, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="verbosity"></param>
        /// <param name="message"></param>
        private static void InternalLog<T>(T verbosity, string message)
        {
            var inMessage = $"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}][{verbosity}]: {message}";

            if (IsWriteToFile)
            {
                if (!File.Exists(LogFileName)) return;
                using var stream = File.AppendText(LogFileName);
                stream.WriteLine(inMessage);
            }

            if (IsWriteToConsole)
            {
                Debug.WriteLine(inMessage);
            }
        }
    }
}