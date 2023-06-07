using InitialProject.Commands;
using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.View.Guest2;
using MenuNavigation.Commands;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = InitialProject.Model.Image;

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
        private readonly ComplexTourService _complexTourService;
        public User LoggedInUser { get; set; }
        public TourRequest Request { get; set; }
        public ComplexTour ComplexTour {get; set; }

        public RelayCommand PreviousWindowCloseCommand { get; set; }
        public DateOnly? DateSlot { get; set; }
        public CreateTourViewModel(User user, TourService tourService, LocationService locationService, ImageRepository imageRepository, CheckpointService checkpointService, TourRequestService tourRequestService, TourRequest request, ComplexTour complexTour, ComplexTourService complexTourService, DateOnly? dateSlot, RelayCommand closeCommand)
        {
            _tourService = tourService;
            _locationService = locationService;
            _imageRepository = imageRepository;
            _checkpointService = checkpointService;
            _tourRequestService = tourRequestService;
            _complexTourService = complexTourService;

            LoggedInUser = user;
            DateSlot = dateSlot;

            InitializeCollections();
            InitializeComboboxes();
            InitializeRelayCommands();
            Enable();

            Request = request;
            if (Request != null)
                HandleRequest();

            ComplexTour = complexTour;

            ImageListBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#007ACC"));
            SetImage(ImageIndex);

            PreviousWindowCloseCommand = closeCommand;

            ButtonBackgroundColorAddDate= new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            ButtonBackgroundColorAddImage = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            ButtonBackgroundColorAddCheckpoint = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            ButtonBackgroundColorSave = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
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

        private void HandleRequest() 
        {
            TourDescription = Request.Description;
            TourLanguage = Request.Language;
            MaximumGuests = Request.MaxGuests.ToString();
            TourCountry = _locationService.GetById(Request.LocationId).Country;

            IsEnabledCityComboBox = true;
            CountrySelectionChangedCommand.Execute(null);
            TourCity = _locationService.GetById(Request.LocationId).City;
            if (DateSlot == null)
            {
                SelectedDateInDatePicker = Request.StartTime;
            }
            else 
            {
                SelectedDateInDatePicker = new DateTime(DateSlot.Value.Year, DateSlot.Value.Month, DateSlot.Value.Day,0,0,0);
            }
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
            DateValidation = string.Empty;
            DateBorderColor = new SolidColorBrush(Colors.Transparent);


            if (SelectedDateInDatePicker == null || string.IsNullOrEmpty(SelectedHour) || string.IsNullOrEmpty(SelectedMinute))
            {
                DateValidation = "Please Enter Date And Time Values!";
                DateBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
                return;
            }

            DateTime dateTime = new DateTime(SelectedDateInDatePicker.Value.Date.Year, SelectedDateInDatePicker.Value.Date.Month, SelectedDateInDatePicker.Value.Date.Day, int.Parse(SelectedHour), int.Parse(SelectedMinute), 0);
           
            if (Request != null && DateSlot == null && (dateTime < Request.StartTime || dateTime > Request.EndTime))
            {
                DateValidation = "The Request Interval Is (" + Request.StartTime.Date.ToString() + " - " + Request.EndTime.Date.ToString() + ")";
                DateBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
                return;
            }
            if(DateSlot != null)
            {
                DateTime start = new DateTime(DateSlot.Value.Year, DateSlot.Value.Month, DateSlot.Value.Day, 0, 0, 0);
                DateTime end = new DateTime(DateSlot.Value.Year, DateSlot.Value.Month, DateSlot.Value.Day, 23, 59, 59);

                if (dateTime < start || dateTime > end){
                    DateValidation = "The Request Interval Is (" + start.ToString() + " - " + end.ToString() + ")";
                    DateBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
                    return;
                }
 
            }
            if (dateTime < DateTime.Now)
            {
                DateValidation = "Please Enter Correct Date Value! (After Today)";
                DateBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
                return;
            }

            TourDates.Add(dateTime);

            //SelectedDateInDatePicker = null;
            //SelectedHour = null;
            //SelectedMinute = null;

        }
        public void RemoveSelectedDate() 
        {
            TourDates.Remove(SelectedDateInList.Value);
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
        private Checkpoint _selectedCheckpoint;
        public Checkpoint SelectedCheckpoint
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
            if (string.IsNullOrEmpty(CheckpointName))
                return;
                
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
        public void RemoveSelectedCheckpoint()
        {
            TourCheckpoints.Remove(SelectedCheckpoint);
            OrderCounter--;

            int counter = 0;

            List<Checkpoint> newCheckpoints = new List<Checkpoint>();

            foreach(Checkpoint c in TourCheckpoints) 
            {
                c.Order = ++counter;
                newCheckpoints.Add(c);
            }

            TourCheckpoints.Clear();

            foreach(Checkpoint c in newCheckpoints) 
            {
                TourCheckpoints.Add(c);
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
        private string _currentImageUrl;
        public string CurrentImageUrl
        {
            get => _currentImageUrl;
            set
            {
                if (value != _currentImageUrl)
                {
                    _currentImageUrl = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _currentImageIndexToShow;
        public string CurrentImageIndexToShow
        {
            get => _currentImageIndexToShow;
            set
            {
                if (value != _currentImageIndexToShow)
                {
                    _currentImageIndexToShow = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _imageIndex;

        public int ImageIndex
        {
            get => _imageIndex;
            set
            {
                if (_imageIndex != value)
                {
                    _imageIndex = value;
                    OnPropertyChanged(nameof(ImageIndex));
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
        public void AddImageUrl() 
        {
            if (string.IsNullOrEmpty(ImageUrl))
                return;

            ImageUrls.Add(ImageUrl);
            ImageUrl = string.Empty;

            if(ImageUrls.Count == 1) 
            {
                SetImage(0);
            }
            else 
            {
                SetImage(ImageUrls.Count -1);
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

                if (ImageUrls.Count != 0)
                {
                    int number = ++index;
                    CurrentImageIndexToShow = number.ToString();
                    CurrentImageUrl = fullFilePath;
                }
           
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

                if (ImageUrls.Count == 0)
                {
                    CurrentImageIndexToShow = string.Empty;
                    CurrentImageUrl = string.Empty;
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

        public void RemoveSelectedImage() 
        {
            ImageUrls.Remove(CurrentImageUrl);
            NextImage();
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
            ClearAllValidations();
            IsDemo = true;

            while (true)
            {

                string targetText = "Osaka Food and Culture";
                foreach (char c in targetText)
                {
                    if (StopDemo) break;
                    TourName += c;
                    await Task.Delay(Duration);
                }

                targetText = "Explore the unique culture and delicious cuisine of Osaka, known as Japan's \"Kitchen\", with a local guide who will take you to the best hidden food spots and show you the city's historic landmarks.";

                foreach (char c in targetText)
                {
                    if (StopDemo) break;
                    TourDescription += c;
                    await Task.Delay(Duration);
                }

                await Task.Delay(Duration * 10);

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
            IsEnabledCityComboBox = false;
            MaximumGuests = string.Empty;
            TourDuration = string.Empty;
            SelectedDateInDatePicker = null;
            SelectedHour = null;
            SelectedMinute = null;
            TourDates.Clear();
            CheckpointName = string.Empty;
            TourCheckpoints.Clear();
            ImageUrl = string.Empty;
            CurrentImageIndexToShow = string.Empty;
            CurrentImageUrl = string.Empty;
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
            IsEnabledCityComboBox = false;
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

            await DateButton();
 
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

            await DateButton();
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

            await CheckpointButton();
            AddCheckpoint();


            foreach (char c in "Kuromon Ichiba Market")
            {
                CheckpointName += c;
                await Task.Delay(Duration);
            }
            await Task.Delay(Duration*10);
            await CheckpointButton();
            AddCheckpoint();


            foreach (char c in "Shitennoji Temple")
            {
                CheckpointName += c;
                await Task.Delay(Duration);
            }
            await Task.Delay(Duration*10);
            await CheckpointButton();
            AddCheckpoint();


            foreach (char c in "Osaka Castle Park")
            {
                CheckpointName += c;
                await Task.Delay(Duration);
            }
            await Task.Delay(Duration*10);
            await CheckpointButton();
            AddCheckpoint();

            await Task.Delay(Duration * 20);
        }
        private async Task StartTourImagesAnimation()
        {
            ImageUrl = "https://planetofhotels.com/guide/sites/default/files/styles/paragraph__hero_banner__hb_image__1880bp/public/hero_banner/shitennoji-temple_optimized.jpg";
            await Task.Delay(Duration*25);
            await ImageButton();
            AddImageUrl();
            await Task.Delay(Duration * 150);
            await SaveButton();
        }
        private async Task ImageButton()
        {
            ButtonBackgroundColorAddImage = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00415E"));
            await Task.Delay(200);
            ButtonBackgroundColorAddImage = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
        }
        private async Task DateButton()
        {
            ButtonBackgroundColorAddDate = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00415E"));
            await Task.Delay(200);
            ButtonBackgroundColorAddDate = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
        }
        private async Task CheckpointButton()
        {
            ButtonBackgroundColorAddCheckpoint = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00415E"));
            await Task.Delay(200);
            ButtonBackgroundColorAddCheckpoint = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
        }
        private async Task SaveButton()
        {
            ButtonBackgroundColorSave = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00415E"));
            await Task.Delay(200);
            ButtonBackgroundColorSave = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
        }
        private async Task ClearAll()
        {
            Empty();
            await Task.Delay(Duration * 10);
        }

        private SolidColorBrush _buttonBackgroundColorAddImage;
        public SolidColorBrush ButtonBackgroundColorAddImage
        {
            get { return _buttonBackgroundColorAddImage; }
            set
            {
                _buttonBackgroundColorAddImage = value;
                OnPropertyChanged(nameof(ButtonBackgroundColorAddImage));
            }
        }
        private SolidColorBrush _buttonBackgroundColorAddDate;
        public SolidColorBrush ButtonBackgroundColorAddDate
        {
            get { return _buttonBackgroundColorAddDate; }
            set
            {
                _buttonBackgroundColorAddDate = value;
                OnPropertyChanged(nameof(ButtonBackgroundColorAddDate));
            }
        }
        private SolidColorBrush _buttonBackgroundColorAddCheckpoint;
        public SolidColorBrush ButtonBackgroundColorAddCheckpoint
        {
            get { return _buttonBackgroundColorAddCheckpoint; }
            set
            {
                _buttonBackgroundColorAddCheckpoint = value;
                OnPropertyChanged(nameof(ButtonBackgroundColorAddCheckpoint));
            }
        }
        private SolidColorBrush _buttonBackgroundColorSave;
        public SolidColorBrush ButtonBackgroundColorSave
        {
            get { return _buttonBackgroundColorSave; }
            set
            {
                _buttonBackgroundColorSave = value;
                OnPropertyChanged(nameof(ButtonBackgroundColorSave));
            }
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

        #region SaveTour

        private ObservableCollection<int> SaveImages() 
        {
            ObservableCollection<int> ids = new ObservableCollection<int>();
            foreach (string url in ImageUrls) 
            {
                ids.Add(_imageRepository.Save(new Image(url)).Id);
            }
            return ids;
        }
        private ObservableCollection<int> SaveCheckpoints()
        {
            ObservableCollection<int> ids = new ObservableCollection<int>();
            foreach (Checkpoint checkpoint in TourCheckpoints)
            {
               ids.Add(_checkpointService.Save(checkpoint).Id);
            }
            return ids;
        }
        private Location GetLocation() 
        {
            return _locationService.GetLocation(TourCountry, TourCity);
        }
        public void Save() 
        {

            if (!Validate()) 
            {
                return;
            }
            foreach(DateTime date in TourDates) 
            {

                ObservableCollection<int> imageIds = new ObservableCollection<int>();
                ObservableCollection<int> checkpointIds = new ObservableCollection<int>();

                imageIds = SaveImages();
                checkpointIds = SaveCheckpoints();

                Tour tour = new Tour(TourName, GetLocation().Id, TourDescription, TourLanguage, int.Parse(MaximumGuests), date, double.Parse(TourDuration), LoggedInUser.Id, imageIds, checkpointIds);
                Tour savedTour = _tourService.Save(tour);
                UpdateCheckpoints(savedTour, savedTour.CheckpointIds);

                if (Request != null)
                    UpdateRequest();

                if (ComplexTour != null && DateSlot != null)
                    UpdateComplexTour(savedTour);

                if (PreviousWindowCloseCommand != null) PreviousWindowCloseCommand.Execute(null);

               // CancelCommand.Execute(null);
            }
        }
        private void UpdateCheckpoints(Tour tour, ObservableCollection<int> checkpointIds) 
        {
            foreach(int id in checkpointIds) 
            {
                Checkpoint checkpoint = _checkpointService.GetById(id);
                checkpoint.TourId = tour.Id;
                _checkpointService.Update(checkpoint);
            }
        }
        private void UpdateRequest()
        {
            Request.GuideId = LoggedInUser.Id;
            Request.Status = Resources.Enums.RequestStatus.accepted;
            Request.ChosenDate = TourDates[0];
            _tourRequestService.Update(Request);
        }
        private void UpdateComplexTour(Tour tour)
        {
            ComplexTour.AcceptedTourIdsByGuideIds.Add(LoggedInUser.Id, tour.Id);
            ComplexTour.AvailableTourRequestIds.Remove(Request.Id);
            _complexTourService.Update(ComplexTour);
        }
        #endregion

        #region Validation

        private string _tourNameValidation;
        public string TourNameValidation
        {
            get => _tourNameValidation;
            set
            {
                if (value != _tourNameValidation)
                {
                    _tourNameValidation = value;
                    OnPropertyChanged();
                }
            }
        }
        private SolidColorBrush _tourNameBorderColor;
        public SolidColorBrush TourNameBorderColor
        {
            get => _tourNameBorderColor;
            set
            {
                if (_tourNameBorderColor != value)
                {
                    _tourNameBorderColor = value;
                    OnPropertyChanged(nameof(TourNameBorderColor));
                }
            }
        }
        private string _tourDescriptionValidation;
        public string TourDescriptionValidation
        {
            get => _tourDescriptionValidation;
            set
            {
                if (value != _tourDescriptionValidation)
                {
                    _tourDescriptionValidation = value;
                    OnPropertyChanged(nameof(TourDescriptionValidation));
                }
            }
        }
        private SolidColorBrush _tourDescriptionBorderColor;
        public SolidColorBrush TourDescriptionBorderColor
        {
            get => _tourDescriptionBorderColor;
            set
            {
                if (_tourDescriptionBorderColor != value)
                {
                    _tourDescriptionBorderColor = value;
                    OnPropertyChanged(nameof(TourDescriptionBorderColor));
                }
            }
        }
        private string _tourLanguageValidation;
        public string TourLanguageValidation
        {
            get => _tourLanguageValidation;
            set
            {
                if (value != _tourLanguageValidation)
                {
                    _tourLanguageValidation = value;
                    OnPropertyChanged(nameof(TourLanguageValidation));
                }
            }
        }
        private SolidColorBrush _tourLanguageBorderColor;
        public SolidColorBrush TourLanguageBorderColor
        {
            get => _tourLanguageBorderColor;
            set
            {
                if (_tourLanguageBorderColor != value)
                {
                    _tourLanguageBorderColor = value;
                    OnPropertyChanged(nameof(TourLanguageBorderColor));
                }
            }
        }

        private string _tourCountryValidation;
        public string TourCountryValidation
        {
            get => _tourCountryValidation;
            set
            {
                if (value != _tourCountryValidation)
                {
                    _tourCountryValidation = value;
                    OnPropertyChanged(nameof(TourCountryValidation));
                }
            }
        }
        private SolidColorBrush _tourCountryBorderColor;
        public SolidColorBrush TourCountryBorderColor
        {
            get => _tourCountryBorderColor;
            set
            {
                if (_tourCountryBorderColor != value)
                {
                    _tourCountryBorderColor = value;
                    OnPropertyChanged(nameof(TourCountryBorderColor));
                }
            }
        }

        private string _tourCityValidation;
        public string TourCityValidation
        {
            get => _tourCityValidation;
            set
            {
                if (value != _tourCityValidation)
                {
                    _tourCityValidation = value;
                    OnPropertyChanged(nameof(TourCityValidation));
                }
            }
        }
        private SolidColorBrush _tourCityBorderColor;
        public SolidColorBrush TourCityBorderColor
        {
            get => _tourCityBorderColor;
            set
            {
                if (_tourCityBorderColor != value)
                {
                    _tourCityBorderColor = value;
                    OnPropertyChanged(nameof(TourCityBorderColor));
                }
            }
        }

        private string _maximumGuestsValidation;
        public string MaximumGuestsValidation
        {
            get => _maximumGuestsValidation;
            set
            {
                if (value != _maximumGuestsValidation)
                {
                    _maximumGuestsValidation = value;
                    OnPropertyChanged(nameof(MaximumGuestsValidation));
                }
            }
        }
        private SolidColorBrush _maximumGuestsBorderColor;
        public SolidColorBrush MaximumGuestsBorderColor
        {
            get => _maximumGuestsBorderColor;
            set
            {
                if (_maximumGuestsBorderColor != value)
                {
                    _maximumGuestsBorderColor = value;
                    OnPropertyChanged(nameof(MaximumGuestsBorderColor));
                }
            }
        }

        private string _durationValidation;
        public string DurationValidation
        {
            get => _durationValidation;
            set
            {
                if (value != _durationValidation)
                {
                    _durationValidation = value;
                    OnPropertyChanged(nameof(DurationValidation));
                }
            }
        }
        private SolidColorBrush _durationBorderColor;
        public SolidColorBrush DurationBorderColor
        {
            get => _durationBorderColor;
            set
            {
                if (_durationBorderColor != value)
                {
                    _durationBorderColor = value;
                    OnPropertyChanged(nameof(DurationBorderColor));
                }
            }
        }
        private string _dateValidation;
        public string DateValidation
        {
            get => _dateValidation;
            set
            {
                if (value != _dateValidation)
                {
                    _dateValidation = value;
                    OnPropertyChanged(nameof(DateValidation));
                }
            }
        }
        private SolidColorBrush _dateBorderColor;
        public SolidColorBrush DateBorderColor
        {
            get => _dateBorderColor;
            set
            {
                if (_dateBorderColor != value)
                {
                    _dateBorderColor = value;
                    OnPropertyChanged(nameof(DateBorderColor));
                }
            }
        }
        private string _dateListValidation;
        public string DateListValidation
        {
            get => _dateListValidation;
            set
            {
                if (value != _dateListValidation)
                {
                    _dateListValidation = value;
                    OnPropertyChanged(nameof(DateListValidation));
                }
            }
        }
        private SolidColorBrush _dateListBorderColor;
        public SolidColorBrush DateListBorderColor
        {
            get => _dateListBorderColor;
            set
            {
                if (_dateListBorderColor != value)
                {
                    _dateListBorderColor = value;
                    OnPropertyChanged(nameof(DateListBorderColor));
                }
            }
        }
        private string _checkpointListValidation;
        public string CheckpointListValidation
        {
            get => _checkpointListValidation;
            set
            {
                if (value != _checkpointListValidation)
                {
                    _checkpointListValidation = value;
                    OnPropertyChanged(nameof(CheckpointListValidation));
                }
            }
        }
        private SolidColorBrush _checkpointListBorderColor;
        public SolidColorBrush CheckpointListBorderColor
        {
            get => _checkpointListBorderColor;
            set
            {
                if (_checkpointListBorderColor != value)
                {
                    _checkpointListBorderColor = value;
                    OnPropertyChanged(nameof(CheckpointListBorderColor));
                }
            }
        }
        private string _imageListValidation;
        public string ImageListValidation
        {
            get => _imageListValidation;
            set
            {
                if (value != _imageListValidation)
                {
                    _imageListValidation = value;
                    OnPropertyChanged(nameof(ImageListValidation));
                }
            }
        }
        private SolidColorBrush _imageListBorderColor;
        public SolidColorBrush ImageListBorderColor
        {
            get => _imageListBorderColor;
            set
            {
                if (_imageListBorderColor != value)
                {
                    _imageListBorderColor = value;
                    OnPropertyChanged(nameof(ImageListBorderColor));
                }
            }
        }
        public void ClearAllValidations()
        {
            TourNameValidation = string.Empty;
            TourDescriptionValidation = string.Empty;
            TourLanguageValidation = string.Empty;
            TourCountryValidation = string.Empty;
            TourCityValidation = string.Empty;
            MaximumGuestsValidation = string.Empty;
            DurationValidation = string.Empty;
            DateValidation = string.Empty;
            DateListValidation = string.Empty;
            CheckpointListValidation = string.Empty;
            ImageListValidation = string.Empty;

            SolidColorBrush borderColor = new SolidColorBrush(Colors.Transparent);

            TourNameBorderColor = borderColor;
            TourDescriptionBorderColor = borderColor;
            TourLanguageBorderColor = borderColor;
            TourCountryBorderColor = borderColor;
            TourCityBorderColor = borderColor;
            MaximumGuestsBorderColor = borderColor;
            DurationBorderColor = borderColor;
            DateBorderColor = borderColor;
            DateListBorderColor = borderColor;
            CheckpointListBorderColor = borderColor;
            ImageListBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#007ACC"));
        }

        private bool Validate()
        {
            ClearAllValidations();
            bool valid = true;

            if (string.IsNullOrEmpty(TourName))
            {
                valid = false;
                TourNameValidation = "Tour Name Required!";
                TourNameBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
            }
            if (string.IsNullOrEmpty(TourDescription))
            {
                valid = false;
                TourDescriptionValidation = "Tour Description Required!";
                TourDescriptionBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
            }
            if (string.IsNullOrEmpty(TourLanguage))
            {
                valid = false;
                TourLanguageValidation = "Tour Language Required!";
                TourLanguageBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
            }
            if (string.IsNullOrEmpty(TourCountry))
            {
                valid = false;
                TourCountryValidation = "Tour Country Required!";
                TourCountryBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
            }
            if (string.IsNullOrEmpty(TourCity))
            {
                valid = false;
                TourCityValidation = "Tour City Required!";
                TourCityBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
            }
            if (string.IsNullOrEmpty(MaximumGuests))
            {
                valid = false;
                MaximumGuestsValidation = "Maximum Guests Number Required!";
                MaximumGuestsBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
            }
            else
            {
                if(!int.TryParse(MaximumGuests, out int number)) 
                {
                    valid = false;
                    MaximumGuestsValidation = "Maximum Guest Number Should Be A Whole Number!";
                    MaximumGuestsBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
                }
            }
            if (string.IsNullOrEmpty(TourDuration))
            {
                valid = false;
                DurationValidation = "Duration Number required!";
                DurationBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
            }
            else 
            {
                if (!double.TryParse(TourDuration, out double number))
                {
                    valid = false;
                    DurationValidation = "Duration Number Should Be A Real Number!";
                    DurationBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
                }
            }
            if (TourDates.Count < 1)
            {
                valid = false;
                DateListValidation = "At Least One Date Is Required!";
                DateListBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
            }
            if (TourCheckpoints.Count < 2)
            {
                valid = false;
                CheckpointListValidation = "At Least Two Checkpoints Required!";
                CheckpointListBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
            }
            if (ImageUrls.Count < 1)
            {
                valid = false;
                ImageListValidation = "At Least One Image Url Is Required!";
                ImageListBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
            }
            return valid;
        }
        #endregion

        #region MostRequested

        public void SetMostRequestedCountry() 
        {
            TourCountry  = _tourRequestService.GetMostRequestedCountry();
            
            if(TourCity != null && !_locationService.GetCitiesByCountry(TourCountry).Contains(TourCity))    
                TourCity = null;

        }
        public void SetMostRequestedCity()
        {
            TourCity = _tourRequestService.GetMostRequestedCity();
            TourCountry = _locationService.GetCountryByCity(TourCity);
        }
        public void SetMostRequestedLanguage()
        {
            TourLanguage = _tourRequestService.GetMostRequestedLanguage();
        }
        #endregion

        #region Commands
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand CreateTourCommand { get; private set; }
        public RelayCommand NextImageCommand { get; private set; }
        public RelayCommand PreviousImageCommand { get; private set; }
        public RelayCommand AddDateCommand { get; private set; }
        public RelayCommand AddCheckpointCommand { get; private set; }
        public RelayCommand AddImageUrlCommand { get; private set; }
        public RelayCommand ImageTextBoxGotFocusCommand { get; private set; }
        public RelayCommand ImageTextBoxLostFocusCommand { get; private set; }
        public RelayCommand HoursComboBoxGotFocusCommand { get; private set; }
        public RelayCommand HoursComboBoxLostFocusCommand { get; private set; }
        public RelayCommand MinutesComboBoxGotFocusCommand { get; private set; }
        public RelayCommand MinutesComboBoxLostFocusCommand { get; private set; }
        public RelayCommand CheckpointTextBoxGotFocusCommand { get; private set; }
        public RelayCommand CheckpointTextBoxLostFocusCommand { get; private set; }
        public RelayCommand DatePickerGotFocusCommand { get; private set; }
        public RelayCommand DatePickerLostFocusCommand { get; private set; }
        public RelayCommand EnterKeyCommand { get; private set; }
        public RelayCommand DemoCommand { get; private set; }
        public RelayCommand OnKeyDownCommand { get; private set; }
        public RelayCommand CreateCommand { get; private set; }
        public RelayCommand CountryCommand { get; private set; }
        public RelayCommand CityCommand { get; private set; }
        public RelayCommand LanguageCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand CountrySelectionChangedCommand { get; private set; }
        public ICommand StopDemoCommand { get; private set; }
        private void InitializeRelayCommands()
        {
            CreateTourCommand = new RelayCommand(CreateTour);

            NextImageCommand = new RelayCommand(NextImage);
            PreviousImageCommand = new RelayCommand(PreviousImage);
            AddDateCommand = new RelayCommand(AddDate);
            AddCheckpointCommand = new RelayCommand(AddCheckpoint);
            AddImageUrlCommand = new RelayCommand(AddImage);

            ImageTextBoxGotFocusCommand = new RelayCommand(ImageTextBoxGotFocus);
            ImageTextBoxLostFocusCommand = new RelayCommand(ImageTextBoxLostFocus);
            HoursComboBoxGotFocusCommand = new RelayCommand(HoursComboBoxGotFocus);
            HoursComboBoxLostFocusCommand = new RelayCommand(HoursComboBoxLostFocus);
            MinutesComboBoxGotFocusCommand = new RelayCommand(MinutesComboBoxGotFocus);
            MinutesComboBoxLostFocusCommand = new RelayCommand(MinutesComboBoxLostFocus);
            CheckpointTextBoxGotFocusCommand = new RelayCommand(CheckpointTextBoxGotFocus);
            CheckpointTextBoxLostFocusCommand = new RelayCommand(CheckpointTextBoxLostFocus);
            DatePickerGotFocusCommand = new RelayCommand(DatePickerGotFocus);
            DatePickerLostFocusCommand = new RelayCommand(DatePickerLostFocus);
            CountrySelectionChangedCommand = new RelayCommand(OnCountrySelectionChanged);

            EnterKeyCommand = new RelayCommand(EnterKeyHandler, CanExecute);
            CreateTourCommand = new RelayCommand(Save, CanExecute);
            DemoCommand = new RelayCommand(StartDemo, CanExecute);
            CountryCommand = new RelayCommand(SetMostRequestedCountry, CanExecute);
            CityCommand = new RelayCommand(SetMostRequestedCity, CanExecute);
            LanguageCommand = new RelayCommand(SetMostRequestedLanguage, CanExecute);
            DeleteCommand = new RelayCommand(Delete, CanExecute);


            StopDemoCommand = new DelegateCommand(StopDemoM,CannotExecute);

        }
        private void OnCountrySelectionChanged(object parameter)
        {
            UpdateCityCoboBox();
        }

        private void StopDemoM(object parameter)
        {   
 
          StopDemo = true;
 
        }
        private bool CanExecute(object parameter)
        {
            return !IsDemo;
        }
        private bool CannotExecute(object parameter)
        {
            return IsDemo;
        }
        private void StartDemo(object parameter)
        {

                StartDemoAsync();
        }

        private void Save(object parameter)
        {

                Save();
        }

        private void SetMostRequestedCountry(object parameter)
        {

               SetMostRequestedCountry();
        }

        private void SetMostRequestedCity(object parameter)
        {
               SetMostRequestedCity();
        }

        private void SetMostRequestedLanguage(object parameter)
        {
              SetMostRequestedLanguage();
        }

        private void Delete(object parameter)
        {
                if (SelectedCheckpoint != null)
                {
                    RemoveSelectedCheckpoint();
                }
                else if (SelectedDateInList != null)
                {
                    RemoveSelectedDate();
                }
                else
                {
                    RemoveSelectedImage();
                }
        }

        private void EnterKeyHandler(object parameter)
        {

            if (IsImageTextBoxFocused)
            {
                AddImageUrl();
            }
     
            if (IsDatePickerFocused || IsHoursComboBoxFocused || IsMinutesComboBoxFocused)
            {
                AddDateTime();
            }

            if (IsCheckpointTextBoxFocused)
            {
                AddCheckpoint();
            }
        }
        private void CreateTour(object parameter)
        {
            Save();
        }
        private void NextImage(object parameter)
        {
            NextImage();
        }
        private void PreviousImage(object parameter)
        {
            PreviousImage();
        }
        private void AddDate(object parameter)
        {
            AddDateTime();
        }
        private void AddCheckpoint(object parameter)
        {
            AddCheckpoint();
        }
        private void AddImage(object parameter)
        {
            AddImageUrl();
        }
        private void ImageTextBoxGotFocus(object parameter)
        {
            IsImageTextBoxFocused = true;
        }

        private void ImageTextBoxLostFocus(object parameter)
        {
            IsImageTextBoxFocused = false;
        }
        private void HoursComboBoxGotFocus(object parameter)
        {
            IsHoursComboBoxFocused = true;
        }

        private void HoursComboBoxLostFocus(object parameter)
        {
            IsHoursComboBoxFocused = false;
        }

        private void MinutesComboBoxGotFocus(object parameter)
        {
            IsMinutesComboBoxFocused = true;
        }

        private void MinutesComboBoxLostFocus(object parameter)
        {
            IsMinutesComboBoxFocused = false;
        }

        private void CheckpointTextBoxGotFocus(object parameter)
        {
           IsCheckpointTextBoxFocused = true;
        }

        private void CheckpointTextBoxLostFocus(object parameter)
        {
            IsCheckpointTextBoxFocused = false;
        }
        private void DatePickerGotFocus(object parameter)
        {
            IsDatePickerFocused = true;
        }

        private void DatePickerLostFocus(object parameter)
        {
            IsDatePickerFocused = false;
        }
        private void StartDemoAsync(object parameter)
        {
            StartDemoAsync();
        }

        #endregion
    }
}
