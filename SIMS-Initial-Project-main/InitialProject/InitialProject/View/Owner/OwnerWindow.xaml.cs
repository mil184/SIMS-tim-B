using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window, INotifyPropertyChanged, IObserver
    {
        public User LoggedInUser { get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        private AccommodationRepository _repository;
        private readonly LocationRepository _locationRepository;
        private readonly ImageRepository _imageRepository;

        public OwnerWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;

            _repository = new AccommodationRepository();
            _repository.Subscribe(this);
            _locationRepository = new LocationRepository();
            _locationRepository.Subscribe(this);
            _imageRepository = new ImageRepository();
            _imageRepository.Subscribe(this);

            Accommodations = new ObservableCollection<Accommodation>(_repository.GetAll());
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterAccommodation registerAccommodation = new RegisterAccommodation(LoggedInUser, _repository, _locationRepository, _imageRepository);
            registerAccommodation.Show();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                string MessageBoxText = "Are You Sure You Want To Delete Accommodation?";
                string Caption = "brisanje profesora";
                MessageBoxButton Button = MessageBoxButton.YesNo;
                MessageBoxImage Icon = MessageBoxImage.Warning;
                MessageBoxResult Result;

                Result = MessageBox.Show(MessageBoxText, Caption, Button, Icon, MessageBoxResult.Yes);
                if (Result == MessageBoxResult.Yes)
                {

                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void IObserver.Update()
        {
            Accommodations.Clear();
            foreach (Accommodation accommodation in _repository.GetAll())
            {
                Accommodations.Add(accommodation);
            }
        }
    }
}
