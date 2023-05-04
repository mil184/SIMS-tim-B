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
    }
}
