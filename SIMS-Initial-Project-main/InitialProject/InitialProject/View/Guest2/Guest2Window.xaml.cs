using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace InitialProject.View.Guest2
{
    public partial class Guest2Window : Window, INotifyPropertyChanged, IObserver
    {
        public User CurrentUser { get; set; }
        public ObservableCollection<Guest2TourDTO> Tours { get; set; }

        private readonly TourRepository _tourRepository;
        private readonly LocationRepository _locationRepository;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointRepository _checkpointRepository;
        private readonly UserRepository _userRepository;

        public Guest2Window(User user)
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

            _userRepository = new UserRepository();
            _userRepository.Subscribe(this);

            Tours = new ObservableCollection<Guest2TourDTO>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Update()
        {
            Tours.Clear();
            foreach (Tour tour in _tourRepository.GetAll())
            {
                Tours.Add(ConvertToDTO(tour));
            }
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        public List<Guest2TourDTO> ConvertToDTOList(List<Tour> tours)
        {
            List<Guest2TourDTO> dtoList = new List<Guest2TourDTO>();

            foreach (Tour tour in tours)
            {
                dtoList.Add(new Guest2TourDTO(
                    tour.Name,
                _locationRepository.GetById(tour.LocationId).Country,
                _locationRepository.GetById(tour.LocationId).City,
                tour.Description,
                tour.Language,
                tour.MaxGuests,
                tour.CurrentGuestCount,
                tour.StartTime,
                tour.Duration,
                _userRepository.GetById(tour.GuideId).Username));
            }

            return dtoList;
        }
        public Guest2TourDTO ConvertToDTO(Tour tour)
        {
            return new Guest2TourDTO(
                tour.Name,
                _locationRepository.GetById(tour.LocationId).Country,
                _locationRepository.GetById(tour.LocationId).City,
                tour.Description,
                tour.Language,
                tour.MaxGuests,
                tour.CurrentGuestCount,
                tour.StartTime,
                tour.Duration,
                _userRepository.GetById(tour.GuideId).Username);
        }
    }
}