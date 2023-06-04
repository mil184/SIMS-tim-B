using InitialProject.Model;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using InitialProject.View.Owner;
using MenuNavigation.Commands;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace InitialProject.ViewModel.Owner
{
    public class AccommodationsPageViewModel : IObserver
    {
        public NavigationService navigationService { get; private set; }
        public OwnerMainWindow MainWindow { get; set; }
        public User LoggedInUser { get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public Accommodation MinAccommodation { get; set; }
        public Accommodation MaxAccommodation { get; set; }

        private readonly AccommodationService _accommodationService;
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly LocationService _locationService;

        private string _leastPopular;
        public string LeastPopular
        {
            get => _leastPopular;
            set
            {
                if (value != _leastPopular)
                {
                    _leastPopular = value;
                }
            }
        }

        private string _mostPopular;
        public string MostPopular
        {
            get => _mostPopular;
            set
            {
                if (value != _mostPopular)
                {
                    _mostPopular = value;
                }
            }
        }

        public RelayCommand AddAccommodationCommand { get; set; }
        public RelayCommand NavigateToStatisticsPageCommand { get; set; }
        public RelayCommand NavigateToMinStatisticsPageCommand { get; set; }
        public RelayCommand NavigateToMaxStatisticsPageCommand { get; set; }
        public RelayCommand RenovateAccommodationCommand { get; set; }

        private void Execute_NavigateToAddAccommodationPageCommand(object obj)
        {
            Page addAccommodation = new AddAccommodationPage(MainWindow, _accommodationService);
            navigationService.Navigate(addAccommodation);
        }

        private void Execute_NavigateToStatisticsPageCommand(object obj)
        {
            Page statistics = new StatisticsPage(SelectedAccommodation);
            navigationService.Navigate(statistics);
        }

        private void Execute_NavigateToMinStatisticsPageCommand(object obj)
        {
            Page statistics = new StatisticsPage(MinAccommodation);
            navigationService.Navigate(statistics);
        }

        private void Execute_NavigateToMaxStatisticsPageCommand(object obj)
        {
            Page statistics = new StatisticsPage(MaxAccommodation);
            navigationService.Navigate(statistics);
        }

        private void Execute_NavigateToRenovateAccommodationPageCommand(object obj)
        {
            Page renovateAccommodation = new RenovateAccommodationPage(navigationService, LoggedInUser, SelectedAccommodation);
            navigationService.Navigate(renovateAccommodation);
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        public AccommodationsPageViewModel(NavigationService navService, OwnerMainWindow window, User user)
        {
            MainWindow = window;
            navigationService = navService;
            LoggedInUser = user;

            _accommodationService = new AccommodationService();
            _accommodationService.Subscribe(this);
            _accommodationReservationService = new AccommodationReservationService();
            _accommodationReservationService.Subscribe(this);
            _locationService = new LocationService();
            _locationService.Subscribe(this);

            AddAccommodationCommand = new RelayCommand(Execute_NavigateToAddAccommodationPageCommand, CanExecute_Command);
            NavigateToStatisticsPageCommand = new RelayCommand(Execute_NavigateToStatisticsPageCommand, CanExecute_Command);
            NavigateToMinStatisticsPageCommand = new RelayCommand(Execute_NavigateToMinStatisticsPageCommand, CanExecute_Command);
            NavigateToMaxStatisticsPageCommand = new RelayCommand(Execute_NavigateToMaxStatisticsPageCommand, CanExecute_Command);
            RenovateAccommodationCommand = new RelayCommand(Execute_NavigateToRenovateAccommodationPageCommand, CanExecute_Command);

            InitializeAccommodations();
            InitializeSuggestions();
        }

        private void InitializeSuggestions()
        {
            int maxLocationId = _accommodationReservationService.GetBusiestLocation(LoggedInUser.Id).Item1;
            int minLocationId = _accommodationReservationService.GetBusiestLocation(LoggedInUser.Id).Item2;

            MaxAccommodation = _accommodationService.GetAccommodationByLocation(maxLocationId);
            MinAccommodation = _accommodationService.GetAccommodationByLocation(minLocationId);

            MostPopular = _locationService.GetById(maxLocationId).City + " is pretty popular!";
            LeastPopular = _locationService.GetById(minLocationId).City + " isn't doing so well!";
        }

        private void InitializeAccommodations()
        {
            Accommodations = new ObservableCollection<Accommodation>();
            FormAccommodations();
        }

        private void FormAccommodations()
        {
            foreach (Accommodation accommodation in _accommodationService.GetByUser(LoggedInUser.Id))
            {
                Accommodations.Add(accommodation);
            }
        }

        public void Update()
        {
            Accommodations.Clear();
            FormAccommodations();
        }
    }
}
