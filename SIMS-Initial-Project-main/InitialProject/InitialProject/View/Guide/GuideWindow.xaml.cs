using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace InitialProject.View.Guide
{
    public partial class GuideWindow : Window, INotifyPropertyChanged, IObserver
    {
        private readonly TourService _tourService;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly LocationService _locationService;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointRepository _checkpointRepository;
        private readonly UserRepository _userRepository;
        public User CurrentUser { get; set; }
        public ObservableCollection<GuideTourDTO> CurrentTours { get; set; }
        public ObservableCollection<GuideTourDTO> UpcomingTours { get; set; }
        public bool TourActive { get; set; }
        public GuideTourDTO SelectedDTO { get; set; }

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

            _checkpointRepository = new CheckpointRepository();
            _checkpointRepository.Subscribe(this);

            _tourReservationRepository = new TourReservationRepository();
            _tourReservationRepository.Subscribe(this);

            _userRepository = new UserRepository();
            _userRepository.Subscribe(this);

            InitializeCollections();
            FindActiveTour();
            SortUpcomingTours();

        }
        private void InitializeCollections()
        {
            CurrentTours = new ObservableCollection<GuideTourDTO>(ConvertToDTO(_tourService.GetTodaysTours()));
            UpcomingTours = new ObservableCollection<GuideTourDTO>(ConvertToDTO(_tourService.GetUpcomingTours()));
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
        private void SortUpcomingTours()
        {
            var view = CollectionViewSource.GetDefaultView(UpcomingTours);
            view.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour(CurrentUser, _tourService, _locationService, _imageRepository, _checkpointRepository);
            createTour.Show();
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
                    tour.StartTime)); 
            }
            return dto;
        }
        public GuideTourDTO ConvertToDTO(Tour tour)
        {
            return new GuideTourDTO(
                    tour.Id,
                    tour.Name,
                    _locationService.GetById(tour.LocationId).Country,
                    _locationService.GetById(tour.LocationId).City,
                    tour.StartTime); 
        }
        public Tour ConvertToTour(GuideTourDTO dto)
        {
            if(dto != null)
            return _tourService.GetById(dto.Id);
            return null;
        }
        private void CurrentToursDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CheckIfTourIsActive();
            Tour selectedTour = ConvertToTour(SelectedDTO);

            if (selectedTour != null)
            {
                HandleSelectedTour(selectedTour);
            }
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
                ShowCheckpoints showCheckpoints = new ShowCheckpoints(selectedTour, _checkpointRepository, _tourService, _tourReservationRepository, _userRepository);
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
            ShowCheckpoints showCheckpoints = new ShowCheckpoints(tour, _checkpointRepository, _tourService, _tourReservationRepository, _userRepository);
            showCheckpoints.ShowDialog();
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {         
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }
        private void ShowActiveTourWarning()
        {
            MessageBox.Show("An active tour is already in progress. Please finish the current tour before starting a new one.", "Active Tour Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private bool ConfirmStartTour(Tour selectedTour)
        {
            var messageBoxResult = MessageBox.Show($"Are you sure you want to start the {selectedTour.Name} tour?", "Start Tour Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return messageBoxResult == MessageBoxResult.Yes;
        }
    }
}
