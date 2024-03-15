using System.Collections.ObjectModel;

namespace Common.Mvvm.Tab
{
    public interface ITabManager
    {
        ITab SelectedTab { get; set; }

        ObservableCollection<ITab> Tabs { get; }

        void NewTab(MenuCommandParameterData menuData);
    }
}