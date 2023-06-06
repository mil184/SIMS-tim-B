using InitialProject.ViewModel.Guide;
using MenuNavigation.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for ShowComplexTour.xaml
    /// </summary>
    public partial class ShowComplexTour : Window
    {
        private readonly ShowComplexTourViewModel _viewModel;
        public ShowComplexTour(ShowComplexTourViewModel showComplexTourViewModel)
        {
            InitializeComponent();
            _viewModel = showComplexTourViewModel;
            DataContext = _viewModel;

            _viewModel.CancelCommand = new RelayCommand(obj =>
            {
                    this.Close();
            });
        }
        private void Request_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.UpdateTimeSlots();
        }

        private void DateMouse_Click(object sender, MouseButtonEventArgs e)
        {
            _viewModel.CreateTour();
        }
    }
}
