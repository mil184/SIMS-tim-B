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

namespace InitialProject.View.Guest1
{
    /// <summary>
    /// Interaction logic for ReserveAccommodation.xaml
    /// </summary>
    public partial class ReserveAccommodation : Window
    {
        public AccommodationReservation Reservation { get; set; }
        public GuestAccommodationDTO SelectedAccommodation { get; set; }

        private readonly AccommodationReservationRepository _accommodationReservationRepository;

        private readonly AccommodationRepository _accommodationRepository;
        
        public ReserveAccommodation(GuestAccommodationDTO selectedAccommodation, AccommodationRepository accommodationRepository)
        {
            InitializeComponent();
            this.DataContext = Reservation;
            SelectedAccommodation = selectedAccommodation;

            Reservation = new AccommodationReservation();
            Reservation.AccommodationId = selectedAccommodation.Id;
            Reservation.StartDate = startDatePicker.SelectedDate.GetValueOrDefault();
            Reservation.EndDate = endDatePicker.SelectedDate.GetValueOrDefault();
            //Reservation.NumberDays = int.Parse(numDaysTextBox.Text);

            _accommodationReservationRepository = new AccommodationReservationRepository();
            _accommodationRepository = accommodationRepository;
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Reservation.StartDate = startDatePicker.SelectedDate.GetValueOrDefault();
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Reservation.EndDate = endDatePicker.SelectedDate.GetValueOrDefault();
        }

        private void NumDaysTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int numDays;
            if (int.TryParse(numDaysTextBox.Text, out numDays))
            {
                Reservation.NumberDays = numDays;
            }
        }

        private void CheckAvailability()
        {
           
        }

        private bool IsAvailable(DateTime startDate, DateTime endDate)
        {
            List<DateTime> availableDates = _accommodationReservationRepository.GetAvailableDates(SelectedAccommodation.Id,startDate, endDate);
            foreach (DateTime date in availableDates)
            {
                if (date.Date >= startDate && date.Date < endDate)
                {
                    return false;
                }
            }
            return true;
        }

        private bool MakeReservation()
        {
            if (IsAvailable(Reservation.StartDate, Reservation.EndDate) &&
                Reservation.NumberDays >= SelectedAccommodation.MinReservationDays)
            {
               // bool success = _accommodationReservationRepository.Create(Reservation);
                //return success;
            }
            return false;
        }



        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = MakeReservation();
            if (success)
            {
                // Mark the accommodation as unavailable for the selected dates
               // _accommodationReservationRepository.MarkUnavailable(SelectedAccommodation.Id, Reservation.StartDate, Reservation.EndDate);

                MessageBox.Show("Reservation successful!");
                Close();
            }
            else
            {
                // Show a new window with available dates in the same range
                if (!IsAvailable(Reservation.StartDate, Reservation.EndDate))
                {
                    List<DateTime> availableDates = _accommodationReservationRepository.GetAvailableDates(SelectedAccommodation.Id, Reservation.StartDate, Reservation.EndDate);
                   // AvailableDatesWindow availableDatesWindow = new AvailableDatesWindow(availableDates, Reservation.StartDate, Reservation.EndDate);
                    //availableDatesWindow.ShowDialog();
                }

                // Show an error message if the number of days is less than the minimum reservation days
                if (Reservation.NumberDays < SelectedAccommodation.MinReservationDays)
                {
                    MessageBox.Show($"Minimum reservation days is {SelectedAccommodation.MinReservationDays}.");
                }
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
