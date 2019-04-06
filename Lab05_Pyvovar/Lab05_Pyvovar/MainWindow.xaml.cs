using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Lab05_Pyvovar.Tools.DataStorage;
using Lab05_Pyvovar.Tools.Managers;
using Lab05_Pyvovar.Tools.Navigation;
using Lab05_Pyvovar.ViewModel;

namespace Lab05_Pyvovar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IContentOwner
    {
        public ContentControl ContentControl
        {
            get { return _contentControl; }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            InitializeApplication();
        }

        private void InitializeApplication()
        {
            StationManager.Initialize(new ProcessDataStorage());
            NavigationManager.Instance.Initialize(new InitializationNavigationModel(this));
            NavigationManager.Instance.Navigate(ViewType.Info);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            StationManager.CloseApp();
        }
    }
}
