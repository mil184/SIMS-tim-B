using InitialProject.ViewModel.Guide;
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

            InitializeShortcuts();
        }
        private void InitializeShortcuts()
        {
            PreviewKeyDown += Escape_PreviewKeyDown;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
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
    }
}
