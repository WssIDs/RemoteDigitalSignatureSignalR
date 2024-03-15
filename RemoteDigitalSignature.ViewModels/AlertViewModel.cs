using Common.Mvvm.Commands;
using RemoteDigitalSignature.ViewModels.Common;

namespace RemoteDigitalSignature.ViewModels;

/// <summary>
/// Список типов диалогового окна
/// </summary>
public enum AlertViewType
{
    /// <summary>
    /// Уведомление
    /// </summary>
    Notify = 1,

    /// <summary>
    /// Успех
    /// </summary>
    Success = 2,

    /// <summary>
    /// Информация
    /// </summary>
    Info = 3,

    /// <summary>
    /// Внимание
    /// </summary>
    Warning = 4,

    /// <summary>
    /// Ошибка
    /// </summary>
    Error = 5,

    /// <summary>
    /// Вопрос
    /// </summary>
    Question = 6,

    /// <summary>
    /// Вопрос c отменой
    /// </summary>
    QuestionCancel = 7,

    /// <summary>
    /// Исключение
    /// </summary>
    Exception = 8,

    /// <summary>
    /// Без иконки
    /// </summary>
    None = 0
}

/// <summary>
/// 
/// </summary>
public class AlertViewModel : BaseViewModel
{
    public AlertViewModel()
    {
        OkCommand = new RelayCommand(OnOkCommandExecuted);
        CancelCommand = new RelayCommand(OnCancelCommandExecuted);

        Title = "Default";
    }

    private AlertViewType _viewType;
    private string? _header;
    private string? _message;
    private bool _isCancel;

    /// <summary>
    /// Тип диалогового окна
    /// </summary>
    public AlertViewType ViewType
    {
        get => _viewType;
        set => Set(ref _viewType, value);
    }

    /// <summary>
    /// Заголовок сообщения
    /// </summary>
    public string? Header
    {
        get => _header;
        set => Set(ref _header, value);
    }

    /// <summary>
    /// Сообщение
    /// </summary>
    public string? Message
    {
        get => _message;
        set => Set(ref _message, value);
    }

    /// <summary>
    ///  
    /// </summary>
    public IRaisedCommand OkCommand { get; }

    private void OnOkCommandExecuted(object? param)
    {
        DialogResult = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IsCancel
    {
        get => _isCancel;
        set => Set(ref _isCancel, value);
    }

    /// <summary>
    ///  
    /// </summary>
    public IRaisedCommand CancelCommand { get; }

    private void OnCancelCommandExecuted(object? param)
    {
        IsCancel = true;
        DialogResult = false;
    }
}