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

        private readonly AccommodationRepository _accommodationRepository;

        private readonly LocationRepository _locationRepository;

        private readonly UserRepository _userRepository;

        private readonly ImageRepository _imageRepository;

        public ObservableCollection<Location> Locations;
        public GuestAccommodationDTO SelectedAccommodation { get; set; }


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

            _locationRepository = new LocationRepository();
            _locationRepository.Subscribe(this);

            AllAccommodations = new ObservableCollection<Accommodation>(_accommodationRepository.GetAll());
            PresentableAccommodations = ConvertToDTO(AllAccommodations);
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedAccommodation != null)
            {
                ReserveAccommodation reservationForm = new ReserveAccommodation(SelectedAccommodation, _accommodationRepository);
                reservationForm.Show();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            PresentableAccommodations.Clear();

            if (searchText != null && searchText != "")
            {
                string Text = searchText.ToLower();
                Text = Text.Replace(" ", String.Empty);
                string Query = Text;
                string selectedSearchParam = (searchParamComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                ObservableCollection<Accommodation> FilteredAccommodations = new ObservableCollection<Accommodation>();
                ObservableCollection<Location> FilteredLocations = SearchLocations(AllAccommodations, Query);

                foreach (Accommodation accommodation in AllAccommodations)
                {
                    if (accommodation.Name.ToLower().Contains(Query))
                    {
                        FilteredAccommodations.Add(accommodation);
                    }
                    else if (FilteredLocations.Any(loc => loc.Id == accommodation.LocationId))
                    {
                        FilteredAccommodations.Add(accommodation);
                    }
                    else if (accommodation.Type.ToString().ToLower().Contains(Query))
                    {
                        FilteredAccommodations.Add(accommodation);
                    }
                    else if (selectedSearchParam == "MaxGuests" && accommodation.MaxGuests.ToString().Contains(Query))
                    {
                        if (int.Parse(Query) > accommodation.MaxGuests)
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
                        if (int.Parse(Query) < accommodation.MinReservationDays)
                        {
                            MessageText = "The number of reservation days cannot be less than min reservation days";
                        }
                        else
                        {
                            FilteredAccommodations.Add(accommodation);
                        }
                    }
                }

                foreach (Accommodation accomodation in FilteredAccommodations)
                {
                    PresentableAccommodations.Add(ConvertToDTO(accomodation));
                }

            }
            else
            {
                foreach (Accommodation accomodation in AllAccommodations)
                {
                    PresentableAccommodations.Add(ConvertToDTO(accomodation));
                }
            }

        }

        private ObservableCollection<Location> SearchLocations(ObservableCollection<Accommodation> accommodations, string query)
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
            throw new NotImplementedException();
        }

        private void ImagesButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
