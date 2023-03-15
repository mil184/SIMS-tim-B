using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using Image = InitialProject.Model.Image;
using System.Linq;
using InitialProject.Resources.Observer;
using System.Configuration;
using System.Diagnostics.Metrics;

namespace InitialProject.View.Guide
{
    public partial class CreateTour : Window,INotifyPropertyChanged
    {
        const int NO_TOUR_ASSIGNED = -1;

        private readonly TourRepository _repository;
        private readonly LocationRepository _locationRepository;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointRepository _checkpointRepository;

        private ObservableCollection<int> ImageIds;
        private ObservableCollection<int> CheckpointIds;

        private int OrderCounter;
        private readonly User LoggedInUser;

        private Checkpoint _startCheckpoint;
        public Checkpoint StartCheckpoint
        {
            get { return _startCheckpoint; }
            set
            {
                if (_startCheckpoint != value)
                {
                    _startCheckpoint = value;
                    OnPropertyChanged(nameof(StartCheckpoint));
                }
            }
        }

        private Checkpoint _endCheckpoint;
        public Checkpoint EndCheckpoint
        {
            get { return _endCheckpoint; }
            set
            {
                if (_endCheckpoint != value)
                {
                    _endCheckpoint = value;
                    OnPropertyChanged(nameof(EndCheckpoint));
                }
            }
        }
        public ObservableCollection<Checkpoint> MiddleCheckpoints { get; set; }
        public ObservableCollection<string> ImageUrls { get; set; } 

        private string _name;
        public string TourName
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
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
        public string TourLanguage
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

        private string _duration;
        public string Duration
        {
            get => _duration;
            set
            {
                if (value != _duration)
                {
                    _duration = value;
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
        private string _hours;
        public string Hours
        {
            get => _hours;
            set
            {
                if (value != _hours)
                {
                    _hours = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _minutes;
        public string Minutes
        {
            get => _minutes;
            set
            {
                if (value != _minutes)
                {
                    _minutes = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateTour(User user, TourRepository _tourRepository, LocationRepository locationRepository, ImageRepository imageRepository, CheckpointRepository checkpointRepository)
        {
            InitializeComponent();
            DataContext = this;

            _repository = _tourRepository;
            _locationRepository = locationRepository;
            _imageRepository =  imageRepository;
            _checkpointRepository = checkpointRepository;

             ImageIds = new ObservableCollection<int>();
             CheckpointIds = new ObservableCollection<int>();
             MiddleCheckpoints = new ObservableCollection<Checkpoint>();
             ImageUrls = new ObservableCollection<string>();

        LoggedInUser = user;

            for (int i = 0; i < 24; i++)
            {
                string hour = i.ToString("D2");
                Hours_cb.Items.Add(hour);
            }
            for (int i = 0; i < 60; i++)
            {
                string minute = i.ToString("D2");
                Minutes_cb.Items.Add(minute);
            }
            foreach (var country in _locationRepository.GetCountries())
            {
                cbCountry.Items.Add(country);
            }

            OrderCounter = 1;
            AddFinalCheckpointButton.IsEnabled = false;

            AddMiddleCheckpointButton.IsEnabled = false;
        }
        private void btnCreateTour_Click(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate = dpDate.SelectedDate.GetValueOrDefault();
            Location TourLocation = _locationRepository.GetLocation(Country, City);

            foreach (string images in ImageUrls)
            {
                ImageIds.Add(_imageRepository.Save(new Image(images)).Id);
            }

            CheckpointIds.Add(_checkpointRepository.Save(StartCheckpoint).Id);

            foreach (Checkpoint checkpoint in MiddleCheckpoints)
            {
                CheckpointIds.Add(_checkpointRepository.Save(checkpoint).Id);
            }
            CheckpointIds.Add(_checkpointRepository.Save(EndCheckpoint).Id);

            Tour tour = new Tour(TourName, TourLocation.Id, Description, TourLanguage, int.Parse(MaxGuests),0, new DateTime(selectedDate.Year,selectedDate.Month,selectedDate.Day,int.Parse(Hours),int.Parse(Minutes),0),int.Parse(Duration), LoggedInUser.Id, ImageIds, CheckpointIds);
            tour = _repository.Save(tour);

            StartCheckpoint.TourId = tour.Id;
            _checkpointRepository.Update(StartCheckpoint);

            foreach (Checkpoint checkpoint in MiddleCheckpoints)
            {
                checkpoint.TourId = tour.Id;
                _checkpointRepository.Update(checkpoint);
            }

            EndCheckpoint.TourId = tour.Id;
            _checkpointRepository.Update(EndCheckpoint);

            Close();
        }
        private void Hours_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Hours = ((ComboBox)sender).SelectedItem.ToString();
        }
        private void Minutes_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Minutes = ((ComboBox)sender).SelectedItem.ToString();
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            string imageUrl = UrlTextBox.Text;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                ImageUrls.Add(imageUrl);
            }
            UrlTextBox.Text = string.Empty;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddStartingCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            StartCheckpoint = new Checkpoint(StartingCheckpointName.Text, 1, false, NO_TOUR_ASSIGNED);

            AddStartingCheckpointButton.IsEnabled = false;
            AddFinalCheckpointButton.IsEnabled = true;
            AddMiddleCheckpointButton.IsEnabled = false;

            StartingCheckpointName.IsEnabled = false;
            FinalCheckpointName.IsEnabled = true;

            StartingCheckpointName.Text = string.Empty;
        }

        private void AddFinalCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            // Add code to handle adding the final checkpoint here
            EndCheckpoint = new Checkpoint(FinalCheckpointName.Text,2, false, NO_TOUR_ASSIGNED);
        
            AddStartingCheckpointButton.IsEnabled = false;
            AddFinalCheckpointButton.IsEnabled = false;
            AddMiddleCheckpointButton.IsEnabled = true;

            FinalCheckpointName.IsEnabled = true;
            MiddleCheckpointName.IsEnabled = true;

            FinalCheckpointName.Text = string.Empty;
        }

        private void AddMiddleCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            // Add code to handle adding a middle checkpoint here

            string middleCheckpointName = MiddleCheckpointName.Text;
            if (!string.IsNullOrEmpty(middleCheckpointName))
            {
                Checkpoint checkpoint = new Checkpoint(MiddleCheckpointName.Text, ++OrderCounter, false, NO_TOUR_ASSIGNED);
                MiddleCheckpoints.Add(checkpoint);
            }
            EndCheckpoint.Order = MiddleCheckpoints.Count() + 2;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndCheckpoint)));

            MiddleCheckpointName.Text = string.Empty;

            AddStartingCheckpointButton.IsEnabled = false;
            AddFinalCheckpointButton.IsEnabled = false;
        }

        private void CbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCountry.SelectedItem != null && _locationRepository != null)
            {
                if(cbCity.Items != null) 
                {
                    cbCity.Items.Clear();
                }

                cbCity.IsEnabled = true;
                foreach (String city in _locationRepository.GetCities(cbCountry.SelectedItem.ToString()))
                {
                    cbCity.Items.Add(city);
                }
            }
        }

        private void CbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCountry.SelectedItem != null && cbCity.SelectedItem != null)
            {
                Country = cbCountry.SelectedItem.ToString();
                City = cbCity.SelectedItem.ToString();
            }
        }
    }
}
