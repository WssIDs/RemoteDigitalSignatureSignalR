using RemoteDigitalSignature.Service.Models;
namespace RemoteDigitalSignature.Service.Abstractions;

public interface IMainSignService
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<CertificateResultWebModel>> GetCerts(bool showOnlyValid = true);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<RevocationListCertificateResultWebModel>> GetRevocationListCerts();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="publicKeyId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    Task<CertificateResultWebModel?> GetCertAsync(string publicKeyId);

    /// <summary>
    /// 
    /// </summary>
    Task<CheckResultWebModel> ImportCrl();

    /// <summary>
    /// 
    /// </summary>
    Task<CheckResultWebModel> ImportRevocationListCertificates(string filename);


    /// <summary>
    /// 
    /// </summary>
    Task<CheckResultWebModel> DownloadImportRevocationListCertificates();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<CheckResultWebModel> TryCheckAvestAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    Task<HashFileResultWebModel> GetFileHashAsync(SignFileRequestWebModel sign);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    Task<HashResultWebModel> GetDataHashAsync(SignDataByteRequestWebModel sign);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    Task<SignFileResultWebModel> SignFileAsync(SignFileRequestWebModel sign);


    /// <summary>
    ///  Подписание файла в формате XaDes
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    Task<XmlSignToXaDesWebModel> SignXmlFileInXmlDsigAsync(SignFileRequestWebModel sign);

    /// <summary>
    /// Проверка файла XML на предмет валидности подписи XmlDsig
    /// </summary>
    /// <param name="verifyData"></param>
    /// <returns></returns>
    Task<XmlSignToXaDesWebModel> VerifySignXmlFileInXmlDsig(SignFileRequestWebModel verifyData);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    Task<SignResultWebModel> SignDataAsync(SignDataByteRequestWebModel sign);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    Task<SignResultWebModel> SignDataAsync(SignDataStringRequestWebModel sign);

    bool ReInitLibrary();

    void SetState(bool isBusy);

    bool GetState();

    void Shutdown();
}