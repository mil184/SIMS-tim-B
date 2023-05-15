using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using InitialProject.ViewModel.Guest2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using InitialProject.Converters;

namespace InitialProject.View.Guest2
{
    public partial class Guest2Window : Window, INotifyPropertyChanged, IObserver
    {
        public User LoggedInUser { get; set; }

        #region Tours
        public Guest2TourDTO SelectedGuest2TourDTO { get; set; }
        public ObservableCollection<Guest2TourDTO> TourDTOs { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }

        public ObservableCollection<Guest2TourDTO> FinishedTourDTOs { get; set; }
        public List<Tour> FinishedTours { get; set; }

        #endregion

        #region Vouchers

        public List<Tour> CheckedTours { get; set; }
        public Tour CurrentlyActiveTour { get; set; }
        public Checkpoint CurrentlyActiveCheckpoint { get; set; }
        public ObservableCollection<Voucher> Vouchers { get; set; }

        #endregion

        public ObservableCollection<TourRequest> TourRequests { get; set; }
        public ObservableCollection<TourRequest> TourRequestsForYear { get; set; }
        public ObservableCollection<Guest2TourRequestDTO> TourRequestDTOs { get; set; }
        public TourRequestDTOConverter TourRequestDTOConverter { get; set; }
        public ObservableCollection<string> TourRequestYears { get; set; }
        public string StatusStatistic { get; set; }
        public string SelectedStatisticYear { get; set; }
        public string SelectedStatisticGuestYear { get; set; }
        public int AcceptedToursCount { get; set; }
        public int DeniedToursCount { get; set; }

        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointService _checkpointService;
        private readonly UserRepository _userRepository;
        private readonly TourReservationService _tourReservationService;
        private readonly TourRatingService _tourRatingService;
        private readonly VoucherService _voucherService;
        private readonly TourRequestService _tourRequestService;

        #region Properties

        private string country;
        public string Country
        {
            get => country;
            set
            {
                if (value != country)
                {
                    country = value;
                    OnPropertyChanged();
                }
            }
        }

        private string city;
        public string City
        {
            get => city;
            set
            {
                if (value != city)
                {
                    city = value;
                    OnPropertyChanged();
                }
            }
        }

        private string tourLanguage;
        public string TourLanguage
        {
            get => tourLanguage;
            set
            {
                if (value != tourLanguage)
                {
                    tourLanguage = value;
                    OnPropertyChanged();
                }
            }
        }

        private string personCount;
        public string PersonCount
        {
            get => personCount;
            set
            {
                if (value != personCount)
                {
                    personCount = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        public Guest2Window(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;

            _tourService = new TourService();
            _tourService.Subscribe(this);

            _imageRepository = new ImageRepository();
            _imageRepository.Subscribe(this);

            _locationService = new LocationService();
            _locationService.Subscribe(this);

            _checkpointService = new CheckpointService();
            _checkpointService.Subscribe(this);

            _userRepository = new UserRepository();
            _userRepository.Subscribe(this);

            _tourReservationService = new TourReservationService();
            _tourReservationService.Subscribe(this);

            _tourRatingService = new TourRatingService();
            _tourRatingService.Subscribe(this);

            _voucherService = new VoucherService();
            _voucherService.Subscribe(this);

            _tourRequestService = new TourRequestService();
            _tourRequestService.Subscribe(this);

            Tours = new ObservableCollection<Tour>(_tourService.GetReservableTours());
            TourDTOs = ConvertToDTO(new List<Tour>(Tours));

            List<Voucher> UserVouchers = _voucherService.GetUserVouchers(LoggedInUser);
            Vouchers = new ObservableCollection<Voucher>(_voucherService.GetActiveVouchers(UserVouchers));

            TourRequestDTOConverter = new TourRequestDTOConverter(_locationService);
            TourRequests = new ObservableCollection<TourRequest>(_tourRequestService.GetAll());
            TourRequestsForYear = new ObservableCollection<TourRequest>();
            TourRequestDTOs = new ObservableCollection<Guest2TourRequestDTO>(TourRequestDTOConverter.ConvertToDTOList(_tourRequestService.GetAll()));

            CheckedTours = new List<Tour>();
            foreach (int id in _tourReservationService.GetCheckedTourIds(LoggedInUser))
            {
                CheckedTours.Add(_tourService.GetById(id));
            }

            if(CheckedTours.Count != 0 && !CheckedTours[0].IsFinished)
            {
                CurrentlyActiveTour = CheckedTours[0];
                CurrentlyActiveCheckpoint = _checkpointService.GetById(CurrentlyActiveTour.CurrentCheckpointId);
            }

            List<Tour> UserTours = new List<Tour>(_tourService.GetUserTours(LoggedInUser));
            FinishedTours = _tourService.GetFinishedTours(UserTours);
            FinishedTourDTOs = ConvertToDTO(FinishedTours);

            TourRequestYears = new ObservableCollection<string>();
            FormTourRequestYears();

            ConfirmArrival();

            

            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Update()
        {
            TourDTOs.Clear();
            FormTours();

            FinishedTourDTOs.Clear();
            FormFinishedTours();

            Vouchers.Clear();
            FormVouchers();

            TourRequestDTOs.Clear();
            FormTourRequestDTOs();

            TourRequestYears.Clear();
            FormTourRequestYears();
        }

        public void ConfirmArrival()
        {

            if (CheckedTours.Count != 0)
            {
                TourReservation tourReservation = _tourReservationService.GetReservationByGuestIdAndTourId(LoggedInUser.Id, CheckedTours[0].Id);

                if (tourReservation.MessageBoxShown)
                {
                    return;
                }

                MessageBox.Show("Please confirm your arrival at " + CheckedTours[0].Name, "ArrivalConfirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (MessageBoxResult.Yes == MessageBoxResult.Yes)
                {
                    tourReservation.GuestArrived = true;
                    tourReservation.MessageBoxShown = true;
                    _tourReservationService.Update(tourReservation);
                }
            }
        }

        public void FormTours()
        {
            foreach (Tour tour in _tourService.GetReservableTours())
            {
                TourDTOs.Add(ConvertToDTO(tour));
            }
        }

        public void FormFinishedTours()
        {
            foreach (Tour userTour in _tourService.GetUserTours(LoggedInUser))
            {
                if (userTour.IsFinished && !_tourReservationService.GetByTourId(userTour.Id).IsRated)
                {
                    FinishedTourDTOs.Add(ConvertToDTO(userTour));
                }
            }
        }
        public void FormVouchers()
        {
            List<Voucher> UserVouchers = _voucherService.GetUserVouchers(LoggedInUser);

            foreach (Voucher voucher in _voucherService.GetActiveVouchers(UserVouchers))
            {
                Vouchers.Add(voucher);
            }
        }

        public void FormTourRequestDTOs()
        {
            foreach(TourRequest tourRequest in _tourRequestService.GetAll())
            {
                TourRequestDTOs.Add(TourRequestDTOConverter.ConvertToDTO(tourRequest));
            }
        }

        public void FormTourRequestYears()
        {
            TourRequestYears.Clear();
            TourRequestYears.Add("All time");

            foreach (TourRequest tourRequest in _tourRequestService.GetAll())
            {
                string year = tourRequest.StartTime.Year.ToString();
                if (!TourRequestYears.Contains(year))
                {
                    TourRequestYears.Add(year);
                }
            }
        }

        private void YearStatisticSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            List<TourRequest> tourRequestsForYear = _tourRequestService.GetByYear(LoggedInUser, SelectedStatisticYear);

            List<TourRequest> acceptedTours = _tourRequestService.GetAcceptedRequests(tourRequestsForYear);
            List<TourRequest> deniedTours = _tourRequestService.GetDeniedRequests(tourRequestsForYear);

            AcceptedToursCount = acceptedTours.Count();
            DeniedToursCount = deniedTours.Count();

            int AcceptedPercentage = AcceptedToursCount * 100 / tourRequestsForYear.Count();
            int DeniedPercentage = DeniedToursCount * 100 / tourRequestsForYear.Count();

            MessageBox.Show(AcceptedToursCount + " of your tours were accepted. " + DeniedToursCount + " of your tours were denied. Total tours for year:" + tourRequestsForYear.Count() + ". " + AcceptedPercentage + "% of your tours were accepted. " + DeniedPercentage + "% of your tours were denied.");

            StatusStatistic = AcceptedToursCount + "of your tours were accepted." + DeniedToursCount + "of your tours were denied.";

        }

        private void GuestYearStatisticSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            List<TourRequest> tourRequestsForYear = _tourRequestService.GetByYear(LoggedInUser, SelectedStatisticGuestYear);
            List<TourRequest> acceptedTourRequestsForYear = _tourRequestService.GetAcceptedRequests(tourRequestsForYear);

            double totalGuests = _tourRequestService.GetTotalGuestCountForYear(acceptedTourRequestsForYear);
            double totalTourRequests = acceptedTourRequestsForYear.Count();

            if (totalTourRequests==0)
            {
                MessageBox.Show("No accepted tour in this year.");
            }
            else
            {
                double averageGuests = (double)totalGuests / (double)totalTourRequests;
                MessageBox.Show("Average guests:" + averageGuests);
            }
            
        }

        public ObservableCollection<Guest2TourDTO> ConvertToDTO(List<Tour> tours)
        {
            ObservableCollection<Guest2TourDTO> dto = new ObservableCollection<Guest2TourDTO>();

            foreach (Tour tour in tours)
            {
                dto.Add(new Guest2TourDTO(
                    tour.Id,
                    tour.Name,
                    _locationService.GetById(tour.LocationId).Country,
                    _locationService.GetById(tour.LocationId).City,
                    tour.Description,
                    tour.Language,
                    tour.MaxGuests,
                    tour.CurrentGuestCount,
                    tour.StartTime,
                    tour.Duration,
                    _userRepository.GetById(tour.GuideId).Username));
            }

            return dto;
        }
        public Guest2TourDTO ConvertToDTO(Tour tour)
        {
            return new Guest2TourDTO(
                tour.Id,
                tour.Name,
                _locationService.GetById(tour.LocationId).Country,
                _locationService.GetById(tour.LocationId).City,
                tour.Description,
                tour.Language,
                tour.MaxGuests,
                tour.CurrentGuestCount,
                tour.StartTime,
                tour.Duration,
                _userRepository.GetById(tour.GuideId).Username);
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGuest2TourDTO != null)
            {
                ReserveTour reserveTourForm = new ReserveTour(SelectedGuest2TourDTO, LoggedInUser, _tourService, _tourReservationService, _voucherService, _locationService, _userRepository);
                reserveTourForm.ShowDialog();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            CheckIfAllEmpty();

            TourDTOs.Clear();

            List<Tour> result = new List<Tour>();

            if (!string.IsNullOrEmpty(Country))
            {
                result = _tourService.GetByCountryName(Country);
            }
            else
            {
                result = _tourService.GetReservableTours();
            }

            if (!string.IsNullOrEmpty(City))
            {
                result = result.Intersect(_tourService.GetByCityName(City)).ToList();
            }
            if (!string.IsNullOrEmpty(TourLanguage))
            {
                result = result.Intersect(_tourService.GetByLanguage(TourLanguage)).ToList();
            }
            if (!string.IsNullOrEmpty(PersonCount))
            {
                result = result.Intersect(_tourService.GetByGuests(int.Parse(PersonCount))).ToList();
            }

            ObservableCollection<Guest2TourDTO> searchResults = ConvertToDTO(result);
            foreach (Guest2TourDTO dto in searchResults)
            {
                TourDTOs.Add(dto);
            }
        }

        public void CheckIfAllEmpty()
        {
            if (string.IsNullOrEmpty(Country) && string.IsNullOrEmpty(City) && string.IsNullOrEmpty(TourLanguage) && string.IsNullOrEmpty(PersonCount))
            {
                foreach (Tour tour in _tourService.GetReservableTours())
                {
                    TourDTOs.Add(ConvertToDTO(tour));
                }
            }
        }
        private void ShowMoreButton_Click(object sender, RoutedEventArgs e)
        {
            ShowTour showTour = new ShowTour(SelectedGuest2TourDTO);
            showTour.Show();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }

        private void RateButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGuest2TourDTO != null)
            {
                RateTourViewModel rateTourViewModel = new RateTourViewModel(SelectedGuest2TourDTO, LoggedInUser, _tourRatingService, _tourReservationService, _tourService, _imageRepository);
                RateTour rateTour = new RateTour(rateTourViewModel);
                rateTour.Show();
            }
        }

        private void RequestButton_Click(object sender, RoutedEventArgs e)
        {
            RequestTourViewModel requestTourViewModel = new RequestTourViewModel(_userRepository, _locationService, _tourRequestService, LoggedInUser);
            RequestTour requestTour = new RequestTour(requestTourViewModel);
            requestTour.Show();
        }

        private void LanguageStatisicButton_Click(object sender, RoutedEventArgs e)
        {
            LanguageStatisticsViewModel languageStatisticsViewModel = new LanguageStatisticsViewModel(_tourRequestService);
            LanguageStatistics languageStatistics = new LanguageStatistics(languageStatisticsViewModel);
            languageStatistics.Show();
        }
    }
}