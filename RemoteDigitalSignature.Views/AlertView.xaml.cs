using System.Windows;

namespace RemoteDigitalSignature.Views
{
    /// <summary>
    /// Interaction logic for AlertView.xaml
    /// </summary>
    public partial class AlertView : Window
    {
        public AlertView()
        {
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth * 0.5f;
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight * 0.8;
            InitializeComponent();
        }
    }
}
