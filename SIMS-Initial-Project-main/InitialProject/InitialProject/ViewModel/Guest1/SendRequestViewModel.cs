using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.View.Guest1;
using MenuNavigation.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.ViewModel.Guest1
{
    public class SendRequestViewModel
    {
        private readonly RescheduleRequestService _rescheduleRequestService;
        public AccommodationReservation SelectedReservation { get; set; }

        private DateTime _newStartDate;
        public DateTime NewStartDate
        {
            get { return _newStartDate; }
            set
            {
                if (_newStartDate != value)
                {
                    _newStartDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _newEndDate;
        public DateTime NewEndDate
        {
            get { return _newEndDate; }
            set
            {
                if (_newEndDate != value)
                {
                    _newEndDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand SendRequestCommand { get; set; }
        public RelayCommand CancelRequestCommand { get; set; }

        public SendRequestViewModel(AccommodationReservation selectedReservation, RescheduleRequestService rescheduleRequestService)
        {
            SelectedReservation = selectedReservation;
            _rescheduleRequestService = rescheduleRequestService;

            _newStartDate = SelectedReservation.StartDate;
            _newEndDate = SelectedReservation.EndDate;

            SendRequestCommand = new RelayCommand(Execute_SendRequestCommand, CanExecute_Command);
            CancelRequestCommand = new RelayCommand(Execute_CancelRequestCommand, CanExecute_Command);
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        public void Execute_SendRequestCommand(object obj)
        {
            var messageBoxResult = MessageBox.Show($"Are you sure you want to reserve another date: {NewStartDate:d} - {NewEndDate:d}?", "Reschedule Request Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (DatesValid(SelectedReservation) && NumberOfDaysValid(SelectedReservation))
                {
                    var rescheduleRequest = new RescheduleRequest(SelectedReservation, NewStartDate, NewEndDate);
                    _rescheduleRequestService.Save(rescheduleRequest);
                    MessageBox.Show("Reschedule request sent successfully.", "Reschedule Request", MessageBoxButton.OK, MessageBoxImage.Information);
                    var window = Application.Current.Windows.OfType<SendRequest>().FirstOrDefault();
                    window.Close();
                }
            }
        }

        public void Execute_CancelRequestCommand(object obj)
        {
            var window = Application.Current.Windows.OfType<SendRequest>().FirstOrDefault();
            window.Close();
        }

        private bool DatesValid(AccommodationReservation selectedReservation)
        {
            if (NewStartDate < DateTime.Today)
            {
                ShowStartDateError();
                return false;
            }
            else if (NewStartDate == selectedReservation.StartDate && NewEndDate == selectedReservation.EndDate)
            {
                ShowSameDateError();
                return false;
            }
            else if (NewEndDate < NewStartDate)
            {
                ShowEndDateError();
                return false;
            }
            return true;
        }

        private bool NumberOfDaysValid(AccommodationReservation selectedReservation)
        {
            int numberOfDays = (NewEndDate - NewStartDate).Days;
            int expectedNumberOfDays = (selectedReservation.EndDate - selectedReservation.StartDate).Days;

            if (numberOfDays != expectedNumberOfDays)
            {
                MessageBox.Show("The number of days must be the same as in the existing reservation.", "Reschedule Request Error", MessageBoxButton.OK);
                return false;
            }
            return true;
        }

        private void ShowStartDateError()
        {
            MessageBox.Show("The new start date cannot be any date that has passed.", "Reschedule Request Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void ShowSameDateError()
        {
            MessageBox.Show("Selected dates are the same as the existing reservation dates. Please select different dates.", "Reschedule Request Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void ShowEndDateError()
        {
            MessageBox.Show("The new end date cannot be earlier than the start date.", "Reschedule Request Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
