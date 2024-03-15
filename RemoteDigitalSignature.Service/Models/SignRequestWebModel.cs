using System.ComponentModel.DataAnnotations;

namespace RemoteDigitalSignature.Service.Models;

/// <summary>
/// 
/// </summary>
public class BaseSignRequestWebModel
{
    /// <summary>
    /// Ключ сертификата
    /// </summary>
    [Required(AllowEmptyStrings = true)]
    public string PublicKeyId { get; set; } = null!;

    /// <summary>
    /// Пароль контейнера
    /// </summary>
    [Required(AllowEmptyStrings = true)]
    public string Password { get; set; } = null!;

    /// <summary>
    /// Возвращать исходные данные
    /// </summary>
    public bool IsReturnData { get; set; } = false;
}

/// <summary>
/// 
/// </summary>
public class SignFileRequestWebModel : BaseSignRequestWebModel
{
    /// <summary>
    /// Путь к файлу
    /// </summary>
    [Required]
    public string Path { get; set; } = null!;
}


/// <summary>
/// 
/// </summary>
public class SignDataByteRequestWebModel : BaseSignRequestWebModel
{
    /// <summary>
    /// Данные для подписи
    /// </summary>
    [Required]
    public byte[] Data { get; set; } = null!;
}

/// <summary>
/// 
/// </summary>
public class SignDataStringRequestWebModel : BaseSignRequestWebModel
{
    /// <summary>
    /// Данные для подписи
    /// </summary>
    [Required]
    public string Data { get; set; } = null!;
}