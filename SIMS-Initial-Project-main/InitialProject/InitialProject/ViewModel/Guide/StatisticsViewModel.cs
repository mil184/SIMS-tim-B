using InitialProject.Model;
using InitialProject.Repository;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InitialProject.ViewModel.Guide
{
    public class StatisticsViewModel
    {
        private readonly TourReservationRepository _tourReservationRepository;
        public Tour SelectedTour { get; set; }
        public SeriesCollection AgeSeriesCollection { get; set; }

        public SeriesCollection VaucherSeriesCollection { get; set; }
        public string[] AgeLabels { get; set; }
        public string[] VaucherLabels { get; set; }

        public StatisticsViewModel(Tour selectedTour, TourReservationRepository tourReservationRepository)
        {
            SelectedTour = selectedTour;
            _tourReservationRepository = tourReservationRepository;

            AgeSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "<18",
                    Values = new ChartValues<double> { _tourReservationRepository.GetUnder18Count(SelectedTour) },
                    Stroke = Brushes.Black, 
                    Fill = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0x00)), 
                    ColumnPadding = 30, 
                    MaxColumnWidth = 120
                },
                new ColumnSeries
                {
                    Title = "18-50",
                    Values = new ChartValues<double> { _tourReservationRepository.GetBetween18And50Count(SelectedTour) },
                    Stroke = Brushes.Black, 
                    Fill = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0xFF)), 
                    ColumnPadding = 30, 
                    MaxColumnWidth = 120 
                },
                new ColumnSeries
                {
                    Title = "50+",
                    Values = new ChartValues<double> { _tourReservationRepository.GetAbove50Count(SelectedTour) },
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
                    Values = new ChartValues<double> { _tourReservationRepository.GetUsedVoucherCount(SelectedTour) },
                    Stroke = Brushes.Black, 
                    Fill = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0x00)), 
                    ColumnPadding = 30, 
                    MaxColumnWidth = 120 
                },
                new ColumnSeries
                {
                    Title = "No",
                    Values = new ChartValues<double> { _tourReservationRepository.GetUnusedVoucherCount(SelectedTour)  },
                    Stroke = Brushes.Black,
                    Fill = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0x00)), 
                    ColumnPadding = 30,
                    MaxColumnWidth = 120
                }
            };

            VaucherLabels = new[] { "Used A Voucher?" };
        }
    }
}
