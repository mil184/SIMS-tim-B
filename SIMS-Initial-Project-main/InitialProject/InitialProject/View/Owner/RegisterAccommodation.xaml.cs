﻿using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Enums;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Image = InitialProject.Model.Image;

namespace InitialProject.View.Owner
{
    public partial class RegisterAccommodation : Window
    {
        private readonly AccommodationRepository _repository;

        private readonly LocationRepository _locationRepository;

        private readonly ImageRepository _imageRepository;

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

        private ObservableCollection<string> _imageUrls = new ObservableCollection<string>();
        public ObservableCollection<string> ImageUrls
        {
            get => _imageUrls;
            set
            {
                if (_imageUrls != value)
                {
                    _imageUrls = value;
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
            _imageRepository = new ImageRepository();

            LoggedInUser = user;

            AccommodationType_cb.Items.Add("Apartment");
            AccommodationType_cb.Items.Add("House");
            AccommodationType_cb.Items.Add("Hut");
        }

        private void btnRegisterAccommodation_Click(object sender, RoutedEventArgs e)
        {
            Location AccommodationLocation = new Location(Country, City);
            _locationRepository.Save(AccommodationLocation);
            Accommodation Accommodation = new Accommodation(AccommodationName, AccommodationLocation.Id, Enum.Parse<AccommodationType>(Type), int.Parse(MaxGuests), int.Parse(MinReservationDays), int.Parse(CancellationPeriod));
            _repository.Save(Accommodation);
            Close();
        }

        public void AccommodationType_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Type = ((ComboBox)sender).SelectedItem.ToString();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            string imageUrl = UrlTextBox.Text;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                ImageUrls.Add(imageUrl);
                _imageRepository.Save(new Image(imageUrl));
            }
            UrlTextBox.Text = string.Empty;
        }
    }
}
