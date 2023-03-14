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

namespace InitialProject.View.Guide
{
    public partial class CreateTour : Window,INotifyPropertyChanged
    {
        private readonly TourRepository _repository;
        private readonly LocationRepository _locationRepository;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointRepository _checkpointRepository;

        public ObservableCollection<Checkpoint> MiddleCheckpoints { get; set; }
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

            OrderCounter = 0;
            AddFinalCheckpointButton.IsEnabled = false;

            AddMiddleCheckpointButton.IsEnabled = false;
        }
        private void btnCreateTour_Click(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate = dpDate.SelectedDate.GetValueOrDefault();
            Location TourLocation = new Location(Country, City);
            _locationRepository.Save(TourLocation);
            Tour tour = new Tour(TourName, TourLocation.Id, Description, TourLanguage, int.Parse(MaxGuests),0, new DateTime(selectedDate.Year,selectedDate.Month,selectedDate.Day,int.Parse(Hours),int.Parse(Minutes),0),int.Parse(Duration), LoggedInUser.Id, ImageIds, CheckpointIds);
            tour = _repository.Save(tour);

            EndCheckpoint.Order = MiddleCheckpoints.Count() + 1;
            _checkpointRepository.Update(EndCheckpoint);

            foreach (int checkpointId in CheckpointIds)
            {
                Checkpoint checkpoint = _checkpointRepository.GetById(checkpointId);
                checkpoint.TourId = tour.Id;
                _checkpointRepository.Update(checkpoint); 
            }
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
                Image image = new Image(imageUrl);
                _imageRepository.Save(image);
                ImageIds.Add(image.Id);
            }
            UrlTextBox.Text = string.Empty;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddStartingCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            Checkpoint checkpoint = _checkpointRepository.Save(new Checkpoint(StartingCheckpointName.Text, 0, false, -5));
            CheckpointIds.Add(checkpoint.Id);
            StartCheckpoint = checkpoint;

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
            Checkpoint checkpoint = _checkpointRepository.Save(new Checkpoint(FinalCheckpointName.Text,1, false, -5));
            CheckpointIds.Add(checkpoint.Id);

            EndCheckpoint = checkpoint;
            EndCheckpoint.Order = 1;
        
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
                Checkpoint checkpoint = _checkpointRepository.Save(new Checkpoint(MiddleCheckpointName.Text, ++OrderCounter, false, -5));
                CheckpointIds.Add(checkpoint.Id);
                MiddleCheckpoints.Add(checkpoint);
            }
            MiddleCheckpointName.Text = string.Empty;

            Checkpoint finalcheckpoint = _checkpointRepository.GetById(EndCheckpoint.Id);
            finalcheckpoint.Order = MiddleCheckpoints.Count() + 1;
            EndCheckpoint = finalcheckpoint;
            _checkpointRepository.Update(finalcheckpoint);

            AddStartingCheckpointButton.IsEnabled = false;
            AddFinalCheckpointButton.IsEnabled = false;
        }
    }
}
