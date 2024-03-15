using System.Drawing;

namespace Common.Mvvm.Dialog
{
    public enum DialogState : uint
    {
        Normal,
        Maximized,
        Minimized
    }

    public enum WindowLocation : uint
    {
        Left, Top, Right, Bottom
    }

    /// <summary>
    /// 
    /// </summary>
    public struct WindowSize
    {
        /// <summary>
        /// 
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Height { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TV">Базовый тип модели представления (<see cref="ViewModelBase"/>)</typeparam>
        /// <param name="title">Заголовок</param>
        /// <returns></returns>
        TV Create<TV>(string title = null) where TV : ViewModelBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TV">Базовый тип модели представления (<see cref="ViewModelBase"/>)</typeparam>
        /// <param name="percentWidth"></param>
        /// <param name="percentHeight"></param>
        /// <returns></returns>
        TV Create<TV>(double percentHeight, double percentWidth) where TV : ViewModelBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TV">Базовый тип модели представления (<see cref="ViewModelBase"/>)</typeparam>
        /// <param name="percentWidth"></param>
        /// <param name="title">Заголовок</param>
        /// <param name="percentHeight"></param>
        /// <returns></returns>
        TV Create<TV>(string title, double percentHeight, double percentWidth) where TV : ViewModelBase;

        /// <summary>
        /// Показать модальный диалог
        /// </summary>
        /// <typeparam name="TV">Базовый тип модели представления (<see cref="ViewModelBase"/>)</typeparam>
        /// <returns></returns>
        bool? ShowViewModelDialog<TV>() where TV : ViewModelBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TV">Базовый тип модели представления (<see cref="ViewModelBase"/>)</typeparam>
        /// <param name="bOwner"></param>
        void ShowViewModel<TV>(bool bOwner = true) where TV : ViewModelBase;

        /// <summary>
        /// Закрытие представления и модели представления
        /// </summary>
        /// <typeparam name="TV">Базовый тип модели представления (<see cref="ViewModelBase"/>)</typeparam>
        void CloseViewModel<TV>() where TV : ViewModelBase;

        /// <summary>
        /// Закрытие представления и модели представления
        /// </summary>
        void CloseApplicationForce();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TV"></typeparam>
        void SetMainViewModel<TV>() where TV : ViewModelBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TV"></typeparam>
        void ChangeSize<TV>(double percentHeight, double percentWidth, bool changePosition = true) where TV : ViewModelBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TV"></typeparam>
        void SetCenter<TV>() where TV : ViewModelBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TV"></typeparam>
        void ChangePosition<TV>(WindowLocation xPostion, WindowLocation yPostion) where TV : ViewModelBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TV"></typeparam>
        WindowSize GetSize<TV>() where TV : ViewModelBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TV"></typeparam>
        DialogState GetDialogState<TV>() where TV : ViewModelBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TV"></typeparam>
        void Hide<TV>() where TV : ViewModelBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TV"></typeparam>
        void Show<TV>(DialogState state) where TV : ViewModelBase;

        bool OpenFileDialog(ref string filename);

        /// <summary>
        /// 
        /// </summary>
        void RestartApp();

        #region Messages

        /// <summary>
        /// Диалог с критической ошибкой
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="title">Заголовок окна</param>
        /// <param name="header">Заголовок сообщения</param>
        void Exception(string message, string title = null, string header = null);

        /// <summary>
        /// Диалог сообщение (уведомление)
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="title">Заголовок окна</param>
        /// <param name="header">Заголовок сообщения</param>
        void Notify(string message, string title = null, string header = null);

        /// <summary>
        /// Диалог с информацией
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="title">Заголовок окна</param>
        /// <param name="header">Заголовок сообщения</param>
        void Warning(string message, string title = null, string header = null);

        /// <summary>
        /// Диалог с информацией
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="title">Заголовок окна</param>
        /// <param name="header">Заголовок сообщения</param>
        void Info(string message, string title = null, string header = null);

        /// <summary>
        /// Диалог с вопросом
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="title">Заголовок окна</param>
        /// <param name="header">Заголовок сообщения</param>
        /// <returns>
        /// <b>true</b> - Да<br/>
        /// <b>false</b> - Нет<br/>
        /// </returns>
        bool Question(string message, string title = null, string header = null);

        /// <summary>
        /// Диалог с вопросом (с возможностью отмены)
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="title">Заголовок окна</param>
        /// <param name="header">Заголовок сообщения</param>
        /// <returns>
        /// <b>true</b> - Да<br/>
        /// <b>false</b> - Нет<br/>
        /// <b>null</b> - Отмена<br/>
        /// </returns>
        bool? QuestionCancel(string message, string title = null, string header = null);

        /// <summary>
        /// Диалог с ошибкой
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="title">Заголовок окна</param>
        /// <param name="header">Заголовок сообщения</param>
        void Error(string message, string title = null, string header = null);

        /// <summary>
        /// Диалог с успешным выполнением
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="title">Заголовок окна</param>
        /// <param name="header">Заголовок сообщения</param>
        void Success(string message, string title = null, string header = null);

        #endregion
    }
}