using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InitialProject.Model.DTO;

namespace InitialProject.View.Guest2
{
    public partial class Guest2Window : Window, INotifyPropertyChanged, IObserver
    {
        public User LoggedInUser { get; set; }
        public Guest2TourDTO SelectedGuest2TourDTO { get; set; }
        public ObservableCollection<Guest2TourDTO> TourDTOs { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }

        public ObservableCollection<Location> Locations;

        public ObservableCollection<Guest2TourDTO> NonReservedTours { get; set; }

        private readonly TourRepository _tourRepository;
        private readonly LocationRepository _locationRepository;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointRepository _checkpointRepository;
        private readonly UserRepository _userRepository;
        private readonly TourReservationRepository _tourReservationRepository;

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged();
            }
        }

        private string messageText;
        public string MessageText
        {
            get { return messageText; }
            set
            {
                messageText = value;
                OnPropertyChanged();
            }
        }

        public Guest2Window(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;

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

            _tourReservationRepository = new TourReservationRepository();
            _tourReservationRepository.Subscribe(this); 



            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            TourDTOs = ConvertToDTO(Tours);

            RemoveReservedTours();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Update()
        {
            TourDTOs.Clear();
            foreach (Tour tour in _tourRepository.GetAll())
            {
                TourDTOs.Add(ConvertToDTO(tour));
            }
        }

        public void RemoveByTourId(int id)
        {
            Guest2TourDTO tourForRemoval = TourDTOs.FirstOrDefault(t => t.TourId == id);
            TourDTOs.Remove(tourForRemoval);
        }

        public void RemoveReservedTours()
        {
            foreach(TourReservation tourReservation in _tourReservationRepository.GetAll())
            {
                if(tourReservation.UserId == LoggedInUser.Id)
                {
                    RemoveByTourId(tourReservation.TourId);
                }
            }
        }

        public ObservableCollection<Guest2TourDTO> ConvertToDTO(ObservableCollection<Tour> tours)
        {
            ObservableCollection<Guest2TourDTO> dto = new ObservableCollection<Guest2TourDTO>();

            foreach (Tour tour in tours)
            {
                dto.Add(new Guest2TourDTO(
                    tour.Id,
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

            return dto;
        }
        public Guest2TourDTO ConvertToDTO(Tour tour)
        {
            return new Guest2TourDTO(
                tour.Id,
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

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGuest2TourDTO != null)
            {
                ReserveTour reserveTourForm = new ReserveTour(SelectedGuest2TourDTO, LoggedInUser, _tourRepository);
                reserveTourForm.ShowDialog();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            TourDTOs.Clear();

            if (searchText != null && searchText != "")
            {
                string Text = searchText.ToLower();
                Text = Text.Replace(" ", String.Empty);
                string Query = Text;
                string selectedSearchParam = (searchParamComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                ObservableCollection<Tour> FilteredTours = new ObservableCollection<Tour>();
                ObservableCollection<Location> FilteredLocations = SearchLocations(Tours, Query);

                foreach (Tour tour in Tours)
                {
                    if (tour.Name.ToLower().Contains(Query))
                    {
                        FilteredTours.Add(tour);
                    }
                    else if (FilteredLocations.Any(loc => loc.Id == tour.LocationId))
                    {
                        FilteredTours.Add(tour);
                    }
                    else if (tour.Language.ToString().ToLower().Contains(Query))
                    {
                        FilteredTours.Add(tour);
                    }
                    else if (tour.Duration.ToString().ToLower().Contains(Query))
                    {
                        FilteredTours.Add(tour);
                    }
                    else if (selectedSearchParam == "Max Guests" && tour.MaxGuests.ToString().Contains(Query))
                    {
                        if (int.Parse(Query) > tour.MaxGuests)
                        {
                            MessageText = "The number of guests cannot be greater than max number of guests";
                        }
                        else
                        {
                            FilteredTours.Add(tour);
                        }
                    }
                    else if (selectedSearchParam == "Current Guest Count" && tour.CurrentGuestCount.ToString().Contains(Query))
                    {
                        if (int.Parse(Query) < tour.CurrentGuestCount)
                        {
                            MessageText = "The number of guests cannot be greater than max number of guests";
                        }
                        else
                        {
                            FilteredTours.Add(tour);
                        }
                    }
                }

                foreach (Tour tour in FilteredTours)
                {
                    TourDTOs.Add(ConvertToDTO(tour));
                }

            }
            else
            {
                foreach (Tour tour in Tours)
                {
                    TourDTOs.Add(ConvertToDTO(tour));
                }
            }
        }

        private ObservableCollection<Location> SearchLocations(ObservableCollection<Tour> tours, string query)
        {
            ObservableCollection<Location> FilteredLocations = new ObservableCollection<Location>();

            foreach (Tour tour in tours)
            {
                Location location = _locationRepository.GetById(tour.LocationId);
                if (location.Country.ToLower().Contains(query) || location.City.ToLower().Contains(query))
                {
                    FilteredLocations.Add(location);
                }
            }
            return FilteredLocations;
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
    }
}