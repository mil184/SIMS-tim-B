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

            MainPage.res1.Content = _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 1).Count;
            MainPage.res2.Content = _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 2).Count;
            MainPage.res3.Content = _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 3).Count;
            MainPage.res4.Content = _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 4).Count;
            MainPage.res5.Content = _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 5).Count;
            MainPage.res6.Content = _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 6).Count;
            MainPage.res7.Content = _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 7).Count;
            MainPage.res8.Content = _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 8).Count;
            MainPage.res9.Content = _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 9).Count;
            MainPage.res10.Content = _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 10).Count;
            MainPage.res11.Content = _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 11).Count;
            MainPage.res12.Content = _accommodationReservationService.GetReservationsByMonth(SelectedAccommodation.Id, SelectedYear, 12).Count;

            MainPage.can1.Content = _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 1).Count;
            MainPage.can2.Content = _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 2).Count;
            MainPage.can3.Content = _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 3).Count;
            MainPage.can4.Content = _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 4).Count;
            MainPage.can5.Content = _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 5).Count;
            MainPage.can6.Content = _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 6).Count;
            MainPage.can7.Content = _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 7).Count;
            MainPage.can8.Content = _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 8).Count;
            MainPage.can9.Content = _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 9).Count;
            MainPage.can10.Content = _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 10).Count;
            MainPage.can11.Content = _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 11).Count;
            MainPage.can12.Content = _reservationCancellationService.GetCancellationsByMonth(SelectedAccommodation.Id, SelectedYear, 12).Count;

            MainPage.resc1.Content = _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 1).Count;
            MainPage.resc2.Content = _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 2).Count;
            MainPage.resc3.Content = _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 3).Count;
            MainPage.resc4.Content = _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 4).Count;
            MainPage.resc5.Content = _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 5).Count;
            MainPage.resc6.Content = _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 6).Count;
            MainPage.resc7.Content = _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 7).Count;
            MainPage.resc8.Content = _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 8).Count;
            MainPage.resc9.Content = _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 9).Count;
            MainPage.resc10.Content = _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 10).Count;
            MainPage.resc11.Content = _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 11).Count;
            MainPage.resc12.Content = _rescheduleRequestService.GetReschedulesByMonth(SelectedAccommodation.Id, SelectedYear, 12).Count;

            MainPage.rec1.Content = _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 1).Count;
            MainPage.rec2.Content = _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 2).Count;
            MainPage.rec3.Content = _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 3).Count;
            MainPage.rec4.Content = _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 4).Count;
            MainPage.rec5.Content = _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 5).Count;
            MainPage.rec6.Content = _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 6).Count;
            MainPage.rec7.Content = _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 7).Count;
            MainPage.rec8.Content = _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 8).Count;
            MainPage.rec9.Content = _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 9).Count;
            MainPage.rec10.Content = _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 10).Count;
            MainPage.rec11.Content = _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 11).Count;
            MainPage.rec12.Content = _renovationRecommendationService.GetRecommendationsByMonth(SelectedAccommodation.Id, SelectedYear, 12).Count;
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
            MainPage.Years.SelectedIndex = 0;
            MainPage.label1.Content = "The busiest year was " + _accommodationReservationService.GetBusiestYear(SelectedAccommodation.Id).ToString() + "!";
            MainPage.label2.Content = "The busiest year was " + _accommodationReservationService.GetBusiestYear(SelectedAccommodation.Id).ToString() + "!";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
