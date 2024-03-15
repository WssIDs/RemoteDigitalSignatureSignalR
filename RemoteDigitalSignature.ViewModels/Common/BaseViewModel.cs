using Common.Mvvm;

namespace RemoteDigitalSignature.ViewModels.Common;

/// <summary>
/// 
/// </summary>
public abstract class BaseViewModel : ViewModelBase
{
    private string _error = string.Empty;

    private bool _isBusy;

    private bool? _dialogResult;

    /// <summary>
    /// Конструктор
    /// </summary>
    protected BaseViewModel()
    {
        Title = "Default";
    }

    /// <summary>
    /// 
    /// </summary>
    public bool? DialogResult
    {
        get => _dialogResult;
        set
        {
            _dialogResult = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Индикатор длительной операции
    /// </summary>
    public bool IsBusy
    {
        get => _isBusy;
        set => Set(ref _isBusy, value);
    }

    /// <summary>
    /// Ошибка
    /// </summary>
    public string Error
    {
        get => _error;
        set => Set(ref _error, value);
    }

    private string _titleDescription = null!;

    /// <summary>
    /// Подробное описание заголовка
    /// </summary>
    public string TitleDescription
    {
        get => _titleDescription;
        set => Set(ref _titleDescription, value);
    }
}
