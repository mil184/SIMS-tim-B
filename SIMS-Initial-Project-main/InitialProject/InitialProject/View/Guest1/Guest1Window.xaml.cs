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

namespace InitialProject.View.Guest1
{
    /// <summary>
    /// Interaction logic for Guest1Window.xaml
    /// </summary>
    public partial class Guest1Window : Window, INotifyPropertyChanged, IObserver
    {
        public User LoggedInUser { get; set; }
        public ObservableCollection<Accommodation> AllAccommodations { get; set; }
        public ObservableCollection<GuestAccommodationDTO> PresentableAccommodations { get; set; }

        public ObservableCollection<Location> Locations;
        public GuestAccommodationDTO SelectedAccommodation { get; set; }


        public AccommodationRatingsDTO SelectedUnratedAccommodation { get; set; }
        public ObservableCollection<AccommodationRatings> AccommodationRatings { get; set; }
        public AccommodationRatings SelectedAccommodationRatings { get; set; }
        public ObservableCollection<AccommodationReservation> UnreviewedReservations { get; set; }


        private readonly AccommodationRepository _accommodationRepository;

        private readonly LocationRepository _locationRepository;

        private readonly UserRepository _userRepository;

        private readonly ImageRepository _imageRepository;

        private readonly AccommodationReservationRepository _accommodationReservationRepository;

        private readonly AccommodationRatingsRepository _accommodationRatingsRepository;


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

        private ObservableCollection<AccommodationRatingsDTO> _unratedAccommodations;
        public ObservableCollection<AccommodationRatingsDTO> UnratedAccommodations
        {
            get => _unratedAccommodations;
            set
            {
                if (_unratedAccommodations != value)
                {
                    _unratedAccommodations = value;
                    OnPropertyChanged();
                }
            }
        }


        public Guest1Window(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;

            _accommodationRepository = new AccommodationRepository();
            _accommodationRepository.Subscribe(this);

            _accommodationReservationRepository = new AccommodationReservationRepository();
            _accommodationReservationRepository.Subscribe(this);

            _locationRepository = new LocationRepository();
            _locationRepository.Subscribe(this);

            _userRepository = new UserRepository();
            _userRepository.Subscribe(this);

            _imageRepository = new ImageRepository();
            _imageRepository.Subscribe(this);

            _accommodationRatingsRepository = new AccommodationRatingsRepository();
            _accommodationRatingsRepository.Subscribe(this);

            AllAccommodations = new ObservableCollection<Accommodation>(_accommodationRepository.GetAll());
            PresentableAccommodations = ConvertToDTO(AllAccommodations);

            CheckForAvailableRatings();
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                ReserveAccommodation reservationForm = new ReserveAccommodation(SelectedAccommodation, LoggedInUser, _accommodationRepository, _accommodationReservationRepository);
                reservationForm.Show();
            }
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            PresentableAccommodations.Clear();

            if (string.IsNullOrEmpty(searchText))
            {
                foreach (Accommodation accommodation in AllAccommodations)
                {
                    PresentableAccommodations.Add(ConvertToDTO(accommodation));
                }
                return;
            }

            string Query = GetQueryText(searchText);
            string[] QueryWords = GetQueryWords(searchText);
            string selectedSearchParam = (searchParamComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            ObservableCollection<Accommodation> filteredAccommodations = FilterAccommodations(AllAccommodations, Query, selectedSearchParam, QueryWords);

            foreach (Accommodation accommodation in filteredAccommodations)
            {
                PresentableAccommodations.Add(ConvertToDTO(accommodation));
            }
        }

        private string GetQueryText(string searchText)
        {
            string text = searchText.ToLower();
            text = text.Replace(" ", string.Empty);
            return text;
        }

        private string[] GetQueryWords(string searchText)
        {
          string[] words = searchText.ToLower().Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
          return words;
        }
        private ObservableCollection<Accommodation> FilterAccommodations(ObservableCollection<Accommodation> accommodations, string query, string selectedSearchParam, string[] queryWords)
        {
            ObservableCollection<Accommodation> filteredAccommodations = new ObservableCollection<Accommodation>();
            ObservableCollection<Location> filteredLocations = SearchLocations(accommodations, queryWords);

            foreach (Accommodation accommodation in accommodations)
            {
                if (MatchesQuery(accommodation, query, selectedSearchParam, filteredLocations))
                {
                    filteredAccommodations.Add(accommodation);
                }
            }

            return filteredAccommodations;
        }

        private ObservableCollection<Location> SearchLocations(ObservableCollection<Accommodation> accommodations, string[] queryWords)
        {
            ObservableCollection<Location> filteredLocations = new ObservableCollection<Location>();
            foreach (Accommodation accommodation in accommodations)
            {
                Location location = _locationRepository.GetById(accommodation.LocationId);
                bool matchesQuery = true;
                foreach (string word in queryWords)
                {
                    if (!location.Country.ToLower().Contains(word) && !location.City.ToLower().Contains(word))
                    {
                        matchesQuery = false;
                        break;
                    }
                }
                if (matchesQuery)
                {
                    filteredLocations.Add(location);
                }
            }
            return filteredLocations;
        }

        private bool MatchesQuery(Accommodation accommodation, string query, string selectedSearchParam, ObservableCollection<Location> filteredLocations)
        {
            int intNum;
            bool isInt = int.TryParse(query, out intNum);

            if (accommodation.Name.ToLower().Replace(" ", "").Contains(query))
            {
                return true;
            }

            if (filteredLocations.Any(loc => loc.Id == accommodation.LocationId))
            {
                return true;
            }

            if (accommodation.Type.ToString().ToLower().Contains(query))
            {
                return true;
            }

            if (isInt && selectedSearchParam == "MaxGuests" && accommodation.MaxGuests == int.Parse(query))
            {
                if (int.Parse(query) > accommodation.MaxGuests)
                {
                    MessageText = "The number of guests cannot be greater than max number of guests";
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (isInt && selectedSearchParam == "MinReservationDays" && accommodation.MinReservationDays == int.Parse(query))
            {
                if (int.Parse(query) < accommodation.MinReservationDays)
                {
                    MessageText = "The number of reservation days cannot be less than min reservation days";
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<GuestAccommodationDTO> ConvertToDTO(ObservableCollection<Accommodation> accommodations)
        {
            ObservableCollection<GuestAccommodationDTO> dto = new ObservableCollection<GuestAccommodationDTO>();
            foreach (Accommodation accommodation in accommodations)
            {
                dto.Add(new GuestAccommodationDTO(accommodation.Id, accommodation.Name,
                    _locationRepository.GetById(accommodation.LocationId).Country,
                     _locationRepository.GetById(accommodation.LocationId).City,
                     accommodation.Type, accommodation.MaxGuests, accommodation.MinReservationDays, accommodation.CancellationPeriod));
            }
            return dto;
        }
        public GuestAccommodationDTO ConvertToDTO(Accommodation accommodation)
        {
            return new GuestAccommodationDTO(accommodation.Id, accommodation.Name,
                  _locationRepository.GetById(accommodation.LocationId).Country,
                   _locationRepository.GetById(accommodation.LocationId).City,
                   accommodation.Type, accommodation.MaxGuests, accommodation.MinReservationDays, accommodation.CancellationPeriod);

        }

        public void Update() 
        {
            FormUnratedReservation();
        }

        private void ImagesButton_Click(object sender, RoutedEventArgs e)
        {
            Accommodation selectedAccomodation = ConvertToAccomodation(SelectedAccommodation);

            List<string> imageUrls = new List<string>();

            foreach (int imageId in selectedAccomodation.ImageIds)
            {
                if (_imageRepository.GetById(imageId) != null)
                    imageUrls.Add(_imageRepository.GetById(imageId).Url);
            }

            AccommodationImages accommodationImages = new AccommodationImages(imageUrls);
            accommodationImages.ShowDialog();
        }
        public Accommodation ConvertToAccomodation(GuestAccommodationDTO dto)
        {
            return _accommodationRepository.GetById(dto.Id);
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }

        public bool RecentlyEnded(AccommodationReservation reservation)
        {
            TimeSpan daysPassed = DateTime.Today - reservation.EndDate;
            return daysPassed.TotalDays >= 0 && daysPassed.TotalDays <= 5;
        }

        public bool OwnerRated(AccommodationReservation reservation)
        {
            return _accommodationRatingsRepository.GetAll().Any(item => item.ReservationId == reservation.Id);
        }


        public void FormUnratedReservation()
        {
            UnratedAccommodations.Clear();
            foreach (AccommodationReservation reservation in _accommodationReservationRepository.GetAll())
            {
                if(RecentlyEnded(reservation) && !OwnerRated(reservation))
                {
                    if(AllAccommodations.Any(a => a.Id == reservation.AccommodationId))
                    {
                        UnratedAccommodations.Add(new AccommodationRatingsDTO(reservation.Id, _userRepository.GetById(reservation.OwnerId).Username, _accommodationRepository.GetById(reservation.AccommodationId).Name));
                        UnreviewedReservations.Add(reservation);
                    }
                }
            }
        }

        public void CheckForAvailableRatings()
        {
            UnratedAccommodations = new ObservableCollection<AccommodationRatingsDTO>();
            UnreviewedReservations = new ObservableCollection<AccommodationReservation>();
            FormUnratedReservation();
        }

        private void Evaluate_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUnratedAccommodation != null)
            {
                Evaluate evaluateAccommodation = new Evaluate(SelectedUnratedAccommodation, _accommodationRatingsRepository, _accommodationReservationRepository, _imageRepository);
                evaluateAccommodation.ShowDialog();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
