    using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.View.Guide.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Image = InitialProject.Model.Image;

namespace InitialProject.View.Guide
{
    public partial class CreateTour : Window,INotifyPropertyChanged,IDataErrorInfo
    {

        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointService _checkpointService;
        private readonly TourRequestService _tourRequestService;

        private User LoggedInUser;
        public ObservableCollection<Checkpoint> Checkpoints { get; set; }
        public ObservableCollection<string> ImageUrls { get; set; }
        public ObservableCollection<DateTime> DateTimes { get; set; }
        public Location TourLocation { get; set; }

        public int OrderCounter { get; set; }

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

        public DateTime? LeftBoundary { get; set; }
        public DateTime? RightBoundary { get; set; }

        public TourRequest Request { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public CreateTour(User user, TourService tourService, LocationService locationService, ImageRepository imageRepository, CheckpointService checkpointService)
        {
            InitializeComponent();
            DataContext = this;

            _tourService = tourService;
            _locationService = locationService;
            _imageRepository = imageRepository;
            _checkpointService = checkpointService;

            LeftBoundary = null;
            RightBoundary = null;
            Request = null;

            LoggedInUser = user;
            OrderCounter = 0;

            InitializeCollections();
            InitializeComboBoxes();
            InitializeCountryDropdown();
            InitializeShortcuts();
            //FillWithTestData();
        }
        public CreateTour(User user, TourService tourService, LocationService locationService, ImageRepository imageRepository, CheckpointService checkpointService, TourRequestService tourRequestService, TourRequest request)
        {
            InitializeComponent();
            DataContext = this;

            _tourService = tourService;
            _locationService = locationService;
            _imageRepository = imageRepository;
            _checkpointService = checkpointService;
            _tourRequestService = tourRequestService;

            LoggedInUser = user;
            OrderCounter = 0;

            Description = request.Description;
            TourLanguage = request.Language;
            MaxGuests = request.MaxGuests.ToString();
            Country = _locationService.GetById(request.LocationId).Country;
            City = _locationService.GetById(request.LocationId).City;

            LeftBoundary = request.StartTime;
            RightBoundary = request.EndTime;
            Request = request;

            InitializeCollections();
            InitializeComboBoxes();
            InitializeCountryDropdown();
            InitializeShortcuts();
            _tourRequestService = tourRequestService;
            //FillWithTestData();
        }
        private void InitializeCollections()
        {
            Checkpoints = new ObservableCollection<Checkpoint>();
            ImageUrls = new ObservableCollection<string>();
            DateTimes = new ObservableCollection<DateTime>();

        }
        private void InitializeComboBoxes()
        {
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
        }

        private void InitializeCountryDropdown()
        {
            foreach (var country in _locationService.GetCountries().OrderBy(c => c))
            {
                cbCountry.Items.Add(country);
            }

            if(Country != null)
                cbCountry.SelectedItem = Country;
            if (City != null)
                cbCity.SelectedItem = City;
        }
        private void InitializeShortcuts()
        {
            PreviewKeyDown += Escape_PreviewKeyDown;
            PreviewKeyDown += Create_PreviewKeyDown;
            PreviewKeyDown += Enter_PreviewKeyDown;
        }

        private void TourCreation() 
        {
            bool valid = true;

            if (!IsValid)
            {
                ShowInvalidInfoWarning();
                valid = false;
            }

            if (ImageUrls.Count() < 1)
            {
                ShowImageWarning();
                valid = false;
            }

            if (Checkpoints.Count() < 2)
            {
                ShowCheckpointWarning();
                valid = false;
            }

            if (DateTimes.Count() < 1)
            {
                ShowNoDateTimeWarning();
                valid = false;
            }

            TourLocation = _locationService.GetLocation(Country, City);

            if (TourLocation == null)
            {
                ShowLocationWarning();
                valid = false;
            }

            if (valid)
            {
                SaveTour();

                Close();
            }
        }
        private void btnCreateTour_Click(object sender, RoutedEventArgs e)
        {
            TourCreation();
        }
        private void SaveTour()
        {
            List<int> imageIds = SaveImages();
            List<int> checkpointIds = SaveCheckpoints();

            foreach (DateTime dateTime in DateTimes)
            {
                Tour tour = new Tour(TourName,
                    TourLocation.Id,
                    Description, 
                    TourLanguage, 
                    int.Parse(MaxGuests), 
                    0, 
                    dateTime,
                    double.Parse(Duration), 
                    LoggedInUser.Id,
                    new ObservableCollection<int>(imageIds),
                    new ObservableCollection<int>(checkpointIds)); 

                tour = _tourService.Save(tour);

                UpdateCheckpointsTourId(tour.Id);
                UpdateRequest(Request);
                
            }
        }
        private List<int> SaveImages()
        {
            List<int> imageIds = new List<int>();
            foreach (string imageUrl in ImageUrls)
            {
                imageIds.Add(_imageRepository.Save(new Image(imageUrl)).Id);
            }
            return imageIds;
        }
        private List<int> SaveCheckpoints()
        {
            List<int> checkpointIds = new List<int>();
            foreach (Checkpoint checkpoint in Checkpoints)
            {
                checkpointIds.Add(_checkpointService.Save(checkpoint).Id);
            }
            return checkpointIds;
        }
        private void UpdateCheckpointsTourId(int tourId)
        {
            foreach (Checkpoint checkpoint in Checkpoints)
            {
                checkpoint.TourId = tourId;
                _checkpointService.Update(checkpoint);
            }
        }
        private void UpdateRequest(TourRequest request)
        {
            request.Status = InitialProject.Resources.Enums.RequestStatus.accepted;
            request.GuideId = LoggedInUser.Id;
            _tourRequestService.Update(request);
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
            AddImage();
        }
        private void AddImage() 
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

        private void AddCheckpointButton_Click(object sender, RoutedEventArgs e)
        {
            AddCheckpoint();
        }

        private void AddCheckpoint() 
        {
            if (string.IsNullOrEmpty(CheckpointTextBox.Text))
                return;

            Checkpoint checkpoint = CreateCheckpoint(CheckpointTextBox.Text, ++OrderCounter);
            Checkpoints.Add(checkpoint);

            CheckpointTextBox.Text = string.Empty;
        }

        private Checkpoint CreateCheckpoint(string name, int order)
        {
            return new Checkpoint(name, order);
        }

        private void CbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCountry.SelectedItem != null && _locationService != null)
            {
                ClearCityItems();
                EnableCitySelection();
                LoadCitiesForSelectedCountry();
            }
        }
        private void ClearCityItems()
        {
            if (cbCity.Items != null)
            {
                cbCity.Items.Clear();
            }
        }
        private void EnableCitySelection()
        {
            cbCity.IsEnabled = true;
        }
        private void LoadCitiesForSelectedCountry()
        {
            foreach (string city in _locationService.GetCitiesByCountry(cbCountry.SelectedItem.ToString()).OrderBy(c => c))
            {
                cbCity.Items.Add(city);
            }
        }
        private void CbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCountry.SelectedItem != null && cbCity.SelectedItem != null)
            {
                UpdateCountryAndCity();
            }
        }
        private void UpdateCountryAndCity()
        {
            Country = cbCountry.SelectedItem.ToString();
            City = cbCity.SelectedItem.ToString();
        }
        private void addDateButton_Click(object sender, RoutedEventArgs e)
        {
            AddDate();
        }

        private void AddDate() 
        {
            if (LeftBoundary != null && RightBoundary != null)
            {
                if (GetSelectedDateTime() < LeftBoundary || GetSelectedDateTime() > RightBoundary)
                {
                    ShowBoundaryTimeWarning();
                    return;
                }
            }
            if (IsDateTimeInputValid())
            {
                DateTime selectedDateTime = GetSelectedDateTime();
                DateTimes.Add(selectedDateTime);
                return;
            }

            ShowInvalidDateTimeWarning();
        }
        private bool IsDateTimeInputValid()
        {
            if (dpDate.SelectedDate == null || Hours == null || Minutes == null)
            {
                return false;
            }

            DateTime selectedDate = dpDate.SelectedDate.GetValueOrDefault();
            DateTime selectedDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, int.Parse(Hours), int.Parse(Minutes), 0);

            return selectedDateTime >= DateTime.Now;
        }
        private DateTime GetSelectedDateTime()
        {
            DateTime selectedDate = dpDate.SelectedDate.GetValueOrDefault();
            DateTime selectedDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, int.Parse(Hours), int.Parse(Minutes), 0);

            return selectedDateTime;
        }
        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                int TryParseNumber;
                double TryParseNumberD;
                if (columnName == "TourName")
                {
                    if (string.IsNullOrEmpty(TourName))
                        return "Name is required";
                }
                else if (columnName == "Description")
                {
                    if (string.IsNullOrEmpty(Description))
                        return "Description is required";
                }
                else if (columnName == "TourLanguage")
                {
                    if (string.IsNullOrEmpty(TourLanguage))
                        return "Language is required";
                }
                else if (columnName == "MaxGuests")
                {
                    if (string.IsNullOrEmpty(MaxGuests))
                        return "Maximum guest number is required";

                    if (!int.TryParse(MaxGuests, out TryParseNumber))
                        return "This field should be a number";
                    else 
                    {
                        if (int.Parse(MaxGuests) <= 0)
                            return "Invalid value of maximum guest number";
                    }
                }
                else if (columnName == "Duration")
                {
                    if (string.IsNullOrEmpty(Duration))
                        return "Duration is required";

                    if (!double.TryParse(Duration, out TryParseNumberD))
                        return "This field should be a number";
                    else
                    {
                        if (double.Parse(Duration) <= 0)
                            return "Invalid duration value";
                    }
                }

                return null;
            }
        }
    
        private readonly string[] _validatedProperties = { "TourName", "Description", "TourLanguage", "MaxGuests", "Duration" };
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
        private void Escape_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        private void Enter_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Hours_cb.IsFocused || Minutes_cb.IsFocused || dpDate.IsKeyboardFocusWithin) 
                {
                    AddDate();
                }
                else if (UrlTextBox.IsFocused || UrlBtn.IsFocused) 
                {
                    AddImage();
                }
                else if(CheckpointTextBox.IsFocused || CheckpointBtn.IsFocused) 
                {
                    AddCheckpoint();
                }
            }
        }
        private void Create_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                TourCreation();
            }
        }
        private void ShowInvalidInfoWarning()
        {
            if (string.IsNullOrEmpty(txtTourName.Text))
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.Text = "Tour Name is required.";

                Popup popup = new Popup();
                popup.AllowsTransparency = true; // Allow the popup to have a transparent background
                popup.Child = messageBox;
                popup.PlacementTarget = txtTourName; // Set the placement target to the TextBox control
                popup.Placement = PlacementMode.Right; // Set the placement mode to right
                popup.VerticalOffset = -23;
                popup.IsOpen = true;

                // Hide the popup when the user starts typing again
                txtTourName.TextChanged += (s, args) =>
                {
                    popup.IsOpen = false;
                };
            }
            if (string.IsNullOrEmpty(txtTourDescription.Text))
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.Text = "Tour Description is required.";

                Popup popup = new Popup();
                popup.AllowsTransparency = true; // Allow the popup to have a transparent background
                popup.Child = messageBox;
                popup.PlacementTarget = txtTourDescription; // Set the placement target to the TextBox control
                popup.Placement = PlacementMode.Right; // Set the placement mode to right
                popup.IsOpen = true;

                // Hide the popup when the user starts typing again
                txtTourDescription.TextChanged += (s, args) =>
                {
                    popup.IsOpen = false;
                };
            }
            if (string.IsNullOrEmpty(txtTourLanguage.Text))
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.Text = "Tour Language is required.";

                Popup popup = new Popup();
                popup.AllowsTransparency = true; // Allow the popup to have a transparent background
                popup.Child = messageBox;
                popup.PlacementTarget = txtTourLanguage; // Set the placement target to the TextBox control
                popup.Placement = PlacementMode.Right; // Set the placement mode to right
                popup.VerticalOffset = -27;
                popup.IsOpen = true;

                // Hide the popup when the user starts typing again
                txtTourLanguage.TextChanged += (s, args) =>
                {
                    popup.IsOpen = false;
                };
            }
           
            if (string.IsNullOrEmpty(txtMaxGuests.Text) || !int.TryParse(MaxGuests, out int TryParseNumber) || int.Parse(MaxGuests) <= 0)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.Text = GetMessageForMaxGuests();

                Popup popup = new Popup();
                popup.AllowsTransparency = true; // Allow the popup to have a transparent background
                popup.Child = messageBox;
                popup.PlacementTarget = txtMaxGuests; // Set the placement target to the TextBox control
                popup.Placement = PlacementMode.Right; // Set the placement mode to right
                popup.VerticalOffset = -20;
                popup.IsOpen = true;

                // Hide the popup when the user starts typing again
                txtMaxGuests.TextChanged += (s, args) =>
                {
                    popup.IsOpen = false;
                };
            }
            if (string.IsNullOrEmpty(txtDuration.Text) || !double.TryParse(Duration, out double tryParseNumber) || double.Parse(Duration) <= 0)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.Text = GetMessageForDuration();

                Popup popup = new Popup();
                popup.AllowsTransparency = true; // Allow the popup to have a transparent background
                popup.Child = messageBox;
                popup.PlacementTarget = txtDuration; // Set the placement target to the TextBox control
                popup.Placement = PlacementMode.Right; // Set the placement mode to right
                popup.VerticalOffset = -10;
                popup.IsOpen = true;

                // Hide the popup when the user starts typing again
                txtDuration.TextChanged += (s, args) =>
                {
                    popup.IsOpen = false;
                };
            }
        }
        private string GetMessageForMaxGuests() 
        {
            if (string.IsNullOrEmpty(txtMaxGuests.Text))
               return "Max Guest Number is required.";

            if (!int.TryParse(MaxGuests, out int tryParseNumber))
                return "Not a number value.";

            if (int.TryParse(MaxGuests, out int TryParseNumber) && int.Parse(MaxGuests) <= 0)
               return "Invalid number value.";

            return "";
        }
        private string GetMessageForDuration()
        {
            if (string.IsNullOrEmpty(txtDuration.Text))
                return "Duration is required.";

            if (!double.TryParse(Duration, out double tryParseNumber))
                return "Not a number value.";

            if (double.TryParse(Duration, out double TryParseNumber) && double.Parse(Duration) <= 0)
                return "Invalid number value.";

            return "";
        }
        private void ShowImageWarning()
        {
            CustomMessageBox messageBox = new CustomMessageBox();
            messageBox.Text = "At least one image is required";

            Popup popup = new Popup();
            popup.AllowsTransparency = true; // Allow the popup to have a transparent background
            popup.Child = messageBox;
            popup.PlacementTarget = UrlTextBox; // Set the placement target to the TextBox control
            popup.Placement = PlacementMode.Right; // Set the placement mode to right
            popup.VerticalOffset = -10;
            popup.HorizontalOffset = 40;
            popup.IsOpen = true;

            // Hide the popup when the user starts typing again
            UrlTextBox.TextChanged += (s, args) =>
            {
                popup.IsOpen = false;
            };
        }
        private void ShowNoDateTimeWarning()
        {
            CustomMessageBox messageBox = new CustomMessageBox();
            messageBox.Text = "At least one date";
            messageBox.TextAdditional = "and time is required";

            Popup popup = new Popup();
            popup.AllowsTransparency = true; // Allow the popup to have a transparent background
            popup.Child = messageBox;
            popup.PlacementTarget = Minutes_cb; // Set the placement target to the TextBox control
            popup.Placement = PlacementMode.Right; // Set the placement mode to right
            popup.VerticalOffset = -10;
            popup.HorizontalOffset = 40;
            popup.IsOpen = true;

            // Hide the popup when the user starts typing again
            Minutes_cb.SelectionChanged += (s, args) =>
            {
                popup.IsOpen = false;
            };
            Hours_cb.SelectionChanged += (s, args) =>
            {
                popup.IsOpen = false;
            };
            dpDate.SelectedDateChanged += (s, args) =>
            {
                popup.IsOpen = false;
            };
        }

        private void ShowBoundaryTimeWarning()
        {
            CustomMessageBox messageBox = new CustomMessageBox();
            messageBox.Text = "Only choose from";

            messageBox.TextAdditional = LeftBoundary.ToString() + " - " + RightBoundary.ToString();

            Popup popup = new Popup();
            popup.AllowsTransparency = true; // Allow the popup to have a transparent background
            popup.Child = messageBox;
            popup.PlacementTarget = Minutes_cb; // Set the placement target to the TextBox control
            popup.Placement = PlacementMode.Right; // Set the placement mode to right
            popup.VerticalOffset = -10;
            popup.HorizontalOffset = 40;
            popup.IsOpen = true;

            // Hide the popup when the user starts typing again
            Minutes_cb.SelectionChanged += (s, args) =>
            {
                popup.IsOpen = false;
            };
            Hours_cb.SelectionChanged += (s, args) =>
            {
                popup.IsOpen = false;
            };
            dpDate.SelectedDateChanged += (s, args) =>
            {
                popup.IsOpen = false;
            };
        }
        private void ShowInvalidDateTimeWarning()
        {
            CustomMessageBox messageBox = new CustomMessageBox();
            messageBox.Text = "Invalid date and time values";
  
            Popup popup = new Popup();
            popup.AllowsTransparency = true; // Allow the popup to have a transparent background
            popup.Child = messageBox;
            popup.PlacementTarget = Minutes_cb; // Set the placement target to the TextBox control
            popup.Placement = PlacementMode.Right; // Set the placement mode to right
            popup.VerticalOffset = -10;
            popup.HorizontalOffset = 40;
            popup.IsOpen = true;

            // Hide the popup when the user starts typing again
            Minutes_cb.SelectionChanged += (s, args) =>
            {
                popup.IsOpen = false;
            };
            Hours_cb.SelectionChanged += (s, args) =>
            {
                popup.IsOpen = false;
            };
            dpDate.SelectedDateChanged += (s, args) =>
            {
                popup.IsOpen = false;
            };
        }
        private void ShowCheckpointWarning()
        {
            CustomMessageBox messageBox = new CustomMessageBox();
            messageBox.Text = "At least two checkpoints";
             messageBox.TextAdditional = "are required";

            Popup popup = new Popup();
            popup.AllowsTransparency = true; // Allow the popup to have a transparent background
            popup.Child = messageBox;
            popup.PlacementTarget = CheckpointTextBox; // Set the placement target to the TextBox control
            popup.Placement = PlacementMode.Right; // Set the placement mode to right
            popup.VerticalOffset = -10;
            popup.HorizontalOffset = 40;
            popup.IsOpen = true;

            // Hide the popup when the user starts typing again
            CheckpointTextBox.TextChanged += (s, args) =>
            {
                popup.IsOpen = false;
            };
        }
        private void ShowLocationWarning()
        {

            CustomMessageBox messageBox = new CustomMessageBox();
            messageBox.Text = "Location is required";

            Popup popup = new Popup();
            popup.AllowsTransparency = true; // Allow the popup to have a transparent background
            popup.Child = messageBox;
            popup.PlacementTarget = cbCity; // Set the placement target to the TextBox control
            popup.Placement = PlacementMode.Right; // Set the placement mode to right
            popup.VerticalOffset = -20;
            popup.IsOpen = true;

            // Hide the popup when the user starts typing again
            cbCity.SelectionChanged += (s, args) =>
            {
                popup.IsOpen = false;
            };
            cbCountry.SelectionChanged += (s, args) =>
            {
                popup.IsOpen = false;
            };

        }
        private void FillWithTestData()
        {
            TourName = "Osaka Food Tour";
            Description = "Embark on a great Japanese journey and try our famous food!";
            TourLanguage = "Japanese";
            MaxGuests = "100";
            Duration = "50";

            cbCountry.SelectedItem = "Japan";
            cbCity.SelectedItem = "Osaka";

            DateTimes.Add(new DateTime(2023, 5, 8, 23, 59, 59));

            ImageUrls.Add("test1");
            ImageUrls.Add("test2");
            ImageUrls.Add("test3");

            Checkpoints.Add(new Checkpoint("The Bridge", 1));
            Checkpoints.Add(new Checkpoint("The Castle", 2));
            Checkpoints.Add(new Checkpoint("The Volcano", 3));
            Checkpoints.Add(new Checkpoint("The X", 4));
            Checkpoints.Add(new Checkpoint("The Something", 5));
            Checkpoints.Add(new Checkpoint("The House", 6));
            Checkpoints.Add(new Checkpoint("The Hotel", 7));
            Checkpoints.Add(new Checkpoint("The Tree", 8));
            Checkpoints.Add(new Checkpoint("The Tower", 9));
            Checkpoints.Add(new Checkpoint("The Field", 10));
        }
    }
}
