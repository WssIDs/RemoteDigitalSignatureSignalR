using System.Diagnostics;
using System.Reflection;
using System.Windows;
using Common.Mvvm.Dialog;
using Microsoft.Extensions.DependencyInjection;
using RemoteDigitalSignature.ViewModels.Common;

namespace RemoteDigitalSignature.Views.DI
{
    public static class ViewConfigureExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            var views = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(Window));

            foreach (var view in views)
            {
                services.AddTransient(view);
                Debug.WriteLine(view.FullName);
            }

            return services;
        }

        /// <summary>
        /// Ассоциация представлений с соотв. моделями представлений
        /// </summary>
        public static void RegisterDialogs(this IContainerRegistry containerRegistry)
        {
            var views = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(Window) && t.Name.EndsWith("View"));

            var assemblyViewModels = Assembly.GetAssembly(typeof(BaseViewModel));

            foreach (var view in views)
            {
                var viewModelName = $"{view.Name}Model";
                var viewModelType = assemblyViewModels
                    ?.GetTypes()
                    .FirstOrDefault(t => t.BaseType == typeof(BaseViewModel) && t.Name == viewModelName);

                if (viewModelType != null) containerRegistry.RegisterDialog(view, viewModelType);
            }
        }
    }
}