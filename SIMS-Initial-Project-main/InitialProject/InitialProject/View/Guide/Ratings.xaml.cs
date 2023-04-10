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
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void RatingsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RatingsOverview overview = new RatingsOverview(_viewModel.SelectedRatingDTO);
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

    }
}
