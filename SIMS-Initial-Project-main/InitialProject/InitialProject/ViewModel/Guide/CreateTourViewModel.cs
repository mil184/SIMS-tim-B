using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Service;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InitialProject.ViewModel.Guide
{
    public class CreateTourViewModel : INotifyPropertyChanged
    {
        #region MAIN
        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointService _checkpointService;
        private readonly TourRequestService _tourRequestService;
        public CreateTourViewModel(User user, TourService tourService, LocationService locationService, ImageRepository imageRepository, CheckpointService checkpointService, TourRequestService tourRequestService, TourRequest request)
        {
            _tourService = tourService;
            _locationService = locationService;
            _imageRepository = imageRepository;
            _checkpointService = checkpointService;
            _tourRequestService = tourRequestService;   

            InitializeCollections();
            InitializeComboboxes();


            Enable();

            IsDemo = false; 
            StopDemo = false;

            if (IsDemo)
                StartDemoAsync();

            //LoggedInUser = user;
 

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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region BasicInfo
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

        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        private void InitializeCountries()
        {
            foreach (var country in _locationService.GetCountries().OrderBy(c => c))
            {
                Countries.Add(country);
            }
        }
        public void UpdateCityCoboBox()
        {
                if (!IsDemo)
                IsEnabledCityComboBox = true;

                Cities.Clear();
                foreach (string city in _locationService.GetCitiesByCountry(TourCountry).OrderBy(c => c))
                {
                    Cities.Add(city);
                }
            
        }
 
        private bool _isCountryComboBoxDropDown;
        public bool IsCountryComboBoxDropDown
        {
            get => _isCountryComboBoxDropDown;
            set
            {
                if (value != _isCountryComboBoxDropDown)
                {
                    _isCountryComboBoxDropDown = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isCityComboBoxDropDown;
        public bool IsCityComboBoxDropDown
        {
            get => _isCityComboBoxDropDown;
            set
            {
                if (value != _isCityComboBoxDropDown)
                {
                    _isCityComboBoxDropDown = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region DateTimes
        public ObservableCollection<DateTime> TourDates { get; set; }

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
        private bool _isDatePickerFocused;
        public bool IsDatePickerFocused
        {
            get { return _isDatePickerFocused; }
            set
            {
                if (_isDatePickerFocused != value)
                {
                    _isDatePickerFocused = value;
                    OnPropertyChanged(nameof(IsDatePickerFocused));
                }
            }
        }

        private bool _isHoursComboBoxFocused;
        public bool IsHoursComboBoxFocused
        {
            get { return _isHoursComboBoxFocused; }
            set
            {
                if (_isHoursComboBoxFocused != value)
                {
                    _isHoursComboBoxFocused = value;
                    OnPropertyChanged(nameof(IsHoursComboBoxFocused));
                }
            }
        }

        private bool _isMinutesComboBoxFocused;
        public bool IsMinutesComboBoxFocused
        {
            get { return _isMinutesComboBoxFocused; }
            set
            {
                if (_isMinutesComboBoxFocused != value)
                {
                    _isMinutesComboBoxFocused = value;
                    OnPropertyChanged(nameof(IsMinutesComboBoxFocused));
                }
            }
        }

        private bool _isDatePickerDropDown;
        public bool IsDatePickerDropDown
        {
            get { return _isDatePickerDropDown; }
            set
            {
                if (_isDatePickerDropDown != value)
                {
                    _isDatePickerDropDown = value;
                    OnPropertyChanged(nameof(IsDatePickerDropDown));
                }
            }
        }

        private bool _isHoursComboBoxDropDown;
        public bool IsHoursComboBoxDropDown
        {
            get { return _isHoursComboBoxDropDown; }
            set
            {
                if (_isHoursComboBoxDropDown != value)
                {
                    _isHoursComboBoxDropDown = value;
                    OnPropertyChanged(nameof(IsHoursComboBoxDropDown));
                }
            }
        }

        private bool _isMinutesComboBoxDropDown;
        public bool IsMinutesComboBoxDropDown
        {
            get { return _isMinutesComboBoxDropDown; }
            set
            {
                if (_isMinutesComboBoxDropDown != value)
                {
                    _isMinutesComboBoxDropDown = value;
                    OnPropertyChanged(nameof(IsMinutesComboBoxDropDown));
                }
            }
        }

        public ObservableCollection<string> Hours { get; set; }
        public ObservableCollection<string> Minutes { get; set; }
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
        public void AddDateTime() 
        {
            DateTime time = new DateTime(SelectedDateInDatePicker.Value.Date.Year, SelectedDateInDatePicker.Value.Date.Month, SelectedDateInDatePicker.Value.Date.Day, int.Parse(SelectedHour), int.Parse(SelectedMinute), 0);
            TourDates.Add(time);

            SelectedDateInDatePicker = null;
            SelectedHour = null;
            SelectedMinute = null;

        }

        #endregion

        #region Checkpoints
        public ObservableCollection<Checkpoint> TourCheckpoints { get; set; }

        public int OrderCounter { get; set; }

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
        public void AddCheckpoint()
        {
            Checkpoint checkpoint = new Checkpoint(CheckpointName, ++OrderCounter);
            TourCheckpoints.Add(checkpoint);
            CheckpointName = string.Empty;
        }
        private bool _isCheckpointTextBoxFocused;
        public bool IsCheckpointTextBoxFocused
        {
            get { return _isCheckpointTextBoxFocused; }
            set
            {
                if (_isCheckpointTextBoxFocused != value)
                {
                    _isCheckpointTextBoxFocused = value;
                    OnPropertyChanged(nameof(IsCheckpointTextBoxFocused));
                }
            }
        }
        #endregion

        #region Images
        public ObservableCollection<string> ImageUrls { get; set; }

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
        public int ImageIndex { get; set; }

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
        #endregion

        #region Demo

        private bool _isDemo;
        public bool IsDemo
        {
            get => _isDemo;
            set
            {
                if (value != _isDemo)
                {
                    _isDemo = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _stopDemo;
        public bool StopDemo
        {
            get => _stopDemo;
            set
            {
                if (value != _stopDemo)
                {
                    _stopDemo = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Duration = 25;

        public async void StartDemoAsync()
        {
            Empty();
            Disable();
            IsDemo = true;

            while (true)
            {
                if (StopDemo) break;
                await StartTourNameAnimation();
                if (StopDemo) break;
                await StartTourDescriptionAnimation();
                if (StopDemo) break;
                await StartTourLanguageAnimation();
                if (StopDemo) break;
                await StartTourCountryAnimation();
                if (StopDemo) break;
                await StartTourCityAnimation();
                if (StopDemo) break;
                await StartMaximumGuestsAnimation();
                if (StopDemo) break;
                await StartDurationAnimation();
                if (StopDemo) break;
                await StartDateTimeAnimation();
                if (StopDemo) break;
                await StartTourCheckpointsAnimation();
                if (StopDemo) break;
                await StartTourImagesAnimation();
                if (StopDemo) break;
                await ClearAll();
                if (StopDemo) break;
            }
            IsDemo = false;
            StopDemo = false;
            Empty();
            Enable();
        }

        private void Empty() 
        {
            TourName = string.Empty;
            TourDescription = string.Empty;
            TourLanguage = string.Empty;
            TourCountry = null;
            TourCity = null;
            MaximumGuests = string.Empty;
            TourDuration = string.Empty;
            SelectedDateInDatePicker = null;
            SelectedHour = string.Empty;
            SelectedMinute = string.Empty;
            TourDates.Clear();
            CheckpointName = string.Empty;
            TourCheckpoints.Clear();
            ImageUrl = string.Empty;
            ImageUrls.Clear();
            OrderCounter = 0;
            ImageSource = new BitmapImage(new Uri("/Resources/Images/image_unavailable.png", UriKind.Relative));
        }
        private void Disable()
        {
            IsEnabledTourNameTextBox = false;
            IsEnabledTourDescriptionTextBox = false;
            IsEnabledTourLanguageTextBox = false;
            IsEnabledCountryComboBox = false;
            IsEnabledCityComboBox = false;
            IsEnabledMaximumGuestsTextBox = false;
            IsEnabledTourDurationTextBox = false;
            IsEnabledDatePicker = false;
            IsEnabledHoursComboBox = false;
            IsEnabledMinutesComboBox = false;
            IsEnabledTourDatesListBox = false;
            IsEnabledCheckpointNameTextBox = false;
            IsEnabledTourCheckpointsListBox = false;
            IsEnabledImageUrlTextBox = false;
            IsEnabledButton = false;

        }
        private void Enable()
        {
            IsEnabledTourNameTextBox = true;
            IsEnabledTourDescriptionTextBox = true;
            IsEnabledTourLanguageTextBox = true;
            IsEnabledCountryComboBox = true;
            IsEnabledCityComboBox = true;
            IsEnabledMaximumGuestsTextBox = true;
            IsEnabledTourDurationTextBox = true;
            IsEnabledDatePicker = true;
            IsEnabledHoursComboBox = true;
            IsEnabledMinutesComboBox = true;
            IsEnabledTourDatesListBox = true;
            IsEnabledCheckpointNameTextBox = true;
            IsEnabledTourCheckpointsListBox = true;
            IsEnabledImageUrlTextBox = true;
            IsEnabledButton = true;

        }
        private async Task StartTourNameAnimation()
        {
            string targetText = "Osaka Food and Culture";
            foreach (char c in targetText)
            {
                TourName += c;
                await Task.Delay(Duration);
            }

            await Task.Delay(Duration*10);
        }
        private async Task StartTourDescriptionAnimation()
        {
            string targetText = "Explore the unique culture and delicious cuisine of Osaka, known as Japan's \"Kitchen\", with a local guide who will take you to the best hidden food spots and show you the city's historic landmarks.";

            foreach (char c in targetText)
            {
                TourDescription += c;
                await Task.Delay(Duration);
            }

            await Task.Delay(Duration*10);
        }
        private async Task StartTourLanguageAnimation()
        {
            string targetText = "Japanese";

            foreach (char c in targetText)
            {
                TourLanguage += c;
                await Task.Delay(Duration);
            }

            await Task.Delay(Duration*10);
        }
        private async Task StartTourCountryAnimation()
        {

            IsCountryComboBoxDropDown = true;
            TourCountry = "Japan";

            await Task.Delay(Duration*25);

            IsCountryComboBoxDropDown = false;

            await Task.Delay(Duration*10);
        }
        private async Task StartTourCityAnimation()
        {

            IsCityComboBoxDropDown = true;
            TourCity = "Osaka";

            await Task.Delay(Duration * 25);

            IsCityComboBoxDropDown = false;

            await Task.Delay(Duration*10);
        }
        private async Task StartMaximumGuestsAnimation()
        {
            string targetText = "125";

            foreach (char c in targetText)
            {
                MaximumGuests += c;
                await Task.Delay(Duration);
            }

            await Task.Delay(Duration*10);
        }
        private async Task StartDurationAnimation()
        {
            string targetText = "12,5";

            foreach (char c in targetText)
            {
                TourDuration += c;
                await Task.Delay(Duration);
            }

            await Task.Delay(Duration*10);
        }
        private async Task StartDateTimeAnimation()
        {
            IsDatePickerDropDown = true;
            await Task.Delay(Duration * 20);
            SelectedDateInDatePicker = new DateTime(2023, 6, 29);
            await Task.Delay(Duration * 20);
            IsDatePickerDropDown = false;
            await Task.Delay(Duration * 20);

            IsHoursComboBoxDropDown = true;
            await Task.Delay(Duration * 10);
            SelectedHour = "12";
            await Task.Delay(Duration * 10);
            IsHoursComboBoxDropDown = false;
            await Task.Delay(Duration * 10);

            IsMinutesComboBoxDropDown = true;
            await Task.Delay(Duration  *10);
            SelectedMinute = "30";
            await Task.Delay(Duration * 10);
            IsMinutesComboBoxDropDown = false;
            await Task.Delay(Duration * 10);

            AddDateTime();
            await Task.Delay(Duration*10);

            IsDatePickerDropDown = true;
            await Task.Delay(Duration * 10);
            SelectedDateInDatePicker = new DateTime(2023, 7, 12);
            await Task.Delay(Duration * 10);
            IsDatePickerDropDown = false;
            await Task.Delay(Duration * 10);

            IsHoursComboBoxDropDown = true;
            await Task.Delay(Duration * 10);
            SelectedHour = "19";
            await Task.Delay(Duration * 10);
            IsHoursComboBoxDropDown = false;
            await Task.Delay(Duration *10);

            IsMinutesComboBoxDropDown = true;
            await Task.Delay(Duration * 10);
            SelectedMinute = "11";
            await Task.Delay(Duration * 10);
            IsMinutesComboBoxDropDown = false;
            await Task.Delay(Duration * 10);

            AddDateTime();
            await Task.Delay(Duration * 20);

        }
        private async Task StartTourCheckpointsAnimation()
        {
            foreach (char c in "Dotonbori Bridge")
            {
                CheckpointName += c;
                await Task.Delay(Duration);
            }
            await Task.Delay(Duration*10);

            AddCheckpoint();


            foreach (char c in "Kuromon Ichiba Market")
            {
                CheckpointName += c;
                await Task.Delay(Duration);
            }
            await Task.Delay(Duration*10);

            AddCheckpoint();


            foreach (char c in "Shitennoji Temple")
            {
                CheckpointName += c;
                await Task.Delay(Duration);
            }
            await Task.Delay(Duration*10);

            AddCheckpoint();


            foreach (char c in "Osaka Castle Park")
            {
                CheckpointName += c;
                await Task.Delay(Duration);
            }
            await Task.Delay(Duration*10);

            AddCheckpoint();

            await Task.Delay(Duration * 20);
        }
        private async Task StartTourImagesAnimation()
        {
            ImageUrl = "https://planetofhotels.com/guide/sites/default/files/styles/paragraph__hero_banner__hb_image__1880bp/public/hero_banner/shitennoji-temple_optimized.jpg";
            await Task.Delay(Duration*50);
            AddImageUrl();
            await Task.Delay(Duration * 150);

        }
        private async Task ClearAll()
        {
            Empty();
            await Task.Delay(Duration * 10);
        }
        #endregion

        #region IsEnabledProperties
        private bool _isEnabledTourNameTextBox;
        public bool IsEnabledTourNameTextBox
        {
            get => _isEnabledTourNameTextBox;
            set
            {
                if (value != _isEnabledTourNameTextBox)
                {
                    _isEnabledTourNameTextBox = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledTourDescriptionTextBox;
        public bool IsEnabledTourDescriptionTextBox
        {
            get => _isEnabledTourDescriptionTextBox;
            set
            {
                if (value != _isEnabledTourDescriptionTextBox)
                {
                    _isEnabledTourDescriptionTextBox = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledTourLanguageTextBox;
        public bool IsEnabledTourLanguageTextBox
        {
            get => _isEnabledTourLanguageTextBox;
            set
            {
                if (value != _isEnabledTourLanguageTextBox)
                {
                    _isEnabledTourLanguageTextBox = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledCountryComboBox;
        public bool IsEnabledCountryComboBox
        {
            get => _isEnabledCountryComboBox;
            set
            {
                if (value != _isEnabledCountryComboBox)
                {
                    _isEnabledCountryComboBox = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledCityComboBox;
        public bool IsEnabledCityComboBox
        {
            get => _isEnabledCityComboBox;
            set
            {
                if (value != _isEnabledCityComboBox)
                {
                    _isEnabledCityComboBox = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledMaximumGuestsTextBox;
        public bool IsEnabledMaximumGuestsTextBox
        {
            get => _isEnabledMaximumGuestsTextBox;
            set
            {
                if (value != _isEnabledMaximumGuestsTextBox)
                {
                    _isEnabledMaximumGuestsTextBox = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledTourDurationTextBox;
        public bool IsEnabledTourDurationTextBox
        {
            get => _isEnabledTourDurationTextBox;
            set
            {
                if (value != _isEnabledTourDurationTextBox)
                {
                    _isEnabledTourDurationTextBox = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledDatePicker;
        public bool IsEnabledDatePicker
        {
            get => _isEnabledDatePicker;
            set
            {
                if (value != _isEnabledDatePicker)
                {
                    _isEnabledDatePicker = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledHoursComboBox;
        public bool IsEnabledHoursComboBox
        {
            get => _isEnabledHoursComboBox;
            set
            {
                if (value != _isEnabledHoursComboBox)
                {
                    _isEnabledHoursComboBox = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledMinutesComboBox;
        public bool IsEnabledMinutesComboBox
        {
            get => _isEnabledMinutesComboBox;
            set
            {
                if (value != _isEnabledMinutesComboBox)
                {
                    _isEnabledMinutesComboBox = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledTourDatesListBox;
        public bool IsEnabledTourDatesListBox
        {
            get => _isEnabledTourDatesListBox;
            set
            {
                if (value != _isEnabledTourDatesListBox)
                {
                    _isEnabledTourDatesListBox = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledCheckpointNameTextBox;
        public bool IsEnabledCheckpointNameTextBox
        {
            get => _isEnabledCheckpointNameTextBox;
            set
            {
                if (value != _isEnabledCheckpointNameTextBox)
                {
                    _isEnabledCheckpointNameTextBox = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledTourCheckpointsListBox;
        public bool IsEnabledTourCheckpointsListBox
        {
            get => _isEnabledTourCheckpointsListBox;
            set
            {
                if (value != _isEnabledTourCheckpointsListBox)
                {
                    _isEnabledTourCheckpointsListBox = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledImageUrlTextBox;
        public bool IsEnabledImageUrlTextBox
        {
            get => _isEnabledImageUrlTextBox;
            set
            {
                if (value != _isEnabledImageUrlTextBox)
                {
                    _isEnabledImageUrlTextBox = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledButton;
        public bool IsEnabledButton
        {
            get => _isEnabledButton;
            set
            {
                if (value != _isEnabledButton)
                {
                    _isEnabledButton = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}
