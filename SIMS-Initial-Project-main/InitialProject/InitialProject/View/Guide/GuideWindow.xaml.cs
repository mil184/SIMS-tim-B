using InitialProject.Converters;
using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using InitialProject.Resources.UIHelper;
using InitialProject.Service;
using InitialProject.View.Guest2;
using InitialProject.ViewModel.Guide;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser.clipper;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace InitialProject.View.Guide
{
    public partial class GuideWindow : Window, INotifyPropertyChanged, IObserver
    {
        #region ContructorAndServices
        private readonly TourService _tourService;
        private readonly TourReservationService _tourReservationService;
        private readonly TourRatingService _tourRatingService;
        private readonly LocationService _locationService;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointService _checkpointService;
        private readonly UserService _userService;
        private readonly VoucherService _voucherService;
        private readonly TourRequestService _tourRequestService;
        private readonly ComplexTourService _complexTourService;
        public User CurrentUser { get; set; }
        public int CurrentTourSortIndex;
        public int CurrentRequestSortIndex;
        public int CurrentComplexTourSortIndex;
        public GuideWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            CurrentUser = user;

            _tourService = new TourService();
            _tourService.Subscribe(this);

            _imageRepository = new ImageRepository();
            _imageRepository.Subscribe(this);

            _locationService = new LocationService();
            _locationService.Subscribe(this);

            _checkpointService = new CheckpointService();
            _checkpointService.Subscribe(this);

            _tourReservationService = new TourReservationService();
            _tourReservationService.Subscribe(this);

            _userService = new UserService();
            _userService.Subscribe(this);

            _voucherService = new VoucherService();
            _voucherService.Subscribe(this);

            _tourRatingService = new TourRatingService();
            _tourRatingService.Subscribe(this);

            _tourRequestService = new TourRequestService();
            _tourRequestService.Subscribe(this);

            _complexTourService = new ComplexTourService();
            _complexTourService.Subscribe(this);

            InitializeCollections();
            InitializeStartingSearchValues();
            InitializeComboBoxes();
            InitializeShortcuts();
            FindActiveTour();

            RequestStartDateInput = null;
            RequestEndDateInput = null;

            CurrentTourSortIndex = 0;
            CurrentRequestSortIndex = 0;
            CurrentComplexTourSortIndex = 0;

        }
        private void InitializeCollections()
        {
            CurrentTours = new ObservableCollection<GuideTourDTO>(GuideDTOConverter.ConvertToDTO(_tourService.GetTodaysTours(CurrentUser), _locationService));
            UpcomingTours = new ObservableCollection<GuideTourDTO>(GuideDTOConverter.ConvertToDTO(_tourService.GetUpcomingTours(CurrentUser), _locationService));
            FinishedTours = new ObservableCollection<GuideTourDTO>(GuideDTOConverter.ConvertToDTO(_tourService.GetFinishedTours(CurrentUser), _locationService));
            RatedTours = new ObservableCollection<GuideTourDTO>(GuideDTOConverter.ConvertToDTO(_tourService.GetRatedTours(CurrentUser), _locationService));
            PendingRequests = new ObservableCollection<GuideRequestDTO>(GuideDTOConverter.ConvertToDTO(_tourRequestService.GetPendingRequests(CurrentUser), _locationService));
            ComplexTourRequests = new ObservableCollection<GuideComplexTourDTO>(GuideDTOConverter.ConvertToDTO(_complexTourService.GetAvailableComplexTours(CurrentUser), _tourRequestService, _locationService, _complexTourService));

            RequestCountries = new ObservableCollection<string>();
            RequestCities = new ObservableCollection<string>();

            StatisticsCountries = new ObservableCollection<string>();
            StatisticsCities = new ObservableCollection<string>();

            StatisticsYears = new ObservableCollection<string>();
            StatisticsMonths = new ObservableCollection<string>();

            RequestParameters = new RequestFilterParameters();
            RequestStatisticsParameters = new RequestFilterParameters();

            Messages = new ObservableCollection<GuideMessage>();

            List<string> currentLanguages = CurrentUser.SuperGuideLanguages;
            List<string> newLanguages = _userService.QualifiesForSuperGuide(CurrentUser);

            CurrentUser.SuperGuideLanguages = newLanguages;
            _userService.Update(CurrentUser);

            List<string> addedLanguages = _userService.GetAddedLanguages(currentLanguages, newLanguages);
            List<string> lostLanguages = _userService.GetLostLanguages(currentLanguages, newLanguages);

            foreach(string language in addedLanguages) 
            {
                Messages.Add(new GuideMessage(language, true));
            }
            foreach (string language in lostLanguages)
            {
                Messages.Add(new GuideMessage(language, false));
            }

        }

        private void InitializeComboBoxes()
        {
            Years_cb.Items.Add("Alltime");
            Years_cb.SelectedItem = Years_cb.Items[0];
            for (int i = 2000; i <= DateTime.Now.Year; i++)
            {
                Years_cb.Items.Add(i.ToString());
            }

            RequestCountries.Add(string.Empty);
            foreach (string country in _locationService.GetCountries())
            {
                RequestCountries.Add(country);
            }

            RequestCities.Add(string.Empty);
            foreach (string city in _locationService.GetCities())
            {
                RequestCities.Add(city);
            }

            StatisticsCountries.Add("-");
            foreach (string country in _locationService.GetCountries())
            {
                StatisticsCountries.Add(country);
            }

            StatisticsCities.Add("-");
            foreach (string city in _locationService.GetCities())
            {
                StatisticsCities.Add(city);
            }

            StatisticsYears.Add("Alltime");
            for (int i = 2000; i <= DateTime.Now.Year; i++)
            {
                StatisticsYears.Add(i.ToString());
            }

            StatisticsMonths.Add("-");
            StatisticsMonths.Add("JAN");
            StatisticsMonths.Add("FEB");
            StatisticsMonths.Add("MAR");
            StatisticsMonths.Add("APR");
            StatisticsMonths.Add("MAY");
            StatisticsMonths.Add("JUN");
            StatisticsMonths.Add("JUL");
            StatisticsMonths.Add("AUG");
            StatisticsMonths.Add("SEP");
            StatisticsMonths.Add("OCT");
            StatisticsMonths.Add("NOV");
            StatisticsMonths.Add("DEC");

            StatisticsYearInput = "Alltime";
            IsMonthClickable = false;
            StatisticsCityInput = "-";
            StatisticsCountryInput = "-";
            StatisticsLanguageInput = string.Empty;
        }
        private void InitializeStartingSearchValues()
        {
            MostVisited = GuideDTOConverter.ConvertToDTO(_tourService.GetMostVisitedTour(_tourService.GetFinishedTours(CurrentUser)), _locationService);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();

        }
        private bool ResignConfirmation() 
        {
            return MessageBoxResult.Yes ==  MessageBox.Show($"Are you sure you want to resign as a tour guide?", "Resign Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
        private void Resign() 
        {

            foreach (Tour tour in _tourService.GetUpcomingTours(CurrentUser))
            {
                List<int> addedUserIds = new List<int>();
                foreach (TourReservation reservation in _tourReservationService.GetReservationsByTourId(tour.Id))
                {
                    if (!addedUserIds.Contains(reservation.UserId))
                    {
                        bool hasVoucherByGuide = false;
                        foreach (Voucher voucher in _voucherService.GetUserVouchers(_userService.GetById(reservation.UserId)))
                        {
                            if (voucher.GuideId == CurrentUser.Id)
                            {
                                UpdateOldVoucher(voucher);
                                hasVoucherByGuide = true;
                            }

                        }
                        if (!hasVoucherByGuide)
                        {
                            AddNewVoucher(tour, reservation.UserId);
                        }
                        addedUserIds.Add(reservation.UserId);
                    }
                }
            }
                _tourService.AbortAllUpcomingTours(CurrentUser);
        }

        public void AddNewVoucher(Tour tour, int guestId) 
        {
            Voucher voucher = new Voucher(tour.Name, DateTime.Now, DateTime.Now.AddYears(2), guestId);
            voucher.GuideId = CurrentUser.Id;
            _voucherService.Save(voucher);
        }
        public void UpdateOldVoucher(Voucher voucher)
        {
            voucher.GuideId = -1;
            _voucherService.Update(voucher);
        }
        #endregion

        #region TodaysToursTab
        public ObservableCollection<GuideTourDTO> CurrentTours { get; set; }
        public GuideTourDTO SelectedCurrentTourDTO { get; set; }
        public bool TourActive { get; set; }

        private GuideTourDTO _activeTour;
        public GuideTourDTO ActiveTour
        {
            get { return _activeTour; }
            set
            {
                if (_activeTour != value)
                {
                    _activeTour = value;
                    OnPropertyChanged(nameof(ActiveTour));
                }
            }
        }
        private void FindActiveTour()
        {
            ActiveTour = null;
            foreach (GuideTourDTO tourdto in CurrentTours)
            {
                Tour tour = GuideDTOConverter.ConvertToTour(tourdto, _tourService);

                if (tour.IsActive)
                {
                    ActiveTour = GuideDTOConverter.ConvertToDTO(tour, _locationService);
                    break;
                }
            }
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTourViewModel createTourViewModel = new CreateTourViewModel(CurrentUser, _tourService, _locationService, _imageRepository, _checkpointService, _tourRequestService, null, null, _complexTourService, null, null);
            CreateTourWindow createTour = new CreateTourWindow(createTourViewModel);
            createTour.ShowDialog();
        }
        private void CurrentToursDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectTodaysTour();
        }

        private void SelectTodaysTour()
        {
            CheckIfTourIsActive();
            Tour selectedTour = GuideDTOConverter.ConvertToTour(SelectedCurrentTourDTO, _tourService);

            if (selectedTour != null)
            {
                HandleSelectedTour(selectedTour);
            }
        }
        private void CheckIfTourIsActive()
        {
            TourActive = false;
            ActiveTour = null;

            foreach (Tour tour in _tourService.GetTodaysTours(CurrentUser))
            {
                if (tour.IsActive)
                {
                    TourActive = true;
                    ActiveTour = GuideDTOConverter.ConvertToDTO(tour, _locationService);
                    break;
                }
            }
        }
        private void HandleSelectedTour(Tour selectedTour)
        {
            if (selectedTour.IsActive)
            {
                ActiveTour = GuideDTOConverter.ConvertToDTO(selectedTour, _locationService);
                ShowCheckpointsViewModel showCheckpointsViewModel = new ShowCheckpointsViewModel(selectedTour, _checkpointService, _tourService, _tourReservationService, _userService);
                ShowCheckpoints showCheckpoints = new ShowCheckpoints(showCheckpointsViewModel);
                showCheckpoints.ShowDialog();
            }
            else if (TourActive)
            {
                ShowActiveTourWarning();
            }
            else
            {
                StartTourConfirmation(selectedTour);
            }
        }
        private void StartTourConfirmation(Tour selectedTour)
        {
            if (ConfirmStartTour(selectedTour))
            {
                StartSelectedTour(selectedTour);
            }
        }
        private void StartSelectedTour(Tour selectedTour)
        {
            SetTourActive(selectedTour);
            SetCurrentCheckpoint(selectedTour);
            UpdateTour(selectedTour);
            ShowCheckpointsForTour(selectedTour);
        }
        private void SetTourActive(Tour tour)
        {
            tour.IsActive = true;
            TourActive = true;
        }
        private void SetCurrentCheckpoint(Tour tour)
        {
            tour.CurrentCheckpointId = tour.CheckpointIds.First();
        }
        private void UpdateTour(Tour tour)
        {
            _tourService.Update(tour);
            ActiveTour = GuideDTOConverter.ConvertToDTO(tour, _locationService);
        }
        private void ShowCheckpointsForTour(Tour tour)
        {
            ShowCheckpointsViewModel showCheckpointsViewModel = new ShowCheckpointsViewModel(tour, _checkpointService, _tourService, _tourReservationService, _userService);
            ShowCheckpoints showCheckpoints = new ShowCheckpoints(showCheckpointsViewModel);
            showCheckpoints.ShowDialog();
        }
        private bool ConfirmStartTour(Tour selectedTour)
        {
            var messageBoxResult = MessageBox.Show($"Are you sure you want to start the {selectedTour.Name} tour?", "Start Tour Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return messageBoxResult == MessageBoxResult.Yes;
        }
        private void StartTourButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCurrentTourDTO != null)
                SelectTodaysTour();
        }
        private void ShowActiveTourWarning()
        {
            MessageBox.Show("An active tour is already in progress. Please finish the current tour before starting a new one.", "Active Tour Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private string _currentToursSearchInput;
        public string CurrentToursSearchInput
        {
            get => _currentToursSearchInput;
            set
            {
                if (value != _currentToursSearchInput)
                {
                    _currentToursSearchInput = value;
                    OnPropertyChanged();
                }
            }
        }
        private void CurrentToursSearchText_Changed(object sender, TextChangedEventArgs e)
        {
            CurrentTours.Clear();
            if (!string.IsNullOrEmpty(CurrentToursSearchInput)) 
            {
                foreach(Tour tour in _tourService.FilterTours(CurrentToursSearchInput, _tourService.GetTodaysTours(CurrentUser))) 
                {
                    CurrentTours.Add(GuideDTOConverter.ConvertToDTO(tour, _locationService));
                }
            }
            else 
            {
                foreach (Tour tour in _tourService.GetTodaysTours(CurrentUser))
                {
                    CurrentTours.Add(GuideDTOConverter.ConvertToDTO(tour, _locationService));
                }
            }
        }
        #endregion

        #region UpcomingToursTab
        public ObservableCollection<GuideTourDTO> UpcomingTours { get; set; }
        public GuideTourDTO SelectedUpcomingTourDTO { get; set; }
        private void UpcomingToursDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectUpcomingTour();
        }

        private void SelectUpcomingTour()
        {
            List<int> vouchersAdded = new List<int>();
            Tour tour = GuideDTOConverter.ConvertToTour(SelectedUpcomingTourDTO, _tourService);

            if (!_tourService.CheckIfTourCanBeAborted(tour))
            {
                ShowAbortTourWarning();
                return;
            }

            if (ConfirmAbortTour(tour))
            {
                AddVouchersToUsers(tour, vouchersAdded);
                AbortTour(tour);
            }
        }
        private bool ConfirmAbortTour(Tour selectedTour)
        {
            var messageBoxResult = MessageBox.Show($"Are you sure you want to abort the {selectedTour.Name} tour?", "Abort Tour Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return messageBoxResult == MessageBoxResult.Yes;
        }
        private void AddVouchersToUsers(Tour tour, List<int> vouchersAdded)
        {
            foreach (int userId in _tourReservationService.GetUncheckedUserIdsByTour(tour))
            {
                if (!vouchersAdded.Contains(userId))
                {
                    vouchersAdded.Add(userId);
                    Voucher voucher = new Voucher(tour.Name, DateTime.Now, DateTime.Now.AddYears(1), userId);
                    _voucherService.Save(voucher);
                }
            }
        }
        private void AbortTour(Tour tour)
        {
            tour.IsAborted = true;
            _tourService.Update(tour);
        }
        private void AbortButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUpcomingTourDTO != null)
                SelectUpcomingTour();
        }
        private void ShowAbortTourWarning()
        {
            MessageBox.Show("You may not abort this tour since it starts in less than two days. You may only abort tours that are more than two days away.", "Abort Tour Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            IntervalChooserViewModel viewModel = new IntervalChooserViewModel(_tourService, _locationService, CurrentUser);
            IntervalChooser intervalChooser = new IntervalChooser(viewModel);
            intervalChooser.ShowDialog();
        }
        private string _upcomingToursSearchInput;
        public string UpcomingToursSearchInput
        {
            get => _upcomingToursSearchInput;
            set
            {
                if (value != _upcomingToursSearchInput)
                {
                    _upcomingToursSearchInput = value;
                    OnPropertyChanged();
                }
            }
        }
        private void UpcomingToursSearchText_Changed(object sender, TextChangedEventArgs e)
        {
            UpcomingTours.Clear();
            if (!string.IsNullOrEmpty(UpcomingToursSearchInput))
            {
                foreach (Tour tour in _tourService.FilterTours(UpcomingToursSearchInput, _tourService.GetUpcomingTours(CurrentUser)))
                {
                    UpcomingTours.Add(GuideDTOConverter.ConvertToDTO(tour, _locationService));
                }
            }
            else
            {
                foreach (Tour tour in _tourService.GetUpcomingTours(CurrentUser))
                {
                    UpcomingTours.Add(GuideDTOConverter.ConvertToDTO(tour, _locationService));
                }
            }
        }
        #endregion

        #region FinishedToursTab
        public ObservableCollection<GuideTourDTO> FinishedTours { get; set; }
        public GuideTourDTO SelectedFinishedTourDTO { get; set; }
        private GuideTourDTO _mostVisited;
        public GuideTourDTO MostVisited
        {
            get { return _mostVisited; }
            set
            {
                if (_mostVisited != value)
                {
                    _mostVisited = value;
                    OnPropertyChanged(nameof(MostVisited));
                }
            }
        }
        private string _years;
        public string Years
        {
            get => _years;
            set
            {
                if (value != _years)
                {
                    _years = value;
                    OnPropertyChanged();
                }
            }
        }

        private void Years_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Years_cb.SelectedItem != null)
            {
                HandleSelection();
            }
        }

        public void HandleSelection()
        {
            if (int.TryParse(Years_cb.SelectedItem.ToString(), out int year))
            {
                HandleYearSelection(year);
            }
            else if (Years_cb.SelectedItem.ToString() == "Alltime")
            {
                HandleAllTimeSelection();
            }
        }
        private void HandleYearSelection(int year)
        {
            List<Tour> toursByYear = _tourService.GetToursByYear(CurrentUser, year);
            if (toursByYear.Count == 0)
            {
                MostVisited = new GuideTourDTO { Name = "No information", Location = "", NumberOfGuestsMessage = "" };
            }
            else
            {
                MostVisited = GuideDTOConverter.ConvertToDTO(_tourService.GetMostVisitedTour(toursByYear), _locationService);
            }
        }
        private void HandleAllTimeSelection()
        {
            MostVisited = GuideDTOConverter.ConvertToDTO(_tourService.GetMostVisitedTour(_tourService.GetFinishedTours(CurrentUser)), _locationService);
        }

        private void FinishedToursDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectFinishedTour();
        }
        private void SelectFinishedTour()
        {
            Tour selectedTour = GuideDTOConverter.ConvertToTour(SelectedFinishedTourDTO, _tourService);
            if (selectedTour != null)
            {
                StatisticsViewModel statisticsViewModel = new StatisticsViewModel(selectedTour, _tourReservationService);
                Statistics statistics = new Statistics(statisticsViewModel);
                statistics.Show();
            }
        }
        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedFinishedTourDTO != null)
                SelectFinishedTour();
        }
        private string _finishedToursSearchInput;
        public string FinishedToursSearchInput
        {
            get => _finishedToursSearchInput;
            set
            {
                if (value != _finishedToursSearchInput)
                {
                    _finishedToursSearchInput = value;
                    OnPropertyChanged();
                }
            }
        }
        private void FinishedToursSearchText_Changed(object sender, TextChangedEventArgs e)
        {
            FinishedTours.Clear();
            if (!string.IsNullOrEmpty(FinishedToursSearchInput))
            {
                foreach (Tour tour in _tourService.FilterTours(FinishedToursSearchInput, _tourService.GetFinishedTours(CurrentUser)))
                {
                    FinishedTours.Add(GuideDTOConverter.ConvertToDTO(tour, _locationService));
                }
            }
            else
            {
                foreach (Tour tour in _tourService.GetFinishedTours(CurrentUser))
                {
                    FinishedTours.Add(GuideDTOConverter.ConvertToDTO(tour, _locationService));
                }
            }
        }
        #endregion

        #region RatedToursTab
        public ObservableCollection<GuideTourDTO> RatedTours { get; set; }
        public GuideTourDTO SelectedRatedTourDTO { get; set; }
        private void RatedToursDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectRatedTour();
        }
        private void SelectRatedTour()
        {
            RatingsViewModel ratingsViewModel = new RatingsViewModel(_userService, _tourRatingService, _tourReservationService, _checkpointService, GuideDTOConverter.ConvertToTour(SelectedRatedTourDTO, _tourService));
            Ratings ratings = new Ratings(ratingsViewModel);
            ratings.Show();
        }
        private void RatingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRatedTourDTO != null)
                SelectRatedTour();
        }
        private string _ratedToursSearchInput;
        public string RatedToursSearchInput
        {
            get => _ratedToursSearchInput;
            set
            {
                if (value != _ratedToursSearchInput)
                {
                    _ratedToursSearchInput = value;
                    OnPropertyChanged();
                }
            }
        }
        private void RatedToursSearchText_Changed(object sender, TextChangedEventArgs e)
        {
            RatedTours.Clear();
            if (!string.IsNullOrEmpty(RatedToursSearchInput))
            {
                foreach (Tour tour in _tourService.FilterTours(RatedToursSearchInput, _tourService.GetRatedTours(CurrentUser)))
                {
                    RatedTours.Add(GuideDTOConverter.ConvertToDTO(tour, _locationService));
                }
            }
            else
            {
                foreach (Tour tour in _tourService.GetRatedTours(CurrentUser))
                {
                    RatedTours.Add(GuideDTOConverter.ConvertToDTO(tour, _locationService));
                }
            }
        }
        #endregion

        #region RequestsTab
        public ObservableCollection<GuideRequestDTO> PendingRequests { get; set; }
        public GuideRequestDTO SelectedPendingRequestDTO { get; set; }
        public ObservableCollection<string> RequestCountries { get; set; }
        public ObservableCollection<string> RequestCities { get; set; }

        public RequestFilterParameters RequestParameters { get; set; }

        private string _requestCity;
        public string RequestCityInput
        {
            get => _requestCity;
            set
            {
                if (value != _requestCity)
                {
                    _requestCity = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _requestCountry;
        public string RequestCountryInput
        {
            get => _requestCountry;
            set
            {
                if (value != _requestCountry)
                {
                    _requestCountry = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _requestLanguage;
        public string RequestLanguageInput
        {
            get => _requestLanguage;
            set
            {
                if (value != _requestLanguage)
                {
                    _requestLanguage = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _requestMaxGuests;
        public string RequestMaxGuestsInput
        {
            get => _requestMaxGuests;
            set
            {
                if (value != _requestMaxGuests)
                {
                    _requestMaxGuests = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime? _requestStartDateInput;
        public DateTime? RequestStartDateInput
        {
            get => _requestStartDateInput;
            set
            {
                if (value != _requestStartDateInput)
                {
                    _requestStartDateInput = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime? _requestEndDateInput;
        public DateTime? RequestEndDateInput
        {
            get => _requestEndDateInput;
            set
            {
                if (value != _requestEndDateInput)
                {
                    _requestEndDateInput = value;
                    OnPropertyChanged();
                }
            }
        }
 
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(RequestCityInput))
            RequestCountryInput = _locationService.GetCountryByCity(RequestCityInput);

            if (!_locationService.GetCitiesByCountry(RequestCountryInput).Contains(RequestCityInput))
                RequestCityInput = string.Empty;

            UpdateSearchParameters();
            UpdateRequests();
        }

        private void Text_Changed(object sender, TextChangedEventArgs e)
        {
            UpdateSearchParameters();
           UpdateRequests();
        }
        private void Date_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSearchParameters();
            UpdateRequests();
        }
        private void UpdateSearchParameters()
        {
            int? maxGuests = null;

            if (int.TryParse(RequestMaxGuestsInput, out int parsedNumber)) 
            {
                maxGuests = parsedNumber;
            }

            RequestParameters.Language = RequestLanguageInput;
            RequestParameters.StartDate = RequestStartDateInput;
            RequestParameters.EndDate = RequestEndDateInput;
            RequestParameters.MaxGuests = maxGuests;
            RequestParameters.City = RequestCityInput;
            RequestParameters.Country = RequestCountryInput;
        }
        private void UpdateRequests()
        {

            PendingRequests.Clear();

            List<TourRequest> result = _tourRequestService.Filter(RequestParameters, CurrentUser);

            List<GuideRequestDTO> searchResults = GuideDTOConverter.ConvertToDTO(result, _locationService);

            foreach (GuideRequestDTO dto in searchResults)
            {
                PendingRequests.Add(dto);
            }

        }
        private void PendingRequests_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CreateTourBasedOnRequest();
        }

        private void CreateTourBasedOnRequest() 
        {
            if (SelectedPendingRequestDTO != null)
            {
                TourRequest request = GuideDTOConverter.ConvertToRequest(SelectedPendingRequestDTO, _tourRequestService);
                CreateTourViewModel createTourViewModel = new CreateTourViewModel(CurrentUser, _tourService, _locationService, _imageRepository, _checkpointService, _tourRequestService, request, null, _complexTourService, null, null);
                CreateTourWindow createTour = new CreateTourWindow(createTourViewModel);
                createTour.ShowDialog();
            }
        }
        private string _pendingRequestsSearchInput;
        public string PendingRequestsSearchInput
        {
            get => _pendingRequestsSearchInput;
            set
            {
                if (value != _pendingRequestsSearchInput)
                {
                    _pendingRequestsSearchInput = value;
                    OnPropertyChanged();
                }
            }
        }
        private void PendingRequestsSearchText_Changed(object sender, TextChangedEventArgs e)
        {
            PendingRequests.Clear();
            if (!string.IsNullOrEmpty(PendingRequestsSearchInput))
            {
                foreach (TourRequest tourRequest in _tourRequestService.Search(PendingRequestsSearchInput, _tourRequestService.GetPendingRequests(CurrentUser)))
                {
                    PendingRequests.Add(GuideDTOConverter.ConvertToDTO(tourRequest, _locationService));
                }
            }
            else
            {
                foreach (TourRequest tourRequest in _tourRequestService.GetPendingRequests(CurrentUser))
                {
                    PendingRequests.Add(GuideDTOConverter.ConvertToDTO(tourRequest, _locationService));
                }
            }
        }
        #endregion

        #region CompexTours
        public ObservableCollection<GuideComplexTourDTO> ComplexTourRequests { get; set; }
        public GuideComplexTourDTO SelectedComplexTourDTO { get; set; }

        private void ComplexTourRequestsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenComplexTourWindow();
        }

        public void OpenComplexTourWindow() 
        {
            if (SelectedComplexTourDTO != null)
            {
                ShowComplexTourViewModel showComplexTourViewModel = new ShowComplexTourViewModel(GuideDTOConverter.ConvertToComplexTour(SelectedComplexTourDTO, _complexTourService), _tourRequestService, _locationService, _complexTourService, CurrentUser, _checkpointService, _imageRepository, _tourService);
                ShowComplexTour showComplexTour = new ShowComplexTour(showComplexTourViewModel);
                showComplexTour.ShowDialog();
            }
        }
        private string _complexToursSearchInput;
        public string ComplexToursSearchInput
        {
            get => _complexToursSearchInput;
            set
            {
                if (value != _complexToursSearchInput)
                {
                    _complexToursSearchInput = value;
                    OnPropertyChanged();
                }
            }
        }
        private void ComplexTourRequestsSearchText_Changed(object sender, TextChangedEventArgs e)
        {
            if (ComplexTourRequests == null) return;
            ComplexTourRequests.Clear();
            if (!string.IsNullOrEmpty(ComplexToursSearchInput))
            {
                foreach (ComplexTour tour in _complexTourService.FilterTours(ComplexToursSearchInput, _complexTourService.GetAvailableComplexTours(CurrentUser)))
                {
                    ComplexTourRequests.Add(GuideDTOConverter.ConvertToDTO(tour, _tourRequestService, _locationService, _complexTourService));
                }
            }
            else
            {
                foreach (ComplexTour tour in _complexTourService.GetAvailableComplexTours(CurrentUser))
                {
                    ComplexTourRequests.Add(GuideDTOConverter.ConvertToDTO(tour, _tourRequestService, _locationService, _complexTourService));
                }
            }
        }
        #endregion

        #region RequestStatisticsTab
        public ObservableCollection<string> StatisticsCountries { get; set; }
        public ObservableCollection<string> StatisticsCities { get; set; }

        public ObservableCollection<string> StatisticsYears { get; set; }
        public ObservableCollection<string> StatisticsMonths { get; set; }

        public RequestFilterParameters RequestStatisticsParameters { get; set; }

        private int _locationRequestsCount;
        public int LocationRequestsCount
        {
            get { return _locationRequestsCount; }
            set
            {
                if (_locationRequestsCount != value)
                {
                    _locationRequestsCount = value;
                    OnPropertyChanged(nameof(LocationRequestsCount));
                }
            }
        }
        private int _languageRequestsCount;
        public int LanguageRequestsCount
        {
            get { return _languageRequestsCount; }
            set
            {
                if (_languageRequestsCount != value)
                {
                    _languageRequestsCount = value;
                    OnPropertyChanged(nameof(LanguageRequestsCount));
                }
            }
        }
        private int _languageLocationRequestsCount;
        public int LanguageLocationRequestsCount
        {
            get { return _languageLocationRequestsCount; }
            set
            {
                if (_languageLocationRequestsCount != value)
                {
                    _languageLocationRequestsCount = value;
                    OnPropertyChanged(nameof(LanguageLocationRequestsCount));
                }
            }
        }

 
        private string _statisticsCity;
        public string StatisticsCityInput
        {
            get => _statisticsCity;
            set
            {
                if (value != _statisticsCity)
                {
                    _statisticsCity = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _statisticsCountry;
        public string StatisticsCountryInput
        {
            get => _statisticsCountry;
            set
            {
                if (value != _statisticsCountry)
                {
                    _statisticsCountry = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _statisticsLanguage;
        public string StatisticsLanguageInput
        {
            get => _statisticsLanguage;
            set
            {
                if (value != _statisticsLanguage)
                {
                    _statisticsLanguage = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _statisticsYear;
        public string StatisticsYearInput
        {
            get => _statisticsYear;
            set
            {
                if (value != _statisticsYear)
                {
                    _statisticsYear = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _statisticsMonth;
        public string StatisticsMonthInput
        {
            get => _statisticsMonth;
            set
            {
                if (value != _statisticsMonth)
                {
                    _statisticsMonth = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isMonthClickable;
        public bool IsMonthClickable
        {
            get => _isMonthClickable;
            set
            {
                if (value != _isMonthClickable)
                {
                    _isMonthClickable = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _languageR;
        public string LanguageR
        {
            get => _languageR;
            set
            {
                if (value != _languageR)
                {
                    _languageR = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _location;
        public string Location
        {
            get => _location;
            set
            {
                if (value != _location)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _languageAndLocation;
        public string LanguageAndLocation
        {
            get => _languageAndLocation;
            set
            {
                if (value != _languageAndLocation)
                {
                    _languageAndLocation = value;
                    OnPropertyChanged();
                }
            }
        }
        private void cbStatisticsYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatisticsYearInput == "Alltime")
            {
                IsMonthClickable = false;
                StatisticsMonthInput = "-";
            }
            else
                IsMonthClickable = true;


            UpdateGridCounts();
        }

        private void cbStatisticsMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGridCounts();
        }

        private void cbStatisticsCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatisticsCityInput != "-")
                StatisticsCountryInput = _locationService.GetCountryByCity(StatisticsCityInput);

            UpdateGridCounts();
        }

        private void cbStatisticsCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (!_locationService.GetCitiesByCountry(StatisticsCountryInput).Contains(StatisticsCityInput))
                StatisticsCityInput = "-";

            UpdateGridCounts();

        }

        private void cbStatisticsLanguage_SelectionChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGridCounts();
        }

        private void UpdateGridCounts()
        {

            RequestStatisticsParameters.Country = StatisticsCountryInput == "-" ? null : StatisticsCountryInput;
            RequestStatisticsParameters.City = StatisticsCityInput == "-" ? null : StatisticsCityInput;
            RequestStatisticsParameters.Language = StatisticsLanguageInput == "-" ? null : StatisticsLanguageInput;


            RequestStatisticsParameters.StartDate = GetStartDate();
            RequestStatisticsParameters.EndDate = GetEndDate();

            //string x = RequestStatisticsParameters.Country + "\n" + RequestStatisticsParameters.City + "\n" + RequestStatisticsParameters.Language + "\n" + RequestStatisticsParameters.StartDate.Value.ToString() + "\n" + RequestStatisticsParameters.EndDate.Value.ToString();

            //MessageBox.Show(x);

            RequestFilterParameters paramsLanguage = new RequestFilterParameters(null, null, RequestStatisticsParameters.Language, null, RequestStatisticsParameters.StartDate, RequestStatisticsParameters.EndDate);
            RequestFilterParameters paramsLocation = new RequestFilterParameters(RequestStatisticsParameters.Country, RequestStatisticsParameters.City , null, null, RequestStatisticsParameters.StartDate, RequestStatisticsParameters.EndDate);
            RequestFilterParameters paramsBoth = new RequestFilterParameters(RequestStatisticsParameters.Country, RequestStatisticsParameters.City, RequestStatisticsParameters.Language, null, RequestStatisticsParameters.StartDate, RequestStatisticsParameters.EndDate);

            LanguageRequestsCount = _tourRequestService.Filter(paramsLanguage, null).Count; 
            LocationRequestsCount = _tourRequestService.Filter(paramsLocation, null).Count;
            LanguageLocationRequestsCount = _tourRequestService.Filter(paramsBoth, null).Count;

            //MessageBox.Show(LanguageRequestsCount.ToString() + "\n" + LocationRequestsCount.ToString() + "\n" + LanguageLocationRequestsCount.ToString());
        }
        private DateTime? GetStartDate() 
        {

            if(StatisticsYearInput == "Alltime") 
            {
                return new DateTime(1, 1, 1, 0, 0, 0);
            }
            if(StatisticsYearInput != "Alltime" && StatisticsMonthInput == "-") 
            {
                return new DateTime(int.Parse(StatisticsYearInput), 1, 1, 0, 0, 0);
            }
            if (StatisticsYearInput != "Alltime" && StatisticsMonthInput != "-")
            {
                return new DateTime(int.Parse(StatisticsYearInput), GetMonthValue().Value, 1, 0, 0, 0);
            }

            return null;
        }
        private DateTime? GetEndDate()
        {

            if (StatisticsYearInput == "Alltime")
            {
                return new DateTime(3000, 12, 31, 23, 59, 59);
            }
            if (StatisticsYearInput != "Alltime" && StatisticsMonthInput == "-")
            {
                return new DateTime(int.Parse(StatisticsYearInput), 12, 31, 23, 59, 59);
            }
            if (StatisticsYearInput != "Alltime" && StatisticsMonthInput != "-")
            {
                return new DateTime(int.Parse(StatisticsYearInput), GetMonthValue().Value, DateTime.DaysInMonth(int.Parse(StatisticsYearInput), GetMonthValue().Value), 23, 59, 59);
            }

            return null;
        }
        private int? GetMonthValue()
        {
            switch (StatisticsMonthInput)
            {
                case "JAN":
                    return 1;
                case "FEB":
                    return 2;
                case "MAR":
                    return 3;
                case "APR":
                    return 4;
                case "MAY":
                    return 5;
                case "JUN":
                    return 6;
                case "JUL":
                    return 7;
                case "AUG":
                    return 8;
                case "SEP":
                    return 9;
                case "OCT":
                    return 10;
                case "NOV":
                    return 11;
                case "DEC":
                    return 12;
                default:
                    return null;
            }
        }
        private int GetYearValue()
        {
            if (StatisticsYearInput == "Alltime")
                return -1;
            return int.Parse(StatisticsYearInput);
        }
        private string GetCityValue()
        {
            if (StatisticsCityInput != "-")
                return StatisticsCityInput;

            return "/";

        }
        private string GetCountryValue()
        {
            if (StatisticsCountryInput != "-")
                return StatisticsCountryInput;

            return "/";

        }
        private string GetLanguageValue()
        {
            if (!string.IsNullOrEmpty(StatisticsLanguageInput))
                return StatisticsLanguageInput;

            return "/";

        }
        #endregion

        #region Shortcuts
        private void InitializeShortcuts()
        {
            PreviewKeyDown += CreateTour_PreviewKeyDown;
            PreviewKeyDown += LogOut_PreviewKeyDown;
            PreviewKeyDown += Enter_PreviewKeyDown;
            PreviewKeyDown += LeftRightArrowKeys_PreviewKeyDown;
            PreviewKeyDown += DataGrid_PreviewKeyDown;
            PreviewKeyDown += SortAsc_PreviewKeyDown;
            PreviewKeyDown += SortDesc_PreviewKeyDown;
            PreviewKeyDown += ReportWindow_PreviewKeyDown;
            PreviewKeyDown += Resign_PreviewKeyUp;
        }
        private void LogOut_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.SystemKey == Key.F4 && e.KeyboardDevice.Modifiers == ModifierKeys.Alt)
            {
                SignInForm signInForm = new SignInForm();
                signInForm.Show();
                Close();
            }
        }
        private void ReportWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.R)
            {
                IntervalChooserViewModel viewModel = new IntervalChooserViewModel(_tourService, _locationService, CurrentUser);
                IntervalChooser intervalChooser = new IntervalChooser(viewModel);
                intervalChooser.ShowDialog();
                e.Handled = true;
            }
        }
        private void CreateTour_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.A)
            {
                CreateTourViewModel createTourViewModel = new CreateTourViewModel(CurrentUser, _tourService, _locationService, _imageRepository, _checkpointService, _tourRequestService, null, null, _complexTourService, null, null);
                CreateTourWindow createTour = new CreateTourWindow(createTourViewModel);
                createTour.ShowDialog();
                e.Handled = true;
            }
        }
        private void Resign_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.SystemKey == Key.Q && e.KeyboardDevice.Modifiers == ModifierKeys.Alt)
            {
                if (ResignConfirmation()) Resign();
                e.Handled = true;
            }
        }
        private void Enter_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                switch (tabControl.SelectedIndex)
                {
                    case 0:
                        if (SelectedCurrentTourDTO != null)
                            SelectTodaysTour();
                        break;
                    case 1:
                        if (SelectedUpcomingTourDTO != null)
                            SelectUpcomingTour();
                        break;
                    case 2:
                        if (SelectedFinishedTourDTO != null)
                            SelectFinishedTour();
                        break;
                    case 3:
                        if (SelectedRatedTourDTO != null)
                            SelectRatedTour();
                        break;
                    case 4:
                        if (SelectedPendingRequestDTO != null)
                            CreateTourBasedOnRequest();
                        break;
                    case 6:
                        if (SelectedComplexTourDTO != null)
                            OpenComplexTourWindow();
                        break;
                    default:
                        return;
                }
                e.Handled = true;
            }
        }
        private void LeftRightArrowKeys_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && (e.Key == Key.Left || e.Key == Key.Right))
            {
                int index = tabControl.SelectedIndex;
                int count = tabControl.Items.Count;

                if (e.Key == Key.Left)
                {
                    index = (index + count - 1) % count;
                }
                else if (e.Key == Key.Right)
                {
                    index = (index + 1) % count;
                }

                tabControl.SelectedIndex = index;
                e.Handled = true;
            }
        }
        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.L)
            {
                var currentDataGrid = GetCurrentDataGrid();
                if (currentDataGrid == null) return;
                var firstItem = GetFirstItem(currentDataGrid);
                SelectAndScrollTo(firstItem, currentDataGrid);
                e.Handled = true;
            }
        }
        private object GetFirstItem(DataGrid dataGrid)
        {
            if (dataGrid.Items.Count > 0)
            {
                return dataGrid.Items[0];
            }
            return null;
        }
        private void SelectAndScrollTo(object item, DataGrid dataGrid)
        {
            if (item != null)
            {
                dataGrid.SelectedItem = item;
                dataGrid.ScrollIntoView(item);
                dataGrid.Focus();
            }
        }

        private void SortAsc_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftShift) || !Keyboard.IsKeyDown(Key.A))
                return;

            var grid = GetCurrentDataGrid();
            if (grid == null || grid.Items.Count == 0)
                return;

            var view = CollectionViewSource.GetDefaultView(grid.ItemsSource);

            string sortColumn = "";

            if (grid == PendingRequestsDataGrid)
            {
                sortColumn = GetNextSortColumnRequests();
            }
            else if (grid == ComplexToursDataGrid)
            {
                sortColumn = GetNextSortColumnComplexTours();
            }
            else
            {
                sortColumn = GetNextSortColumnTours();
            }

            // Check if the column is already sorted in ascending order
            var isAlreadySorted = view.SortDescriptions.Count > 0 && view.SortDescriptions[0].PropertyName == sortColumn && view.SortDescriptions[0].Direction == ListSortDirection.Ascending;

            // Remove the arrow symbol from the previously sorted column header
            if (!isAlreadySorted)
            {
                var prevSortColumn = view.SortDescriptions.Count > 0 ? view.SortDescriptions[0].PropertyName : null;
                var prevHeader = grid.Columns.FirstOrDefault(c => c.SortMemberPath == prevSortColumn);
                if (prevHeader != null)
                {
                    prevHeader.Header = prevHeader.Header.ToString().Replace(" ▲", "").Replace(" ▼", "");
                }
            }

            // Set the new sort order and add the arrow symbol to the sorted column header
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription(sortColumn, ListSortDirection.Ascending));
            view.Refresh();

            var header = grid.Columns.FirstOrDefault(c => c.SortMemberPath == sortColumn);
            if (header != null)
            {
                header.Header = $"{header.Header}{(isAlreadySorted ? "▲" : " ▲")}";
            }
            e.Handled = true;
        }
        private void SortDesc_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftShift) || !Keyboard.IsKeyDown(Key.D))
                return;

            var grid = GetCurrentDataGrid();
            if (grid == null || grid.Items.Count == 0)
                return;

            var view = CollectionViewSource.GetDefaultView(grid.ItemsSource);

            string sortColumn = "";

            if (grid == PendingRequestsDataGrid) 
            { 
                sortColumn = GetNextSortColumnRequests();
            }
            else if (grid == ComplexToursDataGrid)
            {
                sortColumn = GetNextSortColumnComplexTours();
            }
            else 
            {
                sortColumn = GetNextSortColumnTours();
            }
                // Check if the column is already sorted in descending order
                var isAlreadySorted = view.SortDescriptions.Count > 0 && view.SortDescriptions[0].PropertyName == sortColumn && view.SortDescriptions[0].Direction == ListSortDirection.Descending;

            // Remove the arrow symbol from the previously sorted column header
            if (!isAlreadySorted)
            {
                var prevSortColumn = view.SortDescriptions.Count > 0 ? view.SortDescriptions[0].PropertyName : null;
                var prevHeader = grid.Columns.FirstOrDefault(c => c.SortMemberPath == prevSortColumn);
                if (prevHeader != null)
                {
                    prevHeader.Header = prevHeader.Header.ToString().Replace(" ▲", "").Replace(" ▼", "");
                }
            }

            // Set the new sort order and add the arrow symbol to the sorted column header
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription(sortColumn, ListSortDirection.Descending));
            view.Refresh();

            var header = grid.Columns.FirstOrDefault(c => c.SortMemberPath == sortColumn);
            if (header != null)
            {
                header.Header = $"{header.Header} ▼";
            }
            e.Handled = true;
        }
        private DataGrid GetCurrentDataGrid()
        {
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    return CurrentToursDataGrid;
                case 1:
                    return UpcomingToursDataGrid;
                case 2:
                    return FinishedToursDataGrid;
                case 3:
                    return RatedToursDataGrid;
                case 4:
                    return PendingRequestsDataGrid;
                case 6:
                    return ComplexToursDataGrid;
                default:
                    return null;
            }
        }
        private string GetNextSortColumnTours()
        {
            switch (CurrentTourSortIndex)
            {
                case 0:
                    CurrentTourSortIndex++;
                    return "Name";
                case 1:
                    CurrentTourSortIndex++;
                    return "Location";
                case 2:
                    CurrentTourSortIndex = 0;
                    return "StartTime";
                default:
                    return "";
            }
        }
        private string GetNextSortColumnRequests()
        {
            switch (CurrentRequestSortIndex)
            {
                case 0:
                    CurrentRequestSortIndex++;
                    return "Location";
                case 1:
                    CurrentRequestSortIndex++;
                    return "Language";
                case 2:
                    CurrentRequestSortIndex++;
                    return "MaximumGuests";
                case 3:
                    CurrentRequestSortIndex = 0;
                    return "StartTime";
                default:
                    return "";
            }
        }
        private string GetNextSortColumnComplexTours()
        {
            switch (CurrentRequestSortIndex)
            {
                case 0:
                    CurrentRequestSortIndex++;
                    return "Locations";
                case 1:
                    CurrentRequestSortIndex++;
                    return "Languages";
                case 2:
                    CurrentRequestSortIndex = 0;
                    return "NumberOfGuests";

                default:
                    return "";
            }
        }

        #endregion

        #region MessagesGrid
        public ObservableCollection<GuideMessage> Messages { get; set; }
        public GuideMessage SelectedMessage { get; set; }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedMessage != null) 
            {
                CurrentUser.SuperGuideLanguages.Remove(SelectedMessage.Language);
                Messages.Remove(SelectedMessage);
            }        
        }
        #endregion

        #region Update
        public void Update()
        {
            UpdateUpcomingTours();
            UpdateCurrentTours();
            UpdateFinishedTours();
            UpdateActiveTour();
            UpdatePendingRequests();
            UpdateComplexTours();
            UpdateMessages();
        }
        private void UpdateMessages() 
        {
            Messages.Clear();

            List<string> currentLanguages = CurrentUser.SuperGuideLanguages;
            List<string> newLanguages = _userService.QualifiesForSuperGuide(CurrentUser);

            List<string> addedLanguages = _userService.GetAddedLanguages(currentLanguages, newLanguages);
            List<string> lostLanguages = _userService.GetLostLanguages(currentLanguages, newLanguages);

            foreach (string language in addedLanguages)
            {
                Messages.Add(new GuideMessage(language, true));
            }
            foreach (string language in lostLanguages)
            {
                Messages.Add(new GuideMessage(language, false));
            }

        }
        private void UpdateUpcomingTours()
        {
            UpcomingTours.Clear();
            foreach (Tour tour in _tourService.GetUpcomingTours(CurrentUser))
            {
                UpcomingTours.Add(GuideDTOConverter.ConvertToDTO(tour, _locationService));
            }
        }
        private void UpdateCurrentTours()
        {
            CurrentTours.Clear();
            foreach (Tour tour in _tourService.GetTodaysTours(CurrentUser))
            {
                CurrentTours.Add(GuideDTOConverter.ConvertToDTO(tour, _locationService));
            }
        }
        private void UpdateFinishedTours()
        {
            FinishedTours.Clear();
            foreach (Tour tour in _tourService.GetFinishedTours(CurrentUser))
            {
                FinishedTours.Add(GuideDTOConverter.ConvertToDTO(tour, _locationService));
            }
        }
        private void UpdatePendingRequests()
        {
            PendingRequests.Clear();
            foreach (TourRequest request in _tourRequestService.GetPendingRequests(CurrentUser))
            {
                PendingRequests.Add(GuideDTOConverter.ConvertToDTO(request, _locationService));
            }
        }
        private void UpdateComplexTours()
        {
            ComplexTourRequests.Clear();
            foreach (ComplexTour complexTour in _complexTourService.GetAvailableComplexTours(CurrentUser))
            {
                ComplexTourRequests.Add(GuideDTOConverter.ConvertToDTO(complexTour, _tourRequestService, _locationService, _complexTourService));
            }
        }
        private void UpdateActiveTour()
        {
            ActiveTour = null;
            foreach (GuideTourDTO tourdto in CurrentTours)
            {
                Tour tour = GuideDTOConverter.ConvertToTour(tourdto, _tourService);

                if (tour.IsActive)
                {
                    ActiveTour = GuideDTOConverter.ConvertToDTO(tour, _locationService);
                    break;
                }
            }
        }

        #endregion
    
    }
}