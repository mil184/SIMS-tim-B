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
using InitialProject.ViewModel.Guest2;

namespace InitialProject.View.Guest2
{
    public partial class RequestTour : Window
    {
        private readonly RequestTourViewModel _viewModel;
        public RequestTour(RequestTourViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            if (_viewModel.CloseAction == null)
            {
                _viewModel.CloseAction = new Action(this.Close);
            }
        }

        //private void InitializeComboBoxes()
        //{
        //    for (int i = 0; i < 24; i++)
        //    {
        //        string hour = i.ToString("D2");
        //        StartHour_cb.Items.Add(hour);
        //    }
        //    for (int i = 0; i < 60; i++)
        //    {
        //        string minute = i.ToString("D2");
        //        StartMinute_cb.Items.Add(minute);
        //    }
        //    for (int i = 0; i < 24; i++)
        //    {
        //        string hour = i.ToString("D2");
        //        EndHour_cb.Items.Add(hour);
        //    }
        //    for (int i = 0; i < 60; i++)
        //    {
        //        string minute = i.ToString("D2");
        //        EndMinute_cb.Items.Add(minute);
        //    }
        //}

        //private void cbCountrySelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    _viewModel.InitializeCityDropdown();
        //}
    }
}
