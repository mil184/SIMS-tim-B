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
    public partial class ComplexTourRequest : Window
    {
        private readonly ComplexTourRequestViewModel _viewModel;

        public ComplexTourRequest(ComplexTourRequestViewModel viewModel)
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
