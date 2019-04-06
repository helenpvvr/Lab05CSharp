using System.Windows.Controls;
using Lab05_Pyvovar.Tools.Navigation;
using Lab05_Pyvovar.ViewModel;

namespace Lab05_Pyvovar.View
{
    /// <summary>
    /// Interaction logic for InfoView.xaml
    /// </summary>
    public partial class InfoView : UserControl, INavigatable
    {
        private readonly InfoViewModel _infoViewModel = new InfoViewModel();

        public InfoView()
        {
            InitializeComponent();
            DataContext = _infoViewModel;
        }

        INavigatable INavigatable.RefreshInfo()
        {
            return this;
        }
    }
}
