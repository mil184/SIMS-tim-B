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
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public ObservableCollection<GuestAccommodationDTO> AvailableAccommodations { get; set; }

        private readonly AccommodationRepository _accommodationRepository;

        private readonly LocationRepository _locationRepository;

        private readonly UserRepository _userRepository;

        private readonly ImageRepository _imageRepository;

        public ObservableCollection<Location> Locations;


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
        public Guest1Window(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;

            _accommodationRepository = new AccommodationRepository();
            _accommodationRepository.Subscribe(this);

            Accommodations = new ObservableCollection<Accommodation>(_accommodationRepository.GetAll());

            _locationRepository = new LocationRepository();
            _locationRepository.Subscribe(this);

            AvailableAccommodations = new ObservableCollection<GuestAccommodationDTO>(ConvertToDTO(_accommodationRepository.GetAll()));
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
        
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if(searchText != null && searchText != "")
            {
                string Text = searchText.ToLower();
                Text = Text.Replace(" ", String.Empty);
                string Query = Text;
                string selectedSearchParam = (searchParamComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                ObservableCollection<Accommodation> FilteredAccommodations = new ObservableCollection<Accommodation>();
                ObservableCollection<Location> FilteredLocations = SearchLocations(Accommodations.ToList(), Query);

                foreach(Accommodation accommodation in Accommodations)
                {
                    if (accommodation.Name.ToLower().Contains(Query))
                    {
                        FilteredAccommodations.Add(accommodation);
                    }
                    else if(FilteredLocations.Any(loc => loc.Id == accommodation.LocationId))
                    {
                        FilteredAccommodations.Add(accommodation);
                    }
                    else if (accommodation.Type.ToString().ToLower().Contains(Query))
                    {
                        FilteredAccommodations.Add(accommodation);
                    }
                    else if (selectedSearchParam == "MaxGuests" && accommodation.MaxGuests.ToString().Contains(Query))
                    {
                        if (Int32.Parse(Query) > accommodation.MaxGuests)
                        {
                            MessageText = "The number of guests cannot be greater than max number of guests";
                        }
                        else
                        {
                            FilteredAccommodations.Add(accommodation);
                        }
                    }
                    else if (selectedSearchParam == "MinReservationDays" && accommodation.MinReservationDays.ToString().Contains(Query))
                    {
                        if (Int32.Parse(Query) < accommodation.MinReservationDays)
                        {
                            MessageText = "The number of reservation days cannot be less than min reservation days";
                        }
                        else
                        {
                            FilteredAccommodations.Add(accommodation);
                        }
                    }
                    Accommodations = FilteredAccommodations;
                    availableAccommodations.ItemsSource = Accommodations;
                }
            }
            else
            {
                Accommodations.Clear();
                foreach (var accomodation in _accommodationRepository.GetAll())
                {
                    Accommodations.Add(accomodation);
                }

                availableAccommodations.ItemsSource = Accommodations;
            }

        }
        
        private ObservableCollection<Location> SearchLocations(List<Accommodation> accommodations, string query)
        {
            ObservableCollection<Location> FilteredLocations = new ObservableCollection<Location>();
            foreach (Accommodation accommodation in accommodations)
            {
                Location locations = _locationRepository.GetById(accommodation.LocationId);
                if (locations.Country.ToLower().Contains(query) ||
                    locations.City.ToLower().Contains(query))
                {
                    FilteredLocations.Add(locations);
                }
            }
            return FilteredLocations;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
      
        public void Update()
        {
            AvailableAccommodations.Clear();
            foreach (Accommodation accommodation in _accommodationRepository.GetAll())
            {
                AvailableAccommodations.Add(ConvertToDTO(accommodation));
            }
        }
        public List<GuestAccommodationDTO> ConvertToDTO(List<Accommodation> accommodations)
        {
            List<GuestAccommodationDTO> dto = new List<GuestAccommodationDTO>();
            foreach (Accommodation accommodation in accommodations)
            {
                dto.Add(new GuestAccommodationDTO(accommodation.Name,
                    _locationRepository.GetById(accommodation.LocationId).Country,
                     _locationRepository.GetById(accommodation.LocationId).City,
                     accommodation.Type, accommodation.MaxGuests, accommodation.MinReservationDays));
            }
            return dto;
        }
        public GuestAccommodationDTO ConvertToDTO(Accommodation accommodation)
        {
            return new GuestAccommodationDTO(accommodation.Name,
                    _locationRepository.GetById(accommodation.LocationId).Country,
                     _locationRepository.GetById(accommodation.LocationId).City,
                       accommodation.Type, accommodation.MaxGuests, accommodation.MinReservationDays);
        }
    }
}
