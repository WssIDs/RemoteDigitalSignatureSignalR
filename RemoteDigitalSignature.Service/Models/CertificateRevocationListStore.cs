using Common;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace RemoteDigitalSignature.Service.Models;

/// <summary>
/// 
/// </summary>
public class CertModel : DataAnnotationObservableObject
{
    private string _path = null!;
    private string _name = null!;

    /// <summary>
    /// 
    /// </summary>
    [Required(ErrorMessage = "Путь к загрузке сертификата или СОС обязателен")]
    public string Path 
    {
        get => _path;
        set => Set(ref _path, value);
    }
    /// <summary>
    /// 
    /// </summary>
    [Required(ErrorMessage = "Имя сертификата или СОС обязательно")]
    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }
}

/// <summary>
/// 
/// </summary>
public class CertificateRevocationListStore : DataAnnotationObservableObject
{
    private ObservableCollection<CertModel> _certificates = null!;
    private string? _cryptLibraryPath;
    private bool _isLocal;
    private bool _useSwagger;

    public CertificateRevocationListStore()
    {
        Certificates = new ObservableCollection<CertModel>
        {
            // Cert List
            //new CertModel
            //{
            //    Path = "https://nces.by/wp-content/uploads/certificates/pki/kuc.cer",
            //    Name = "COK Корневой удостоверяющий центр ГосСУОК"
            //},
            //new CertModel
            //{
            //    Path = "https://nces.by/wp-content/uploads/certificates/pki/ruc.cer",
            //    Name = "COK Республиканский удостоверяющий центр ГосСУОК"
            //},
            //new CertModel
            //{
            //    Path = "https://nces.by/wp-content/uploads/certificates/pki/ruc_old.cer",
            //    Name = "COK Республиканский удостоверяющий центр ГосСУОК (старый)"
            //},
            //new CertModel
            //{
            //    Path = "https://nces.by/wp-content/uploads/certificates/atrib-cert-ul.cer",
            //    Name = "Сертификат службы атрибутных сертификатов юридических лиц"
            //},

            new CertModel 
            {
                Path = "https://nces.by/wp-content/uploads/certificates/pki/kuc.cer",
                Name = "Сертификат открытого ключа (СОК КУЦ)"
            },
            new CertModel
            {
                Path = "https://nces.by/wp-content/uploads/certificates/pki/ruc_old.cer",
                Name = "Сертификат открытого ключа (СОК РУЦ); (старый)"
            },
            new CertModel
            {
                Path = "https://nces.by/wp-content/uploads/certificates/pki/ruc.cer",
                Name = "Сертификат открытого ключа (СОК РУЦ); (новый)"
            },

            // Revocation List
            new CertModel
            {
                Path = "https://nces.by/wp-content/uploads/certificates/pki/kuc.crl",
                Name = "COC Корневой удостоверяющий центр ГосСУОК"
            },
            new CertModel
            {
                Path = "https://nces.by/wp-content/uploads/certificates/pki/ruc.crl",
                Name = "COC Республиканский удостоверяющий центр ГосСУОК"
            },
            new CertModel
            {
                Path = "https://nces.by/wp-content/uploads/certificates/pki/cas_ruc.crl",
                Name = "COC cлужбы атрибутных сертификатов юридических лиц"
            }
        };
    }

    public string Name { get; set; } = "Сертификаты";

    /// <summary>
    /// 
    /// </summary>
    public bool UseSwagger
    {
        get => _useSwagger;
        set => Set(ref _useSwagger, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public string? CryptLibraryPath
    {
        get => _cryptLibraryPath;
        set => Set(ref _cryptLibraryPath, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IsLocal
    {
        get => _isLocal;
        set => Set(ref _isLocal, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<CertModel> Certificates
    {
        get => _certificates;
        set => Set(ref _certificates, value);
    }
}