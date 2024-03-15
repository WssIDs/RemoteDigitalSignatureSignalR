namespace Common.Mvvm.Dialog
{
    /// <summary>
    /// 
    /// </summary>
    public class RegistryType
    {
        public string Name { get; } = "None";
        public string View { get; }
        public string ViewModel { get; }


        public RegistryType(string name, string view, string viewModel)
            : this(view, viewModel)
        {
            Name = name;
        }

        public RegistryType(string view, string viewModel)
        {
            View = view;
            ViewModel = viewModel;
        }

        public override int GetHashCode()
        {
            return (Name + View + ViewModel).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is not RegistryType registryType) return false;
            return string.Equals(Name, registryType.Name) && string.Equals(View, registryType.View) &&
                   string.Equals(ViewModel, registryType.ViewModel);
        }
    }
}