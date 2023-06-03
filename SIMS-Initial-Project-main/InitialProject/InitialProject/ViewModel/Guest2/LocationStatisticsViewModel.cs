using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using LiveCharts;
using LiveCharts.Wpf;
using MenuNavigation.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace InitialProject.ViewModel.Guest2
{
    public class LocationStatisticsViewModel
    {
        public Action CloseAction { get; set; }

        private readonly TourRequestService _tourRequestService;

        public SeriesCollection LocationSeriesCollection { get; set; }

        public List<int> RequestCounts { get; set; }
        public List<string> Cities { get; set; }

        public int LanguageButtonClickCount { get; set; }
        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";

        public RelayCommand ExitCommand { get; set; }
        public RelayCommand ChangeLanguageCommand { get; set; }


        public LocationStatisticsViewModel(TourRequestService tourRequestService, string lang)
        {
            _tourRequestService = tourRequestService;

            LocationSeriesCollection = new SeriesCollection();

            Cities = _tourRequestService.GetRequestedCities();
            RequestCounts = _tourRequestService.GetRequestCountForCity(Cities);

            InitializeChart();

            app = (App)Application.Current;
            app.ChangeLanguage(lang);
            InitializeLanguageButton(lang);

            ExitCommand = new RelayCommand(Execute_ExitCommand);
            ChangeLanguageCommand = new RelayCommand(Execute_ChangeLanguageCommand);

        }

        private void Execute_ChangeLanguageCommand(object obj)
        {
            LanguageButtonClickCount++;

            if (LanguageButtonClickCount % 2 == 1)
            {
                app.ChangeLanguage(ENG);
                return;
            }

            app.ChangeLanguage(SRB);
        }

        private void Execute_ExitCommand(object obj)
        {
            CloseAction();
        }

        private void InitializeChart()
        {
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

        public void InitializeLanguageButton(string lang)
        {
            if (lang == SRB)
            {
                LanguageButtonClickCount = 0;
                return;
            }

            LanguageButtonClickCount = 1;
        }

    }
}
