using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.View.Guest2
{
    public partial class ZeroSpacesForReservation : Window, INotifyPropertyChanged, IObserver
    {
        public User LoggedInUser { get; set; }
        public Guest2TourDTO SelectedTour { get; set; }
        public Guest2TourDTO NewSelectedTour { get; set; }

        public List<Tour> ToursByCity;
        public List<Tour> ToursByCityWithoutSelected;

        public ObservableCollection<Guest2TourDTO> Tours { get; set; }

        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly UserRepository _userRepository;
        private readonly VoucherService _voucherService;
        private readonly TourReservationService _tourReservationService;

        public ZeroSpacesForReservation(Guest2TourDTO selectedTour, User user, TourService tourService, LocationService locationService, UserRepository userRepository, VoucherService voucherService)
        {
            InitializeComponent();
            DataContext = this;

            SelectedTour = selectedTour;
            LoggedInUser = user;

            _tourService = tourService;
            _locationService = locationService;
            _userRepository = userRepository;
            _voucherService = voucherService;

            ToursByCity = _tourService.GetByCityName(selectedTour.City);
            ToursByCityWithoutSelected = _tourService.RemoveFromListById(ToursByCity, selectedTour.TourId);
            
            Tours = ConvertToDTOList(ToursByCity);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Update()
        {
            Tours.Clear();
            foreach (Tour tour in _tourService.GetAll())
            {
                Tours.Add(ConvertToDTO(tour));
            }
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewSelectedTour != null)
            {
                ReserveTour reserveTourForm = new ReserveTour(NewSelectedTour, LoggedInUser, _tourService, _tourReservationService, _voucherService, _locationService, _userRepository);
                reserveTourForm.ShowDialog();
            }
            Close();
        }

        public ObservableCollection<Guest2TourDTO> ConvertToDTOList(List<Tour> tours)
        {
            ObservableCollection<Guest2TourDTO> dtoList = new ObservableCollection<Guest2TourDTO>();

            foreach (Tour tour in tours)
            {
                dtoList.Add(new Guest2TourDTO(
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

            return dtoList;
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
