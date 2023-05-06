using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using InitialProject.Resources.UIHelper;
using InitialProject.Service;
using InitialProject.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace InitialProject.View.Guide
{
    public partial class GuideWindow : Window, INotifyPropertyChanged, IObserver
    {
        private readonly TourService _tourService;
        private readonly TourReservationService _tourReservationService;
        private readonly TourRatingService _tourRatingService;
        private readonly LocationService _locationService;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointService _checkpointService;
        private readonly UserRepository _userRepository;
        private readonly VoucherRepository _voucherRepository;

        public User CurrentUser { get; set; }
        public ObservableCollection<GuideTourDTO> CurrentTours { get; set; }
        public ObservableCollection<GuideTourDTO> UpcomingTours { get; set; }
        public ObservableCollection<GuideTourDTO> FinishedTours { get; set; }
        public ObservableCollection<GuideTourDTO> RatedTours { get; set; }

        public bool TourActive { get; set; }
        public GuideTourDTO SelectedCurrentTourDTO { get; set; }
        public GuideTourDTO SelectedUpcomingTourDTO { get; set; }
        public GuideTourDTO SelectedFinishedTourDTO { get; set; }
        public GuideTourDTO SelectedRatedTourDTO { get; set; }

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

            _userRepository = new UserRepository();
            _userRepository.Subscribe(this);

            _voucherRepository = new VoucherRepository();
            _voucherRepository.Subscribe(this);

            _tourRatingService = new TourRatingService();
            _tourRatingService.Subscribe(this);

            InitializeCollections();
            InitializeStartingSearchValues();
            InitializeComboBoxes();
            InitializeShortcuts();
            FindActiveTour();
            SortTours();

            CurrentUser.Username = "Gorana";

        }
        private void InitializeCollections()
        {
            CurrentTours = new ObservableCollection<GuideTourDTO>(ConvertToDTO(_tourService.GetTodaysTours()));
            UpcomingTours = new ObservableCollection<GuideTourDTO>(ConvertToDTO(_tourService.GetUpcomingTours()));
            FinishedTours = new ObservableCollection<GuideTourDTO>(ConvertToDTO(_tourService.GetFinishedTours()));
            RatedTours = new ObservableCollection<GuideTourDTO>(ConvertToDTO(_tourService.GetRatedTours()));
        }
        private void InitializeComboBoxes()
        {
            Years_cb.Items.Add("Alltime");
            Years_cb.SelectedItem = Years_cb.Items[0];
            for (int i = 2000; i <= DateTime.Now.Year; i++)
            {
                Years_cb.Items.Add(i.ToString());
            }
        }
        private void InitializeStartingSearchValues()
        {
            MostVisited = ConvertToDTO(_tourService.GetMostVisitedTour(_tourService.GetFinishedTours()));
        }
        private void InitializeShortcuts()
        {
            PreviewKeyDown += CreateTour_PreviewKeyDown;
            PreviewKeyDown += LogOut_PreviewKeyDown;
            PreviewKeyDown += Enter_PreviewKeyDown;
            PreviewKeyDown += ArrowKeys_PreviewKeyDown;
            PreviewKeyDown += DataGrid_PreviewKeyDown;
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
            List<Tour> toursByYear = _tourService.GetToursByYear(year);
            if (toursByYear.Count == 0)
            {
                MostVisited = new GuideTourDTO { Name = "No information", Location = "", NumberOfGuestsMessage = "" };
            }
            else
            {
                MostVisited = ConvertToDTO(_tourService.GetMostVisitedTour(toursByYear));
            }
        }
        private void HandleAllTimeSelection()
        {
            MostVisited = ConvertToDTO(_tourService.GetMostVisitedTour(_tourService.GetFinishedTours()));
        }


        private void FindActiveTour()
        {
            ActiveTour = null;
            foreach (GuideTourDTO tourdto in CurrentTours)
            {
                Tour tour = ConvertToTour(tourdto);

                if (tour.IsActive)
                {
                    ActiveTour = ConvertToDTO(tour);
                    break;
                }
            }
        }
        private void SortTours()
        {
            var view1 = CollectionViewSource.GetDefaultView(UpcomingTours);
            view1.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour(CurrentUser, _tourService, _locationService, _imageRepository, _checkpointService);
            createTour.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Update()
        {
            UpdateUpcomingTours();
            UpdateCurrentTours();
            UpdateFinishedTours();
            UpdateActiveTour();
        }
        private void UpdateUpcomingTours()
        {
            UpcomingTours.Clear();
            foreach (Tour tour in _tourService.GetUpcomingTours())
            {
                UpcomingTours.Add(ConvertToDTO(tour));
            }
        }
        private void UpdateCurrentTours()
        {
            CurrentTours.Clear();
            foreach (Tour tour in _tourService.GetTodaysTours())
            {
                CurrentTours.Add(ConvertToDTO(tour));
            }
        }
        private void UpdateFinishedTours()
        {
            FinishedTours.Clear();
            foreach (Tour tour in _tourService.GetFinishedTours())
            {
                FinishedTours.Add(ConvertToDTO(tour));
            }
        }
        private void UpdateActiveTour()
        {
            ActiveTour = null;
            foreach (GuideTourDTO tourdto in CurrentTours)
            {
                Tour tour = ConvertToTour(tourdto);

                if (tour.IsActive)
                {
                    ActiveTour = ConvertToDTO(tour);
                    break;
                }
            }
        }
        public List<GuideTourDTO> ConvertToDTO(List<Tour> tours)
        {
            List<GuideTourDTO> dto = new List<GuideTourDTO>();
            foreach (Tour tour in tours)
            {
                dto.Add(new GuideTourDTO(
                    tour.Id,
                    tour.Name,
                    _locationService.GetById(tour.LocationId).Country,
                    _locationService.GetById(tour.LocationId).City,
                    tour.StartTime,
                    tour.CurrentGuestCount));
            }
            return dto;
        }
        public GuideTourDTO ConvertToDTO(Tour tour)
        {

            if (tour == null)
                return null;

            return new GuideTourDTO(
                    tour.Id,
                    tour.Name,
                    _locationService.GetById(tour.LocationId).Country,
                    _locationService.GetById(tour.LocationId).City,
                    tour.StartTime,
                    tour.CurrentGuestCount);
        }
        public Tour ConvertToTour(GuideTourDTO dto)
        {
            if (dto != null)
                return _tourService.GetById(dto.Id);
            return null;
        }

        private void CurrentToursDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectTodaysTour();
        }

        private void SelectTodaysTour()
        {
            CheckIfTourIsActive();
            Tour selectedTour = ConvertToTour(SelectedCurrentTourDTO);

            if (selectedTour != null)
            {
                HandleSelectedTour(selectedTour);
            }
        }

        private void FinishedToursDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectFinishedTour();
        }
        private void SelectFinishedTour() 
        {
            Tour selectedTour = ConvertToTour(SelectedFinishedTourDTO);
            if (selectedTour != null)
            {
                StatisticsViewModel statisticsViewModel = new StatisticsViewModel(selectedTour, _tourReservationService);
                Statistics statistics = new Statistics(statisticsViewModel);
                statistics.Show();
            }
        }
        private void UpcomingToursDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectUpcomingTour();
        }

        private void SelectUpcomingTour() 
        {
            List<int> vouchersAdded = new List<int>();
            Tour tour = ConvertToTour(SelectedUpcomingTourDTO);

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
        private void AddVouchersToUsers(Tour tour, List<int> vouchersAdded)
        {
            foreach (int userId in _tourReservationService.GetUserIdsByTour(tour))
            {
                if (!vouchersAdded.Contains(userId))
                {
                    vouchersAdded.Add(userId);
                    Voucher voucher = new Voucher(tour.Name, DateTime.Now, DateTime.Now.AddYears(1), userId);
                    _voucherRepository.Save(voucher);
                }
            }
        }
        private void AbortTour(Tour tour)
        {
            tour.IsAborted = true;
            _tourService.Update(tour);
        }

        private void RatedToursDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectRatedTour();
        }
        private void SelectRatedTour() 
        {
            RatingsViewModel ratingsViewModel = new RatingsViewModel(_userRepository, _tourRatingService, _tourReservationService, _checkpointService, ConvertToTour(SelectedRatedTourDTO));
            Ratings ratings = new Ratings(ratingsViewModel);
            ratings.Show();
        }
        private void CheckIfTourIsActive()
        {
            TourActive = false;
            ActiveTour = null;

            foreach (Tour tour in _tourService.GetTodaysTours())
            {
                if (tour.IsActive)
                {
                    TourActive = true;
                    ActiveTour = ConvertToDTO(tour);
                    break;
                }
            }
        }
        private void HandleSelectedTour(Tour selectedTour)
        {
            if (selectedTour.IsActive)
            {
                ActiveTour = ConvertToDTO(selectedTour);
                ShowCheckpoints showCheckpoints = new ShowCheckpoints(selectedTour, _checkpointService, _tourService, _tourReservationService, _userRepository, _tourRatingService);
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
            ActiveTour = ConvertToDTO(tour);
        }
        private void ShowCheckpointsForTour(Tour tour)
        {
            ShowCheckpoints showCheckpoints = new ShowCheckpoints(tour, _checkpointService, _tourService, _tourReservationService, _userRepository, _tourRatingService);
            showCheckpoints.ShowDialog();
        }   
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {         
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();

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
        private void CreateTour_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.A)
            {
                e.Handled = true;
                Window createTour = new CreateTour(CurrentUser, _tourService, _locationService, _imageRepository, _checkpointService);
                createTour.ShowDialog();
            }
        }
        private void Enter_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                switch (tabControl.SelectedIndex) 
                {
                    case 0:
                        if(SelectedCurrentTourDTO != null)
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
                    default:
                        return;
                }
                e.Handled = true;
            }
        }
        private void ArrowKeys_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
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
                switch (tabControl.SelectedIndex)
                {
                    case 0:
                        if (CurrentToursDataGrid.Items.Count > 0)
                        {
                            CurrentToursDataGrid.SelectedItem = CurrentToursDataGrid.Items[0];
                            CurrentToursDataGrid.ScrollIntoView(CurrentToursDataGrid.SelectedItem);
                            CurrentToursDataGrid.Focus();
                        }
                        break;
                    case 1:
                        if (UpcomingToursDataGrid.Items.Count > 0)
                        {
                            UpcomingToursDataGrid.SelectedItem = UpcomingToursDataGrid.Items[0];
                            UpcomingToursDataGrid.ScrollIntoView(UpcomingToursDataGrid.SelectedItem);
                            UpcomingToursDataGrid.Focus();
                        }
                        break;
                    case 2:
                        if (FinishedToursDataGrid.Items.Count > 0)
                        {
                            FinishedToursDataGrid.SelectedItem = FinishedToursDataGrid.Items[0];
                            FinishedToursDataGrid.Focus();
                            FinishedToursDataGrid.ScrollIntoView(FinishedToursDataGrid.SelectedItem);
                        }
                        break;
                    case 3:
                        if (RatedToursDataGrid.Items.Count > 0)
                        {
                            RatedToursDataGrid.SelectedItem = RatedToursDataGrid.Items[0];
                            RatedToursDataGrid.ScrollIntoView(RatedToursDataGrid.SelectedItem);
                            RatedToursDataGrid.Focus();
                        }
                        break;
                    default:
                        return;
                }
                e.Handled = true;
            }
        }

        private void ShowActiveTourWarning()
        {
            MessageBox.Show("An active tour is already in progress. Please finish the current tour before starting a new one.", "Active Tour Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void ShowAbortTourWarning()
        { 
            MessageBox.Show("You may not abort this tour as you are breaking the 2 day rule.", "Abort Tour Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private bool ConfirmStartTour(Tour selectedTour)
        {
            var messageBoxResult = MessageBox.Show($"Are you sure you want to start the {selectedTour.Name} tour?", "Start Tour Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return messageBoxResult == MessageBoxResult.Yes;
        }
        private bool ConfirmAbortTour(Tour selectedTour)
        {
            var messageBoxResult = MessageBox.Show($"Are you sure you want to abort the {selectedTour.Name} tour?", "Abort Tour Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return messageBoxResult == MessageBoxResult.Yes;
        }

    }
}
