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

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ReserveAccommodation(GuestAccommodationDTO selectedAccommodation, AccommodationRepository accommodationRepository)
        {
            InitializeComponent();
            this.DataContext = Reservation;
            SelectedAccommodation = selectedAccommodation;

            Reservation = new AccommodationReservation();
            Reservation.AccommodationId = selectedAccommodation.Id;

            _accommodationReservationRepository = new AccommodationReservationRepository();
            _accommodationRepository = accommodationRepository;
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            StartDate = startDatePicker.SelectedDate.GetValueOrDefault();
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            EndDate = endDatePicker.SelectedDate.GetValueOrDefault();
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
                bool success = _accommodationReservationRepository.Create(SelectedAccommodation.Id, Reservation.StartDate, Reservation.EndDate);
                return success;
            }
            return false;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartDate <= DateTime.Now || EndDate < StartDate || StartDate == null || EndDate == null)
            {
                MessageBox.Show("Please enter a valid date", "Date warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int numDays;
            if(numDaysTextBox.Text == null || !int.TryParse(numDaysTextBox.Text, out numDays) || int.Parse(numDaysTextBox.Text) < SelectedAccommodation.MinReservationDays)
            {
                MessageBox.Show("Please enter a valid number of days", "Reservation days warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Reservation.StartDate = startDatePicker.SelectedDate.GetValueOrDefault();
            Reservation.EndDate = endDatePicker.SelectedDate.GetValueOrDefault();
            Reservation.NumberDays = int.Parse(numDaysTextBox.Text);

            bool success = MakeReservation();
            if (success)
            {
                _accommodationReservationRepository.MarkUnavailable(SelectedAccommodation.Id, Reservation.StartDate, Reservation.EndDate);

                MessageBox.Show("Reservation successful!");
                Close();
            }
            else
            {
                // Show a new window with available dates in the same range
                if (!IsAvailable(Reservation.StartDate, Reservation.EndDate))
                {
                    List<DateTime> availableDates = _accommodationReservationRepository.GetAvailableDates(SelectedAccommodation.Id, Reservation.StartDate, Reservation.EndDate);
                   // SuggestedDates suggestedDates = new SuggestedDates(availableDates, Reservation.StartDate, Reservation.EndDate);
                    //suggestedDates.ShowDialog();
                }
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
