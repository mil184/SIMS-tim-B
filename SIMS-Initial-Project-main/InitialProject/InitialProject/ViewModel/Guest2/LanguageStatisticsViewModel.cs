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
    public class LanguageStatisticsViewModel
    {
        public Action CloseAction { get; set; }

        private readonly TourRequestService _tourRequestService;

        public SeriesCollection LanguageSeriesCollection { get; set; }

        public List<int> RequestCounts { get; set; }
        public List<string> Languages { get; set; }

        public int LanguageButtonClickCount { get; set; }
        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";

        public RelayCommand ExitCommand { get; set; }
        public RelayCommand ChangeLanguageCommand { get; set; }

        public LanguageStatisticsViewModel(TourRequestService tourRequestService, string lang)
        {
            _tourRequestService = tourRequestService;

            LanguageSeriesCollection = new SeriesCollection();

            Languages = _tourRequestService.GetLanguages();
            RequestCounts = _tourRequestService.GetRequestCountForLanguage(Languages);

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

        public void InitializeChart()
        {
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
