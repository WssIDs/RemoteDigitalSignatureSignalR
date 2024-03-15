using Common.Mvvm.Commands;
using RemoteDigitalSignature.Service.Models;
using RemoteDigitalSignature.ViewModels.Common;

namespace RemoteDigitalSignature.ViewModels;

public class AddEditCertificateViewModel : BaseViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public AddEditCertificateViewModel()
    {
        Title = "Добавить сертификат или СОС для загрузки";
        Certificate = new();

        SaveCertificateCommand = new RelayCommand(OnSaveCertificateCommandExecuted, CanSaveCertificateCommandExecute);
    }

    private bool _isEdit;

    /// <summary>
    /// 
    /// </summary>
    public bool IsEdit
    {
        get => _isEdit;
        set 
        {
            Set(ref _isEdit, value);

            if (_isEdit)
            {
                Title = "Редактировать сертификат или СОС для загрузки";
            }
        }
    }


    private CertModel _certificate = null!;

    public CertModel Certificate
    {
        get => _certificate;
        set => _certificate = value;
    }

    public IRaisedCommand SaveCertificateCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    private void OnSaveCertificateCommandExecuted(object param)
    {
        DialogResult = true;
    }

    private bool CanSaveCertificateCommandExecute(object param)
    {
        return !HasErrors && !Certificate.HasErrors;
    }
}
