using InitialProject.ViewModel.Guide;
using System.Windows;

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
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.PreviousImage();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.NextImage();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
