using System.Windows.Controls;

namespace Common.Mvvm.Dialog
{
    public interface IContainerRegistry
    {
        /// <summary>
        /// Ассоциировать представление с моделью представления
        /// </summary>
        /// <typeparam name="T">Представление</typeparam>
        /// <typeparam name="TVm">Модель представления</typeparam>
        void RegisterDialog<T, TVm>() where T : ContentControl where TVm : ViewModelBase;

        /// <summary>
        /// Ассоциировать представление с моделью представления
        /// </summary>
        void RegisterDialog(Type contentControlType, Type viewModelType);
        
        /*/// <summary>
        /// Ассоциировать представление с моделью представления
        /// </summary>
        /// <typeparam name="T">Представление</typeparam>
        /// <typeparam name="TVm">Модель представления</typeparam>
        void RegisterDialog<T,TVm>(T view, TVm viewModel) where T : ContentControl where TVm : ViewModelBase;*/

        /// <summary>
        /// Ассоциировать представление с моделью представления
        /// </summary>
        /// <typeparam name="T">Представление</typeparam>
        /// <typeparam name="TVm">Модель представления</typeparam>
        void RegisterDialog<T, TVm>(string name) where T : ContentControl where TVm : ViewModelBase;

        /// <summary>
        /// Удалить ассоциацию представления с моделью представления
        /// </summary>
        /// <typeparam name="TVm">Модель представления</typeparam>
        void UnRegisterDialog<TVm>() where TVm : ViewModelBase;

        /// <summary>
        /// Получить тип зарегистрированного представления
        /// </summary>
        /// <typeparam name="TVm">Модель представления</typeparam>
        /// <returns></returns>
        string GetDialogType<TVm>() where TVm : ViewModelBase;

        /// <summary>
        /// Получить объект зарегистрированного представления
        /// </summary>
        /// <typeparam name="TVm">Модель представления</typeparam>
        /// <returns></returns>
        ContentControl GetDialog<TVm>() where TVm : ViewModelBase;
    }
}