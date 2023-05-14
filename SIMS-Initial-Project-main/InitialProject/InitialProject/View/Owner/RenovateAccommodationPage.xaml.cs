using InitialProject.Model;
using InitialProject.ViewModel.Owner;
using System.Windows.Controls;

namespace InitialProject.View.Owner
{
    public partial class RenovateAccommodationPage : Page
    {
        public OwnerMainWindow MainWindow { get; set; }
        public User LoggedInUser { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public RenovateAccommodationPageViewModel _viewModel { get; set; }
        public RenovateAccommodationPage(OwnerMainWindow window, User user, Accommodation selectedAccommodation)
        {
            InitializeComponent();
            MainWindow = window;
            LoggedInUser = user;
            SelectedAccommodation = selectedAccommodation;
            _viewModel = new RenovateAccommodationPageViewModel(MainWindow, LoggedInUser, SelectedAccommodation);
            DataContext = _viewModel;
        }
    }
}
