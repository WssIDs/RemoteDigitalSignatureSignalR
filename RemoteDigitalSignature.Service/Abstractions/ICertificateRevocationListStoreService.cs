using RemoteDigitalSignature.Service.Models;

namespace RemoteDigitalSignature.Service.Abstractions;

public class DownaloadedCert
{
    /// <summary>
    /// 
    /// </summary>
    public string Path { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public bool IsImported { get; set; }
}

public interface ICertificateRevocationListStoreService
{
    /// <summary>
    /// 
    /// </summary>
    CertificateRevocationListStore Store { get; set; }

    /// <summary>
    /// 
    /// </summary>
    List<DownaloadedCert> DownloadedCertificates { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task InitAsync();

    /// <summary>
    /// 
    /// </summary>
    void InitCryptoLibrary();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task DownloadAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task ClearCertFolderAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}
