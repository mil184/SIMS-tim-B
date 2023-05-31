using InitialProject.Model;
using InitialProject.ViewModel.Owner;
using System.Windows.Controls;

namespace InitialProject.View.Owner
{
    public partial class RatingsPage : Page
    {
        public User LoggedInUser { get; set; }
        public RatingsPageViewModel _viewModel { get; set; }

        public RatingsPage(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            _viewModel = new RatingsPageViewModel(LoggedInUser);
            DataContext = _viewModel;
        }
    }
}
