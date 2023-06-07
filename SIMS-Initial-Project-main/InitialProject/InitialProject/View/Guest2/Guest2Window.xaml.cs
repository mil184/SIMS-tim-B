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
        public Checkpoint CurrentlyActiveCheckpoint1 { get; set; }
        public ObservableCollection<Checkpoint> ActiveTourCheckpoints { get; set; }

        public int LanguageButtonClickCount { get; set; }
        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";

        public ObservableCollection<Guest2TourDTO> NonReservedTours { get; set; }
        public ObservableCollection<TourRequest> TourRequests { get; set; }
        public ObservableCollection<TourRequest> TourRequestsForYear { get; set; }
        public ObservableCollection<Guest2TourRequestDTO> TourRequestDTOs { get; set; }
        public TourRequestDTOConverter TourRequestDTOConverter { get; set; }
        public ObservableCollection<string> TourRequestYears { get; set; }
        public string StatusStatistic { get; set; }
        public string GuestStatistic { get; set; }
        public string SelectedStatisticYear { get; set; }
        public string SelectedStatisticGuestYear { get; set; }
        public int AcceptedToursCount { get; set; }
        public int DeniedToursCount { get; set; }

        public ObservableCollection<ComplexTourRequestDTO> ComplexTourDTOs { get; set; }

        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointService _checkpointService;
        private readonly UserService _userService;
        private readonly TourReservationService _tourReservationService;
        private readonly TourRatingService _tourRatingService;
        private readonly VoucherService _voucherService;
        private readonly TourRequestService _tourRequestService;
        private readonly ComplexTourService _complexTourService;

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

            _userService = new UserService();
            _userService.Subscribe(this);

            _userService = new UserService();
            _userService.Subscribe(this);

            _tourReservationService = new TourReservationService();
            _tourReservationService.Subscribe(this);

            _tourRatingService = new TourRatingService();
            _tourRatingService.Subscribe(this);

            _voucherService = new VoucherService();
            _voucherService.Subscribe(this);

            _tourRequestService = new TourRequestService();
            _tourRequestService.Subscribe(this);

            _complexTourService = new ComplexTourService();
            _complexTourService.Subscribe(this);

            Tours = new ObservableCollection<Tour>(_tourService.GetReservableTours()
         .OrderByDescending(tour => _userService.GetById(tour.GuideId).SuperGuideLanguages.Count > 0 ? 1 : 0));

            TourDTOs = ConvertToDTO(new List<Tour>(Tours));

            List<Voucher> UserVouchers = _voucherService.GetUserVouchers(LoggedInUser);
            Vouchers = new ObservableCollection<Voucher>(_voucherService.GetActiveVouchers(UserVouchers));

            TourRequestDTOConverter = new TourRequestDTOConverter(_locationService);
            TourRequests = new ObservableCollection<TourRequest>(_tourRequestService.GetGuestRequests(LoggedInUser));
            TourRequestsForYear = new ObservableCollection<TourRequest>();
            TourRequestDTOs = new ObservableCollection<Guest2TourRequestDTO>(TourRequestDTOConverter.ConvertToDTOList(_tourRequestService.GetGuestRequests(LoggedInUser)));
            
            ComplexTourDTOs = new ObservableCollection<ComplexTourRequestDTO>(ComplexTourRequestDTOConverter.ConvertToDTOList(_complexTourService.GetAllByUser(LoggedInUser), _locationService, _tourRequestService));

            CheckedTours = new List<Tour>();
            foreach (int id in _tourReservationService.GetCheckedTourIds(LoggedInUser))
            {
                CheckedTours.Add(_tourService.GetById(id));
            }

            if (CheckedTours.Count != 0 && !CheckedTours[0].IsFinished)
            {
                CurrentlyActiveTour = CheckedTours[0];
                CurrentlyActiveCheckpoint = _checkpointService.GetById(CurrentlyActiveTour.CurrentCheckpointId);

                ActiveTourCheckpoints = new ObservableCollection<Checkpoint>();
                foreach (int id in CurrentlyActiveTour.CheckpointIds)
                {
                    ActiveTourCheckpoints.Add(_checkpointService.GetById(id));
                }
            }

            List<Tour> UserTours = new List<Tour>(_tourService.GetUserTours(LoggedInUser));
            FinishedTours = _tourService.GetFinishedTours(UserTours);
            FinishedTourDTOs = ConvertToDTO(FinishedTours);

            TourRequestYears = new ObservableCollection<string>();
            FormTourRequestYears();

            ConfirmArrival();

            app = (App)Application.Current;
            app.ChangeLanguage(SRB);
            LanguageButtonClickCount = 0;
            NotifyOnAcceptedRequest();
            _tourRequestService.UpdateInvalidRequests();

            NotifyOnAcceptedRequest();
            NotifyAcceptedLanguages();
            NotifyAcceptedLocations();

            AlterVoucherSectionVisibility();
            AlterTourTrackingVisibility();
            AlterComplexToursDataGridVisibility();

            foreach(var complexTour in _complexTourService.GetAllByUser(LoggedInUser))
            {
                _complexTourService.AlterStatus(complexTour);
            }
        }

        private void NotifyAcceptedLanguages()
        {
            List<TourRequest> requestsAcceptedLanguages = _tourRequestService.CheckForOthersAcceptedLanguage(LoggedInUser);

            if (requestsAcceptedLanguages.Count != 0)
            {
                List<string> languages = new List<string>();
                string message = "";

                if (app.Lang == ENG)
                {
                    message = "Others have acepted the language(s): ";
                }
                
                if (app.Lang == SRB)
                {
                    message = "Drugi su prihvatili jezik(e): ";
                }

                foreach (TourRequest req in requestsAcceptedLanguages)
                {
                    message += req.Language + ", ";
                    languages.Add(req.Language);
                }
                message = message.TrimEnd(',', ' ');

                bool flag = false;
                foreach (TourRequest req in _tourRequestService.GetInvalidandPendingRequests(LoggedInUser))
                {
                    if (languages.Contains(req.Language) && !req.SameDetailsMessageShownLanguage)
                    {
                        flag = true;
                        TourRequest request = req;
                        req.SameDetailsMessageShownLanguage = true;
                        _tourRequestService.Update(req);
                    }
                }
                if (flag)
                    MessageBox.Show(message);
            }
        }
        private void NotifyAcceptedLocations()
        {
            List<TourRequest> requestsAcceptedLocations = _tourRequestService.CheckForOthersAcceptedLocation(LoggedInUser);
            
            if (requestsAcceptedLocations.Count != 0)
            {
                List<int> locationIds = new List<int>();
                string message = "";

                if (app.Lang == ENG)
                {
                    message = "Others have acepted the location(s): ";
                }

                if (app.Lang == SRB)
                {
                    message = "Drugi su prihvatili lokaciju/e: ";
                }

                foreach (TourRequest req in requestsAcceptedLocations)
                {
                    message += _locationService.GetById(req.LocationId).City + ", " + _locationService.GetById(req.LocationId).Country + " ,also ";
                    locationIds.Add(req.LocationId);
                }
                int lastIndex = message.LastIndexOf("also");
                if (lastIndex >= 0)
                {
                    message = message.Substring(0, lastIndex) + message.Substring(lastIndex + "also".Length);
                }

                bool flag = false;
                foreach (TourRequest req in _tourRequestService.GetInvalidandPendingRequests(LoggedInUser))
                {
                    if (locationIds.Contains(req.LocationId) && !req.SameDetailsMessageShownLocation)
                    {
                        flag = true;
                        TourRequest request = req;
                        req.SameDetailsMessageShownLocation = true;
                        _tourRequestService.Update(req);
                    }
                }
                if (flag)
                    MessageBox.Show(message);
            }
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

            ComplexTourDTOs.Clear();
            FormComplexTourRequestDTOs();
        }

        public void NotifyOnAcceptedRequest()
        {
            foreach (TourRequest tourRequest in _tourRequestService.GetGuestRequests(LoggedInUser))
            {
                string location = _locationService.GetLocationStringbyId(tourRequest.LocationId);

                if (tourRequest.Status == InitialProject.Resources.Enums.RequestStatus.accepted && !tourRequest.MessageShown)
                {
                    GetAcceptedRequestMessageBox(location, tourRequest);
                    tourRequest.MessageShown = true;
                    _tourRequestService.Update(tourRequest);
                }
            }
        }

        private void GetAcceptedRequestMessageBox(string location, TourRequest tourRequest)
        {
            if (app.Lang == ENG)
            {
                MessageBox.Show("Your tour request in " + location + " has been accepted. Start time is " + tourRequest.ChosenDate);
                return;
            }

            MessageBox.Show("Tvoj zahtev na lokaciji " + location + " je prihvaćen. Početak je " + tourRequest.ChosenDate);
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

                MessageBoxResult result = new MessageBoxResult();

                if (app != null && app.Lang == ENG)
                {
                    result = MessageBox.Show("Please confirm your arrival at " + CheckedTours[0].Name, "Arrival Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);
                }

                if (app != null && app.Lang == SRB)
                {
                    result = MessageBox.Show("Potvrdite svoj dolazak na turi " + CheckedTours[0].Name, "Potvrda dolaska", MessageBoxButton.YesNo, MessageBoxImage.Information);
                }

                if (result == MessageBoxResult.Yes)
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
                ShowTourSelectionWarningMessage(app.Lang);
                return;
            }

            if (!IsPersonCountInputValid())
            {
                ShowGuestCountWarningMessage(app.Lang);
                return;
            }

            if (!IsAverageAgeInputValid())
            {
                ShowAverageAgeWarningMessage(app.Lang);
                return;
            }

            #endregion

            Tour selectedTour = _tourService.GetById(SelectedGuest2TourDTO.TourId);
            int personCount = int.Parse(PersonCount);
            int spacesLeft = selectedTour.MaxGuests - selectedTour.CurrentGuestCount;

            if (personCount > spacesLeft && selectedTour.CurrentGuestCount != selectedTour.MaxGuests)
            {
                ShowSpacesLeftMessage(spacesLeft, app.Lang);
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

            Update();
        }

        #region InvalidInputWarnings

        private void ShowTourSelectionWarningMessage(string lang)
        {
            if (lang == ENG)
            {
                MessageBox.Show("Please select a tour.", "Tour selection warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else if (lang == SRB)
            {
                MessageBox.Show("Molim Vas odaberite turu.", "Upozorenje o odabiru ture", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ShowGuestCountWarningMessage(string lang)
        {
            if (lang == ENG)
            {
                MessageBox.Show("Please enter the number of guests.", "Guest count warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else if (lang == SRB)
            {
                MessageBox.Show("Molim Vas unesite broj gostiju.", "Upozorenje o unosu broja gostiju", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ShowAverageAgeWarningMessage(string lang)
        {
            if (lang == ENG)
            {
                MessageBox.Show("Please enter the average age of guests.", "Average age warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else if (lang == SRB)
            {
                MessageBox.Show("Molim Vas unesite prosečne godine gostiju.", "Upozorenje o unosu prosečnih godina gostiju", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

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

        //private bool IsVoucherValid()
        //{
        //    return SelectedVoucher != null ^ NoVoucherBtn.IsChecked == true;
        //}

        private void ShowSpacesLeftMessage(int spacesLeft, string lang)
        {
            if (lang == ENG)
            {
                if (spacesLeft == 1)
                    MessageBox.Show("You've tried adding too many guests. There is only 1 space left.");
                else
                   MessageBox.Show("You've tried adding too many guests. There are only " + spacesLeft.ToString() + " spaces left.");
            }

            if (lang == SRB)
            {
                if (spacesLeft == 1)
                    MessageBox.Show("Pokušali ste da dodate previše gostiju. Ostalo je još jedno slobodno mesto.");
                else
                    MessageBox.Show("Pokušali ste da dodate previše gostiju. Ostalo je još " + spacesLeft.ToString() + " slobodnih mesta.");
            }

        }

        private void HandleZeroSpacesForReservation(Tour selectedTour)
        {
            ZeroSpacesForReservationViewModel zeroSpacesForReservationViewModel = new ZeroSpacesForReservationViewModel(SelectedGuest2TourDTO, _tourService, _locationService, _userService, app.Lang);
            ZeroSpacesForReservation zeroSpacesForReservation = new ZeroSpacesForReservation(zeroSpacesForReservationViewModel);
            zeroSpacesForReservation.Show();
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
                voucherId,
                System.DateTime.Now);

            if (CheckIfReservationAlreadyExists(tourReservation))
            {
                UpdateExistingReservation(tourReservation);
            }
            else
            {
                SaveNewReservation(tourReservation);
            }

            TourReservationMessage tourReservationMessage = new TourReservationMessage(tourReservation, LoggedInUser, _tourService, _locationService, _userService);
            tourReservationMessage.Show();

            
            Vouchers.Add(_tourReservationService.AcquireVoucher(tourReservation));

            Update();
        }

        private void ShowSuccessfulReservationMessage(string lang)
        {
            if (lang == ENG)
            {
                MessageBox.Show("Reservation successful!");
            }

            else if (lang == SRB)
            {
                MessageBox.Show("Uspešna rezervacija!");
            }
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
                    if (app.Lang == ENG)
                    {
                        if (string.IsNullOrEmpty(PersonCount))
                            return "Number of guests is required!";

                        if (!int.TryParse(PersonCount, out TryParseNumber))
                            return "This field should be a number!";
                        else
                        {
                            if (int.Parse(PersonCount) <= 0)
                                return "This number should be greater than zero!";
                        }
                    }

                    if (app.Lang == SRB)
                    {
                        if (string.IsNullOrEmpty(PersonCount))
                            return "Broj gostiju je neophodan!";

                        if (!int.TryParse(PersonCount, out TryParseNumber))
                            return "Ovo polje treba biti broj!";
                        else
                        {
                            if (int.Parse(PersonCount) <= 0)
                                return "Ovaj broj treba biti veći od nule!";
                        }
                    }
                }
                else if (columnName == "AverageAge")
                {
                    

                    if (app.Lang == ENG)
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

                    if (app.Lang == SRB)
                    {
                        if (string.IsNullOrEmpty(PersonCount))
                            return "Prosečne godine gostiju su neophodne!";

                        if (!int.TryParse(PersonCount, out TryParseNumber))
                            return "Ovo polje treba biti broj!";
                        else
                        {
                            if (int.Parse(PersonCount) <= 0)
                                return "Ovaj broj treba biti veći od nule!";
                        }
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

        public void FormTourRequestDTOs()
        {
            foreach(TourRequest tourRequest in _tourRequestService.GetGuestRequests(LoggedInUser))
            {
                TourRequestDTOs.Add(TourRequestDTOConverter.ConvertToDTO(tourRequest));
            }
        }

        public void FormTourRequestYears()
        {
            TourRequestYears.Clear();
            TourRequestYears.Add("Svih vremena");

            foreach (TourRequest tourRequest in _tourRequestService.GetGuestRequests(LoggedInUser))
            {
                string year = tourRequest.StartTime.Year.ToString();
                if (!TourRequestYears.Contains(year))
                {
                    TourRequestYears.Add(year);
                }
            }
        }

        public void FormComplexTourRequestDTOs()
        {
            foreach(var complexTour in _complexTourService.GetAllByUser(LoggedInUser))
            {
                ComplexTourDTOs.Add(ComplexTourRequestDTOConverter.ConvertToDTO(complexTour, _locationService, _tourRequestService));
            }
        }

        private void YearStatisticSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            List<TourRequest> tourRequestsForYear = _tourRequestService.GetByYear(LoggedInUser, SelectedStatisticYear);

            List<TourRequest> acceptedTours = _tourRequestService.GetStatusRequests(tourRequestsForYear, InitialProject.Resources.Enums.RequestStatus.accepted);
            List<TourRequest> deniedTours = _tourRequestService.GetStatusRequests(tourRequestsForYear, InitialProject.Resources.Enums.RequestStatus.invalid);

            AcceptedToursCount = acceptedTours.Count();
            DeniedToursCount = deniedTours.Count();

            if (tourRequestsForYear.Count() == 0)
            {
                if (app.Lang == ENG)
                {
                    StatusStatisticTB.Text = "There were no accepted tours in this year.";
                }

                if (app.Lang == SRB)
                {
                    StatusStatisticTB.Text = "Nema prihvaćenih tura u odabranoj godini.";
                }

                return;
            }

            int AcceptedPercentage = AcceptedToursCount * 100 / tourRequestsForYear.Count();
            int DeniedPercentage = DeniedToursCount * 100 / tourRequestsForYear.Count();

            if (app.Lang == ENG)
            {
                StatusStatisticTB.Text =
                    AcceptedPercentage + "% of your tours were accepted. \n" +
                    DeniedPercentage + "% of your tours were denied.";
            }

            if (app.Lang == SRB)
            {
                StatusStatisticTB.Text =
                    AcceptedPercentage + "% tvojih tura su prihvaćene. \n" +
                    DeniedPercentage + "% tvojih tura su odbijene.";
            }
        }

        private void GuestYearStatisticSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            List<TourRequest> tourRequestsForYear = _tourRequestService.GetByYear(LoggedInUser, SelectedStatisticGuestYear);
            List<TourRequest> acceptedTourRequestsForYear = _tourRequestService.GetStatusRequests(tourRequestsForYear, InitialProject.Resources.Enums.RequestStatus.accepted);

            double totalGuests = _tourRequestService.GetTotalGuestCountForYear(acceptedTourRequestsForYear);
            double totalTourRequests = acceptedTourRequestsForYear.Count();

            if (totalTourRequests==0)
            {       
                if (app.Lang == ENG)
                {
                    GuestStatisticTB.Text = "There were no accepted tours in this year.";
                }

                if (app.Lang == SRB)
                {
                    GuestStatisticTB.Text = "Nema prihvaćenih tura u odabranoj godini.";
                }

                return;
            }
            
            double averageGuests = (double)totalGuests / (double)totalTourRequests;

            if (app.Lang == ENG)
            {
                GuestStatisticTB.Text = "Average guests:" + averageGuests;
            }

            if (app.Lang == SRB)
            {
                GuestStatisticTB.Text = "Prosečan broj gostiju:" + averageGuests;
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
                    _userService.GetById(tour.GuideId).Username));
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
                _userService.GetById(tour.GuideId).Username);

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
                ShowUnselectedTourForRating(app.Lang);
                return;
            }
              
            RateTourViewModel rateTourViewModel = new RateTourViewModel(SelectedGuest2TourDTO, LoggedInUser, _tourRatingService, _tourReservationService, _tourService, _imageRepository, app.Lang);
            RateTour rateTour = new RateTour(rateTourViewModel);
            rateTour.Show();
        }

        private void ShowUnselectedTourForRating(string lang)
        {
            if (lang == ENG)
            {
                MessageBox.Show("Please select a tour in order to rate it!", "Tour selection warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else if (lang == SRB)
            {
                MessageBox.Show("Molim Vas odaberite turu koju želite da ocenite.", "Upozorenje o odabiru ture", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void RequestButton_Click(object sender, RoutedEventArgs e)
        {

            RequestTourViewModel requestTourViewModel = new RequestTourViewModel(_userService, _locationService, _tourRequestService, LoggedInUser, app.Lang);
            RequestTour requestTour = new RequestTour(requestTourViewModel);
            requestTour.Show();
        }

        private void LanguageStatisicButton_Click(object sender, RoutedEventArgs e)
        {
            LanguageStatisticsViewModel languageStatisticsViewModel = new LanguageStatisticsViewModel(_tourRequestService, app.Lang);
            LanguageStatistics languageStatistics = new LanguageStatistics(languageStatisticsViewModel);
            languageStatistics.Show();
        }

        private void LocationStatisicButton_Click(object sender, RoutedEventArgs e)
        {
            LocationStatisticsViewModel locationStatisticsViewModel = new LocationStatisticsViewModel(_tourRequestService, app.Lang);
            LocationStatistics locationStatistics = new LocationStatistics(locationStatisticsViewModel);
            locationStatistics.Show();
        }

        #region Information popups

        private void FinishedToursImage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            FinishedToursPopup.IsOpen = true;
        }

        private void FinishedToursImage_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            FinishedToursPopup.IsOpen = false;
        }

        private void ComplexTours_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ComplexToursPopup.IsOpen = true;
        }

        private void ComplexTours_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ComplexToursPopup.IsOpen = false;
        }

        private void FiltrationImage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            FiltrationPopup.IsOpen = true;
        }

        private void FiltrationImage_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            FiltrationPopup.IsOpen = false;
        }

        private void ToursImage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ToursPopup.IsOpen = true;
        }

        private void ToursImage_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ToursPopup.IsOpen = false;
        }

        private void VouchersImage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VouchersPopup.IsOpen = true;
        }

        private void VouchersImage_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VouchersPopup.IsOpen = false;
        }

        #endregion

        private void AlterVoucherSectionVisibility()
        {
            if (Vouchers.Count == 0)
            {
                VouchersDataGrid.Visibility = Visibility.Collapsed;
                NoVoucherLabel.Visibility = Visibility.Visible;
            }
        }

        private void AlterTourTrackingVisibility()
        {
            if (_tourService.GetActiveTours().Count == 0 || _tourService.GetAll().Count == 0)
            {
                NoTourActive.Visibility = Visibility.Visible;
                CurrentlyActiveTourColumn.Visibility = Visibility.Collapsed;
                TourTrackingColumn.Visibility = Visibility.Collapsed;
            }
        }

        private void AlterComplexToursDataGridVisibility()
        {
            if (_complexTourService.GetAllByUser(LoggedInUser).Count == 0)
            {
                NoComplexToursLabel.Visibility = Visibility.Visible;
                ComplexTourRequestsDataGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowCurrentlyActiveTourButton_Click(object sender, RoutedEventArgs e)
        {
            ShowTour showTour = new ShowTour(ConvertToDTO(CurrentlyActiveTour));
            showTour.Show();
        }

        private void ComplexWindow(object sender, RoutedEventArgs e)
        {
            ComplexTourRequestViewModel complexTourRequestViewModel = new ComplexTourRequestViewModel(_locationService, _tourRequestService, _complexTourService, LoggedInUser, app.Lang);
            ComplexTourRequest complexTourRequest = new ComplexTourRequest(complexTourRequestViewModel);
            complexTourRequest.Show();
        }

        
    }
}