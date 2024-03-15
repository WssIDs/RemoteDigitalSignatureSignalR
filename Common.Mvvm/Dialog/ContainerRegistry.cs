using System.Windows;
using System.Windows.Controls;
using Common.Exceptions;
using Common.Mvvm.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Mvvm.Dialog;

public class ContainerRegistry : IContainerRegistry
{
    private readonly List<RegistryType> _registryTypes = new();

    public void RegisterDialog<T, TVm>() where T : ContentControl where TVm : ViewModelBase
    {
        var type = new RegistryType(typeof(T).ToString(), typeof(TVm).ToString());
        if (_registryTypes.Any(r => r.Equals(type)))
            throw new CommonException("Ошибка регистрации представления и модели", new CommonException($"Для типа {type.ViewModel} уже зарегистрировано представление {type.View}"));
        _registryTypes.Add(type);
    }

    public void RegisterDialog(Type contentControlType, Type viewModelType)
    {
        var type = new RegistryType(contentControlType.ToString(), viewModelType.ToString());
        if (_registryTypes.Any(r => r.Equals(type)))
            throw new CommonException("Ошибка регистрации представления и модели", new CommonException($"Для типа {type.ViewModel} уже зарегистрировано представление {type.View}"));
        _registryTypes.Add(type);
    }

    /*public void RegisterDialog<T, TVm>(T view, TVm viewModel) where T : ContentControl where TVm : ViewModelBase
    {
        var type = new RegistryType(typeof(T).ToString(), typeof(TVm).ToString());
        if (_registryTypes.Any(r => r.Equals(type)))
            throw new CommonException("Ошибка регистрации представления и модели", new Exception($"Для типа {type.ViewModel} уже зарегистрировано представление {type.View}"));
        _registryTypes.Add(type);
    }*/

    public void RegisterDialog<T, TVm>(string name) where T : ContentControl where TVm : ViewModelBase
    {
        var type = new RegistryType(name, typeof(T).ToString(), typeof(TVm).ToString());
        if (_registryTypes.Any(r => r.Name.Equals(name)))
            throw new CommonException("Ошибка регистрации представления и модели", new CommonException($"Для имении {type.Name} и типа {type.ViewModel} уже зарегистрировано представление {type.View}"));
        _registryTypes.Add(type);
    }

    public void UnRegisterDialog<TVm>() where TVm : ViewModelBase
    {
        var type = _registryTypes.FirstOrDefault(r => r.ViewModel.Equals(typeof(TVm).ToString()));

        if (type != null)
        {
            _registryTypes.Remove(type);
        }
    }

    public string GetDialogType<TVm>() where TVm : ViewModelBase
    {
        var type = _registryTypes.FirstOrDefault(r => r.ViewModel.Equals(typeof(TVm).ToString()));
        if (type == null)
            throw new CommonException("Ошибка получения модели", new CommonException($"Модель с данным типом {typeof(TVm)} не зарегистрирована"));
        if (string.IsNullOrEmpty(type.View))
            throw new CommonException("Ошибка получения представления", new CommonException($"Для типа {typeof(TVm)} не зарегистрировано представлений"));
        return type.View;
    }

    public ContentControl GetDialog<TVm>() where TVm : ViewModelBase
    {
        var type = GetType(GetDialogType<TVm>());

        var scope = SimpleIoC.Current.CreateScope();

        if (type != null)
        {
            var instance = scope.ServiceProvider.GetRequiredService(type);

            if (type.BaseType != typeof(UserControl)) return (ContentControl) instance;
            var iWindow  = scope.ServiceProvider.GetRequiredService<IWindow>();

            if (iWindow is not Window wnd) return (ContentControl) instance;
            wnd.DataContext = (instance as ContentControl)?.DataContext;
            wnd.Content = instance as ContentControl;

            return wnd;

        }

        return null;
    }

    private Type GetType(string typeName)
    {
        var type = Type.GetType(typeName);
        if (type != null) return type;
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = a.GetType(typeName);
            if (type != null)
                return type;
        }

        return null;
    }
}