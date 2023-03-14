using InitialProject.Model;
using InitialProject.Repository;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System;
using System.Globalization;
using System.Windows.Data;
using InitialProject.Resources.Observer;

namespace InitialProject.View.Guide
{
    public partial class GuideWindow : Window, INotifyPropertyChanged, IObserver
    {
        public User LoggedInUser { get; set; }
        public ObservableCollection<Tour> CurrentTours { get; set; }
        public ObservableCollection<Tour> UpcomingTours { get; set; }

        private readonly TourRepository _repository;
        private readonly LocationRepository _locationRepository;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointRepository _checkpointRepository;

        public GuideWindow(User user)
        {
            InitializeComponent();

            DataContext = this;
            LoggedInUser = user;

            _repository = new TourRepository();
            _repository.Subscribe(this);
            _imageRepository = new ImageRepository();
            _imageRepository.Subscribe(this);
            _locationRepository = new LocationRepository();
            _locationRepository.Subscribe(this);
            _checkpointRepository = new CheckpointRepository();
            _checkpointRepository.Subscribe(this);

            CurrentTours = new ObservableCollection<Tour>(_repository.GetTodaysTours());
            UpcomingTours = new ObservableCollection<Tour>(_repository.GetUpcomingTours());
            var view = CollectionViewSource.GetDefaultView(UpcomingTours);
            view.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour(LoggedInUser, _repository, _locationRepository, _imageRepository, _checkpointRepository);
            createTour.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void IObserver.Update()
        {
            UpcomingTours.Clear();
            foreach (Tour tour in _repository.GetUpcomingTours())
            {
                UpcomingTours.Add(tour);
            }

            CurrentTours.Clear();
            foreach (Tour tour in _repository.GetTodaysTours())
            {
                CurrentTours.Add(tour);
            }
        }
    }

}
