using Common.Mvvm.Commands;
using Common.Mvvm.Dialog;
using RemoteDigitalSignature.Service.Abstractions;
using RemoteDigitalSignature.Service.Models;
using RemoteDigitalSignature.ViewModels.Common;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace RemoteDigitalSignature.ViewModels;

/// <summary>
/// 
/// </summary>
public class SettingsViewModel : BaseViewModel
{
    private readonly ICertificateRevocationListStoreService _certificateRevocationListStoreService;
    
    private readonly IDirectoryDialogService _directoryDialogService;

    //private readonly IMainSignService _mainSignService;

    public SettingsViewModel(ICertificateRevocationListStoreService certificateRevocationListStoreService, IDirectoryDialogService directoryDialogService)
    {
        _certificateRevocationListStoreService = certificateRevocationListStoreService;
        _directoryDialogService = directoryDialogService;
        //_mainSignService = mainSignService;

        Title = "Настройки";

        EditCertificateCommand = new RelayCommand(OnEditCertificateCommandExecuted, CanEditCertificateCommandExecute);
        AddCertificateCommand = new RelayCommand(OnAddCertificateCommandExecuted);
        RemoveCertificateCommand = new RelayCommand(OnRemoveCertificateCommandExecuted, CanRemoveCertificateCommandExecute);
        DefaultCertificateCommand = new AsyncRelayCommand(OnDefaultCertificateCommandExecuted);

        SelectCryptoDirectoryCommand = new AsyncRelayCommand(OnSelectCryptoDirectoryCommandExecuted);
        ClearCryptoDirectoryCommand = new AsyncRelayCommand(OnClearCryptoDirectoryCommandExecuted);

        LoadedCommand = new RelayCommand(OnLoadedCommandExecuted);
        //_mainSignService = mainSignService;
    }


    private bool _runAtStartupWindows;

    /// <summary>
    /// 
    /// </summary>
    public bool RunAtStartupWindows
    {
        get
        {
            var mainAssembly = Assembly.GetEntryAssembly();

            if (mainAssembly != null)
            {
                var fileVersion = FileVersionInfo.GetVersionInfo(mainAssembly.Location);

                var startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                var filename = Path.Combine(startupFolder, fileVersion.ProductName! + ".lnk");
                _runAtStartupWindows = File.Exists(filename);
            }

            return _runAtStartupWindows;
        }
        set
        {
            var mainAssembly = Assembly.GetEntryAssembly();

            if (mainAssembly != null)
            {
                var fileVersion = FileVersionInfo.GetVersionInfo(mainAssembly.Location);

                var startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

                var filename = Path.Combine(startupFolder, fileVersion.ProductName! + ".lnk");

                //if (File.Exists(filename))
                //{
                if (!value)
                {
                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }
                }
                else
                {
                    if (!File.Exists(filename))
                    {
                        string pathToExe = mainAssembly.Location;

                        IWshRuntimeLibrary.WshShell shell = new();
                        IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(filename);

                        shortcut.Description = fileVersion.Comments;
                        shortcut.TargetPath = Path.ChangeExtension(mainAssembly.Location, ".exe");
                        shortcut.Save();
                    }
                }
            }

            Set(ref _runAtStartupWindows, value);
        }
    }


    private CertificateRevocationListStore _store = null!;

    public CertificateRevocationListStore Store
    {
        get => _store;
        set => Set(ref _store, value);
    }

    private CertModel? _selectedCertificate;

    public CertModel? SelectedCertificate
    {
        get => _selectedCertificate;
        set => Set(ref _selectedCertificate, value);
    }


    public async Task InitStoreAsync()
    {
        await _certificateRevocationListStoreService.InitAsync();
        Store = _certificateRevocationListStoreService.Store;
    }

    public IRaisedCommand EditCertificateCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    private void OnEditCertificateCommandExecuted(object param)
    {
        var vm = DialogService.Create<AddEditCertificateViewModel>();
        if (vm != null)
        {
            if (SelectedCertificate != null)
            {
                var oldCert = new CertModel
                {
                    Name = SelectedCertificate.Name,
                    Path = SelectedCertificate.Path,
                };

                vm.Certificate = SelectedCertificate;
                vm.IsEdit = true;

                if (DialogService.ShowViewModelDialog<AddEditCertificateViewModel>() == true)
                {

                }
                else
                {
                    SelectedCertificate.Name = oldCert.Name;
                    SelectedCertificate.Path = oldCert.Path;
                }
            }
        }


    }

    private bool CanEditCertificateCommandExecute(object param)
    {
        return SelectedCertificate != null;
    }

    public IRaisedCommand AddCertificateCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    private void OnAddCertificateCommandExecuted(object param)
    {
        var vm = DialogService.Create<AddEditCertificateViewModel>();
        if (vm != null)
        {
            // await vm.InitStoreAsync();

            if (DialogService.ShowViewModelDialog<AddEditCertificateViewModel>() == true)
            {
                Store.Certificates.Add(vm.Certificate);
                SelectedCertificate = Store.Certificates.FirstOrDefault(c => c.Name == vm.Certificate.Name && c.Path == vm.Certificate.Path);
            }
        }
    }

    public IRaisedCommand RemoveCertificateCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    private void OnRemoveCertificateCommandExecuted(object param)
    {

        if (SelectedCertificate != null)
        {
            if (DialogService.Question("Действительно хотите удалить?", "Удаление сертификата") == true)
            {
                Store.Certificates.Remove(SelectedCertificate);
            }
        }
    }

    private bool CanRemoveCertificateCommandExecute(object param)
    {
        return SelectedCertificate != null;
    }


    public IRaisedCommand DefaultCertificateCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    private async Task OnDefaultCertificateCommandExecuted(object param)
    {
        if (DialogService.Question("Сбросить настройки по умолчанию?", "Сброс настроек") == true)
        {
            var store = new CertificateRevocationListStore();

            if (store != null)
            {
                _certificateRevocationListStoreService.Store = store;
                _certificateRevocationListStoreService.InitCryptoLibrary();
                await _certificateRevocationListStoreService.SaveAsync();
                Store = _certificateRevocationListStoreService.Store;
            }
        }
    }

    public IRaisedCommand SelectCryptoDirectoryCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    private async Task OnSelectCryptoDirectoryCommandExecuted(object _)
    {
        var dirPath = _certificateRevocationListStoreService.Store.CryptLibraryPath;

        var DefaultProgramFiles = Environment.Is64BitOperatingSystem ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        var filename = "AvCryptMail.dll";

        var avestPath = Path.Combine(DefaultProgramFiles, "Avest");

        if (avestPath == null)
        {
            if (Directory.Exists(avestPath))
            {
                dirPath = avestPath;
            }
        }

        if (dirPath == null)
        {
            if(!Directory.Exists(dirPath))
            {
                dirPath = null;
            }
        }

        _directoryDialogService.InitialDirectory = dirPath;

        if (_directoryDialogService.Open("Выбрать директорию криптопровайдера"))
        {
            if(_directoryDialogService.SelectedDirectory != null)
            {
                if(Directory.Exists(_directoryDialogService.SelectedDirectory))
                {
                    if (File.Exists(Path.Combine(_directoryDialogService.SelectedDirectory, filename)))
                    {
                        var lastPath = _certificateRevocationListStoreService.Store.CryptLibraryPath;
                        _certificateRevocationListStoreService.Store.CryptLibraryPath = _directoryDialogService.SelectedDirectory;
                        await _certificateRevocationListStoreService.SaveAsync();
                        //_mainSignService.ReInitLibrary();

                        if (lastPath != _certificateRevocationListStoreService.Store.CryptLibraryPath)
                        {
                            DialogService.Info($"Для корректной загрузки библиотеки {_certificateRevocationListStoreService.Store.CryptLibraryPath}\\{filename} модуль будет перезапущен.", "Загрузка библиотеки");
                            DialogService.RestartApp();
                        }
                        else
                        {
                            DialogService.Success($"Библиотека {_certificateRevocationListStoreService.Store.CryptLibraryPath}\\{filename} успешно загружена", "Загрузка библиотеки");
                        }
                    }
                    else
                    {
                        DialogService.Warning($"Файл {filename} не найден в директории {_directoryDialogService.SelectedDirectory}", "Выбор директории криптопровайдера");
                    }
                }
            }
        }
    }

    public IRaisedCommand ClearCryptoDirectoryCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    private async Task OnClearCryptoDirectoryCommandExecuted(object _)
    {
        if (DialogService.Question("Очистить путь к криптопровайдеру?\nПуть будет указан по умолчанию для библиотеки AvCryptMail.dll", "Директория криптопровайдера") == true)
        {
            _certificateRevocationListStoreService.Store.CryptLibraryPath = null;
            await _certificateRevocationListStoreService.SaveAsync();
        }
    }

    public IRaisedCommand LoadedCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    private void OnLoadedCommandExecuted(object param)
    {
        DialogService.ChangePosition<SettingsViewModel>(WindowLocation.Right, WindowLocation.Bottom);
    }
}