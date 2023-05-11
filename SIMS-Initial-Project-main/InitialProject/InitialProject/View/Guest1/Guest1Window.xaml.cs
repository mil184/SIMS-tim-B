 using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InitialProject.Model.DTO;
using InitialProject.Resources.Enums;
using InitialProject.Service;
using System.Diagnostics.Metrics;
using InitialProject.View.Guest2;
using InitialProject.ViewModel.Guest1;

namespace InitialProject.View.Guest1
{
    /// <summary>
    /// Interaction logic for Guest1Window.xaml
    /// </summary>
    public partial class Guest1Window : Window, INotifyPropertyChanged, IObserver
    {
        public User LoggedInUser { get; set; }
        public ObservableCollection<Accommodation> AllAccommodations { get; set; }
        public ObservableCollection<GuestAccommodationDTO> PresentableAccommodations { get; set; }
        public ObservableCollection<Location> Locations;
        public GuestAccommodationDTO SelectedAccommodation { get; set; }

        public AccommodationRatingsDTO SelectedUnratedAccommodation { get; set; }
        public ObservableCollection<AccommodationRatings> AccommodationRatings { get; set; }
        public AccommodationRatings SelectedAccommodationRatings { get; set; }
        public ObservableCollection<AccommodationReservation> UnratedReservations { get; set; }

        public ObservableCollection<AccommodationReservation> PresentableReservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }
        public ObservableCollection<RescheduleRequest> AllReschedules { get; set; }

        public ObservableCollection<Guest1RatingsDTO> GuestRatings { get; set; }

        private readonly AccommodationService _accommodationService;
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly LocationService _locationService;
        private readonly UserRepository _userRepository;
        private readonly ImageRepository _imageRepository;
        private readonly AccommodationRatingsRepository _accommodationRatingsRepository;
        private readonly RescheduleRequestRepository _rescheduleRequestRepository;
        private readonly GuestReviewRepository _guestReviewRepository;

        private string searchName;
        public string SearchName
        {
            get { return searchName; }
            set
            {
                searchName = value;

                OnPropertyChanged(nameof(SearchName));
            }
        }

        private string searchCountry;
        public string SearchCountry
        {
            get { return searchCountry; }
            set
            {
                searchCountry = value;

                OnPropertyChanged(nameof(SearchCountry));
            }
        }

        private string searchCity;
        public string SearchCity
        {
            get { return searchCity; }
            set
            {
                searchCity = value;

                OnPropertyChanged(nameof(SearchCity));
            }
        }

        private string searchGuests;
        public string SearchGuests
        {
            get { return searchGuests; }
            set
            {
                searchGuests = value;

                OnPropertyChanged(nameof(SearchGuests));
            }
        }

        private string searchDays;
        public string SearchDays
        {
            get { return searchDays; }
            set
            {
                searchDays = value;

                OnPropertyChanged(nameof(SearchDays));
            }
        }

        private string searchType;
        public string SearchType
        {
            get { return searchType; }
            set
            {
                searchType = value;

                OnPropertyChanged(nameof(SearchType));
            }
        }

        private ObservableCollection<AccommodationRatingsDTO> _unratedAccommodations;
        public ObservableCollection<AccommodationRatingsDTO> UnratedAccommodations
        {
            get => _unratedAccommodations;
            set
            {
                if (_unratedAccommodations != value)
                {
                    _unratedAccommodations = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1Window(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;

            _accommodationService = new AccommodationService();
            _accommodationService.Subscribe(this);

            _accommodationReservationService = new AccommodationReservationService();
            _accommodationReservationService.Subscribe(this);

            _locationService = new LocationService();
            _locationService.Subscribe(this);

            _locationService = new LocationService();
            _locationService.Subscribe(this);

            _userRepository = new UserRepository();
            _userRepository.Subscribe(this);

            _imageRepository = new ImageRepository();
            _imageRepository.Subscribe(this);

            _accommodationRatingsRepository = new AccommodationRatingsRepository();
            _accommodationRatingsRepository.Subscribe(this);

            _rescheduleRequestRepository = new RescheduleRequestRepository();
            _rescheduleRequestRepository.Subscribe(this);

            _guestReviewRepository  = new GuestReviewRepository();
            _guestReviewRepository.Subscribe(this);

            AllAccommodations = new ObservableCollection<Accommodation>(_accommodationService.GetAll());
            PresentableAccommodations = ConvertToDTO(new List<Accommodation>(AllAccommodations));

            UnratedAccommodations = new ObservableCollection<AccommodationRatingsDTO>();
            FormUnratedReservation();
            UnratedReservations = new ObservableCollection<AccommodationReservation>();

            PresentableReservations = new ObservableCollection<AccommodationReservation>(_accommodationReservationService.GetAll());
            AllReschedules = new ObservableCollection<RescheduleRequest>(_rescheduleRequestRepository.GetAll());

            GuestRatings = new ObservableCollection<Guest1RatingsDTO>();
            FormGuestRatings();
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                ReserveAccommodation reservationForm = new ReserveAccommodation(SelectedAccommodation, LoggedInUser, _accommodationService, _accommodationReservationService);
                reservationForm.Show();
            }
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            CheckIfAllEmpty();
            PresentableAccommodations.Clear();

            List<Accommodation> result = new List<Accommodation>();


            if (!string.IsNullOrEmpty(SearchName))
            {
                result = _accommodationService.GetByName(SearchName);
            }
            else
            {
                result = _accommodationService.GetAll();
            }

            if (!string.IsNullOrEmpty(SearchCountry))
            {
                result = result.Intersect(_accommodationService.GetByCountryName(SearchCountry)).ToList();
            }
            if (!string.IsNullOrEmpty(SearchCity))
            {
                result = result.Intersect(_accommodationService.GetByCityName(SearchCity)).ToList();
            }
            if (!string.IsNullOrEmpty(SearchGuests))
            {
                result = result.Intersect(_accommodationService.GetByMaxGuests(int.Parse(SearchGuests))).ToList();
            }
            if (!string.IsNullOrEmpty(SearchDays))
            {
                result = result.Intersect(_accommodationService.GetByMinDays(int.Parse(SearchDays))).ToList();
            }
            if (!string.IsNullOrEmpty(SearchType))
            {
                if (SearchType.Split(':')[1].Trim() != "-")
                {
                    result = result.Intersect(_accommodationService.GetByType(SearchType.Split(':')[1].Trim())).ToList();
                }
            }

            ObservableCollection<GuestAccommodationDTO> searchResults = ConvertToDTO(result);
            foreach (GuestAccommodationDTO dto in searchResults)
            {
                PresentableAccommodations.Add(dto);
            }
        }

        public void CheckIfAllEmpty()
        {
            if (string.IsNullOrEmpty(SearchName) && string.IsNullOrEmpty(SearchCountry) && string.IsNullOrEmpty(SearchCity) && string.IsNullOrEmpty(SearchGuests) && string.IsNullOrEmpty(SearchDays))
            {
                foreach (Accommodation accommodation in _accommodationService.GetAll())
                {
                    PresentableAccommodations.Add(ConvertToDTO(accommodation));
                }      
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<GuestAccommodationDTO> ConvertToDTO(List<Accommodation> accommodations)
        {
            ObservableCollection<GuestAccommodationDTO> dto = new ObservableCollection<GuestAccommodationDTO>();
            foreach (Accommodation accommodation in accommodations)
            {
                dto.Add(new GuestAccommodationDTO(accommodation.Id, accommodation.Name,
                    _locationService.GetById(accommodation.LocationId).Country,
                     _locationService.GetById(accommodation.LocationId).City,
                     accommodation.Type, accommodation.MaxGuests, accommodation.MinReservationDays, accommodation.CancellationPeriod, accommodation.OwnerId));
            }
            return dto;
        }
        public GuestAccommodationDTO ConvertToDTO(Accommodation accommodation)
        {
            return new GuestAccommodationDTO(accommodation.Id, accommodation.Name,
                   _locationService.GetById(accommodation.LocationId).Country,
                   _locationService.GetById(accommodation.LocationId).City,
                   accommodation.Type, accommodation.MaxGuests, accommodation.MinReservationDays, accommodation.CancellationPeriod, accommodation.OwnerId);

        }

        public AccommodationRatingsDTO ConvertToDTO(AccommodationReservation reservation)
        {
            return new AccommodationRatingsDTO(reservation.Id,
                _userRepository.GetById(reservation.OwnerId).Username,
                _accommodationService.GetById(reservation.AccommodationId).Name);

        }
        public ObservableCollection<AccommodationRatingsDTO> ConvertToDTO(ObservableCollection<AccommodationReservation> reservations)
        {
            ObservableCollection<AccommodationRatingsDTO> dto = new ObservableCollection<AccommodationRatingsDTO>();
            foreach (AccommodationReservation reservation in reservations)
            {
                dto.Add(ConvertToDTO(reservation));
            }
            return dto;
        }

        public void Update() 
        {
            FormUnratedReservation();
            FormGuestRatings();
        }

        private void ImagesButton_Click(object sender, RoutedEventArgs e)
        {
            Accommodation selectedAccomodation = ConvertToAccomodation(SelectedAccommodation);

            List<string> imageUrls = new List<string>();

            foreach (int imageId in selectedAccomodation.ImageIds)
            {
                if (_imageRepository.GetById(imageId) != null)
                    imageUrls.Add(_imageRepository.GetById(imageId).Url);
            }

            AccommodationImages accommodationImages = new AccommodationImages(imageUrls);
            accommodationImages.ShowDialog();
        }
        public Accommodation ConvertToAccomodation(GuestAccommodationDTO dto)
        {
            return _accommodationService.GetById(dto.Id);
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }

        public bool RecentlyEnded(AccommodationReservation reservation)
        {
            TimeSpan daysPassed = DateTime.Today - reservation.EndDate;
            return daysPassed.TotalDays >= 0 && daysPassed.TotalDays <= 5;
        }

        public void FormUnratedReservation()
        {
            UnratedAccommodations.Clear();
            var reservations = _accommodationReservationService.GetUnratedAccommodations().Where(r => RecentlyEnded(r)); 
            UnratedAccommodations = ConvertToDTO(new ObservableCollection<AccommodationReservation>(reservations));
        }


        public void CheckForAvailableRatings()
        {
            UnratedAccommodations = new ObservableCollection<AccommodationRatingsDTO>();
            UnratedReservations = new ObservableCollection<AccommodationReservation>();
            FormUnratedReservation();
        }

        private void Evaluate_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUnratedAccommodation != null)
            {
                Evaluate evaluateAccommodation = new Evaluate(SelectedUnratedAccommodation, _accommodationRatingsRepository, _accommodationReservationService, _imageRepository);
                evaluateAccommodation.ShowDialog();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CancelReservation_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedReservation != null)
            {
               
                var messageBoxResult = MessageBox.Show($"Are you sure you want to cancel reservation for date: {SelectedReservation.StartDate:d} - {SelectedReservation.EndDate:d}", "Cancel Reservation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult == MessageBoxResult.Yes)
                {

                    if (CancellationPeriodPassed(SelectedReservation))
                    {
                        return;
                    }

                    _accommodationReservationService.Remove(SelectedReservation);
                    MessageBox.Show("Reservation canceled successfully.");

                    RefreshPresentableReservations();
                }
            }
        }

        public void RefreshPresentableReservations()
        {
            PresentableReservations.Clear();
            var reservations = _accommodationReservationService.GetAll();
            foreach (var reservation in reservations)
            {
                PresentableReservations.Add(reservation);
            }
        }
        private void SendRequest_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedReservation != null)
            {
                SendRequestViewModel sendRequestViewModel = new SendRequestViewModel(SelectedReservation, _rescheduleRequestRepository);
                SendRequest sendRequest = new SendRequest(sendRequestViewModel);
                sendRequest.ShowDialog();
            }
        }

        public bool CancellationPeriodPassed(AccommodationReservation reservation)
        {
            TimeSpan timeLeft = reservation.StartDate - DateTime.Now;
            if (timeLeft.TotalHours <= 24)
            {
                ShowCancelWarning();
                return true;
            }
            else if (timeLeft.TotalDays <= reservation.CancellationPeriod)
            {
                ShowCancelPeriodWarning();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CheckRescheduleRequestsStatus()
        {
            var requests = _rescheduleRequestRepository.GetAll().Where(r => r.GuestId == LoggedInUser.Id);

            foreach (var request in requests)
            {
                if (request.Status == RescheduleRequestStatus.approved || request.Status == RescheduleRequestStatus.rejected)
                {
                    string message = $"The status of reschedule request {request.Id} has been changed to {request.Status}.";
                    MessageBox.Show(message, "Reschedule Request Status Change", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void GuestWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            CheckRescheduleRequestsStatus();
        }


        private void ShowCancelWarning()
        {
            MessageBox.Show("Cannot cancel reservation. Less than 24 hours left before start date.", "Cancel reservation warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ShowCancelPeriodWarning()
        {
            MessageBox.Show($"Cannot cancel reservation. Less than {SelectedReservation.CancellationPeriod} left before start date.", "Cancel reservation warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void FormGuestRatings()
        {
            foreach (GuestReview review in _guestReviewRepository.GetAll())
            {
                if (review.GuestId == LoggedInUser.Id && _accommodationReservationService.GetById(review.ReservationId).IsRated)
                { 
                   Guest1RatingsDTO dto = new Guest1RatingsDTO(review.Cleanness, review.Behavior, review.Comment);
                   GuestRatings.Add(dto);
                }
            }
        }
    }
}
