using Accessibility;
using InitialProject.Converters;
using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using InitialProject.Resources.UIHelper;
using InitialProject.Service;
using InitialProject.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace InitialProject.View.Guide
{
    public partial class ShowCheckpoints : Window, INotifyPropertyChanged, IObserver
    {
        private readonly Tour SelectedTour;

        private readonly CheckpointService _checkpointService;
        private readonly TourService _tourService;
        private readonly TourReservationService _tourReservationService;
        private readonly UserRepository _userRepository;

        public Tour ActiveTour {get; set;}
        public ObservableCollection<UserDTO> UnmarkedGuests { get; set; }
        public UserDTO SelectedUserDTO { get; set; }

        public ObservableCollection<Checkpoint> Checkpoints { get; set; }

        private Checkpoint currentCheckpoint;
        public Checkpoint CurrentCheckpoint
        {
            get { return currentCheckpoint; }
            set
            {
                if (currentCheckpoint != value)
                {
                    currentCheckpoint = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ShowCheckpoints(Tour tour, CheckpointService checkpointService, TourService tourService, TourReservationService tourReservationService, UserRepository userRepository)
        {
            InitializeComponent();
            DataContext = this;

            SelectedTour = tour;
            _checkpointService = checkpointService;
            _tourService = tourService;
            _tourReservationService = tourReservationService;
            _userRepository = userRepository;

            ActiveTour = tour;

            InitializeCollections();
            SetPropertyValues();

        }
        private void InitializeCollections()
        {
            Checkpoints = new ObservableCollection<Checkpoint>();
            UnmarkedGuests = new ObservableCollection<UserDTO>();
        }
        private void SetPropertyValues()
        {
            foreach(int checkpointId in ActiveTour.CheckpointIds)
            {
                Checkpoints.Add(_checkpointService.GetById(checkpointId));
            }

            foreach (int guestId in _tourReservationService.GetUncheckedUserIdsByTour(ActiveTour)) 
            {
                UnmarkedGuests.Add(GuideDTOConverter.ConvertToDTO(_userRepository.GetById(guestId), _tourReservationService, ActiveTour, _checkpointService));
            }

            CurrentCheckpoint = _checkpointService.GetById(ActiveTour.CurrentCheckpointId);

        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        private void presentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void nextCheckpointButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentCheckpoint.IsActive = false;
            _checkpointService.Update(CurrentCheckpoint);

            int currentIndex = Checkpoints.IndexOf(CurrentCheckpoint);
            CurrentCheckpoint = Checkpoints[++currentIndex];
            CurrentCheckpoint.IsActive = true;
            _checkpointService.Update(CurrentCheckpoint);

            ActiveTour.CurrentCheckpointId = CurrentCheckpoint.Id;
            _tourService.Update(ActiveTour);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void endTourButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
