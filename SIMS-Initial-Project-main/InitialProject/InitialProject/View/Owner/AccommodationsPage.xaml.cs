using InitialProject.Model;
using InitialProject.ViewModel.Owner;
using System.Windows.Controls;

namespace InitialProject.View.Owner
{
    public partial class AccommodationsPage : Page
    {
        public AccommodationsPageViewModel _viewModel { get; set; }
        public OwnerMainWindow MainWindow { get; set; }
        public User LoggedInUser { get; set; }

        public AccommodationsPage(OwnerMainWindow window, User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            MainWindow = window;
            _viewModel = new AccommodationsPageViewModel(window, user);
            DataContext = _viewModel;
        }
    }
}
