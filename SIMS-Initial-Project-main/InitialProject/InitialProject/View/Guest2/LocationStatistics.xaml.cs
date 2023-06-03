using InitialProject.ViewModel.Guest2;
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

namespace InitialProject.View.Guest2
{
    /// <summary>
    /// Interaction logic for LocationStatistics.xaml
    /// </summary>
    public partial class LocationStatistics : Window
    {
        private readonly LocationStatisticsViewModel _viewModel;
        public LocationStatistics(LocationStatisticsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            if (_viewModel.CloseAction == null)
            {
                _viewModel.CloseAction = new Action(this.Close);
            }
        }
    }
}
