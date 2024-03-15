using Common.Mvvm.Commands;
using Common.Mvvm.Dialog;
using System.ComponentModel;
using System.Reflection;
using System.Diagnostics;
using Common.Exceptions;
using RemoteDigitalSignature.ViewModels.Common;
using RemoteDigitalSignature.Service.Abstractions;

namespace RemoteDigitalSignature.ViewModels;

/// <summary>
/// 
/// </summary>
public class MainViewModel : BaseViewModel
{
    private readonly ICertificateRevocationListStoreService _certificateRevocationListStoreService;
    private readonly IMainSignService _mainSigningService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mainSigningService"></param>
    public MainViewModel(IMainSignService mainSigningService, ICertificateRevocationListStoreService certificateRevocationListStoreService)
    {
        _mainSigningService = mainSigningService;
        _certificateRevocationListStoreService = certificateRevocationListStoreService;

        Title = "Главное окно";
        ShowInTaskbar = false;
        IsFirstRun = true;

        var mainAssembly = Assembly.GetEntryAssembly();

        if (mainAssembly != null)
        {
            _fileVersionInfo = FileVersionInfo.GetVersionInfo(mainAssembly.Location);
            TrayTitle = $"{_fileVersionInfo.Comments} {_fileVersionInfo.ProductVersion}";
            Title = $"{_fileVersionInfo.Comments} {_fileVersionInfo.ProductVersion}";
        }

        CheckAvestTitle = "Проверить Avest";

        CryptoLibraryPath = _certificateRevocationListStoreService.Store.CryptLibraryPath;

        CloseCommand = new RelayCommand(OnCloseCommandExecuted, CanCloseCommandExecute);
        ChangeStateCommand = new RelayCommand(OnChangeStateCommandExecuted);
        ShowCommand = new RelayCommand(OnShowCommandExecuted);

        CheckAvestCommand = new AsyncRelayCommand<bool>(OnCheckAvestCommandExecuted);
        OpenWorkingDirectoryCommand = new RelayCommand(OnOpenWorkingDirectoryCommandExecuted);
        RestartCommand = new RelayCommand(OnRestartCommandExecuted);
        ImportRevokedCerificatesCommand = new AsyncRelayCommand<bool>(OnImportRevokedCerificatesCommandExecuted);

        SettingsCommand = new AsyncRelayCommand(OnSettingsCommandExecuted, CanSettingsCommandExecuted);
        AboutCommand = new RelayCommand(OnAboutCommandExecuted, CanAboutCommandExecuted);

        CheckAvestCommand.Execute(false);
    }

    private readonly FileVersionInfo? _fileVersionInfo;

    private int _imageRowSpan = 1;

    /// <summary>
    /// 
    /// </summary>
    public int ImageRowSpan
    {
        get => _imageRowSpan;
        set => Set(ref _imageRowSpan, value);
    }

    private string _trayTitle = null!;

    /// <summary>
    /// 
    /// </summary>
    public string TrayTitle
    {
        get => _trayTitle;
        set => Set(ref _trayTitle, value);
    }

    private string _checkAvestTitle = null!;

    /// <summary>
    /// 
    /// </summary>
    public string CheckAvestTitle
    {
        get => _checkAvestTitle;
        set => Set(ref _checkAvestTitle, value);
    }

    private string _errorMessage = null!;
    
    /// <summary>
    /// 
    /// </summary>
    public string ErrorMessage
    {
        get => _errorMessage;
        set => Set(ref _errorMessage, value);
    }

    private string? _cryptoLibraryPath;
    
    /// <summary>
    /// 
    /// </summary>
    public string? CryptoLibraryPath
    {
        get => _cryptoLibraryPath;
        set => Set(ref _cryptoLibraryPath, value);
    }

    private bool _avestResult;

    public bool AvestResult
    {
        get => _avestResult;
        set
        {
            Set(ref _avestResult, value);
            ImageRowSpan = _avestResult ? 1 : 2;
        }
    }

    private bool _isFirstRun;

    /// <summary>
    /// 
    /// </summary>
    public bool IsFirstRun
    {
        get => _isFirstRun;
        set => Set(ref _isFirstRun, value);
    }

    private bool _showInTaskbar;

    /// <summary>
    /// 
    /// </summary>
    public bool ShowInTaskbar
    {
        get => _showInTaskbar;
        set => Set(ref _showInTaskbar, value);
    }

    #region ContextMenuCommands

    /// <summary>
    /// 
    /// </summary>
    public IRaisedCommand CheckAvestCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <exception cref="CommonException"></exception>
    private async Task OnCheckAvestCommandExecuted(bool param)
    {
        try
        {
            CryptoLibraryPath = _certificateRevocationListStoreService.Store.CryptLibraryPath;

            var res = await _mainSigningService.TryCheckAvestAsync();

            var isValid = res.IsValid;

            if (!res.IsValid)
            {
                if (res.Errors != null && res.Errors.Count != 0)
                {
                    throw new CommonException(string.Join("\n", res.Errors.Select(x => x.Message)));
                }
            }

            var crls = await _mainSigningService.GetRevocationListCerts();

            if (!crls.Any())
            {
                throw new CommonException("СОС не найдены в хранилище");
            }
            else
            {
                foreach (var crl in crls)
                {
                    if (!crl.IsValid)
                    {
                        if (crl.Error != null)
                        {
                            throw new CommonException(crl.Error.Message);
                        }
                    }
                    else
                    {
                        isValid = true;
                        ErrorMessage = string.Empty;
                    }
                }
            }

            AvestResult = isValid;
        }
        catch (Exception ex)
        {
            AvestResult = false;
            ErrorMessage = ex.Message;
        }

        if (param)
        {
            if (AvestResult)
            {
                DialogService.Success("Проверка прошла успешно", Title, "Проверка Avest");
            }
            else
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    DialogService.Error(ErrorMessage, Title,
                        "Проверка Avest");
                }
            }
        }
    }

    public IRaisedCommand OpenWorkingDirectoryCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <exception cref="CommonException"></exception>
    private void OnOpenWorkingDirectoryCommandExecuted(object param)
    {
        var dir = Directory.GetCurrentDirectory();

        Process.Start(new ProcessStartInfo()
        {
            FileName = dir,
            UseShellExecute = true,
            Verb = "open"
        });
    }


    public IRaisedCommand RestartCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    private void OnRestartCommandExecuted(object _)
    {
        DialogService.RestartApp();
    }

    public IRaisedCommand ImportRevokedCerificatesCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    private async Task OnImportRevokedCerificatesCommandExecuted(bool param)
    {
        await _certificateRevocationListStoreService.DownloadAsync();

        List<string>? errors = null;

        foreach (var cert in _certificateRevocationListStoreService.DownloadedCertificates)
        {
            var res = await _mainSigningService.ImportRevocationListCertificates(cert.Path);

            if (!res.IsValid)
            {
                if (errors == null)
                {
                    errors = new List<string>();
                }

                if (res.Errors != null)
                {
                    errors.AddRange(res.Errors.Select(r => r.Message));
                }
            }
            else
            {
                cert.IsImported = true;
            }
        }

        ///if (param)
        //{
        //if (errors == null)
        //{
            var importedCerts = _certificateRevocationListStoreService.DownloadedCertificates.Where(cert => cert.IsImported).Select(cert => cert.Name).ToList();

            if (importedCerts.Count > 0)
            {
                DialogService.Success($"Импорт сертификатов и списка отозванных сертификатов прошел успешно\nИмпортировано:\n{string.Join("\n", importedCerts)}", Title, "Импорт СОС и сертификатов");
            }
        //}
        //else
        //{
            if(errors != null && errors.Count > 0)
            {
                //if (errors != null)
                DialogService.Error(string.Join("\n", errors), Title,
                    "Импорт СОС и сертификатов");
            }
        //}
        //}

        await _certificateRevocationListStoreService.ClearCertFolderAsync();
        CheckAvestCommand.Execute(false);
    }

    #endregion

    #region DialogCommands

    public IRaisedCommand CloseCommand { get; }

    private void OnCloseCommandExecuted(object param)
    {
        DialogService.CloseApplicationForce();
    }

    private bool CanCloseCommandExecute(object param)
    {
        return true;
    }


    public IRaisedCommand ClosingCommand => new ActionCommand<CancelEventArgs>(
    args =>
    {
        args.Cancel = true;
        DialogService.Hide<MainViewModel>();
    });

    public IRaisedCommand ChangeStateCommand { get; }

    private void OnChangeStateCommandExecuted(object param)
    {
        var state = DialogService.GetDialogState<MainViewModel>();

        if (state == DialogState.Minimized)
        {
            ShowInTaskbar = false;
            DialogService.Hide<MainViewModel>();
        }
    }

    public IRaisedCommand ShowCommand { get; }

    private void OnShowCommandExecuted(object param)
    {
        ShowInTaskbar = true;
        DialogService.Show<MainViewModel>(DialogState.Normal);
        if (IsFirstRun)
        {
            DialogService.SetCenter<MainViewModel>();
            IsFirstRun = false;
        }
    }

    #endregion

    private bool _settingsIsOpened;

    public IRaisedCommand SettingsCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    private async Task OnSettingsCommandExecuted(object param)
    {
        var vm = DialogService.Create<SettingsViewModel>();
        if (vm != null)
        {
            await vm.InitStoreAsync();
        }

        _settingsIsOpened = true;
        DialogService.ShowViewModelDialog<SettingsViewModel>();
        _settingsIsOpened = false;

        await _certificateRevocationListStoreService.SaveAsync();
    }

    private bool CanSettingsCommandExecuted(object param)
    {
        return !_settingsIsOpened;
    }

    private bool _aboutIsOpened;

    public IRaisedCommand AboutCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    private void OnAboutCommandExecuted(object param)
    {
        var vm = DialogService.Create<AboutViewModel>();
        if (vm != null)
        {
            _aboutIsOpened = true;
            DialogService.ShowViewModelDialog<AboutViewModel>();
            _aboutIsOpened = false;
        }
    }

    private bool CanAboutCommandExecuted(object param)
    {
        return !_aboutIsOpened;
    }
}
