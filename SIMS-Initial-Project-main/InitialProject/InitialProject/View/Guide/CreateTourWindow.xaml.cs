using InitialProject.ViewModel.Guide;
using MenuNavigation.Commands;
using Org.BouncyCastle.Asn1.BC;
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
    public partial class CreateTourWindow : Window
    {
        private CreateTourViewModel _viewModel;
        public CreateTourWindow(CreateTourViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;

            _viewModel.CancelCommand = new RelayCommand(obj =>
            {
                if (!_viewModel.IsDemo)
                    this.Close();
            });

        }

    }
}
