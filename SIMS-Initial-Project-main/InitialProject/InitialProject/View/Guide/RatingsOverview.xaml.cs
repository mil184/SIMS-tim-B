using InitialProject.ViewModel.Guide;
using MenuNavigation.Commands;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.View.Guide
{
    /// <summary>
    /// Interaction logic for RatingsOverview.xaml
    /// </summary>
    public partial class RatingsOverview 
    {
        private readonly RatingsOverviewViewModel _viewModel;
        public RatingsOverview(RatingsOverviewViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _viewModel.CancelCommand = new RelayCommand(obj =>
            {
                    this.Close();
            });
        }

    }
}
