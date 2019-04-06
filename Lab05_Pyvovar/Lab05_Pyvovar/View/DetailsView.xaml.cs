using System.Windows.Controls;
using Lab05_Pyvovar.Tools.Navigation;
using Lab05_Pyvovar.ViewModel;

namespace Lab05_Pyvovar.View
{
    /// <summary>
    /// Interaction logic for DetailsView.xaml
    /// </summary>
    public partial class DetailsView : UserControl, INavigatable
    {
        private readonly DetailViewModel _detailViewModel = new DetailViewModel();

        public DetailsView()
        {
            InitializeComponent();
            DataContext = _detailViewModel;
        }

        INavigatable INavigatable.RefreshInfo()
        {
            _detailViewModel.RefreshInfo();
            return this;
        }
    }
}
