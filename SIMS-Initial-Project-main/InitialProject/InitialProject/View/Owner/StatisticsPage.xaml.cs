using InitialProject.Model;
using InitialProject.ViewModel.Owner;
using System.Windows.Controls;

namespace InitialProject.View.Owner
{
    public partial class StatisticsPage : Page
    {
        public Accommodation SelectedAccommodation { get; set; }
        public StatisticsPageViewModel _viewModel { get; set; }
        public StatisticsPage(Accommodation accommodation)
        {
            InitializeComponent();
            SelectedAccommodation = accommodation;
            _viewModel = new StatisticsPageViewModel(this, SelectedAccommodation);
            DataContext = _viewModel;
        }
    }
}
