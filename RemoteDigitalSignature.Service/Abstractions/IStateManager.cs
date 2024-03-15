namespace RemoteDigitalSignature.Service.Abstractions;

/// <summary>
/// 
/// </summary>
public interface IStateManager
{
    /// <summary>
    /// 
    /// </summary>
    public bool IsReInitLibrary { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsBusy { get; set; }
}
