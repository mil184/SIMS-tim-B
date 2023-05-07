using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using InitialProject.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
    /// Interaction logic for Ratings.xaml
    /// </summary>
    public partial class Ratings : Window
    {
        private readonly RatingsViewModel _viewModel;

        public Ratings(RatingsViewModel viewModel)
        {
            InitializeComponent();
            InitializeShortcuts();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }
        private void InitializeShortcuts()
        {
            PreviewKeyDown += Report_PreviewKeyDown;
            PreviewKeyDown += Enter_PreviewKeyDown;
            PreviewKeyDown += Escape_PreviewKeyDown;
            PreviewKeyDown += DataGrid_PreviewKeyDown;
            PreviewKeyDown += UpDownArrowKeys_PreviewKeyDown;
            PreviewKeyDown += SortAsc_PreviewKeyDown;
            PreviewKeyDown += SortDesc_PreviewKeyDown;
        }
        private void RatingsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RatingsOverviewViewModel ratingsOverviewViewModel = new RatingsOverviewViewModel(_viewModel.SelectedRatingDTO);
            RatingsOverview overview = new RatingsOverview(ratingsOverviewViewModel);
            overview.Show();
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ReportTourRating();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Escape_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        private void Report_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.R)
            {
                _viewModel.ReportTourRating();
            }
        }
        private void Enter_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && _viewModel.SelectedRatingDTO != null)
            {
                RatingsOverviewViewModel ratingsOverviewViewModel = new RatingsOverviewViewModel(_viewModel.SelectedRatingDTO);
                RatingsOverview overview = new RatingsOverview(ratingsOverviewViewModel);
                overview.Show();
            }
        }
        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.L)
            {
                if (RatingsDataGrid.Items.Count > 0)
                {
                    RatingsDataGrid.SelectedItem = RatingsDataGrid.Items[0];
                    RatingsDataGrid.ScrollIntoView(RatingsDataGrid.SelectedItem);
                    RatingsDataGrid.Focus();
                }
                e.Handled = true;
            }
        }
        private void UpDownArrowKeys_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down || e.Key == Key.Up)
            {

                DataGrid currentDataGrid = RatingsDataGrid;
                int current_index = currentDataGrid.SelectedIndex;
                if (Keyboard.IsKeyDown(Key.Down))
                {
                    if (current_index < currentDataGrid.Items.Count - 1)
                    {
                        // Select the next item in the DataGrid
                        currentDataGrid.SelectedItem = currentDataGrid.Items[current_index + 1];
                        currentDataGrid.ScrollIntoView(currentDataGrid.SelectedItem);
                        currentDataGrid.Focus();
                    }
                    else
                    {
                        currentDataGrid.SelectedItem = currentDataGrid.Items[0];
                        currentDataGrid.ScrollIntoView(currentDataGrid.SelectedItem);
                        currentDataGrid.Focus();
                    }
                }
                else if (Keyboard.IsKeyDown(Key.Up))
                {
                    if (current_index > 0)
                    {
                        // Select the previous item in the DataGrid
                        currentDataGrid.SelectedItem = currentDataGrid.Items[current_index - 1];
                        currentDataGrid.ScrollIntoView(currentDataGrid.SelectedItem);
                        currentDataGrid.Focus();
                    }
                    else
                    {
                        // The currently selected item is the first item in the DataGrid, select the last item instead
                        currentDataGrid.SelectedItem = currentDataGrid.Items[currentDataGrid.Items.Count - 1];
                        currentDataGrid.ScrollIntoView(currentDataGrid.SelectedItem);
                        currentDataGrid.Focus();
                    }
                }
                e.Handled = true;
            }
        }
        private void SortAsc_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftShift) || !Keyboard.IsKeyDown(Key.A))
                return;

            var grid = RatingsDataGrid;
            if (grid == null || grid.Items.Count == 0)
                return;

            var view = CollectionViewSource.GetDefaultView(grid.ItemsSource);
            var sortColumn = GetNextSortColumn();
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription(sortColumn, ListSortDirection.Ascending));
            view.Refresh();
            e.Handled = true;
        }
        private void SortDesc_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftShift) || !Keyboard.IsKeyDown(Key.D))
                return;

            var grid = RatingsDataGrid;
            if (grid == null || grid.Items.Count == 0)
                return;

            var view = CollectionViewSource.GetDefaultView(grid.ItemsSource);
            var sortColumn = GetNextSortColumn();
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription(sortColumn, ListSortDirection.Descending));
            view.Refresh();
            e.Handled = true;
        }
        private string GetNextSortColumn()
        {
            switch (_viewModel.CurrentSortIndex)
            {
                case 0:
                    _viewModel.CurrentSortIndex++;
                    return "Username";
                case 1:
                    _viewModel.CurrentSortIndex++;
                    return "Arrival";
                case 2:
                    _viewModel.CurrentSortIndex++;
                    return "Rating";
                case 3:
                    _viewModel.CurrentSortIndex = 0;
                    return "Valid";
                default:
                    return "";
            }
        }
    }
}
