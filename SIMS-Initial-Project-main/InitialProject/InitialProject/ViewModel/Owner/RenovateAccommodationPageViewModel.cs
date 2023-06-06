using InitialProject.Model;
using InitialProject.Service;
using InitialProject.View.Owner;
using MenuNavigation.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace InitialProject.ViewModel.Owner
{
    public class RenovateAccommodationPageViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public NavigationService navigationService { get; set; }
        public User LoggedInUser { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public ObservableCollection<DateTime> Dates { get; set; }
        public DateTime SelectedDate { get; set; }

        private readonly AccommodationRenovationService _accommodationRenovationService;
        private bool IsClicked { get; set; }

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

        private DateTime _selectedSlotEndDate;
        public DateTime SelectedSlotEndDate
        {
            get => _selectedSlotEndDate;
            set
            {
                if (value != _selectedSlotEndDate)
                {
                    _selectedSlotEndDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _selectedTimeSpan;
        public string SelectedTimeSpan
        {
            get => _selectedTimeSpan;
            set
            {
                if (value != _selectedTimeSpan)
                {
                    _selectedTimeSpan = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand FindTimeSlotsCommand { get; set; }
        public RelayCommand SelectionChangedCommand { get; set; }
        public RelayCommand RenovateCommand { get; set; }

        private void Execute_FindTimeSlotsCommand(object obj)
        {
            IsClicked = true;
            Dates.Clear();
            FormDates();
        }

        private void Execute_SelectionChangedCommand(object obj)
        {
            SelectedTimeSpan = "The renovations will end on: ";
            DateTime date = SelectedDate.AddDays(int.Parse(Duration) - 1);
            DateOnly dateOnly = new DateOnly(date.Year, date.Month, date.Day);
            SelectedTimeSpan += dateOnly.ToString();
        }

        private void Execute_RenovateCommand(object obj)
        {
            AccommodationRenovation renovation = new AccommodationRenovation(SelectedAccommodation.Id, SelectedAccommodation.OwnerId, int.Parse(Duration), StartDate, StartDate.AddDays(int.Parse(Duration)-1), Description);
            _accommodationRenovationService.Save(renovation);
            Page renovations = new RenovationsPage(LoggedInUser);
            navigationService.Navigate(renovations);
        }

        public bool CanFind_Command(object obj)
        {
            TimeSpan duration = EndDate - StartDate;
            int NumberOfDays = (int)duration.TotalDays;
            int days;
            if (!int.TryParse(Duration, out days)) return false;
            return StartDate <= EndDate && days <= ++NumberOfDays && StartDate >= DateTime.Now;
        }

        public bool CanExecute_Command(object obj)
        {
            return true;
        }

        public RenovateAccommodationPageViewModel(NavigationService navService, User user, Accommodation selectedAccommodation)
        {
            navigationService = navService;
            LoggedInUser = user;
            SelectedAccommodation = selectedAccommodation;
            IsClicked = false;

            FindTimeSlotsCommand = new RelayCommand(Execute_FindTimeSlotsCommand, CanFind_Command);
            SelectionChangedCommand = new RelayCommand(Execute_SelectionChangedCommand, CanExecute_Command);
            RenovateCommand = new RelayCommand(Execute_RenovateCommand, CanExecute_Command);

            _accommodationRenovationService = new AccommodationRenovationService();

            InitializeDates();
        }

        private void InitializeDates()
        {
            Dates = new ObservableCollection<DateTime>();
        }

        private void FormDates()
        {
            foreach (DateTime date in _accommodationRenovationService.GetAvailableDates(SelectedAccommodation.Id, int.Parse(Duration), StartDate, EndDate))
            {
                Dates.Add(date);
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
                if (columnName == "Duration")
                {
                    if (string.IsNullOrEmpty(Duration))
                        if (IsClicked)
                        return "This field is required";

                    if (!int.TryParse(Duration, out TryParseNumber))
                        if (IsClicked)
                            return "This field should be a number";
                }
                else if (columnName == "Description")
                {
                    if (string.IsNullOrEmpty(Description))
                        if (IsClicked)
                            return "This field is required";
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Duration", "Description" };

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
