using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Resources.Enums;
using InitialProject.Service;
using InitialProject.View.Guest1;
using MenuNavigation.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.ViewModel.Guest1
{
    public class AvailableDatesViewModel
    {
        private readonly AccommodationService _accommodationService;
        private readonly AccommodationReservationService _accommodationReservationService;
        public AvailableAccommodationsDTO SelectedAvailableAccommodation { get; set; }
        public DatesDTO SelectedDate { get; set; }
        public User LoggedInUser { get; set; }
        public RelayCommand ReserveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

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

        private int _numberOfDays;
        public int NumberOfDays
        {
            get { return _numberOfDays; }
            set
            {
                _numberOfDays = value;
                LoadAvailableDates();
                OnPropertyChanged("NumberOfDays");
            }
        }

        private int _numberOfGuests;
        public int NumberOfGuests
        {
            get { return _numberOfGuests; }
            set
            {
                _numberOfGuests = value;
                OnPropertyChanged("NumberOfGuests");
            }
        }

        public AvailableDatesViewModel(AvailableAccommodationsDTO selectedAvailableAccommodation, AccommodationService accommodationService, AccommodationReservationService accommodationReservationService, int numberOfDays, int numberOfGuests, User user)
        {
            SelectedAvailableAccommodation = selectedAvailableAccommodation;
            _accommodationService = accommodationService;
            _accommodationReservationService = accommodationReservationService;
            DateIntervals = new ObservableCollection<DatesDTO>();
            NumberOfDays = numberOfDays;
            NumberOfGuests = numberOfGuests;
            LoggedInUser = user;

            ReserveCommand = new RelayCommand(Execute_ReserveCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);

            LoadAvailableDates();
        }

        private void LoadAvailableDates()
        {
            var suggestedDateRange = GetSuggestedDateRange();
            DateTime startDate = suggestedDateRange.startDate;
            DateTime endDate = suggestedDateRange.endDate;

            List<DateTime> availableDates = _accommodationReservationService.GetAvailableDates(SelectedAvailableAccommodation.Id, NumberOfDays, startDate, endDate);

            DateIntervals.Clear();

            ObservableCollection<DateTime> selectedDates = new ObservableCollection<DateTime>(availableDates);

            List<DatesDTO> dateRanges = FindDateRanges(selectedDates);

            foreach (var dateRange in dateRanges)
            {
                if (IsValidDateRange(selectedDates, selectedDates.IndexOf(dateRange.StartDate)))
                {
                    AddDateInterval(dateRange.StartDate, dateRange.EndDate);
                }
            }
        }

        private List<DatesDTO> FindDateRanges(ObservableCollection<DateTime> dates)
        {
            var dateRanges = new List<DatesDTO>();

            for (int i = 0; i < dates.Count - NumberOfDays + 1; i++)
            {
                DateTime startDate = dates[i];
                DateTime endDate = dates[i].AddDays(NumberOfDays - 1);
                dateRanges.Add(new DatesDTO { StartDate = startDate, EndDate = endDate });
            }

            return dateRanges;
        }

        private bool IsValidDateRange(ObservableCollection<DateTime> dates, int startIndex)
        {
            for (int i = startIndex + 1; i < startIndex + NumberOfDays; i++)
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

        private void AddDateInterval(DateTime startDate, DateTime endDate)
        {
            if (endDate - startDate == TimeSpan.FromDays(NumberOfDays - 1))
            {
                DateIntervals.Add(new DatesDTO
                {
                    StartDate = startDate,
                    EndDate = endDate
                });
            }
        }

        private (DateTime startDate, DateTime endDate) GetSuggestedDateRange()
        {
            int daysToAdd = 25;
            var newStartDate = DateTime.Today.AddDays(-daysToAdd);

            if (newStartDate < DateTime.Today)
                newStartDate = DateTime.Today;

            var newEndDate = DateTime.Today.AddDays(daysToAdd);
            return (newStartDate.Date, newEndDate.Date);
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        public void Execute_ReserveCommand(object obj)
        {
            if (SelectedDate != null)
            {
                var messageBoxResult = MessageBox.Show($"Are you sure you want to reserve the date: {SelectedDate.StartDate:d} - {SelectedDate.EndDate:d}", "Reserve Accomodation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    var accommodation = _accommodationService.GetById(SelectedAvailableAccommodation.Id);
                    var reservation = new AccommodationReservation(LoggedInUser.Id, SelectedAvailableAccommodation.Id, SelectedDate.StartDate, SelectedDate.EndDate, NumberOfDays, NumberOfGuests, accommodation.OwnerId, false, accommodation.CancellationPeriod, false);
                    _accommodationReservationService.Save(reservation);

                    MessageBox.Show("Reservation created successfully.");
                    
                    var window = Application.Current.Windows.OfType<AvailableDates>().FirstOrDefault();
                    window.Close();
                }
                return;
            }
        }

        public void Execute_CancelCommand(object obj)
        {
            var window = Application.Current.Windows.OfType<AvailableDates>().FirstOrDefault();
            window.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
