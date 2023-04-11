using InitialProject.ViewModel.Owner;
using System.Windows;

namespace InitialProject.View.Owner
{
    public partial class ReviewGuest : Window
    {
        private readonly ReviewGuestViewModel _viewModel;

        public ReviewGuest(ReviewGuestViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void CleannessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _viewModel.Cleanness = (int)CleannessSlider.Value + 1;
        }

        private void BehaviourSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _viewModel.Behaviour = (int)BehaviourSlider.Value + 1;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnReviewGuest_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Review();
            Close();
        }
    }
}
