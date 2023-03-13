using InitialProject.Resources.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Microsoft.VisualBasic;

namespace InitialProject.View.Owner
{
    /// <summary>
    /// Interaction logic for RegisterAccommodation.xaml
    /// </summary>
    public partial class RegisterAccommodation : Window
    {
        private readonly AccommodationRepository _repository;

        private readonly LocationRepository _locationRepository;

        public User LoggedInUser { get; set; }

        private string _accommodationName;
        public string AccommodationName
        {
            get => _accommodationName;
            set
            {
                if (value != _accommodationName)
                {
                    _accommodationName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _country;
        public string Country
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

        private string _city;
        public string City
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

        private string _type;
        public string Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _maxGuests;
        public string MaxGuests
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

        private string _minReservationDays;
        public string MinReservationDays
        {
            get => _minReservationDays;
            set
            {
                if (value != _minReservationDays)
                {
                    _minReservationDays = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _cancellationPeriod;
        public string CancellationPeriod
        {
            get => _cancellationPeriod;
            set
            {
                if (value != _cancellationPeriod)
                {
                    _cancellationPeriod = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RegisterAccommodation(User user)
        {
            InitializeComponent();
            DataContext = this;

            _repository = new AccommodationRepository();
            _locationRepository = new LocationRepository();

            LoggedInUser = user;

            AccommodationType_cb.Items.Add("Apartment");
            AccommodationType_cb.Items.Add("House");
            AccommodationType_cb.Items.Add("Hut");
        }

        private void btnRegisterAccommodation_Click(object sender, RoutedEventArgs e)
        {
            Location AccommodationLocation = new Location(Country, City);
            _locationRepository.Save(AccommodationLocation);
            Accommodation Accommodation = new Accommodation(0, AccommodationName, AccommodationLocation.Id, Enum.Parse<AccommodationType>(Type), int.Parse(MaxGuests), int.Parse(MinReservationDays), int.Parse(CancellationPeriod));
            _repository.Save(Accommodation);
            Close();
        }

        public void AccommodationType_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Type = ((ComboBox)sender).SelectedItem.ToString();
        }
    }
}
