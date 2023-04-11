using InitialProject.ViewModel.Owner;
using System.Windows;

namespace InitialProject.View.Owner
{
    public partial class ReviewRequest : Window
    {
        private readonly ReviewRequestViewModel _viewModel;

        public ReviewRequest(ReviewRequestViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            textBlock.Text = _viewModel.CheckAvailability();
        }

        private void btnDecline_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Decline();
            Close();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Accept();
            Close();
        }
    }
}
