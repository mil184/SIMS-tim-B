using InitialProject.ViewModel.Guide;
using MenuNavigation.Commands;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.View.Guide
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        private readonly StatisticsViewModel _viewModel;
        public Statistics(StatisticsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _viewModel.CancelCommand = new RelayCommand(obj =>
            {
                    this.Close();
            });
        }
    }
}
