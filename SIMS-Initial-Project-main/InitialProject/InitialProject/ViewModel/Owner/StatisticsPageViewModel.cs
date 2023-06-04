using InitialProject.Model;
using InitialProject.Service;
using InitialProject.View.Owner;
using LiveCharts;
using LiveCharts.Wpf;
using MenuNavigation.Commands;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InitialProject.ViewModel.Owner
{
    public class StatisticsPageViewModel : INotifyPropertyChanged
    {
        public StatisticsPage MainPage { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public SeriesCollection Series1 { get; set; }
        public string[] Labels1 { get; set; }
        public SeriesCollection Series2 { get; set; }
        public string[] Labels2 { get; set; }
        public int SelectedYear { get; set; }

        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly ReservationCancellationService _reservationCancellationService;

        private readonly RescheduleRequestService _rescheduleRequestService;
        private readonly RenovationRecommendationService _renovationRecommendationService;

        public RelayCommand ChangeYearCommand { get; set; }

        private void Execute_ChangeYearCommand(object obj)
        {
            SelectedYear = int.Parse(MainPage.Years.SelectedItem.ToString());
            MainPage.MonthlyReservations.Content = "JAN: " + _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 1).Count
                + " FEB: " + _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 2).Count
                + " MAR: " + _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 3).Count
                + " APR: " + _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 4).Count
                + " MAY: " + _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 5).Count
                + " JUN: " + _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 6).Count
                + " JUL: " + _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 7).Count
                + " AUG: " + _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 8).Count
                + " SEP: " + _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 9).Count
                + " OCT: " + _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 10).Count
                + " NOV: " + _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 11).Count
                + " DEC: " + _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 12).Count;

            MainPage.MonthlyCancellations.Content = "JAN: " + _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 1).Count
                + " FEB: " + _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 2).Count
                + " MAR: " + _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 3).Count
                + " APR: " + _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 4).Count
                + " MAY: " + _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 5).Count
                + " JUN: " + _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 6).Count
                + " JUL: " + _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 7).Count
                + " AUG: " + _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 8).Count
                + " SEP: " + _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 9).Count
                + " OCT: " + _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 10).Count
                + " NOV: " + _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 11).Count
                + " DEC: " + _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 12).Count;

            MainPage.MonthlyReschedules.Content = "JAN: " + _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 1).Count
                + " FEB: " + _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 2).Count
                + " MAR: " + _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 3).Count
                + " APR: " + _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 4).Count
                + " MAY: " + _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 5).Count
                + " JUN: " + _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 6).Count
                + " JUL: " + _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 7).Count
                + " AUG: " + _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 8).Count
                + " SEP: " + _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 9).Count
                + " OCT: " + _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 10).Count
                + " NOV: " + _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 11).Count
                + " DEC: " + _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 12).Count;

            MainPage.MonthlyRecommendations.Content = "JAN: " + _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 1).Count
                + " FEB: " + _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 2).Count
                + " MAR: " + _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 3).Count
                + " APR: " + _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 4).Count
                + " MAY: " + _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 5).Count
                + " JUN: " + _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 6).Count
                + " JUL: " + _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 7).Count
                + " AUG: " + _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 8).Count
                + " SEP: " + _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 9).Count
                + " OCT: " + _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 10).Count
                + " NOV: " + _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 11).Count
                + " DEC: " + _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 12).Count;
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        public StatisticsPageViewModel(StatisticsPage page, Accommodation accommodation)
        {
            MainPage = page;
            SelectedAccommodation = accommodation;

            _accommodationReservationService = new AccommodationReservationService();
            _reservationCancellationService = new ReservationCancellationService();
            _rescheduleRequestService = new RescheduleRequestService();
            _renovationRecommendationService = new RenovationRecommendationService();

            ChangeYearCommand = new RelayCommand(Execute_ChangeYearCommand, CanExecute_Command);

            InitializeComboBoxes();

            Series1 = new SeriesCollection();
            Labels1 = new string[] { "2023", "2022", "2021", "2020", "2019" };

            Series2 = new SeriesCollection();
            Labels2 = new string[] { "2023", "2022", "2021", "2020", "2019" };

            Series1.Add(new ColumnSeries
            {
                Title = "Reservations",
                Values = new ChartValues<int> { _accommodationReservationService.GetReservationsByYear(SelectedAccommodation.Id, 2023).Count,
                    _accommodationReservationService.GetReservationsByYear(SelectedAccommodation.Id, 2022).Count,
                    _accommodationReservationService.GetReservationsByYear(SelectedAccommodation.Id, 2021).Count,
                    _accommodationReservationService.GetReservationsByYear(SelectedAccommodation.Id, 2020).Count,
                    _accommodationReservationService.GetReservationsByYear(SelectedAccommodation.Id, 2019).Count, }
            });

            Series1.Add(new ColumnSeries
            {
                Title = "Cancellations",
                Values = new ChartValues<int> { _reservationCancellationService.GetCancellationsByYear(SelectedAccommodation.Id, 2023).Count,
                    _reservationCancellationService.GetCancellationsByYear(SelectedAccommodation.Id, 2022).Count,
                    _reservationCancellationService.GetCancellationsByYear(SelectedAccommodation.Id, 2021).Count,
                    _reservationCancellationService.GetCancellationsByYear(SelectedAccommodation.Id, 2020).Count,
                    _reservationCancellationService.GetCancellationsByYear(SelectedAccommodation.Id, 2019).Count }
            });

            Series2.Add(new ColumnSeries
            {
                Title = "Reschedules",
                Values = new ChartValues<int> { _rescheduleRequestService.GetReschedulesByYear(SelectedAccommodation.Id, 2023).Count,
                    _rescheduleRequestService.GetReschedulesByYear(SelectedAccommodation.Id, 2022).Count,
                    _rescheduleRequestService.GetReschedulesByYear(SelectedAccommodation.Id, 2021).Count,
                    _rescheduleRequestService.GetReschedulesByYear(SelectedAccommodation.Id, 2020).Count,
                    _rescheduleRequestService.GetReschedulesByYear(SelectedAccommodation.Id, 2019).Count, }
            });

            Series2.Add(new ColumnSeries
            {
                Title = "Renovations",
                Values = new ChartValues<int> { _renovationRecommendationService.GetRecommendationsByYear(SelectedAccommodation.Id, 2023).Count,
                    _renovationRecommendationService.GetRecommendationsByYear(SelectedAccommodation.Id, 2022).Count,
                    _renovationRecommendationService.GetRecommendationsByYear(SelectedAccommodation.Id, 2021).Count,
                    _renovationRecommendationService.GetRecommendationsByYear(SelectedAccommodation.Id, 2020).Count,
                    _renovationRecommendationService.GetRecommendationsByYear(SelectedAccommodation.Id, 2019).Count, }
            });
        }

        public void InitializeComboBoxes()
        {
            for (int i = 2010; i <= DateTime.Now.Year; i++)
            {
                MainPage.Years.Items.Add(i.ToString());
            }
            MainPage.label.Content = "The busiest year was: " + _accommodationReservationService.GetBusiestYear(SelectedAccommodation.Id).ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
