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

        private AccommodationRepository _accommodationRepository;

        private LocationRepository _locationRepository;

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

        private void searchBox_TextChanged(object sender, RoutedEventArgs e)
        {

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
