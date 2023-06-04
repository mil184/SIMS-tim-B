using InitialProject.Model;
using InitialProject.ViewModel.Owner;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace InitialProject.View.Owner
{
    public partial class RenovateAccommodationPage : Page
    {
        public NavigationService navigationService { get; set; }
        public User LoggedInUser { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public RenovateAccommodationPageViewModel _viewModel { get; set; }
        public RenovateAccommodationPage(NavigationService navService, User user, Accommodation selectedAccommodation)
        {
            InitializeComponent();
            navigationService = navService;
            LoggedInUser = user;
            SelectedAccommodation = selectedAccommodation;
            _viewModel = new RenovateAccommodationPageViewModel(navigationService, LoggedInUser, SelectedAccommodation);
            DataContext = _viewModel;
        }
    }
}
