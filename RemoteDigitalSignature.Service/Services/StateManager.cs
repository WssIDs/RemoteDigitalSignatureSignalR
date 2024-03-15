using RemoteDigitalSignature.Service.Abstractions;

namespace RemoteDigitalSignature.Service.Services;

/// <summary>
/// 
/// </summary>
public class StateManager : IStateManager
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
