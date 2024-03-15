using Common.Mvvm.Commands;
using Common.Mvvm.Dialog;
using RemoteDigitalSignature.ViewModels.Common;
using System.Diagnostics;
using System.Reflection;

namespace RemoteDigitalSignature.ViewModels;
/// <summary>
/// 
/// </summary>
public class AboutViewModel : BaseViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public AboutViewModel()
    {
        Title = "О программе";

        var mainAssembly = Assembly.GetEntryAssembly();

        if (mainAssembly != null)
        {
            var fileVersion = FileVersionInfo.GetVersionInfo(mainAssembly.Location);
 
            if (fileVersion != null)
            {
                FileVersion = fileVersion.FileVersion;
                Version = fileVersion.ProductVersion;
                ProductName = fileVersion.ProductName;
                Description = fileVersion.Comments;
            }
        }

        LoadedCommand = new RelayCommand(OnLoadedCommandExecuted);
    }

    private string? _fileVersion;

    /// <summary>
    /// 
    /// </summary>
    public string? FileVersion
    {
        get => _fileVersion;
        set => Set(ref _fileVersion, value);
    }

    private string? _version;

    /// <summary>
    /// 
    /// </summary>
    public string? Version
    {
        get => _version;
        set => Set(ref _version, value);
    }

    private string? _productName;

    /// <summary>
    /// 
    /// </summary>
    public string? ProductName
    {
        get => _productName;
        set => Set(ref _productName, value);
    }

    private string? _description;

    /// <summary>
    /// 
    /// </summary>
    public string? Description
    {
        get => _description;
        set => Set(ref _description, value);
    }


    public IRaisedCommand LoadedCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    private void OnLoadedCommandExecuted(object param)
    {
        DialogService.ChangePosition<AboutViewModel>(WindowLocation.Right, WindowLocation.Bottom);
    }
}
