using InitialProject.Model.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Service;
using System.Windows.Input;
using InitialProject.View.Guest2;
using System.Windows;
using MenuNavigation.Commands;
using System.Collections;

namespace InitialProject.ViewModel.Guest2
{
    public class ComplexTourRequestViewModel : INotifyPropertyChanged, IObserver
    {
        public Action CloseAction { get; set; }
        public User LoggedInUser { get; set; }

        public ComplexTour ComplexTour { get; set; }
        public ObservableCollection<Location> TourRequestLocations { get; set; }

        public ObservableCollection<String> cbCountryItemsSource { get; set; }
        public ObservableCollection<String> cbCityItemsSource { get; set; }

        public ObservableCollection<string> StartHours { get; set; }
        public ObservableCollection<string> StartMinutes { get; set; }
        public ObservableCollection<string> EndHours { get; set; }
        public ObservableCollection<string> EndMinutes { get; set; }

        private readonly LocationService _locationService;
        private readonly TourRequestService _tourRequestService;
        private readonly ComplexTourService _complexTourService;

        public RelayCommand SubmitRequestCommand { get; set; }
        public RelayCommand SubmitComplexTourRequestCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand CountrySelectionChangedCommand { get; set; }

        #region Properties

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

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _language;
        public string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
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

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value != _startDate)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _startHour;
        public int StartHour
        {
            get => _startHour;
            set
            {
                if (value != _startHour)
                {
                    _startHour = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _startMinute;
        public int StartMinute
        {
            get => _startHour;
            set
            {
                if (value != _startMinute)
                {
                    _startMinute = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (value != _endDate)
                {
                    _endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _endHour;
        public int EndHour
        {
            get => _endHour;
            set
            {
                if (value != _endHour)
                {
                    _endHour = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _endMinute;
        public int EndMinute
        {
            get => _endMinute;
            set
            {
                if (value != _endMinute)
                {
                    _endMinute = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime StartDateTime => StartDate.Date + new TimeSpan(StartHour, StartMinute, 0);
        public DateTime EndDateTime => EndDate.Date + new TimeSpan(EndHour, EndMinute, 0);

        public bool HasErrors => throw new NotImplementedException();

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";

        public int counter;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Update()
        {
            //
        }

        public ComplexTourRequestViewModel(LocationService locationService, TourRequestService tourRequestService, ComplexTourService complexTourService, User user, string lang)
        {
            _locationService = locationService;
            _locationService.Subscribe(this);

            _tourRequestService = tourRequestService;
            _tourRequestService.Subscribe(this);

            _complexTourService = complexTourService;
            _complexTourService.Subscribe(this);

            cbCountryItemsSource = new ObservableCollection<string>();
            cbCityItemsSource = new ObservableCollection<string>();

            LoggedInUser = user;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            ComplexTour = new ComplexTour(LoggedInUser.Id);
            TourRequestLocations = new ObservableCollection<Location>();

            InitializeCountryDropdown();
            InitializeTimeComboBoxes();

            SubmitRequestCommand = new RelayCommand(Execute_SubmitRequestCommand, CanExecute_SubmitRequestCommand);
            SubmitComplexTourRequestCommand = new RelayCommand(Execute_SubmitComplexTourRequestCommand, CanExecute_SubmitComplexTourRequestCommand);
            ExitCommand = new RelayCommand(Execute_ExitCommand);
            CountrySelectionChangedCommand = new RelayCommand(Execute_CountrySelectionChangedCommand);

            app = (App)Application.Current;

            counter = 0;
        }

        private bool CanExecute_SubmitComplexTourRequestCommand(object obj)
        {
            return ComplexTour.TourRequestIds.Count() > 1;
        }

        private bool CanExecute_SubmitRequestCommand(object obj)
        {

            return Country != null && 
                City != null && 
                Description != null && Description != "" &&
                Language != null && Language != "" &&
                MaxGuests != null && MaxGuests != "" && !MaxGuests.StartsWith("-") &&
                EndDate > StartDate;
        }

        private void Execute_SubmitComplexTourRequestCommand(object obj)
        {

            _complexTourService.Save(ComplexTour);
            MessageBox.Show("Successfully submitted a complex tour!");
            CloseAction();
        }



        private void Execute_SubmitRequestCommand(object obj)
        {
            CreateTourRequest();
            ClearPropertyInputs();
        }

        private void CreateTourRequest()
        {
            Location tourLocation = _locationService.GetLocation(Country, City);
            TourRequest tourRequest = new TourRequest(
                tourLocation.Id,
                Description,
                Language,
                int.Parse(MaxGuests),
                StartDateTime,
                EndDateTime,
                LoggedInUser.Id,
                true
                );

            TourRequest savedRequest = _tourRequestService.Save(tourRequest);

            ComplexTour.TourRequestIds.Add(savedRequest.Id);
            ComplexTour.AvailableTourRequestIds.Add(savedRequest.Id);

            TourRequestLocations.Add(tourLocation);

            MessageBox.Show("Successfully submitted a tour!");

            counter++;
        }

        private bool IsRegularTourCountValid()
        {

            MessageBox.Show("USAO U METODU IS VALID");

            if (ComplexTour.TourRequestIds.Count == 0)
            {
                MessageBox.Show("USAO U IF == 0");


                if (app.Lang == ENG)
                {
                    MessageBox.Show("Please add at least one tour request!", "Tour request warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                if (app.Lang == SRB)
                {
                    MessageBox.Show("Molim Vas dodajte barem jedan zahtev za turu!", "Upozorenje o zahtevu za turu", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                return false;
            }

            if (ComplexTour.TourRequestIds.Count == 1)
            {
                if (app.Lang == ENG)
                {
                    MessageBox.Show("Complex tours consist of more than one regular tour!", "Tour requests warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                if (app.Lang == SRB)
                {
                    MessageBox.Show("Složene ture se sastoje od više od jedne obične ture!", "Upozorenje o zahtevima za turu", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                return false;
            }

            return true;
        }

        private void ClearPropertyInputs()
        {
            Description = "";
            Language = "";
            MaxGuests = "";
            Country = null;
            City = null;
        }

        private void Execute_ExitCommand(object obj)
        {
            CloseAction();
        }

        private void InitializeTimeComboBoxes()
        {
            StartHours = new ObservableCollection<string>();
            StartMinutes = new ObservableCollection<string>();
            EndHours = new ObservableCollection<string>();
            EndMinutes = new ObservableCollection<string>();

            for (int i = 0; i < 24; i++)
            {
                string hour = i.ToString("D2");
                StartHours.Add(hour);
            }
            for (int i = 0; i < 60; i++)
            {
                string minute = i.ToString("D2");
                StartMinutes.Add(minute);
            }
            for (int i = 0; i < 24; i++)
            {
                string hour = i.ToString("D2");
                EndHours.Add(hour);
            }
            for (int i = 0; i < 60; i++)
            {
                string minute = i.ToString("D2");
                EndMinutes.Add(minute);
            }
        }

        private void InitializeCountryDropdown()
        {
            cbCountryItemsSource = new ObservableCollection<String>(_locationService.GetCountries().OrderBy(c => c));
        }

        public void InitializeCityDropdown()
        {
            ClearCityItems();
            LoadCitiesForSelectedCountry();
        }

        private void Execute_CountrySelectionChangedCommand(object obj)
        {
            InitializeCityDropdown();
        }

        public void ClearCityItems()
        {
            if (cbCityItemsSource != null)
            {
                cbCityItemsSource.Clear();
            }
        }

        public void ClearCountryItems()
        {
            if (cbCountryItemsSource != null)
            {
                cbCountryItemsSource.Clear();
            }
        }

        public void LoadCitiesForSelectedCountry()
        {
            foreach (string city in _locationService.GetCitiesByCountry(Country).OrderBy(c => c))
            {
                cbCityItemsSource.Add(city);
            }
        }
    }
}
