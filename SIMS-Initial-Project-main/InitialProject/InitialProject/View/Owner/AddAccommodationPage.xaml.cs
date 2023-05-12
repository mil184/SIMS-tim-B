using InitialProject.Service;
using InitialProject.ViewModel.Owner;
using System.Windows.Controls;

namespace InitialProject.View.Owner
{
    public partial class AddAccommodationPage : Page
    {
        public OwnerMainWindow MainWindow { get; set; }
        public AddAccommodationPageViewModel _viewModel { get; set; }

        public AddAccommodationPage(OwnerMainWindow window, AccommodationService service)
        {
            InitializeComponent();
            MainWindow = window;
            _viewModel = new AddAccommodationPageViewModel(MainWindow, this, service);
            DataContext = _viewModel;
        }
    }
}
