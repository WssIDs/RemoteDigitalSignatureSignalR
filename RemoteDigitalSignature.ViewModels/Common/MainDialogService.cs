using Common.Mvvm.Dialog;

namespace RemoteDigitalSignature.ViewModels.Common;

/// <summary>
/// Сервис для работы с диалоговыми окнами
/// </summary>
public class MainDialogService : DialogService
{
    public MainDialogService(IContainerRegistry containerRegistry) : base(containerRegistry) {}

    #region Messages

    public override bool? QuestionCancel(string message, string? title = null, string? header = null)
    {
        var viewModel = Create<AlertViewModel>();
        viewModel.ViewType = AlertViewType.QuestionCancel;
        viewModel.Message = message;
        viewModel.Title = title;

        viewModel.Header = !string.IsNullOrEmpty(header) ? header : title;

        var result = ShowViewModelDialog<AlertViewModel>();

        return viewModel.IsCancel && result == false ? null : result;
    }

    public override void Error(string message, string? title = null, string? header = null)
    {
        var viewModel = Create<AlertViewModel>();
        viewModel.ViewType = AlertViewType.Error;
        viewModel.Title = title;
        viewModel.Message = message;

        viewModel.Header = !string.IsNullOrEmpty(header) ? header : title;

        ShowViewModelDialog<AlertViewModel>();
    }

    public override void Exception(string message, string? title = null, string? header = null)
    {
        var viewModel = Create<AlertViewModel>();
        viewModel.ViewType = AlertViewType.Exception;
        viewModel.Title = title;
        viewModel.Message = message;

        viewModel.Header = !string.IsNullOrEmpty(header) ? header : title;

        ShowViewModelDialog<AlertViewModel>();
    }

    public override void Info(string message, string? title = null, string? header = null)
    {
        var viewModel = Create<AlertViewModel>();
        viewModel.ViewType = AlertViewType.Info;
        viewModel.Message = message;
        viewModel.Title = title;

        viewModel.Header = !string.IsNullOrEmpty(header) ? header : title;

        ShowViewModelDialog<AlertViewModel>();
    }

    public override void Notify(string message, string? title = null, string? header = null)
    {
        var viewModel = Create<AlertViewModel>();
        viewModel.ViewType = AlertViewType.Notify;
        viewModel.Message = message;
        viewModel.Title = title;

        viewModel.Header = !string.IsNullOrEmpty(header) ? header : title;

        ShowViewModelDialog<AlertViewModel>();
    }

    public override bool Question(string message, string? title = null, string? header = null)
    {
        var viewModel = Create<AlertViewModel>();
        viewModel.ViewType = AlertViewType.Question;
        viewModel.Message = message;
        viewModel.Title = title;

        viewModel.Header = !string.IsNullOrEmpty(header) ? header : title;

        return ShowViewModelDialog<AlertViewModel>() == true;
    }

    public override void Success(string message, string? title = null, string? header = null)
    {
        var viewModel = Create<AlertViewModel>();
        viewModel.ViewType = AlertViewType.Success;
        viewModel.Message = message;
        viewModel.Title = title;

        viewModel.Header = !string.IsNullOrEmpty(header) ? header : title;

        ShowViewModelDialog<AlertViewModel>();
    }

    public override void Warning(string message, string? title = null, string? header = null)
    {
        var viewModel = Create<AlertViewModel>();
        viewModel.ViewType = AlertViewType.Warning;
        viewModel.Message = message;
        viewModel.Title = title;

        viewModel.Header = !string.IsNullOrEmpty(header) ? header : title;

        ShowViewModelDialog<AlertViewModel>();
    }

    #endregion
}