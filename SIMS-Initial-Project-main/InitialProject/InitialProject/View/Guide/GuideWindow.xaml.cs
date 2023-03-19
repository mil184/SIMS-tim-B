using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
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
using System.Windows.Media;

namespace InitialProject.View.Guide
{
    public partial class GuideWindow : Window, INotifyPropertyChanged, IObserver
    {
        public User CurrentUser { get; set; }

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
        public ObservableCollection<GuideTourDTO> CurrentTours { get; set; }
        public ObservableCollection<GuideTourDTO> UpcomingTours { get; set; }

        public bool TourActive { get; set; }
        public GuideTourDTO SelectedDTO { get; set; }

        private readonly TourRepository _tourRepository;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly LocationRepository _locationRepository;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointRepository _checkpointRepository;
        private readonly UserRepository _userRepository;

        public GuideWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            CurrentUser = user;

            _tourRepository = new TourRepository();
            _tourRepository.Subscribe(this);

            _imageRepository = new ImageRepository();
            _imageRepository.Subscribe(this);

            _locationRepository = new LocationRepository();
            _locationRepository.Subscribe(this);

            _checkpointRepository = new CheckpointRepository();
            _checkpointRepository.Subscribe(this);

            _tourReservationRepository = new TourReservationRepository();
            _tourReservationRepository.Subscribe(this);

            _userRepository = new UserRepository();
            _userRepository.Subscribe(this);


            CurrentTours = new ObservableCollection<GuideTourDTO>(ConvertToDTO(_tourRepository.GetTodaysTours()));
            UpcomingTours = new ObservableCollection<GuideTourDTO>(ConvertToDTO(_tourRepository.GetUpcomingTours()));

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

            var view = CollectionViewSource.GetDefaultView(UpcomingTours);
            view.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour(CurrentUser, _tourRepository, _locationRepository, _imageRepository, _checkpointRepository);
            createTour.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Update()
        {
            UpcomingTours.Clear();
            foreach (Tour tour in _tourRepository.GetUpcomingTours())
            {
                UpcomingTours.Add(ConvertToDTO(tour));
            }

            CurrentTours.Clear();
            foreach (Tour tour in _tourRepository.GetTodaysTours())
            {
                CurrentTours.Add(ConvertToDTO(tour));
            }

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
                    _locationRepository.GetById(tour.LocationId).Country,
                    _locationRepository.GetById(tour.LocationId).City,
                    tour.StartTime)); 
            }
            return dto;
        }
        public GuideTourDTO ConvertToDTO(Tour tour)
        {
            return new GuideTourDTO(
                    tour.Id,
                    tour.Name,
                    _locationRepository.GetById(tour.LocationId).Country,
                    _locationRepository.GetById(tour.LocationId).City,
                    tour.StartTime); 
        }
        public Tour ConvertToTour(GuideTourDTO dto)
        {
            if(dto != null)
            return _tourRepository.GetById(dto.Id);
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

            Update();
        }

        private void CheckIfTourIsActive()
        {
            TourActive = false;
            ActiveTour = null;

            foreach (Tour tour in _tourRepository.GetTodaysTours())
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
                ShowCheckpoints showCheckpoints = new ShowCheckpoints(selectedTour, _checkpointRepository, _tourRepository, _tourReservationRepository, _userRepository);
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
        private void ShowActiveTourWarning()
        {
            MessageBox.Show("An active tour is already in progress. Please finish the current tour before starting a new one.", "Active Tour Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void StartTourConfirmation(Tour selectedTour)
        {
            var messageBoxResult = MessageBox.Show($"Are you sure you want to start the {selectedTour.Name} tour?", "Start Tour Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                StartSelectedTour(selectedTour);
            }
        }

        private void StartSelectedTour(Tour selectedTour)
        {
            selectedTour.IsActive = true;
            TourActive = true;
            selectedTour.CurrentCheckpointId = selectedTour.CheckpointIds.First();
            _tourRepository.Update(selectedTour);
            ActiveTour = ConvertToDTO(selectedTour);
            ShowCheckpoints showCheckpoints = new ShowCheckpoints(selectedTour, _checkpointRepository, _tourRepository, _tourReservationRepository, _userRepository);
            showCheckpoints.ShowDialog();
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {         
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }
    }
}
