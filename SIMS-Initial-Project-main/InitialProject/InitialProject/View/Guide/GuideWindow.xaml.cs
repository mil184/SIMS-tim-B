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

namespace InitialProject.View.Guide
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class GuideWindow : Window
    {
        public User LoggedInUser { get; set; }
        private Tour _currentTour;
        public Tour CurrentTour
        {
            get => _currentTour;
            set
            {
                _currentTour = value;
                OnPropertyChanged(nameof(CurrentTour));
                OnPropertyChanged(nameof(CurrentTourCountry));
                OnPropertyChanged(nameof(CurrentTourCity));
            }
        }
        public List<Tour> UpcomingTours { get; set; }

        private readonly TourRepository _repository;
        private readonly LocationRepository _locationRepository;

        private string _currentTourCountry;
        public string CurrentTourCountry
        {
            get {
                if (CurrentTour != null)
                {
                    return _locationRepository.GetById(CurrentTour.Id).Country;
                }
                return null;
            } 
            set
            {
                    _currentTourCountry = value;                  
            }
        }
        private string _currentTourCity;
        public string CurrentTourCity
        {
            get
            {
                if (CurrentTour != null)
                {
                    return _locationRepository.GetById(CurrentTour.Id).City;
                }
                return null;
            }
            set
            {
                _currentTourCity = value;
            }
        }
        public GuideWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            _repository = new TourRepository();
            _locationRepository = new LocationRepository();

            CurrentTour = _repository.GetCurrentTour();

            UpcomingTours = _repository.GetUpcomingTours();
            var view = CollectionViewSource.GetDefaultView(UpcomingTours);
            view.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour(LoggedInUser);
            createTour.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StackPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("Clicked");
        }

    }

}
