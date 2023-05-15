using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace InitialProject.ViewModel.Guest2
{
    public class LanguageStatisticsViewModel
    {
        private readonly TourRequestService _tourRequestService;

        public SeriesCollection LanguageSeriesCollection { get; set; }

        public List<int> RequestCounts { get; set; }
        public List<string> Languages { get; set; }

        public LanguageStatisticsViewModel(TourRequestService tourRequestService)
        {
            _tourRequestService = tourRequestService;

            LanguageSeriesCollection = new SeriesCollection();

            Languages = _tourRequestService.GetLanguages();
            RequestCounts = _tourRequestService.GetRequestCountForLanguage(Languages);

            Random random = new Random();

            foreach (var language in Languages)
            {
                var columnSeries = new ColumnSeries
                {
                    Title = language,
                    Values = new ChartValues<int> { _tourRequestService.CountPerLanguage(language) },
                    Stroke = Brushes.Black,
                    Fill = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256))),
                    ColumnPadding = 30,
                    MaxColumnWidth = 120
                };

                LanguageSeriesCollection.Add(columnSeries);
            }
        }
    }
}
