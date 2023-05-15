using InitialProject.Model;
using InitialProject.Service;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InitialProject.ViewModel.Owner
{
    public class StatisticsPageViewModel : INotifyPropertyChanged
    {
        public Accommodation SelectedAccommodation { get; set; }
        public SeriesCollection Series1 { get; set; }
        public string[] Labels1 { get; set; }
        public SeriesCollection Series2 { get; set; }
        public string[] Labels2 { get; set; }

        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly ReservationCancellationService _reservationCancellationService;

        private readonly RescheduleRequestService _rescheduleRequestService;

        ObservableCollection<string> Years { get; set; }

        public StatisticsPageViewModel(Accommodation accommodation)
        {
            SelectedAccommodation = accommodation;

            _accommodationReservationService = new AccommodationReservationService();
            _reservationCancellationService = new ReservationCancellationService();
            _rescheduleRequestService = new RescheduleRequestService();

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
        }

        private void InitializeComboBoxes()
        {
            Years = new ObservableCollection<string>
            {
                "Alltime"
            };
            for (int i = 2000; i <= DateTime.Now.Year; i++)
            {
                Years.Add(i.ToString());
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
