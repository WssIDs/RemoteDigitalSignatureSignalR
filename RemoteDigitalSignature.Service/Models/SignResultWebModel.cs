namespace RemoteDigitalSignature.Service.Models;

public class ErrorWebModel
{
    /// <summary>
    /// 
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Message { get; set; } = null!;
}

public class BaseResultWebModel
{
    /// <summary>
    /// Валидна ли подпись
    /// </summary>
    public bool IsValid { get; set; }
}

public class BaseWithErrorResultWebModel : BaseResultWebModel
{
    /// <summary>
    /// 
    /// </summary>
    public ErrorWebModel? Error { get; set; }
}

public class XmlSignToXaDesWebModel : BaseWithErrorResultWebModel
{
    public XmlSignToXaDesWebModel()
    {
        SignXml = string.Empty;
    }
    
    public string SignXml { get; set; }
}


public class BaseWithMultiplyErrorErrorResultWebModel : BaseResultWebModel
{
    /// <summary>
    /// 
    /// </summary>
    public List<ErrorWebModel>? Errors { get; set; }
}

/// <summary>
/// 
/// </summary>
public class HashWebModel
{
    /// <summary>
    /// Исходные данные
    /// </summary>
    public byte[]? Data { get; set; }

    /// <summary>
    /// Хеш данных
    /// </summary>
    public string? Hash { get; set; }

    /// <summary>
    /// Валидна ли подпись
    /// </summary>
    public bool IsValid { get; set; }
}

/// <summary>
/// 
/// </summary>
public class SignWebModel : HashWebModel
{
    /// <summary>
    /// ЭЦП данных
    /// </summary>
    public byte[]? Signature { get; set; }
}

public class SignResultWebModel : BaseWithErrorResultWebModel
{
    /// <summary>
    /// 
    /// </summary>
    public SignWebModel? SignData { get; set; }
}

public class SignFileResultWebModel : SignResultWebModel
{
    /// <summary>
    /// 
    /// </summary>
    public string? InitialPath { get; set; }
}

public class HashResultWebModel : BaseWithErrorResultWebModel
{
    /// <summary>
    /// 
    /// </summary>
    public HashWebModel? HashData { get; set; }
}

public class HashFileResultWebModel : HashResultWebModel
{
    /// <summary>
    /// 
    /// </summary>
    public string? InitialPath { get; set; }
}