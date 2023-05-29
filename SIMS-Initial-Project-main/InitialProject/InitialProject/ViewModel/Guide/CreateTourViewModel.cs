using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Service;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InitialProject.ViewModel.Guide
{
    public class CreateTourViewModel : INotifyPropertyChanged
    {
        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointService _checkpointService;
        private readonly TourRequestService _tourRequestService;

        private string _tourName;
        public string TourName
        {
            get => _tourName;
            set
            {
                if (value != _tourName)
                {
                    _tourName = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _tourDescription;
        public string TourDescription
        {
            get => _tourDescription;
            set
            {
                if (value != _tourDescription)
                {
                    _tourDescription = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tourLanguage;
        public string TourLanguage
        {
            get => _tourLanguage;
            set
            {
                if (value != _tourLanguage)
                {
                    _tourLanguage = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tourCountry;
        public string TourCountry
        {
            get => _tourCountry;
            set
            {
                if (value != _tourCountry)
                {
                    _tourCountry = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tourCity;
        public string TourCity
        {
            get => _tourCity;
            set
            {
                if (value != _tourCity)
                {
                    _tourCity = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _maximumGuests;
        public string MaximumGuests
        {
            get => _maximumGuests;
            set
            {
                if (value != _maximumGuests)
                {
                    _maximumGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _duration;
        public string TourDuration
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
        private DateTime? _selectedDateInDatePicker;
        public DateTime? SelectedDateInDatePicker
        {
            get => _selectedDateInDatePicker;
            set
            {
                if (value != _selectedDateInDatePicker)
                {
                    _selectedDateInDatePicker = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime? _selectedDateInList;
        public DateTime? SelectedDateInList
        {
            get => _selectedDateInList;
            set
            {
                if (value != _selectedDateInList)
                {
                    _selectedDateInList = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedHour;
        public string SelectedHour
        {
            get => _selectedHour;
            set
            {
                if (value != _selectedHour)
                {
                    _selectedHour = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedMinute;
        public string SelectedMinute
        {
            get => _selectedMinute;
            set
            {
                if (value != _selectedMinute)
                {
                    _selectedMinute = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _checkpointName;
        public string CheckpointName
        {
            get => _checkpointName;
            set
            {
                if (value != _checkpointName)
                {
                    _checkpointName = value;
                    OnPropertyChanged();
                }
            }

        }
        private string _selectedCheckpoint;
        public string SelectedCheckpoint
        {
            get => _selectedCheckpoint;
            set
            {
                if (value != _selectedCheckpoint)
                {
                    _selectedCheckpoint = value;
                    OnPropertyChanged();
                }
            }

        }
        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                if (value != _imageUrl)
                {
                    _imageUrl = value;
                    OnPropertyChanged();
                }
            }
        }
        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }
        public int ImageIndex { get; set; }
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<string> Hours { get; set; }
        public ObservableCollection<string> Minutes { get; set; }

        public ObservableCollection<Checkpoint> TourCheckpoints { get; set; }
        public ObservableCollection<string> ImageUrls { get; set; }
        public ObservableCollection<DateTime> TourDates { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool StopDemo { get; set; }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool _cityComboBoxIsEnabled;
        public bool CityComboBoxIsEnabled
        {
            get => _cityComboBoxIsEnabled;
            set
            {
                if (value != _cityComboBoxIsEnabled)
                {
                    _cityComboBoxIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isCheckpointTextBoxFocused;
        public bool IsCheckpointTextBoxFocused
        {
            get => _isCheckpointTextBoxFocused;
            set
            {
                if (value != _isCheckpointTextBoxFocused)
                {
                    _isCheckpointTextBoxFocused = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isImageTextBoxFocused;
        public bool IsImageTextBoxFocused
        {
            get => _isImageTextBoxFocused;
            set
            {
                if (value != _isImageTextBoxFocused)
                {
                    _isImageTextBoxFocused = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isLeftButtonFocused;
        public bool IsLeftButtonFocused
        {
            get => _isLeftButtonFocused;
            set
            {
                if (value != _isLeftButtonFocused)
                {
                    _isLeftButtonFocused = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isWindowEnabled;
        public bool IsWindowEnabled
        {
            get => _isWindowEnabled;
            set
            {
                if (value != _isWindowEnabled)
                {
                    _isWindowEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public CreateTourViewModel(User user, TourService tourService, LocationService locationService, ImageRepository imageRepository, CheckpointService checkpointService, TourRequestService tourRequestService, TourRequest request, bool isDemo)
        {
            _tourService = tourService;
            _locationService = locationService;
            _imageRepository = imageRepository;
            _checkpointService = checkpointService;
            _tourRequestService = tourRequestService;

            InitializeCollections();
            InitializeComboboxes();

            StopDemo = false;
            if (isDemo)
                StartDemoAsync();

            //LeftBoundary = null;
            //RightBoundary = null;
            //Request = null;

            //LoggedInUser = user;
            //OrderCounter = 0;

            //InitializeCollections();
            //InitializeComboBoxes();
            //InitializeCountryDropdown();
            //InitializeShortcuts();
            //FillWithTestData();
            ImageIndex = 0;


            SetImage(ImageIndex);
        }
        private void InitializeCollections()
        {
            Countries = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
            Hours = new ObservableCollection<string>();
            Minutes = new ObservableCollection<string>();
            TourCheckpoints = new ObservableCollection<Checkpoint>();
            ImageUrls = new ObservableCollection<string>();
            TourDates = new ObservableCollection<DateTime>();
        }
        private void InitializeComboboxes() 
        {
            InitializeCountries();
            InitializeHours();
            InitializeMinutes();
        }
        private void InitializeCountries() 
        {
            foreach (var country in _locationService.GetCountries().OrderBy(c => c))
            {
                Countries.Add(country);
            }
        }
        private void InitializeHours()
        {
            for (int i = 0; i < 24; i++)
            {
                string hour = i.ToString("D2");
                Hours.Add(hour);
            }
        }
        private void InitializeMinutes()
        {
            for (int i = 0; i < 60; i++)
            {
                string minute = i.ToString("D2");
                Minutes.Add(minute);
            }
        }
        public void UpdateCityCoboBox() 
        {
            CityComboBoxIsEnabled = true;
            Cities.Clear();
            foreach (string city in _locationService.GetCitiesByCountry(TourCountry).OrderBy(c => c))
            {
                Cities.Add(city);
            }
        }
        public void AddImageUrl() 
        {
            ImageUrls.Add(ImageUrl);
            ImageUrl = string.Empty;

            if(ImageUrls.Count == 1) 
            {
                SetImage(0);
            }
        }
        public void PreviousImage()
        {
            ImageIndex--;
            if (ImageIndex < 0)
            {
                ImageIndex = ImageUrls.Count - 1;
            }

            SetImage(ImageIndex);
        }
        public void NextImage()
        {
            ImageIndex++;
            if (ImageIndex > ImageUrls.Count - 1)
            {
                ImageIndex = 0;
            }

            SetImage(ImageIndex);
        }
        private void SetImage(int index)
        {
            try
            {
                var fullFilePath = ImageUrls[index];

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                bitmap.EndInit();

                ImageSource = bitmap;
            }
            catch (Exception ex)
            {
                // Handle the exception by setting the Source to a default image
                BitmapImage defaultImage = new BitmapImage(new Uri("/Resources/Images/image_unavailable.png", UriKind.Relative));
                ImageSource = defaultImage;
            }
        }
        private async Task StartDemoAsync() 
        {
            IsWindowEnabled = false;
            await StartTourNameAnimation();
            await StartTourDescriptionAnimation();
            await StartTourLanguageAnimation();
        }

        private async Task StartTourNameAnimation()
        {
            string targetText = "Osaka Food and Culture";
            int durationPerCharacter =  25; // Duration (in milliseconds) for each character to appear
            int delayBetweenCharacters = 25; // Delay (in milliseconds) between each character

            foreach (char c in targetText)
            {
                TourName += c;
                await Task.Delay(durationPerCharacter);
            }

            await Task.Delay(delayBetweenCharacters);
        }
        private async Task StartTourDescriptionAnimation()
        {
            string targetText = "Explore the unique culture and delicious cuisine of Osaka, known as Japan's \"Kitchen\", with a local guide who will take you to the best hidden food spots and show you the city's historic landmarks.";
            int durationPerCharacter = 25; // Duration (in milliseconds) for each character to appear
            int delayBetweenCharacters = 25; // Delay (in milliseconds) between each character

            foreach (char c in targetText)
            {
                TourDescription += c;
                await Task.Delay(durationPerCharacter);
            }

            await Task.Delay(delayBetweenCharacters);
        }
        private async Task StartTourLanguageAnimation()
        {
            string targetText = "Japanese";
            int durationPerCharacter = 25; // Duration (in milliseconds) for each character to appear
            int delayBetweenCharacters = 25; // Delay (in milliseconds) between each character

            foreach (char c in targetText)
            {
                TourLanguage += c;
                await Task.Delay(durationPerCharacter);
            }

            await Task.Delay(delayBetweenCharacters);
        }
    }
}
