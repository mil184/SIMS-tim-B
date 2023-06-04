using InitialProject.Model;
using InitialProject.ViewModel.Owner;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace InitialProject.View.Owner
{
    public partial class RescheduleRequestsPage : Page
    {
        public RescheduleRequestsPageViewModel _viewModel { get; set; }
        public User LoggedInUser { get; set; }
        public NavigationService navigationService { get; set; }

        public RescheduleRequestsPage(NavigationService navService, User user)
        {
            InitializeComponent();
            navigationService = navService;
            LoggedInUser = user;
            _viewModel = new RescheduleRequestsPageViewModel(navigationService, LoggedInUser);
            DataContext = _viewModel;
        }
    }
}
