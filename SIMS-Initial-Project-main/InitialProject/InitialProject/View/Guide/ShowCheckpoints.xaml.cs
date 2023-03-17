using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
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
    public partial class ShowCheckpoints : Window, INotifyPropertyChanged
    {

        private readonly Tour SelectedTour;

        private readonly CheckpointRepository _repository;
        private readonly TourRepository _tourRepository;
        public ObservableCollection<Checkpoint> Checkpoints { get; set; }
        public ObservableCollection<User> Guests { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ShowCheckpoints(Tour tour, CheckpointRepository checkpointRepository,TourRepository tourRepository)
        {
            InitializeComponent();
            this.DataContext = this;

            SelectedTour = tour;
            _repository = checkpointRepository;
            _tourRepository = tourRepository;   

            Checkpoints = new ObservableCollection<Checkpoint>();
            Guests = new ObservableCollection<User>();

            Guests.Add(new User("DEJAN", "dejo", InitialProject.Resources.Enums.UserType.example));
            Guests.Add(new User("MILICA", "dejo", InitialProject.Resources.Enums.UserType.example));
            Guests.Add(new User("JELENA", "dejo", InitialProject.Resources.Enums.UserType.example));
            Guests.Add(new User("MILOS", "dejo", InitialProject.Resources.Enums.UserType.example));

            foreach (int id in SelectedTour.CheckpointIds)
            {
                Checkpoints.Add(_repository.GetById(id));
            }

            Checkpoints.OrderBy(c => c.Order);
  
            // Attach the Loaded event handler to the ListBox
            listBox.Loaded += ListBox_Loaded;
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the current checkpoint ID
            int currentCheckpointId = SelectedTour.CurrentCheckpointId;

            // Get the ID of the first checkpoint in the tour
            int firstCheckpointId = Checkpoints.Min(c => c.Id);

            // Check and disable all checkboxes based on the current checkpoint ID and the first checkpoint ID
            // Keep track of the previously selected item
            ListBoxItem previousListBoxItem = null;

            for (int i = 0; i < listBox.Items.Count; i++)
            {
                Checkpoint checkpoint = (Checkpoint)listBox.Items[i];
                CheckBox checkbox = FindVisualChild<CheckBox>(listBox.ItemContainerGenerator.ContainerFromIndex(i));

                if (checkpoint.Id <= currentCheckpointId)
                {
                    checkbox.IsChecked = true;
                    checkbox.IsEnabled = false;

                    ListBoxItem listBoxItem = listBox.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                    if (listBoxItem != null)
                    {
                        listBoxItem.Background = Brushes.LightGray;
                    }
                }
                else if (checkpoint.Id == currentCheckpointId + 1)
                {
                    checkbox.IsChecked = false;
                    checkbox.IsEnabled = true;

                    ListBoxItem listBoxItem = listBox.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                    if (listBoxItem != null)
                    {
                        listBoxItem.Background = Brushes.White;
                    }
                }
                else
                {
                    checkbox.IsChecked = false;
                    checkbox.IsEnabled = checkpoint.Id == firstCheckpointId + 1;

                    ListBoxItem listBoxItem = listBox.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                    if (listBoxItem != null)
                    {
                        listBoxItem.Background = Brushes.White;
                    }
                }

                if (checkpoint.Id == SelectedTour.CurrentCheckpointId)
                {
                    ListBoxItem listBoxItem = listBox.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                    if (listBoxItem != null)
                    {
                        // Remove border from the previous item, if any
                        if (previousListBoxItem != null)
                        {
                            previousListBoxItem.BorderThickness = new Thickness(0);
                            previousListBoxItem.BorderBrush = null;
                        }

                        // Apply border to the current item
                        listBoxItem.BorderThickness = new Thickness(3);
                        listBoxItem.BorderBrush = Brushes.Blue;

                        // Update the previous item
                        previousListBoxItem = listBoxItem;
                    }
                }
            }
        }



        private void checkpointCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox currentCheckbox = (CheckBox)sender;
            currentCheckbox.IsEnabled = false;
            int currentIndex = listBox.ItemContainerGenerator.IndexFromContainer(listBox.ItemContainerGenerator.ContainerFromItem(currentCheckbox.DataContext));
            int nextIndex = currentIndex + 1;

            // If we're at the last checkpoint, don't do anything
            if (nextIndex >= Checkpoints.Count)
            {
                SelectedTour.CurrentCheckpointId = -1;
                SelectedTour.IsActive = false;
                _tourRepository.Update(SelectedTour);
                Close();
                return;
            }

            // Enable the next checkbox and update the current checkpoint
            CheckBox nextCheckbox = FindVisualChild<CheckBox>(listBox.ItemContainerGenerator.ContainerFromIndex(nextIndex));
            nextCheckbox.IsEnabled = true;
            SelectedTour.CurrentCheckpointId = Checkpoints[nextIndex].Id;
            _tourRepository.Update(SelectedTour);

            // Disable all checkboxes before the current checkpoint
            for (int i = 0; i < currentIndex; i++)
            {
                CheckBox checkbox = FindVisualChild<CheckBox>(listBox.ItemContainerGenerator.ContainerFromIndex(i));
                checkbox.IsChecked = true;
                checkbox.IsEnabled = false;
            }

            // Enable all checkboxes after the current checkpoint
            for (int i = nextIndex + 1; i < Checkpoints.Count; i++)
            {
                CheckBox checkbox = FindVisualChild<CheckBox>(listBox.ItemContainerGenerator.ContainerFromIndex(i));
                checkbox.IsChecked = false;
                checkbox.IsEnabled = false;
            }

            // Set the current checkpoint to the one that was just checked
            SelectedTour.CurrentCheckpointId = Checkpoints[currentIndex].Id;
            _tourRepository.Update(SelectedTour);

            // Update the ListBoxItem backgrounds and borders
            ListBox_Loaded(listBox, null);

            // Remove border from the previous item, if any
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
            SelectedTour.CurrentCheckpointId = -1;
            SelectedTour.IsActive = false;
            _tourRepository.Update(SelectedTour);
            Close();
        }
    }
}
