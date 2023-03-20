using Accessibility;
using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
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
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace InitialProject.View.Guide
{
    public partial class ShowCheckpoints : Window, INotifyPropertyChanged, IObserver
    {
        private readonly Tour SelectedTour;

        private readonly CheckpointRepository _repository;
        private readonly TourRepository _tourRepository;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly UserRepository _userRepository;
        public ObservableCollection<Checkpoint> Checkpoints { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ShowCheckpoints(Tour tour, CheckpointRepository checkpointRepository, TourRepository tourRepository, TourReservationRepository tourReservationRepository, UserRepository userRepository)
        {
            InitializeComponent();
            this.DataContext = this;

            SelectedTour = tour;
            _repository = checkpointRepository;
            _tourRepository = tourRepository;
            _tourReservationRepository = tourReservationRepository;
            _userRepository = userRepository;

            Checkpoints = new ObservableCollection<Checkpoint>();
            UnmarkedGuests = new ObservableCollection<UserDTO>();

            List<int> UnmarkedGuestsId = _tourReservationRepository.GetUserIdsByTour(SelectedTour);

            foreach(int id in UnmarkedGuestsId) 
            {
                if(!UnmarkedGuests.Contains(ConvertToDTO(_userRepository.GetById(id))))
                UnmarkedGuests.Add(ConvertToDTO(_userRepository.GetById(id)));
            }

            foreach (int id in SelectedTour.CheckpointIds)
            {
                Checkpoints.Add(_repository.GetById(id));
            }

            Checkpoints.OrderBy(c => c.Order);
  
            // Attach the Loaded event handler to the ListBox
            listBox.Loaded += ListBox_Loaded;
        }

        public UserDTO ConvertToDTO(User user) 
        {
            return new UserDTO(
                user.Id,
                user.Username,
                _repository.GetById(_tourReservationRepository.GetReservationByGuestIdAndTourId(user.Id, SelectedTour.Id).CheckpointArrivalId).Name
                );
        }
        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            int currentCheckpointId = SelectedTour.CurrentCheckpointId;
            int firstCheckpointId = Checkpoints.Min(c => c.Id);
            ListBoxItem previousListBoxItem = null;

            for (int i = 0; i < listBox.Items.Count; i++)
            {
                Checkpoint checkpoint = (Checkpoint)listBox.Items[i];
                CheckBox checkbox = FindVisualChild<CheckBox>(listBox.ItemContainerGenerator.ContainerFromIndex(i));
                ListBoxItem listBoxItem = listBox.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;

                if (checkpoint.Id <= currentCheckpointId)
                {
                    DisableCheckbox(checkbox, listBoxItem);
                    SetBackground(listBoxItem, Brushes.LightGray);
                }
                else if (checkpoint.Id == currentCheckpointId + 1)
                {
                    EnableCheckbox(checkbox, listBoxItem);
                    SetBackground(listBoxItem, Brushes.White);
                }
                else
                {
                    bool isEnabled = checkpoint.Id == firstCheckpointId + 1;
                    SetCheckbox(checkbox, isEnabled, listBoxItem);
                    SetBackground(listBoxItem, Brushes.White);
                }

                if (checkpoint.Id == SelectedTour.CurrentCheckpointId)
                {
                    SetListBoxItemBorder(listBoxItem, previousListBoxItem);
                    previousListBoxItem = listBoxItem;
                }
            }
        }

        private void DisableCheckbox(CheckBox checkbox, ListBoxItem listBoxItem)
        {
            checkbox.IsChecked = true;
            checkbox.IsEnabled = false;
            SetBackground(listBoxItem, Brushes.LightGray);
        }

        private void EnableCheckbox(CheckBox checkbox, ListBoxItem listBoxItem)
        {
            checkbox.IsChecked = false;
            checkbox.IsEnabled = true;
            SetBackground(listBoxItem, Brushes.White);
        }

        private void SetCheckbox(CheckBox checkbox, bool isEnabled, ListBoxItem listBoxItem)
        {
            checkbox.IsChecked = false;
            checkbox.IsEnabled = isEnabled;
            SetBackground(listBoxItem, Brushes.White);
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
                listBoxItem.BorderThickness = new Thickness(3);
                listBoxItem.BorderBrush = Brushes.Blue;

                if (previousListBoxItem != null)
                {
                    previousListBoxItem.BorderThickness = new Thickness(0);
                    previousListBoxItem.BorderBrush = null;
                }
            }
        }

        private void checkpointCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox currentCheckbox = (CheckBox)sender;
            currentCheckbox.IsEnabled = false;

            int currentIndex = GetCurrentIndex(currentCheckbox);
            int nextIndex = currentIndex + 1;

            if (nextIndex >= Checkpoints.Count)
            {
                EndTour();
                return;
            }

            UpdateNextCheckpoint(nextIndex);

            DisablePreviousCheckpoints(currentIndex);
            DisableFollowingCheckpoints(nextIndex);

            UpdateCurrentCheckpoint(currentIndex);

            UpdateListBoxItemBackgrounds(currentIndex);
            RemovePreviousListBoxItemBorder(currentIndex);
        }

        private int GetCurrentIndex(CheckBox currentCheckbox)
        {
            return listBox.ItemContainerGenerator.IndexFromContainer(
                listBox.ItemContainerGenerator.ContainerFromItem(currentCheckbox.DataContext));
        }

        private void EndTour()
        {
            SelectedTour.CurrentCheckpointId = -1;
            SelectedTour.IsActive = false;
            _tourRepository.Update(SelectedTour);

            MessageBox.Show($"The {SelectedTour.Name} tour finished", "End Tour Information", MessageBoxButton.OK, MessageBoxImage.Information);

            Close();
        }

        private void UpdateNextCheckpoint(int nextIndex)
        {
            CheckBox nextCheckbox = FindVisualChild<CheckBox>(listBox.ItemContainerGenerator.ContainerFromIndex(nextIndex));
            nextCheckbox.IsEnabled = true;

            SelectedTour.CurrentCheckpointId = Checkpoints[nextIndex].Id;
            _tourRepository.Update(SelectedTour);
        }

        private void DisablePreviousCheckpoints(int currentIndex)
        {
            for (int i = 0; i < currentIndex; i++)
            {
                CheckBox checkbox = FindVisualChild<CheckBox>(listBox.ItemContainerGenerator.ContainerFromIndex(i));
                checkbox.IsChecked = true;
                checkbox.IsEnabled = false;
            }
        }

        private void DisableFollowingCheckpoints(int nextIndex)
        {
            for (int i = nextIndex + 1; i < Checkpoints.Count; i++)
            {
                CheckBox checkbox = FindVisualChild<CheckBox>(listBox.ItemContainerGenerator.ContainerFromIndex(i));
                checkbox.IsChecked = false;
                checkbox.IsEnabled = false;
            }
        }

        private void UpdateCurrentCheckpoint(int currentIndex)
        {
            SelectedTour.CurrentCheckpointId = Checkpoints[currentIndex].Id;
            _tourRepository.Update(SelectedTour);
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

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T foundChild = FindVisualChild<T>(child);
                    if (foundChild != null)
                    {
                        return foundChild;
                    }
                }
            }
            return null;
        }

        private void endTourButton_Click(object sender, RoutedEventArgs e)
        {
            var messageBoxResult = MessageBox.Show($"Are you sure you want to finish the {SelectedTour.Name} tour?", "Finish Tour Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SelectedTour.CurrentCheckpointId = -1;
                SelectedTour.IsActive = false;
                _tourRepository.Update(SelectedTour);
                MessageBox.Show($"The {SelectedTour.Name} tour finished", "Finish Tour Information", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        
        }
        private void presentButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGuest = guestsGrid.SelectedItem as UserDTO;
            if (selectedGuest != null)
            {
                TourReservation reservation = _tourReservationRepository.GetReservationByGuestIdAndTourId(selectedGuest.UserId, SelectedTour.Id);
                reservation.CheckpointArrivalId = _tourRepository.GetById(SelectedTour.Id).CurrentCheckpointId;
                _tourReservationRepository.Update(reservation);
                foreach (UserDTO guest in UnmarkedGuests) 
                {
                    if(guest.UserId == selectedGuest.UserId) 
                    {
                        guest.CheckpointArrivalName = _repository.GetById(SelectedTour.CurrentCheckpointId).Name;
                      
                    }
                }
                Update();
                
            }
        }

        public void Update()
        {
            UnmarkedGuests.Clear();

            foreach (int id in _tourReservationRepository.GetUserIdsByTour(SelectedTour))
            {
                if (!UnmarkedGuests.Contains(ConvertToDTO(_userRepository.GetById(id))))
                    UnmarkedGuests.Add(ConvertToDTO(_userRepository.GetById(id)));
            }
        }
    }
}
