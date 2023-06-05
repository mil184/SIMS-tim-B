using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using LiveCharts;
using LiveCharts.Wpf;
using MenuNavigation.Commands;
using System.Windows.Media;

namespace InitialProject.ViewModel.Guide
{
    public class StatisticsViewModel
    {
        private readonly TourReservationService _tourReservationService;
        public Tour SelectedTour { get; set; }
        public SeriesCollection AgeSeriesCollection { get; set; }

        public SeriesCollection VaucherSeriesCollection { get; set; }
        public string[] AgeLabels { get; set; }
        public string[] VaucherLabels { get; set; }

        public StatisticsViewModel(Tour selectedTour, TourReservationService tourReservationService)
        {
            SelectedTour = selectedTour;
            _tourReservationService = tourReservationService;

            AgeSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "<18",
                    Values = new ChartValues<double> { _tourReservationService.GetUnder18Count(SelectedTour) },
                    Stroke = Brushes.Black, 
                    Fill = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0x00)), 
                    ColumnPadding = 30, 
                    MaxColumnWidth = 120
                },
                new ColumnSeries
                {
                    Title = "18-50",
                    Values = new ChartValues<double> { _tourReservationService.GetBetween18And50Count(SelectedTour) },
                    Stroke = Brushes.Black, 
                    Fill = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0xFF)), 
                    ColumnPadding = 30, 
                    MaxColumnWidth = 120 
                },
                new ColumnSeries
                {
                    Title = "50+",
                    Values = new ChartValues<double> { _tourReservationService.GetAbove50Count(SelectedTour) },
                    Stroke = Brushes.Black, 
                    Fill = new SolidColorBrush(Color.FromRgb(0xFF, 0x66, 0x00)), 
                    ColumnPadding = 30, 
                    MaxColumnWidth = 120 
                }
            };

            AgeLabels = new[] { "Age Groups" };

            VaucherSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Yes",
                    Values = new ChartValues<double> { _tourReservationService.GetUsedVoucherCount(SelectedTour) },
                    Stroke = Brushes.Black, 
                    Fill = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0x00)), 
                    ColumnPadding = 30, 
                    MaxColumnWidth = 120 
                },
                new ColumnSeries
                {
                    Title = "No",
                    Values = new ChartValues<double> { _tourReservationService.GetUnusedVoucherCount(SelectedTour)  },
                    Stroke = Brushes.Black,
                    Fill = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0x00)), 
                    ColumnPadding = 30,
                    MaxColumnWidth = 120
                }
            };

            VaucherLabels = new[] { "Used A Voucher?" };
        }

        public RelayCommand CancelCommand { get; set; }
    }
}
