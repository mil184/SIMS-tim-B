using InitialProject.Model;
using InitialProject.ViewModel.Owner;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace InitialProject.View.Owner
{
    public partial class ForumsPage : Page
    {
        public User LoggedInUser { get; set; }
        public NavigationService navigationService { get; set; }
        public ForumsPageViewModel _viewModel { get; set; }
        public ForumsPage(NavigationService navService, User user)
        {
            InitializeComponent();
            navigationService = navService;
            LoggedInUser = user;
            _viewModel = new ForumsPageViewModel(navigationService, LoggedInUser);
            DataContext = _viewModel;
        }
    }
}
