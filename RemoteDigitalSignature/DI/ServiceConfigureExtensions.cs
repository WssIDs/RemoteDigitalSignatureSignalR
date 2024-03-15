using System;
using System.Net.Http;
using Common.Mvvm.Dialog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RemoteDigitalSignature.Service.Abstractions;
using RemoteDigitalSignature.Service.Services;
using RemoteDigitalSignature.ViewModels.Common;

namespace RemoteDigitalSignature.DI;

/// <summary>
/// 
/// </summary>
public static partial class ServiceConfigureExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        services.AddLogging(configure => configure.AddConsole());//.AddFile());
        return services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDialogService, MainDialogService>();
        services.AddSingleton<IContainerRegistry, ContainerRegistry>();
        services.AddSingleton<IDirectoryDialogService, DirectoryDialogService>();

        services.AddHttpClient("netClient", client =>
        {
            client.BaseAddress = new Uri("https://esb-identity.ivcmf.by:433");
        }).ConfigurePrimaryHttpMessageHandler(_ =>
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback +=
                (_, _, _, _) => true;

            return handler;
        });

        services.AddSingleton<IStateManager, StateManager>();
        services.AddTransient<IMainSignService, MainSignService>();
        services.AddSingleton<ICertificateRevocationListStoreService, CertificateRevocationListStoreService>();
        services.AddTransient<IXmlSignToXmlDsigService, XmlSignToXmlDsigService>();

        return services;
    }
}