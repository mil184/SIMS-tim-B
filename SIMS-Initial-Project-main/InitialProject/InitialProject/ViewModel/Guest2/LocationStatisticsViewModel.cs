using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace InitialProject.ViewModel.Guest2
{
    public class LocationStatisticsViewModel
    {
        private readonly TourRequestService _tourRequestService;
        private readonly LocationService _locationService;

        public SeriesCollection LocationSeriesCollection { get; set; }

        public List<int> RequestCounts { get; set; }
        public List<string> Cities { get; set; }

        public LocationStatisticsViewModel(TourRequestService tourRequestService, LocationService locationService)
        {
            _tourRequestService = tourRequestService;
            _locationService = locationService;

            LocationSeriesCollection = new SeriesCollection();

            Cities = _tourRequestService.GetRequestedCities();
            RequestCounts = _tourRequestService.GetRequestCountForCity(Cities);

            Random random = new Random();

            foreach (var city in Cities)
            {
                var columnSeries = new ColumnSeries
                {
                    Title = city,
                    Values = new ChartValues<int> { _tourRequestService.CountPerCity(city) },
                    Stroke = Brushes.Black,
                    Fill = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256))),
                    ColumnPadding = 30,
                    MaxColumnWidth = 120
                };

                LocationSeriesCollection.Add(columnSeries);
            }
        }

    }
}
