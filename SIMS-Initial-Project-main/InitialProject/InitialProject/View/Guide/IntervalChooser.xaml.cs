using InitialProject.Model.DTO;
using InitialProject.Resources.Observer;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using iTextSharp.text.pdf.parser.clipper;
using Path = System.IO.Path;
using InitialProject.Service;
using InitialProject.Converters;
using InitialProject.Model;
using InitialProject.ViewModel.Guide;
using MenuNavigation.Commands;

namespace InitialProject.View.Guide
{
    /// <summary>
    /// Interaction logic for IntervalChooser.xaml
    /// </summary>
    public partial class IntervalChooser : Window
    {
        private readonly IntervalChooserViewModel _viewModel;

        public IntervalChooser(IntervalChooserViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _viewModel.CancelCommand = new RelayCommand(obj =>
            {
                    this.Close();
            });

        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.DateSelectionChanged();
        }

    }
}
