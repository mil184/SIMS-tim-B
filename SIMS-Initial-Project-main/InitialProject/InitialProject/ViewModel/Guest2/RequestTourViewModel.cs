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

namespace InitialProject.ViewModel.Guest2
{
    public class RequestTourViewModel : INotifyPropertyChanged, IObserver
    {
        public Action CloseAction { get; set; }
        public User LoggedInUser { get; set; }

        public ObservableCollection<String> cbCountryItemsSource { get; set; }
        public ObservableCollection<String> cbCityItemsSource { get; set; }

        public ObservableCollection<string> StartHours { get; set; }
        public ObservableCollection<string> StartMinutes { get; set; }
        public ObservableCollection<string> EndHours { get; set; }
        public ObservableCollection<string> EndMinutes { get; set; }

        private readonly LocationService _locationService;
        private readonly TourRequestService _tourRequestService;
        private readonly UserService _userService;

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

        #endregion

        public RelayCommand SubmitRequestCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand ChangeLanguageCommand { get; set; }
        public RelayCommand CountrySelectionChangedCommand { get; set; }

        public int LanguageButtonClickCount { get; set; }
        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RequestTourViewModel(UserService userService, LocationService locationService, TourRequestService tourRequestService, User user, string lang)
        {
            _userService = userService;
            _locationService = locationService;
            _tourRequestService = tourRequestService;
            _tourRequestService.Subscribe(this);

            cbCountryItemsSource = new ObservableCollection<string>();
            cbCityItemsSource = new ObservableCollection<string>();

            LoggedInUser = user;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            InitializeCountryDropdown();
            InitializeTimeComboBoxes();

            SubmitRequestCommand = new RelayCommand(Execute_SubmitRequestCommand, CanExecute_SubmitRequestCommand);
            ExitCommand = new RelayCommand(Execute_ExitCommand);
            ChangeLanguageCommand = new RelayCommand(Execute_ChangeLanguageCommand);
            CountrySelectionChangedCommand = new RelayCommand(Execute_CountrySelectionChangedCommand);

            app = (App)Application.Current;
            app.ChangeLanguage(lang);
            InitializeLanguageButton(lang);
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

        private void InitializeLanguageButton(string lang)
        {
            if (lang == SRB)
            {
                LanguageButtonClickCount = 0;
                return;
            }

            LanguageButtonClickCount = 1;
        }

        private void Execute_SubmitRequestCommand(object obj)
        {
            CreateTourRequest();
            CloseAction();
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

        private void Execute_ChangeLanguageCommand(object obj)
        {
            LanguageButtonClickCount++;

            if (LanguageButtonClickCount % 2 == 1)
            {
                app.ChangeLanguage(ENG);
                return;
            }

            app.ChangeLanguage(SRB);
        }

        private void Execute_ExitCommand(object obj)
        {
            CloseAction();
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
                false
                ) ;

            _tourRequestService.Save(tourRequest);
        }

        public void Update()
        {
            //
        }

        private void InitializeCountryDropdown()
        {
            cbCountryItemsSource = new ObservableCollection<String>(_locationService.GetCountries().OrderBy(c => c));
        }

        private void Execute_CountrySelectionChangedCommand(object obj)
        {
            InitializeCityDropdown();
        }

        public void InitializeCityDropdown()
         {         
            ClearCityItems();
            LoadCitiesForSelectedCountry();     
         }

        public void ClearCityItems()
        {
            if (cbCityItemsSource != null)
            {
                cbCityItemsSource.Clear();
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
