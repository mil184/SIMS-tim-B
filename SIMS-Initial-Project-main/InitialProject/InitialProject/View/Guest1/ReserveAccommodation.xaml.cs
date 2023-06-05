using InitialProject.Model.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InitialProject.Service;
using InitialProject.Resources.Enums;

namespace InitialProject.View.Guest1
{
    /// <summary>
    /// Interaction logic for ReserveAccommodation.xaml
    /// </summary>
    public partial class ReserveAccommodation : Window
    {
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly AccommodationService _accommodationService;
        private readonly UserService _userService;

        private AccommodationReservation _reservation;
        public AccommodationReservation Reservation
        {
            get { return _reservation; }
            set
            {
                if (_reservation != value)
                {
                    _reservation = value;
                    OnPropertyChanged(nameof(Reservation));
                }
            }
        }

        private GuestAccommodationDTO _selectedAccommodation;
        public GuestAccommodationDTO SelectedAccommodation
        {
            get { return _selectedAccommodation; }
            set
            {
                if (_selectedAccommodation != value)
                {
                    _selectedAccommodation = value;
                    OnPropertyChanged(nameof(SelectedAccommodation));
                }
            }
        }

        private ObservableCollection<DatesDTO> _dateIntervals;
        public ObservableCollection<DatesDTO> DateIntervals
        {
            get { return _dateIntervals; }
            set
            {
                _dateIntervals = value;
                OnPropertyChanged("DateIntervals");
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        private int _numberOfDays;
        public int NumberOfDays
        {
            get => _numberOfDays;
            set
            {
                if (value != _numberOfDays)
                {
                    _numberOfDays = value;
                    OnPropertyChanged();
                }
            }
        }
        public User LoggedInUser { get; set; }

        private bool _isAvailable;
        public bool Available
        {
            get => _isAvailable;
            set
            {
                if (value != _isAvailable)
                {
                    _isAvailable = value;
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

        private int _ownerId;
        public int OwnerId
        {
            get => _ownerId;
            set
            {
                if (value != _ownerId)
                {
                    _ownerId = value;
                    OnPropertyChanged();
                }
            }
        }


        private int _cancellationPeriod;
        public int CancellationPeriod
        {
            get => _cancellationPeriod;
            set
            {
                if (value != _cancellationPeriod)
                {
                    _cancellationPeriod = value;
                    OnPropertyChanged();
                }
            }
        }
        public ReserveAccommodation(GuestAccommodationDTO selectedAccommodation, User user, AccommodationService accommodationService, AccommodationReservationService accommodationReservationService)
        {
            InitializeComponent();
            DataContext = this;

            SelectedAccommodation = selectedAccommodation;
            LoggedInUser = user;

            Reservation = new AccommodationReservation();
            Reservation.AccommodationId = selectedAccommodation.Id;


            _accommodationService = accommodationService;
            _accommodationReservationService = accommodationReservationService;

            DateIntervals = new ObservableCollection<DatesDTO>();

            _userService = new UserService();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DateIntervals.Clear();

            if (!ValidateInput()) return;

            var allFreeDates = GetAllFreeDates();
            AddDateRanges(FindDateRanges(allFreeDates));

            if (DateIntervals.Count == 0)
            {
                if (ShowSuggestedDatesDialog())
                {
                    var suggestedDateRange = GetSuggestedDateRange();
                    StartDate = suggestedDateRange.startDate;
                    EndDate = suggestedDateRange.endDate;

                    allFreeDates = GetAllFreeDates();
                    AddDateRanges(FindDateRanges(allFreeDates));
                }
            }
        }

        private bool ValidateInput()
        {
            if (!ValidateDates()) return false;
            if (!ValidateNumberOfDays()) return false;
            if (!ValidateNumberOfGuests()) return false;
            return true;
        }

        private bool ShowSuggestedDatesDialog()
        {
            var messageBoxResult = MessageBox.Show($"There are no available dates to reserve right now, would you like to see suggested dates?", "Suggested Accomodation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return messageBoxResult == MessageBoxResult.Yes;
        }

        private (DateTime startDate, DateTime endDate) GetSuggestedDateRange()
        {
            const int daysToAdd = 25;
            var newStartDate = StartDate.AddDays(-daysToAdd);

            if (newStartDate < DateTime.Today)
                newStartDate = DateTime.Today;

            var newEndDate = EndDate.AddDays(daysToAdd);
            return (newStartDate.Date, newEndDate.Date);
        }

        private ObservableCollection<DateTime> GetAllFreeDates()
        {
            int accommodationId = Reservation.AccommodationId;
            int duration = NumberOfDays;
            DateTime startDate = StartDate;
            DateTime endDate = EndDate;

            return new ObservableCollection<DateTime>(_accommodationReservationService.GetAvailableDates(accommodationId, duration, startDate, endDate));
        }

        private void AddDateRanges(List<DatesDTO> dateRanges)
        {
            foreach (var dateRange in dateRanges)
            {
                DateIntervals.Add(dateRange);
            }
        }

        private bool ValidateDates()
        {
            StartDate = startDatePicker.SelectedDate.GetValueOrDefault();  
            EndDate = endDatePicker.SelectedDate.GetValueOrDefault();

            if (StartDate == default || EndDate == default)
            {
                ShowNoDateTimeWarning();
                return false;
            }

            if (StartDate.Date < DateTime.Today || EndDate.Date < DateTime.Today || StartDate.Date > EndDate.Date)
            {
                ShowInvalidDateTimeWarning();
                return false;
            }

            return true;
        }

        private bool ValidateNumberOfDays()
        {
            if (!int.TryParse(numDaysTextBox.Text, out int numberOfDays))
            {
                ShowInvalidNumberWarning();
                return false;
            }

            if (numberOfDays <= 0)
            {
                ShowNoNumberWarning();
                return false;
            }

            if (numberOfDays < SelectedAccommodation.MinReservationDays)
            {
                ShowMinimumReservationDaysWarning();
                return false;
            }

            NumberOfDays = numberOfDays;
            return true;
        }

        private bool ValidateNumberOfGuests()
        {
            if (!int.TryParse(maxGuestsTextBox.Text, out int numberGuests))
            {
                ShowInvalidInputWarning();
                return false;
            }

            if (numberGuests <= 0)
            {
                ShowNoInputWarning();
                return false;
            }

            if (numberGuests > SelectedAccommodation.MaxGuests)
            {
                ShowMaxGuestsWarning();
                return false;
            }

            MaxGuests = numberGuests;
            return true;
        }

        private List<DatesDTO> FindDateRanges(ObservableCollection<DateTime> dates)
        {
            var dateRanges = new List<DatesDTO>();

            for (int i = 0; i < dates.Count; i++)  
            {
                DateTime startDate = dates[i];
                DateTime endDate = dates[i].AddDays(NumberOfDays - 1);
                dateRanges.Add(new DatesDTO { StartDate = startDate, EndDate = endDate });
            }

            return dateRanges;
        }

        private bool IsValidDateRange(ObservableCollection<DateTime> dates, int startIndex)
        {
            for (int i = startIndex + 1; i <= startIndex + NumberOfDays - 2; i++)  
            {
                if (!IsDateFollowsPreviousDate(dates, i))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsDateFollowsPreviousDate(ObservableCollection<DateTime> dates, int index)
        {
            return dates[index].Subtract(dates[index - 1]).Days == 1; 
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = (sender as DataGrid)?.SelectedItem as DatesDTO;

            if (selectedItem != null)
            {
                var messageBoxResult = MessageBox.Show($"Are you sure you want to reserve the date: {selectedItem.StartDate:d} - {selectedItem.EndDate:d}", "Reserve Accomodation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    var reservation = new AccommodationReservation(LoggedInUser.Id, SelectedAccommodation.Id, selectedItem.StartDate, selectedItem.EndDate, NumberOfDays, MaxGuests, SelectedAccommodation.OwnerId, false, SelectedAccommodation.CancellationPeriod, false);
                    _accommodationReservationService.Save(reservation);

                    MessageBox.Show("Reservation created successfully.");
                    if (LoggedInUser.Type == UserType.superguest)
                    {
                        UpdateUserBonusPoints(LoggedInUser.Id);
                    }
                    Close();
                }
                return;
            }
        }

        public void UpdateUserBonusPoints(int userId)
        {
            var user = _userService.GetById(userId);
            user.NumberOfReservations += 1;
            user.BonusPoints -= 1;
            _userService.Update(user);
            string message = $"You have {user.BonusPoints} bonus points left.";
            MessageBox.Show(message, "Information about bonus points", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void ShowNoDateTimeWarning()
        {
            MessageBox.Show("Please enter at least one date and time.", "Date and time warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void ShowInvalidDateTimeWarning()
        {
            MessageBox.Show("Please choose a valid date and time.", "Date and time warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void ShowNoNumberWarning()
        {
            MessageBox.Show("Please choose a number of days.", "Number of days warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void ShowInvalidNumberWarning()
        {
            MessageBox.Show("Please choose a valid number.", "Number of days warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void ShowMinimumReservationDaysWarning()
        {
            MessageBox.Show($"The number of days needs to be at least {SelectedAccommodation.MinReservationDays}, the selected accommodation's minimum reservation days.", "Number of days warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ShowNoInputWarning()
        {
            MessageBox.Show("Please choose a max number of guests.", "Max number of guests warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void ShowInvalidInputWarning()
        {
            MessageBox.Show("Please choose a valid number.", "Max number of guests warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void ShowMaxGuestsWarning()
        {
            MessageBox.Show($"The max number of guests needs to be at least {SelectedAccommodation.MaxGuests}, the selected accommodation's max guests.", "Max number of guests warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
