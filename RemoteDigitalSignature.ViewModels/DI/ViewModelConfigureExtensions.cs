using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RemoteDigitalSignature.ViewModels.Common;

namespace RemoteDigitalSignature.ViewModels.DI;

public static partial class ViewModelConfigureExtensions
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        var viewModels = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(BaseViewModel));

        foreach (var viewModel in viewModels)
        {
            services.AddTransient(viewModel);
            Debug.WriteLine(viewModel.FullName);
        }

        return services;
    }
}