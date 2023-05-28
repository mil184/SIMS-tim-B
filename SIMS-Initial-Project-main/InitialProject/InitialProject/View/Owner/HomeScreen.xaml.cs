using InitialProject.Model;
using InitialProject.ViewModel.Owner;
using System.Windows.Controls;

namespace InitialProject.View.Owner
{
    public partial class HomeScreen : Page
    {
        public OwnerMainWindow MainWindow { get; set; }
        public User LoggedInUser { get; set; }
        public HomeScreenViewModel _viewModel { get; set; }

        public HomeScreen(OwnerMainWindow window, User user)
        {
            InitializeComponent();
            MainWindow = window;
            LoggedInUser = user;
            _viewModel = new HomeScreenViewModel(MainWindow);
            DataContext = _viewModel;
        }
    }
}
