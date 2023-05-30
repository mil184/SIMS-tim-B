using InitialProject.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.View.Guide
{
    /// <summary>
    /// Interaction logic for CreateTourWindow.xaml
    /// </summary>
    /// 

    public partial class CreateTourWindow : Window
    {
        private CreateTourViewModel _viewModel;
        public CreateTourWindow(CreateTourViewModel viewModel)
        {
            InitializeComponent();
            InitializeShortcuts();

            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.NextImage();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.PreviousImage();
        }

        private void btnCreateTour_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Save();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.UpdateCityCoboBox();
        }
        private void InitializeShortcuts()
        {
            PreviewKeyDown += Enter_PreviewKeyDown;
            PreviewKeyDown += Escape_PreviewKeyDown;
            PreviewKeyDown += Create_PreviewKeyDown;
            PreviewKeyDown += Demo_PreviewKeyDown;
            PreviewKeyDown += OnKeyDown;
            PreviewKeyDown += Country_PreviewKeyDown;
            PreviewKeyDown += City_PreviewKeyDown;
            PreviewKeyDown += Language_PreviewKeyDown;
            PreviewKeyDown += Delete_PreviewKeyDown;
        }
        private void Enter_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                if(_viewModel.IsImageTextBoxFocused) 
                {
                    _viewModel.AddImageUrl();
                }

                if (_viewModel.IsDatePickerFocused || _viewModel.IsHoursComboBoxFocused || _viewModel.IsMinutesComboBoxFocused)
                {
                    _viewModel.AddDateTime();
                }
                if (_viewModel.IsCheckpointTextBoxFocused)
                {
                    _viewModel.AddCheckpoint();
                }   
                e.Handled = true;
            }
        }
        private void ImageTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.IsImageTextBoxFocused = true;
        }

        private void ImageTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.IsImageTextBoxFocused = false;
        }

        private void DatePicker_GotFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.IsDatePickerFocused = true;
        }
        private void DatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.IsDatePickerFocused = false;
        }
        private void HoursComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.IsHoursComboBoxFocused = true;
        }
        private void HoursComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.IsHoursComboBoxFocused = false;
        }
        private void MinutesComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.IsMinutesComboBoxFocused = true;
        }
        private void MinutesComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.IsMinutesComboBoxFocused = false;
        }

        private void CheckpointTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.IsCheckpointTextBoxFocused = true;
        }
        private void CheckpointTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.IsCheckpointTextBoxFocused = false;
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

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.D){
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
        private void Create_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!_viewModel.IsDemo && Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                _viewModel.Save();
            }
        }
        private void Country_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (!_viewModel.IsDemo && Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.D1)
            {
                _viewModel.SetMostRequestedCountry();
                e.Handled = true;
            }
        }
        private void City_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (!_viewModel.IsDemo && Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.D2)
            {
                _viewModel.SetMostRequestedCity();
                e.Handled = true;
            }
        }
        private void Language_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (!_viewModel.IsDemo && Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.D3)
            {
                _viewModel.SetMostRequestedLanguage();
                e.Handled = true;
            }
        }
        private void Delete_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!_viewModel.IsDemo && e.Key == Key.Delete)
            {
                if (_viewModel.SelectedCheckpoint != null) 
                {
                    _viewModel.RemoveSelectedCheckpoint();
                }
                if (_viewModel.SelectedDateInList != null)
                {
                    _viewModel.RemoveSelectedDate();
                }
                else
                {
                    _viewModel.RemoveSelectedImage();
                }
                e.Handled = true;
            }
        }
    }
}
