using InitialProject.Model;
using InitialProject.ViewModel.Owner;
using System.Windows;
using System.Windows.Navigation;

namespace InitialProject.View.Owner
{
    public partial class OwnerMainWindow : Window
    {
        public OwnerMainWindowViewModel _viewModel { get; set; }
        public User LoggedInUser { get; set; }

        public NavigationService navigationService { get; set; }

        public OwnerMainWindow(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            navigationService = Main.NavigationService;
            Main.Content = new HomeScreen(this, LoggedInUser);
            _viewModel = new OwnerMainWindowViewModel(this, navigationService);
            DataContext = _viewModel;
        }
    }
}
