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
    public partial class ShowCheckpoints : Window
    {
        private readonly ShowCheckpointsViewModel _viewModel;
        public ShowCheckpoints(ShowCheckpointsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeShortcuts();
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem == null && listBox.Items.Count > 0)
            {
                if (_viewModel.SelectedCheckpoint != null)
                {
                    listBox.SelectedIndex = _viewModel.SelectedCheckpoint.Order - 1;
                }
                else 
                {
                    listBox.SelectedIndex = 0;
                }
            }
        }

        public void Update()
        {

        }
        private void UpdateCheckpointButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.UpdateCheckpoint();

            if (_viewModel.ActiveTour.IsFinished)
                Close();
        }
        private void EndTourButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.EndTour();
            Close();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void CheckUser_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CheckUser();

        }
        private void InitializeShortcuts()
        {
            PreviewKeyDown += Enter_PreviewKeyDown;
            PreviewKeyDown += OnKeyDown;
            PreviewKeyDown += Escape_PreviewKeyDown;
            PreviewKeyDown += EndTour_PreviewKeyDown;
            PreviewKeyDown += UpdateCheckpoint_PreviewKeyDown;
            PreviewKeyDown += Demo_PreviewKeyDown;
        }
        private void Enter_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter && _viewModel.SelectedGuestDTO != null && !_viewModel.IsDemo)
            {
                _viewModel.CheckUser();
                e.Handled = true;
            }
        }
        private void Demo_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (!_viewModel.IsDemo && Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.D)
            {
                _viewModel.StartDemoAsync();
                e.Handled = true;
            }
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                return;
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.D)
            {
                return;
            }
            if (_viewModel.IsDemo)
            {
                _viewModel.StopDemo = true;
                e.Handled = true;

            }
        }
        private void Escape_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!_viewModel.IsDemo && e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        private void EndTour_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!_viewModel.IsDemo && Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.E)
            {
                _viewModel.EndTour();
                this.Close();
            }
        }
        private void UpdateCheckpoint_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!_viewModel.IsDemo && Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                _viewModel.UpdateCheckpoint();
            }
        }
    }
}
