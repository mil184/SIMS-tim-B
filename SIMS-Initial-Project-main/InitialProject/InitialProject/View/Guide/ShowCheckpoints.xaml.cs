using Accessibility;
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
        public ObservableCollection<Checkpoint> Checkpoints { get; set; }
        public int Index { get; set; }

        private ObservableCollection<UserDTO> unmarkedGuests;
        public ObservableCollection<UserDTO> UnmarkedGuests
        {
            get { return unmarkedGuests; }
            set
            {
                if (unmarkedGuests != value)
                {
                    unmarkedGuests = value;
                    OnPropertyChanged(nameof(UnmarkedGuests));
                }
            }
        }
        public UserDTO SelectedUserDTO { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ShowCheckpoints(Tour tour, CheckpointService checkpointService, TourService tourService, TourReservationService tourReservationService, UserRepository userRepository, TourRatingService tourRatingService)
        {
            InitializeComponent();
            DataContext = this;

            SelectedTour = tour;
            _checkpointService = checkpointService;
            _tourService = tourService;
            _tourReservationService = tourReservationService;
            _userRepository = userRepository;


            InitializeCollections();
            LoadUnmarkedGuests();
            LoadTourCheckpoints();
            InitializeShortcuts();

            // Attach the LOADED EVENT handler to the ListBox, this is becaue of the visuals bg color, border, checked
            listBox.Loaded += ListBox_Loaded;
        }
        private void InitializeCollections()
        {
            Checkpoints = new ObservableCollection<Checkpoint>();
            UnmarkedGuests = new ObservableCollection<UserDTO>();
            Index = 1;
        }
        private void LoadUnmarkedGuests()
        {
            List<int> unmarkedGuestsId = _tourReservationService.GetAllUserIdsByTour(SelectedTour);

            foreach (int id in unmarkedGuestsId)
            {
                User user = _userRepository.GetById(id);
                UserDTO userDto = ConvertUserToDTO(user);

                if (!HasUser(userDto.UserId))
                    UnmarkedGuests.Add(userDto);
            }
        }
        private void InitializeShortcuts()
        {
            PreviewKeyDown += Enter_PreviewKeyDown;
            PreviewKeyDown += Escape_PreviewKeyDown;
            PreviewKeyDown += DataGrid_PreviewKeyDown;
            PreviewKeyDown += NextCheckpoint_PreviewKeyDown;
            PreviewKeyDown += EndTour_PreviewKeyDown;
        }
        private bool HasUser(int userId)
        {
            foreach (UserDTO user in UnmarkedGuests)
            {
                if (user.UserId == userId)
                {
                    return true;
                }
            }
            return false;
        }
        private void LoadTourCheckpoints()
        {
            foreach (int id in SelectedTour.CheckpointIds)
            {
                Checkpoint checkpoint = _checkpointService.GetById(id);
                Checkpoints.Add(checkpoint);
            }
            Checkpoints.OrderBy(c => c.Order);
        }
        public UserDTO ConvertUserToDTO(User user)
        {
            int checkpointId = _tourReservationService.GetReservationByGuestIdAndTourId(user.Id, SelectedTour.Id).CheckpointArrivalId;
            String checkpointName;
            if (checkpointId == -1)
            {
                checkpointName = "Not Arrived Yet";
            }
            else
            {
                checkpointName = _checkpointService.GetById(_tourReservationService.GetReservationByGuestIdAndTourId(user.Id, SelectedTour.Id).CheckpointArrivalId).Name;
            }
            return new UserDTO(
                user.Id,
                user.Username,
                checkpointName
                );
        }

        // This method is called when the ListBox is loaded into the user interface. It sets the VISUAL APPEARANCE of each ListBox item
        // based on the current state of the application, including the currently selected checkpoint and the first checkpoint in the list.
        // The method iterates through each ListBox item, gets the corresponding Checkpoint, CheckBox, and ListBoxItem controls, and calls 
        // the SetCheckpointCheckboxAndBackground method to set the state of the CheckBox and the background color of the ListBoxItem.
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                listBox.ScrollIntoView(listBox.SelectedItem);
            }
        }
        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            int currentCheckpointId = SelectedTour.CurrentCheckpointId;
            int firstCheckpointId = Checkpoints.Min(c => c.Id);
            ListBoxItem previousListBoxItem = null;

            for (int i = 0; i < listBox.Items.Count; i++)
            {
                Checkpoint checkpoint = (Checkpoint)listBox.Items[i];
                ListBoxItem listBoxItem = listBox.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;

                SetBackground(checkpoint, listBoxItem, currentCheckpointId, firstCheckpointId, ref previousListBoxItem);
            }
        }
        private void SetBackground(Checkpoint checkpoint, ListBoxItem listBoxItem, int currentCheckpointId, int firstCheckpointId, ref ListBoxItem previousListBoxItem)
        {
            if (checkpoint.Id < currentCheckpointId)
            {
                SetBackground(listBoxItem, Brushes.LightGray);
            }

            else if (checkpoint.Id == currentCheckpointId + 1)
            {
                SetBackground(listBoxItem, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2F2F2")));
            }
            else
            {
                SetBackground(listBoxItem, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2F2F2")));
            }
            if (checkpoint.Id == SelectedTour.CurrentCheckpointId)
            {
                SetListBoxItemBorder(listBoxItem, previousListBoxItem);
                previousListBoxItem = listBoxItem;
            }
        }
        private void SetBackground(ListBoxItem listBoxItem, Brush brush)
        {
            if (listBoxItem != null)
            {
                listBoxItem.Background = brush;
            }
        }
        private void SetListBoxItemBorder(ListBoxItem listBoxItem, ListBoxItem previousListBoxItem)
        {
            if (listBoxItem != null)
            {
                listBoxItem.BorderThickness = new Thickness(5);
                listBoxItem.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));

                if (previousListBoxItem != null)
                {
                    previousListBoxItem.BorderThickness = new Thickness(0);
                    previousListBoxItem.BorderBrush = null;
                }
            }
        }
        private void UpdateListBoxItemBackgrounds(int currentIndex)
        {
            ListBox_Loaded(listBox, null);
        }
        private void RemovePreviousListBoxItemBorder(int currentIndex)
        {
            if (currentIndex > 0)
            {
                ListBoxItem previousListBoxItem = listBox.ItemContainerGenerator.ContainerFromIndex(currentIndex - 1) as ListBoxItem;
                ListBoxItem currentListBoxItem = listBox.ItemContainerGenerator.ContainerFromIndex(currentIndex) as ListBoxItem;

                if (previousListBoxItem != null && currentListBoxItem != null && previousListBoxItem != currentListBoxItem)
                {
                    previousListBoxItem.BorderThickness = new Thickness(0);
                    previousListBoxItem.BorderBrush = null;
                }
            }
        }
        private void EndTour()
        {
            UpdateTourStatus();
            Close();
        }
        private void UpdateTourStatus()
        {
            SelectedTour.IsActive = false;
            SelectedTour.IsFinished = true;
            _tourService.Update(SelectedTour);
        }
        private void UpdateNextCheckpoint(int nextIndex)
        {
            SelectedTour.CurrentCheckpointId = Checkpoints[nextIndex].Id;
            _tourService.Update(SelectedTour);
        }
        private void UpdateCurrentCheckpoint(int currentIndex)
        {
            SelectedTour.CurrentCheckpointId = Checkpoints[currentIndex].Id;
            _tourService.Update(SelectedTour);
        }
        private void endTourButton_Click(object sender, RoutedEventArgs e)
        {
            var messageBoxResult = ShowEndTourConfirmationMessage();

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                EndTour();
            }
        }
        private void presentButton_Click(object sender, RoutedEventArgs e)
        {
            MarkPresent();
        }

        private void MarkPresent() 
        {
            if (SelectedUserDTO == null) return;

            if (SelectedUserDTO.CheckpointArrivalName != "Not Arrived Yet") return;

            UpdateReservationCheckpointArrivalId();
            UpdateGuestCheckpointArrivalNameInUnmarkedGuests();
            UpdateGuestsGrid();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void UpdateReservationCheckpointArrivalId()
        {
            var selectedGuest = SelectedUserDTO;
            if (selectedGuest != null)
            {
                int currentCheckpointId = SelectedTour.CurrentCheckpointId;
                TourReservation reservation = GetReservationByGuestIdAndTourId(selectedGuest.UserId, SelectedTour.Id);
                UpdateReservationCheckpointArrival(reservation, currentCheckpointId);
            }
        }
        private TourReservation GetReservationByGuestIdAndTourId(int guestId, int tourId)
        {
            return _tourReservationService.GetReservationByGuestIdAndTourId(guestId, tourId);
        }
        private void UpdateReservationCheckpointArrival(TourReservation reservation, int checkpointId)
        {
            reservation.CheckpointArrivalId = checkpointId;
            _tourReservationService.Update(reservation);
        }
        private void UpdateGuestCheckpointArrivalNameInUnmarkedGuests()
        {
            var selectedGuest = SelectedUserDTO;
            if (selectedGuest != null)
            {
                string checkpointName = GetCheckpointNameById(SelectedTour.CurrentCheckpointId);
                UpdateGuestCheckpointArrivalName(selectedGuest.UserId, checkpointName);
            }
        }
        private string GetCheckpointNameById(int checkpointId)
        {
            return _checkpointService.GetById(checkpointId).Name;
        }
        private void UpdateGuestCheckpointArrivalName(int guestId, string checkpointName)
        {
            foreach (UserDTO guest in UnmarkedGuests)
            {
                if (guest.UserId == guestId)
                {
                    guest.CheckpointArrivalName = checkpointName;
                    TourReservation reservation = _tourReservationService.GetReservationByGuestIdAndTourId(guestId, SelectedTour.Id);
                    reservation.GuestChecked = true;
                    _tourReservationService.Update(reservation);
                }
            }
        }
        private void UpdateGuestsGrid()
        {
            Update();
        }
        public void Update()
        {
            UnmarkedGuests.Clear();

            foreach (int id in _tourReservationService.GetAllUserIdsByTour(SelectedTour))
            {
                if (!UnmarkedGuests.Contains(ConvertUserToDTO(_userRepository.GetById(id))))
                    UnmarkedGuests.Add(ConvertUserToDTO(_userRepository.GetById(id)));
            }
        }
        private MessageBoxResult ShowEndTourConfirmationMessage()
        {
            return MessageBox.Show($"Are you sure you want to finish the {SelectedTour.Name} tour?", "Finish Tour Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
        private void Escape_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        private void Enter_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SelectedUserDTO != null)
            {
                MarkPresent();
                e.Handled = true;
            }
        }
        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.L)
            {
                if (guestsGrid.Items.Count > 0)
                {
                    guestsGrid.SelectedItem = guestsGrid.Items[0];
                    guestsGrid.ScrollIntoView(guestsGrid.SelectedItem);
                    guestsGrid.Focus();
                }
                e.Handled = true;
            }
        }
        private void EndTour_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.E)
            {
                EndTour();
                e.Handled = true;
            }
        }
        private void NextCheckpoint_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.S)
            {
                NextCheckpoint();
                e.Handled = true;
            }
        }
        private void nextCheckpointButton_Click(object sender, RoutedEventArgs e)
        {
            NextCheckpoint();
        }

        private void NextCheckpoint() 
        {
            int currentIndex = Index;
            int nextIndex = currentIndex + 1;

            if (nextIndex >= Checkpoints.Count)
            {
                EndTour();
                return;
            }

            UpdateNextCheckpoint(nextIndex);

            UpdateCurrentCheckpoint(currentIndex);

            UpdateListBoxItemBackgrounds(currentIndex);
            RemovePreviousListBoxItemBorder(currentIndex);

            Index++;
        }

    }
}
