using InitialProject.Model;
using InitialProject.ViewModel.Owner;
using System.Windows.Controls;

namespace InitialProject.View.Owner
{
    public partial class GuestReviewPage : Page
    {
        public User LoggedInUser { get; set; }
        public GuestReviewPageViewModel _viewModel { get; set; }

        public GuestReviewPage(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            _viewModel = new GuestReviewPageViewModel(user);
            DataContext = _viewModel;
        }
    }
}
