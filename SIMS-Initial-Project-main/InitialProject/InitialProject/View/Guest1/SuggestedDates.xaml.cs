using InitialProject.Model;
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
    /// Interaction logic for SuggestedDates.xaml
    /// </summary>
    public partial class SuggestedDates : Window
    {
        public DateTime SelectedDate { get; set; }
        public AccommodationReservation Reservation { get; set; }

        public SuggestedDates(List<DateTime> dates, DateTime startDate, DateTime endDate)
        {
            InitializeComponent();
            Reservation = new AccommodationReservation();
            Reservation.StartDate = startDate;
            Reservation.EndDate = endDate;

            dateComboBox.ItemsSource = dates;
        }
        private void dateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateComboBox.SelectedItem != null)
            {
                SelectedDate = (DateTime)dateComboBox.SelectedItem;
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDate != null)
            {
                Reservation.StartDate = SelectedDate;
                Reservation.EndDate = SelectedDate.AddDays(Reservation.NumberDays);
                Close();
            }
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
