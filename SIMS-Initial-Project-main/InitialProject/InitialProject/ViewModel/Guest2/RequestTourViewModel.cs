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

namespace InitialProject.ViewModel.Guest2
{
    public class RequestTourViewModel : INotifyPropertyChanged, IObserver
    {
        private readonly UserRepository _userRepository;
        private readonly LocationService _locationService;
        private readonly TourRequestService _tourRequestService;

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

        private int _maxGuests;
        public int MaxGuests
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


        #endregion

        public DateTime StartDateTime => StartDate.Date + new TimeSpan(StartHour, StartMinute, 0);
        public DateTime EndDateTime => EndDate.Date + new TimeSpan(EndHour, EndMinute, 0);

       

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RequestTourViewModel(UserRepository userRepository, LocationService locationService, TourRequestService tourRequestService)
        {
            _userRepository = userRepository;
            _locationService = locationService;
            _tourRequestService = tourRequestService;
            _tourRequestService.Subscribe(this);

            
        }

        private void CreateTourRequest()
        {
            Location tourLocation = _locationService.GetLocation(Country, City);

            TourRequest tourRequest = new TourRequest(
                //tourLocation.Id,
                3,
                Description,
                Language,
                MaxGuests,
                StartDateTime,
                EndDateTime);

            _tourRequestService.Save(tourRequest);
        }

        public void RequestTourButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTourRequest();
        }

        public void Update()
        {
            //
        }

        //private void CbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cbCountry.SelectedItem != null && _locationService != null)
        //    {
        //        ClearCityItems();
        //        EnableCitySelection();
        //        LoadCitiesForSelectedCountry();
        //    }
        //}
        //private void ClearCityItems()
        //{
        //    if (cbCity.Items != null)
        //    {
        //        cbCity.Items.Clear();
        //    }
        //}
        //private void EnableCitySelection()
        //{
        //    cbCity.IsEnabled = true;
        //}
        //private void LoadCitiesForSelectedCountry()
        //{
        //    foreach (string city in _locationService.GetCities(cbCountry.SelectedItem.ToString()).OrderBy(c => c))
        //    {
        //        cbCity.Items.Add(city);
        //    }
        //}
        //private void CbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cbCountry.SelectedItem != null && cbCity.SelectedItem != null)
        //    {
        //        UpdateCountryAndCity();
        //    }
        //}
        //private void UpdateCountryAndCity()
        //{
        //    Country = cbCountry.SelectedItem.ToString();
        //    City = cbCity.SelectedItem.ToString();
        //}

    }
}
