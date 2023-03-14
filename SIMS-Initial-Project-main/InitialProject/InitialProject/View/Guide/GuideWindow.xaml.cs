using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace InitialProject.View.Guide
{
    public partial class GuideWindow : Window, INotifyPropertyChanged, IObserver
    {
        public User CurrentUser { get; set; }
        public ObservableCollection<Tour> CurrentTours { get; set; }
        public ObservableCollection<Tour> UpcomingTours { get; set; }

        private readonly TourRepository _tourRepository;
        private readonly LocationRepository _locationRepository;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointRepository _checkpointRepository;

        public GuideWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            CurrentUser = user;

            _tourRepository = new TourRepository();
            _tourRepository.Subscribe(this);

            _imageRepository = new ImageRepository();
            _imageRepository.Subscribe(this);

            _locationRepository = new LocationRepository();
            _locationRepository.Subscribe(this);

            _checkpointRepository = new CheckpointRepository();
            _checkpointRepository.Subscribe(this);

            CurrentTours = new ObservableCollection<Tour>(_tourRepository.GetTodaysTours());
            UpcomingTours = new ObservableCollection<Tour>(_tourRepository.GetUpcomingTours());
            var view = CollectionViewSource.GetDefaultView(UpcomingTours);
            view.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour(CurrentUser, _tourRepository, _locationRepository, _imageRepository, _checkpointRepository);
            createTour.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Update()
        {
            UpcomingTours.Clear();
            foreach (Tour tour in _tourRepository.GetUpcomingTours())
            {
                UpcomingTours.Add(tour);
            }

            CurrentTours.Clear();
            foreach (Tour tour in _tourRepository.GetTodaysTours())
            {
                CurrentTours.Add(tour);
            }
        }
    }


}
