﻿using InitialProject.Model;
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
using System.Diagnostics.Metrics;
using System.Linq;
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
        private readonly TourService _tourService;
        private readonly TourReservationService _tourReservationService;
        private readonly TourRatingService _tourRatingService;
        private readonly LocationService _locationService;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointService _checkpointService;
        private readonly UserRepository _userRepository;
        private readonly VoucherRepository _voucherRepository;
        private readonly TourRequestService _tourRequestService;

        public User CurrentUser { get; set; }
        public ObservableCollection<GuideTourDTO> CurrentTours { get; set; }
        public ObservableCollection<GuideTourDTO> UpcomingTours { get; set; }
        public ObservableCollection<GuideTourDTO> FinishedTours { get; set; }
        public ObservableCollection<GuideTourDTO> RatedTours { get; set; }
        public ObservableCollection<GuideRequestDTO> PendingRequests { get; set; }

        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public bool TourActive { get; set; }

        public int CurrentSortIndex;
        public GuideTourDTO SelectedCurrentTourDTO { get; set; }
        public GuideTourDTO SelectedUpcomingTourDTO { get; set; }
        public GuideTourDTO SelectedFinishedTourDTO { get; set; }
        public GuideTourDTO SelectedRatedTourDTO { get; set; }
        public GuideRequestDTO SelectedPendingRequestDTO { get; set; }

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
        private string _city;
        public string CityInput
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _country;
        public string CountryInput
        {
            get => _country;
            set
            {
                if (value != _country)
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _language;
        public string LanguageInput
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _maxGuests;
        public string MaxGuestsInput
        {
            get => _maxGuests;
            set
            {
                if (value != _maxGuests)
                {
                    _maxGuests = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime? _startDateInput;
        public DateTime? StartDateInput
        {
            get => _startDateInput;
            set
            {
                if (value != _startDateInput)
                {
                    _startDateInput = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime? _endDateInput;
        public DateTime? EndDateInput
        {
            get => _endDateInput;
            set
            {
                if (value != _endDateInput)
                {
                    _endDateInput = value;
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

            _tourRequestService = new TourRequestService();
            _tourRequestService.Subscribe(this);

            InitializeCollections();
            InitializeStartingSearchValues();
            InitializeComboBoxes();
            InitializeShortcuts();
            FindActiveTour();

            StartDateInput = null;
            EndDateInput = null;

            CurrentUser.Username = "Gorana";
            CurrentSortIndex = 0;

            //AddMessage("Hello, world!");
            //AddMessage("How are you today?");
            //AddMessage("Do you want to grab lunch?");
            //AddMessage("Do you want to grab lunch?");
            //AddMessage("Do you want to grab lunch?");
        }
        private void InitializeCollections()
        {
            CurrentTours = new ObservableCollection<GuideTourDTO>(ConvertToDTO(_tourService.GetTodaysTours()));
            UpcomingTours = new ObservableCollection<GuideTourDTO>(ConvertToDTO(_tourService.GetUpcomingTours()));
            FinishedTours = new ObservableCollection<GuideTourDTO>(ConvertToDTO(_tourService.GetFinishedTours()));
            RatedTours = new ObservableCollection<GuideTourDTO>(ConvertToDTO(_tourService.GetRatedTours()));
            PendingRequests = new ObservableCollection<GuideRequestDTO>(ConvertToDTO(_tourRequestService.GetPendingRequests(CurrentUser)));

            Countries = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
        }
        private void InitializeComboBoxes()
        {
            Years_cb.Items.Add("Alltime");
            Years_cb.SelectedItem = Years_cb.Items[0];
            for (int i = 2000; i <= DateTime.Now.Year; i++)
            {
                Years_cb.Items.Add(i.ToString());
            }

            Countries.Add(string.Empty);
            foreach (string country in _locationService.GetCountries())
            {
                Countries.Add(country);
            }

            Cities.Add(string.Empty);
            foreach (string city in _locationService.GetCities())
            {
                Cities.Add(city);
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
            PreviewKeyDown += LeftRightArrowKeys_PreviewKeyDown;
            PreviewKeyDown += UpDownArrowKeys_PreviewKeyDown;
            PreviewKeyDown += DataGrid_PreviewKeyDown;
            PreviewKeyDown += SortAsc_PreviewKeyDown;
            PreviewKeyDown += SortDesc_PreviewKeyDown;
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
            UpdatePendingRequests();
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
        private void UpdatePendingRequests()
        {
            PendingRequests.Clear();
            foreach (TourRequest request in _tourRequestService.GetPendingRequests(CurrentUser))
            {
                PendingRequests.Add(ConvertToDTO(request));
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
        public List<GuideRequestDTO> ConvertToDTO(List<TourRequest> requests)
        {
            List<GuideRequestDTO> dto = new List<GuideRequestDTO>();
            foreach (TourRequest request in requests)
            {
                dto.Add(ConvertToDTO(request));
            }
            return dto;
        }
        public GuideRequestDTO ConvertToDTO(TourRequest request)
        {

            if (request == null)
                return null;

            return new GuideRequestDTO(
                    request.Id,
                    _locationService.GetById(request.LocationId).City + ", " +
                    _locationService.GetById(request.LocationId).Country,
                    request.Language,
                    request.MaxGuests.ToString(),
                    request.StartTime,
                    request.EndTime,
                    request.Description
                    );
        }
        public TourRequest ConvertToRequest(GuideRequestDTO dto)
        {
            if (dto != null)
                return _tourRequestService.GetById(dto.Id);
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
            foreach (int userId in _tourReservationService.GetUncheckedUserIdsByTour(tour))
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
                    default:
                        return;
                }
                e.Handled = true;
            }
        }
        private void LeftRightArrowKeys_PreviewKeyDown(object sender, KeyEventArgs e)
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
                var currentDataGrid = GetCurrentDataGrid();
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
        private void UpDownArrowKeys_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Down || e.Key == Key.Up))
            {
                DataGrid currentDataGrid = GetCurrentDataGrid();
                if (currentDataGrid == null) return;
                if (currentDataGrid.Items.Count < 1) return;

                int current_index = currentDataGrid.SelectedIndex;

                if (current_index == -1 && currentDataGrid == FinishedToursDataGrid)
                {
                    Years_cb.Focus();
                    e.Handled = true;
                    return;
                }
                if (Keyboard.IsKeyDown(Key.Down))
                {
                    MoveToNextItem(currentDataGrid, current_index);
                }
                else if (Keyboard.IsKeyDown(Key.Up))
                {
                    MoveToPreviousItem(currentDataGrid, current_index);
                }
                e.Handled = true;
            }
        }

        private void MoveToNextItem(DataGrid currentDataGrid, int current_index)
        {
            if (current_index < currentDataGrid.Items.Count - 1)
            {
                // Select the next item in the DataGrid
                currentDataGrid.SelectedItem = currentDataGrid.Items[current_index + 1];
                currentDataGrid.ScrollIntoView(currentDataGrid.SelectedItem);
                currentDataGrid.Focus();
            }
            else
            {
                if (currentDataGrid != FinishedToursDataGrid)
                {
                    MoveToFirstItem(currentDataGrid);
                }
                else
                {
                    UnselectSelectedItem(currentDataGrid);
                    FocusOnComboBox();
                }
            }
        }


        private void MoveToPreviousItem(DataGrid currentDataGrid, int current_index)
        {
            if (current_index > 0)
            {
                // Select the previous item in the DataGrid
                currentDataGrid.SelectedItem = currentDataGrid.Items[current_index - 1];
                currentDataGrid.ScrollIntoView(currentDataGrid.SelectedItem);
                currentDataGrid.Focus();
            }
            else
            {
                if (currentDataGrid != FinishedToursDataGrid)
                {
                    MoveToLastItem(currentDataGrid);
                }
                else
                {
                    UnselectSelectedItem(currentDataGrid);
                    FocusOnComboBox();
                }
            }
        }

        private void MoveToFirstItem(DataGrid currentDataGrid)
        {
            if (Years_cb.IsDropDownOpen)
            {
                // Move to the first item of the combo box
                Years_cb.SelectedIndex = 0;
            }
            else
            {
                // Move to the first item of the data grid
                currentDataGrid.SelectedItem = currentDataGrid.Items[0];
                currentDataGrid.ScrollIntoView(currentDataGrid.SelectedItem);
                currentDataGrid.Focus();
            }
        }
        private void MoveToLastItem(DataGrid currentDataGrid)
        {
            if (Years_cb.IsDropDownOpen)
            {
                // Move to the last item of the combo box
                Years_cb.SelectedIndex = Years_cb.Items.Count - 1;
            }
            else
            {
                // Move to the last item of the data grid
                currentDataGrid.SelectedItem = currentDataGrid.Items[currentDataGrid.Items.Count - 1];
                currentDataGrid.ScrollIntoView(currentDataGrid.SelectedItem);
                currentDataGrid.Focus();
            }
        }
        private void UnselectSelectedItem(DataGrid currentDataGrid)
        {
            currentDataGrid.SelectedItem = null;
        }
        private void FocusOnComboBox()
        {
            Years_cb.Focus();
        }
        private void SortAsc_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftShift) || !Keyboard.IsKeyDown(Key.A))
                return;

            var grid = GetCurrentDataGrid();
            if (grid == null || grid.Items.Count == 0)
                return;

            var view = CollectionViewSource.GetDefaultView(grid.ItemsSource);
            var sortColumn = GetNextSortColumn();

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
            var sortColumn = GetNextSortColumn();

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
                default:
                    return null;
            }
        }
        private string GetNextSortColumn()
        {
            switch (CurrentSortIndex)
            {
                case 0:
                    CurrentSortIndex++;
                    return "Name";
                case 1:
                    CurrentSortIndex++;
                    return "Location";
                case 2:
                    CurrentSortIndex = 0;
                    return "StartTime";
                default:
                    return "";
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

        private void StartTourButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCurrentTourDTO != null)
                SelectTodaysTour();
        }
        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void AbortButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUpcomingTourDTO != null)
                SelectUpcomingTour();
        }
        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedFinishedTourDTO != null)
                SelectFinishedTour();
        }
        private void RatingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRatedTourDTO != null)
                SelectRatedTour();
        }

        private void CbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CountryInput = _locationService.GetCountryByCity(CityInput);
            UpdateRequests();
        }

        private void CbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_locationService.GetCitiesByCountry(CountryInput).Contains(CityInput))
                CityInput = string.Empty;

            UpdateRequests();
        }

        private void txtGuests_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateRequests();
        }

        private void txtLanguage_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateRequests();
        }

        private void dpStartDate_DateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRequests();
        }

        private void dpEndDate_DateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRequests();
        }

        private void UpdateRequests()
        {
            CheckIfAllEmpty();

            PendingRequests.Clear();

            List<TourRequest> result = new List<TourRequest>();

            if (!string.IsNullOrEmpty(CountryInput))
            {
                result = _tourRequestService.GetByCountry(CurrentUser, CountryInput);
            }
            else
            {
                result = _tourRequestService.GetPendingRequests(CurrentUser);
            }

            if (!string.IsNullOrEmpty(CityInput))
            {
                result = result.Intersect(_tourRequestService.GetByCity(CurrentUser, CityInput)).ToList();
            }
            if (!string.IsNullOrEmpty(LanguageInput))
            {
                result = result.Intersect(_tourRequestService.GetByLanguage(CurrentUser, LanguageInput)).ToList();
            }
            if (!string.IsNullOrEmpty(MaxGuestsInput) && int.TryParse(MaxGuestsInput, out int integer))
            {
                result = result.Intersect(_tourRequestService.GetByMaxGuests(CurrentUser, int.Parse(MaxGuestsInput))).ToList();
            }
            if (StartDateInput != null)
            {
                result = result.Intersect(_tourRequestService.GetByStartDate(CurrentUser, StartDateInput)).ToList();
            }
            if (EndDateInput != null)
            {
                result = result.Intersect(_tourRequestService.GetByEndDate(CurrentUser, EndDateInput)).ToList();
            }

            List<GuideRequestDTO> searchResults = ConvertToDTO(result);

            foreach (GuideRequestDTO dto in searchResults)
            {
                PendingRequests.Add(dto);
            }

        }
        public void CheckIfAllEmpty()
        {
            if (string.IsNullOrEmpty(CountryInput) && string.IsNullOrEmpty(CityInput) && string.IsNullOrEmpty(LanguageInput) && string.IsNullOrEmpty(MaxGuestsInput))
            {
                foreach (TourRequest request in _tourRequestService.GetPendingRequests(CurrentUser))
                {
                    PendingRequests.Add(ConvertToDTO(request));
                }
            }
        }

        private void PendingRequests_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedPendingRequestDTO != null)
            {
                TourRequest request = ConvertToRequest(SelectedPendingRequestDTO);
                CreateTour createTour = new CreateTour(CurrentUser, _tourService, _locationService, _imageRepository, _checkpointService, _tourRequestService, request);
                createTour.ShowDialog();
            }
        }
        private void AddMessage(string text)
        {
            var messageGrid = new Grid();
            messageGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            messageGrid.ColumnDefinitions.Add(new ColumnDefinition());

            var messageTextBlock = new TextBlock
            {
                Text = text,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 10, 0)
            };
            Grid.SetColumn(messageTextBlock, 0);

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10, 0, 10, 0)
            };
            Grid.SetColumn(buttonPanel, 1);

            var acceptButton = new Button
            {
                Content = "Accept",
                Margin = new Thickness(5),
                Width = 75,
                DataContext = messageGrid
            };
            acceptButton.Click += AcceptButton_Click;
            buttonPanel.Children.Add(acceptButton);

            var declineButton = new Button
            {
                Content = "Decline",
                Margin = new Thickness(5),
                Width = 75,
                DataContext = messageGrid
            };
            declineButton.Click += DeclineButton_Click;
            buttonPanel.Children.Add(declineButton);

            messageGrid.Children.Add(messageTextBlock);
            messageGrid.Children.Add(buttonPanel);
            messageGrid.RenderTransform = new TranslateTransform { Y = 100 };

            MessagesStackPanel.Children.Add(messageGrid);

            var storyboard = new Storyboard();
            var animation = new DoubleAnimation
            {
                From = 100,
                To = 0,
                Duration = TimeSpan.FromSeconds(1)
            };
            Storyboard.SetTarget(animation, messageGrid);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(FrameworkElement.RenderTransform).(TranslateTransform.Y)"));
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            var messageGrid = ((sender as Button).DataContext as Grid);

            // Get the index of the clicked message grid
            int clickedIndex = MessagesStackPanel.Children.IndexOf(messageGrid);

            // Remove the clicked message grid from the stack panel
            MessagesStackPanel.Children.Remove(messageGrid);

            // Move all the message grids above the clicked one up by the height of the clicked message grid
            for (int i = clickedIndex - 1; i >= 0; i--)
            {
                var message = MessagesStackPanel.Children[i] as FrameworkElement;

                // Create a storyboard to animate the message grid up
                var storyboard = new Storyboard();
                var animation = new DoubleAnimation
                {
                    To = -message.ActualHeight,
                    Duration = TimeSpan.FromSeconds(0.5)
                };
                Storyboard.SetTarget(animation, message);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(FrameworkElement.RenderTransform).(TranslateTransform.Y)"));
                storyboard.Children.Add(animation);

                // Start the storyboard and adjust the message's margin
                storyboard.Begin();
                message.Margin = new Thickness(0, -message.ActualHeight, 0, 0);
            }
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            var messageGrid = ((sender as Button).DataContext as Grid);

            // Get the index of the clicked message grid
            int clickedIndex = MessagesStackPanel.Children.IndexOf(messageGrid);

            // Remove the clicked message grid from the stack panel
            MessagesStackPanel.Children.Remove(messageGrid);

            // Move all the message grids above the clicked one up by the height of the clicked message grid
            for (int i = clickedIndex - 1; i >= 0; i--)
            {
                var message = MessagesStackPanel.Children[i] as FrameworkElement;

                // Create a storyboard to animate the message grid up
                var storyboard = new Storyboard();
                var animation = new DoubleAnimation
                {
                    To = -message.ActualHeight,
                    Duration = TimeSpan.FromSeconds(0.5)
                };
                Storyboard.SetTarget(animation, message);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(FrameworkElement.RenderTransform).(TranslateTransform.Y)"));
                storyboard.Children.Add(animation);

                // Start the storyboard and adjust the message's margin
                storyboard.Begin();
                message.Margin = new Thickness(0, -message.ActualHeight, 0, 0);
            }

        }
    }
}