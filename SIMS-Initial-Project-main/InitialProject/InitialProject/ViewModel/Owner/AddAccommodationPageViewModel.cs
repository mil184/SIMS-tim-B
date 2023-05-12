﻿using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Enums;
using InitialProject.Service;
using InitialProject.View.Owner;
using MenuNavigation.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Image = InitialProject.Model.Image;

namespace InitialProject.ViewModel.Owner
{
    public class AddAccommodationPageViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public OwnerMainWindow MainWindow { get; set; }
        public User LoggedInUser { get; set; }
        public AddAccommodationPage CurrentPage { get; set; }

        private readonly AccommodationService _service;

        private readonly LocationService _locationService;

        private readonly ImageRepository _imageRepository;


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

        private ObservableCollection<int> _imageIds = new ObservableCollection<int>();

        public RelayCommand AddAccommodationCommand { get; set; }
        public RelayCommand CountrySelectionChangeCommand { get; set; }
        public RelayCommand CitySelectionChangeCommand { get; set; }
        public RelayCommand TypeSelectionChangeCommand { get; set; }
        public RelayCommand AddImageCommand { get; set; }

        private void Execute_AddAccommodationCommand(object obj)
        {
            if (IsValid)
            {
                Location AccommodationLocation = _locationService.GetLocation(Country, City);
                Accommodation Accommodation = new Accommodation(AccommodationName, LoggedInUser.Id, AccommodationLocation.Id, Enum.Parse<AccommodationType>(Type), int.Parse(MaxGuests), int.Parse(MinReservationDays), int.Parse(CancellationPeriod), _imageIds);
                _service.Save(Accommodation);
                MainWindow.Main.Content = new HomeScreen(MainWindow, LoggedInUser);
            }
            else
            {
                MessageBox.Show("Cannot Register Accommodation", "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Execute_CountrySelectionChangeCommand(object obj)
        {
            if (CurrentPage.cbCountry.SelectedItem != null && _locationService != null)
            {
                CurrentPage.txtSelectCountry.Visibility = Visibility.Hidden;
                if (CurrentPage.cbCity.Items != null)
                {
                    CurrentPage.cbCity.Items.Clear();
                }

                CurrentPage.cbCity.IsEnabled = true;
                foreach (String city in _locationService.GetCities(CurrentPage.cbCountry.SelectedItem.ToString()))
                {
                    CurrentPage.cbCity.Items.Add(city);
                }
            }
        }

        private void Execute_CitySelectionChangeCommand(object obj)
        {
            if (CurrentPage.cbCountry.SelectedItem != null && CurrentPage.cbCity.SelectedItem != null)
            {
                if (CurrentPage.txtSelectCity != null) CurrentPage.txtSelectCity.Visibility = Visibility.Hidden;
                Country = CurrentPage.cbCountry.SelectedItem.ToString();
                City = CurrentPage.cbCity.SelectedItem.ToString();
            }
        }

        private void Execute_TypeSelectionChangeCommand(object obj)
        {
            Type = CurrentPage.AccommodationType_cb.SelectedItem.ToString();
            if (CurrentPage.txtSelectType != null) CurrentPage.txtSelectType.Visibility = Visibility.Hidden;
        }

        private void Execute_AddImageCommand(object obj)
        {
            string imageUrl = CurrentPage.UrlTextBox.Text;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                ImageUrls.Add(imageUrl);
                Image image = new Image(imageUrl);
                _imageRepository.Save(image);
                _imageIds.Add(image.Id);
            }
            CurrentPage.UrlTextBox.Text = string.Empty;
        }

        public bool CanExecute_Command(object obj)
        {
            return true;
        }

        public AddAccommodationPageViewModel(OwnerMainWindow window, AddAccommodationPage page, AccommodationService service)
        {
            MainWindow = window;
            CurrentPage = page;
            LoggedInUser = MainWindow.LoggedInUser;
            _service = service;

            _locationService = new LocationService();
            _imageRepository = new ImageRepository();

            CurrentPage.txtSelectCountry.Visibility = Visibility.Visible;
            CurrentPage.txtSelectCity.Visibility = Visibility.Visible;
            CurrentPage.txtSelectType.Visibility = Visibility.Visible;

            AddAccommodationCommand = new RelayCommand(Execute_AddAccommodationCommand, CanExecute_Command);
            CountrySelectionChangeCommand = new RelayCommand(Execute_CountrySelectionChangeCommand, CanExecute_Command);
            CitySelectionChangeCommand = new RelayCommand(Execute_CitySelectionChangeCommand, CanExecute_Command);
            TypeSelectionChangeCommand = new RelayCommand(Execute_TypeSelectionChangeCommand, CanExecute_Command);
            AddImageCommand = new RelayCommand(Execute_AddImageCommand, CanExecute_Command);

            InitializeCountries();

            InitializeAccommodationType_cb();
        }

        private void InitializeAccommodationType_cb()
        {
            CurrentPage.AccommodationType_cb.Items.Add("Apartment");
            CurrentPage.AccommodationType_cb.Items.Add("House");
            CurrentPage.AccommodationType_cb.Items.Add("Hut");
        }

        private void InitializeCountries()
        {
            foreach (var country in _locationService.GetCountries())
            {
                CurrentPage.cbCountry.Items.Add(country);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                int TryParseNumber;
                if (columnName == "AccommodationName")
                {
                    if (string.IsNullOrEmpty(AccommodationName))
                        return "This field is required";
                }
                else if (columnName == "MaxGuests")
                {
                    if (string.IsNullOrEmpty(MaxGuests))
                        return "This field is required";

                    if (!int.TryParse(MaxGuests, out TryParseNumber))
                        return "This field should be a number";
                }
                else if (columnName == "MinReservationDays")
                {
                    if (string.IsNullOrEmpty(MinReservationDays))
                        return "This field is required";

                    if (!int.TryParse(MinReservationDays, out TryParseNumber))
                        return "This field should be a number";
                }
                else if (columnName == "CancellationPeriod")
                {
                    if (string.IsNullOrEmpty(CancellationPeriod))
                        return "This field is required";

                    if (!int.TryParse(CancellationPeriod, out TryParseNumber))
                        return "This field should be a number";
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "AccommodationName", "MaxGuests", "MinReservationDays", "CancellationPeriod" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
    }
}
