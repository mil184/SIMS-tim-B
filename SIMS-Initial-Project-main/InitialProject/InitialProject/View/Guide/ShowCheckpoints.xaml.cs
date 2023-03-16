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
    public partial class ShowCheckpoints : Window,INotifyPropertyChanged
    {

        private readonly Tour selectedTour;
        private readonly CheckpointRepository _repository;

        public ObservableCollection<Checkpoint> Checkpoints { get; set; }


        private int currentCheckpointIndex = 0;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ShowCheckpoints(Tour tour, CheckpointRepository checkpointRepository)
        {
            InitializeComponent();
            this.DataContext = this;

            selectedTour = tour;
            _repository = checkpointRepository;

            Checkpoints = new ObservableCollection<Checkpoint>();

            foreach (int id in selectedTour.CheckpointIds)
            {
                Checkpoints.Add(_repository.GetById(id));
            }

            Checkpoints.OrderBy(c => c.Order);

            // Attach the Loaded event handler
            Loaded += ShowCheckpoints_Loaded;
        }

        private void ShowCheckpoints_Loaded(object sender, RoutedEventArgs e)
        {
            // Disable buttons and checkboxes except for the first one
            for (int i = 1; i < listBox.Items.Count; i++)
            {
                var listBoxItem = listBox.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                var checkpointCheckbox = FindVisualChild<CheckBox>(listBoxItem);
                var button = FindVisualChild<Button>(listBoxItem);
                checkpointCheckbox.IsEnabled = false;
                button.IsEnabled = false;
            }

            // Check the first checkbox
            var firstListBoxItem = listBox.ItemContainerGenerator.ContainerFromIndex(0) as ListBoxItem;
            var firstCheckpointCheckbox = FindVisualChild<CheckBox>(firstListBoxItem);
            firstCheckpointCheckbox.IsChecked = true;

            // Remove the button from the last item
            var lastListBoxItem = listBox.ItemContainerGenerator.ContainerFromIndex(listBox.Items.Count - 1) as ListBoxItem;
            var lastButton = FindVisualChild<Button>(lastListBoxItem);
            lastButton.Visibility = Visibility.Collapsed;
        }




        public CheckpointDTO ConvertToDTO(Checkpoint checkpoint) 
        {
            return new CheckpointDTO(checkpoint.Id, checkpoint.Name, checkpoint.Order);
        }

        public List<CheckpointDTO> ConvertToDTO(List<Checkpoint> checkpoints) 
        {
            List<CheckpointDTO> dtos = new List<CheckpointDTO>();

            foreach (Checkpoint checkpoint in checkpoints) 
            {
                dtos.Add(ConvertToDTO(checkpoint));
            }

            return dtos;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var listBoxItem = FindVisualParent<ListBoxItem>(button);

            // Enable the next checkbox
            currentCheckpointIndex++;
            if (currentCheckpointIndex < listBox.Items.Count)
            {
                var nextListBoxItem = listBox.ItemContainerGenerator.ContainerFromIndex(currentCheckpointIndex) as ListBoxItem;
                var nextCheckpointCheckbox = FindVisualChild<CheckBox>(nextListBoxItem);
                nextCheckpointCheckbox.IsEnabled = true;
            }

            // Disable the current button
            button.IsEnabled = false;

            // Check the current checkpoint
            var checkpointCheckbox = FindVisualChild<CheckBox>(listBoxItem);
            checkpointCheckbox.IsChecked = true;
        }
        private void Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            var listBoxItem = FindVisualParent<ListBoxItem>(checkbox);

            // Disable the checkbox
            checkbox.IsEnabled = false;

            // Enable the button in the same row as the checkbox
            var button = FindVisualChild<Button>(listBoxItem);
            button.IsEnabled = true;
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child != null && child is T typedChild)
                {
                    return typedChild;
                }

                var childOfChild = FindVisualChild<T>(child);

                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }

            return null;
        }

        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null)
            {
                if (parent is T t)
                {
                    return t;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }
    }
}
