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

namespace InitialProject.View.Guest2
{
    public partial class Guest2Window : Window, INotifyPropertyChanged, IObserver, IDataErrorInfo
    {
        public User LoggedInUser { get; set; }

        public Guest2TourDTO SelectedGuest2TourDTO { get; set; }
        public ObservableCollection<Guest2TourDTO> TourDTOs { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }

        public ObservableCollection<Guest2TourDTO> FinishedTourDTOs { get; set; }
        public List<Tour> FinishedTours { get; set; }

        public Voucher SelectedVoucher { get; set; }
        public ObservableCollection<Voucher> Vouchers { get; set; }

        public List<Tour> CheckedTours { get; set; }
        public Tour CurrentlyActiveTour { get; set; }
        public Checkpoint CurrentlyActiveCheckpoint { get; set; }

        public ObservableCollection<Location> Locations;

        public int LanguageButtonClickCount { get; set; }
        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";

        public ObservableCollection<Guest2TourDTO> NonReservedTours { get; set; }

        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointService _checkpointService;
        private readonly UserRepository _userRepository;
        private readonly TourReservationService _tourReservationService;
        private readonly TourRatingService _tourRatingService;
        private readonly VoucherService _voucherService;

        #region Properties

        private string _personCount;
        public string PersonCount
        {
            get => _personCount;
            set
            {
                if (value != _personCount)
                {
                    _personCount = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _averageAge;
        public string AverageAge
        {
            get => _averageAge;
            set
            {
                if (value != _averageAge)
                {
                    _averageAge = value;
                    OnPropertyChanged();
                }
            }
        }

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

        private string duration;
        public string Duration
        {
            get => duration;
            set
            {
                if (value != duration)
                {
                    duration = value;
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

        private string guests;
        public string Guests
        {
            get => guests;
            set
            {
                if (value != guests)
                {
                    guests = value;
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

            Tours = new ObservableCollection<Tour>(_tourService.GetReservableTours());
            TourDTOs = ConvertToDTO(new List<Tour>(Tours));

            List<Voucher> UserVouchers = _voucherService.GetUserVouchers(LoggedInUser);
            Vouchers = new ObservableCollection<Voucher>(_voucherService.GetActiveVouchers(UserVouchers));

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

            ConfirmArrival();

            app = (App)Application.Current;
            app.ChangeLanguage(SRB);
            LanguageButtonClickCount = 0;
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

        #region Reservation
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (!IsSelectionValid())
            {
                MessageBox.Show("Please select a tour.", "Tour selection warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsPersonCountInputValid())
            {
                MessageBox.Show("Please enter the number of guests.", "Guest count warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsAverageAgeInputValid())
            {
                MessageBox.Show("Please enter the average age of guests.", "Average age warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsVoucherValid())
            {
                MessageBox.Show("Please state whether you want to use a voucher.", "Voucher warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            #endregion

            Tour selectedTour = _tourService.GetById(SelectedGuest2TourDTO.TourId);
            int personCount = int.Parse(PersonCount);
            int spacesLeft = selectedTour.MaxGuests - selectedTour.CurrentGuestCount;

            if (personCount > spacesLeft && selectedTour.CurrentGuestCount != selectedTour.MaxGuests)
            {
                ShowSpacesLeftMessage(spacesLeft);
                return;
            }

            if (selectedTour.CurrentGuestCount == selectedTour.MaxGuests)
            {
                HandleZeroSpacesForReservation(selectedTour);
                return;
            }

            SaveOrUpdateReservation(selectedTour, personCount);
            UpdateSelectedTour(selectedTour, personCount);

            PersonCountTB.Text = "";
            AverageAgeTB.Text = "";
        }

        private bool IsSelectionValid()
        {
            return SelectedGuest2TourDTO != null;
        }

        private bool IsPersonCountInputValid()
        {
            return PersonCount != null;
        }

        private bool IsAverageAgeInputValid()
        {
            return AverageAge != null;
        }

        private bool IsVoucherValid()
        {
            return SelectedVoucher != null ^ NoVoucherBtn.IsChecked == true;
        }

        private void ShowSpacesLeftMessage(int spacesLeft)
        {
            if (spacesLeft == 1)
                MessageBox.Show("You've tried adding too many guests. There is only 1 space left.");
            else
                MessageBox.Show("You've tried adding too many guests. There are only " + spacesLeft.ToString() + " spaces left.");
        }

        private void HandleZeroSpacesForReservation(Tour selectedTour)
        {
            var zeroSpacesForReservation = new ZeroSpacesForReservation(SelectedGuest2TourDTO, LoggedInUser, _tourService, _locationService, _userRepository, _voucherService);
            zeroSpacesForReservation.ShowDialog();
            Close();
        }

        private void SaveOrUpdateReservation(Tour selectedTour, int personCount)
        {
            int voucherId = -1;
            if (SelectedVoucher != null)
            {
                voucherId = SelectedVoucher.Id;
            }

            TourReservation tourReservation = new TourReservation(
                LoggedInUser.Id,
                SelectedGuest2TourDTO.TourId,
                personCount,
                double.Parse(AverageAge),
                voucherId);

            if (CheckIfReservationAlreadyExists(tourReservation))
            {
                UpdateExistingReservation(tourReservation);
            }
            else
            {
                SaveNewReservation(tourReservation);
            }

            MessageBox.Show("Reservation successful!");
        }

        private void UpdateExistingReservation(TourReservation tourReservation)
        {
            TourReservation existingReservation = _tourReservationService.GetReservationByGuestIdAndTourId(LoggedInUser.Id, SelectedGuest2TourDTO.TourId);
            tourReservation.Id = existingReservation.Id;
            tourReservation.PersonCount = existingReservation.PersonCount + int.Parse(PersonCount);
            tourReservation.AverageAge = (existingReservation.AverageAge + double.Parse(AverageAge)) / 2;

            if (existingReservation.UsedVoucherId == -1 && SelectedVoucher != null)
            {
                tourReservation.UsedVoucherId = SelectedVoucher.Id;
            }
            else
            {
                tourReservation.UsedVoucherId = existingReservation.UsedVoucherId;
            }

            _tourReservationService.Update(tourReservation);
        }

        private void SaveNewReservation(TourReservation tourReservation)
        {
            _tourReservationService.Save(tourReservation);

            if (tourReservation.UsedVoucherId != -1)
            {
                Voucher voucher = _voucherService.GetById(tourReservation.UsedVoucherId);
                voucher.IsActive = false;
                _voucherService.Update(voucher);
            }
        }

        private void UpdateSelectedTour(Tour selectedTour, int personCount)
        {
            selectedTour.CurrentGuestCount += personCount;
            _tourService.Update(selectedTour);
        }

        public bool CheckIfReservationAlreadyExists(TourReservation tourReservation)
        {
            foreach (TourReservation reservation in _tourReservationService.GetAll())
            {
                if (reservation.TourId == tourReservation.TourId && reservation.UserId == tourReservation.UserId)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Validation

        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                int TryParseNumber;
                if (columnName == "PersonCount")
                {
                    if(string.IsNullOrEmpty(PersonCount))
                        return "Number of guests is required";

                    if(!int.TryParse(PersonCount, out TryParseNumber))
                        return "This field should be a number";
                    else
                    {
                        if (int.Parse(PersonCount) <= 0)
                            return "Invalid value";
                    }
                }
                else if (columnName == "AverageAge")
                {
                    if (string.IsNullOrEmpty(AverageAge))
                        return "Average age of guests is required";

                    if (!int.TryParse(AverageAge, out TryParseNumber))
                        return "This field should be a number";
                    else
                    {
                        if (int.Parse(AverageAge) <= 0)
                            return "Invalid value";
                    }
                }
                return null;
            }
        }

        private readonly string[] _validatedProperties = { "PersonCount", "AverageAge" };

        #endregion

        #region Home tab

        private void TourReservationButtonClick(object sender, RoutedEventArgs e)
        {
            this.tab.SelectedIndex = 1;
        }

        private void TrackingButton_Click(object sender, RoutedEventArgs e)
        {
            this.tab.SelectedIndex = 2;
        }

        private void RatingButton_Click(object sender, RoutedEventArgs e)
        {
            this.tab.SelectedIndex = 3;
        }

        private void RequestingButton_Click(object sender, RoutedEventArgs e)
        {
            this.tab.SelectedIndex = 4;
        }

        private void ComplexRequestingButton_Click(object sender, RoutedEventArgs e)
        {
            this.tab.SelectedIndex = 5;
        }

        private void VouchersButton_Click(object sender, RoutedEventArgs e)
        {
            this.tab.SelectedIndex = 6;
        }

        #endregion

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
            if(!string.IsNullOrEmpty(Duration))
            {
                result = result.Intersect(_tourService.GetByDuration(int.Parse(Duration))).ToList();
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

        private void RateButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGuest2TourDTO == null)
            {
                MessageBox.Show("Please select a tour in order to rate it!", "Tour selection warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
                
            else
            { 
                RateTourViewModel rateTourViewModel = new RateTourViewModel(SelectedGuest2TourDTO, LoggedInUser, _tourRatingService, _tourReservationService, _tourService, _imageRepository);
                RateTour rateTour = new RateTour(rateTourViewModel);
                rateTour.Show();
            }
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }

        private void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            LanguageButtonClickCount++;

            if (LanguageButtonClickCount % 2 == 1)
            {
                app.ChangeLanguage(ENG);
                return;
            }

            app.ChangeLanguage(SRB);
        }
    }
}