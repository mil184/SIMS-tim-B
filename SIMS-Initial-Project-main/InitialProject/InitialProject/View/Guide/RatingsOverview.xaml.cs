using InitialProject.ViewModel.Guide;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.View.Guide
{
    /// <summary>
    /// Interaction logic for RatingsOverview.xaml
    /// </summary>
    public partial class RatingsOverview 
    {
        private readonly RatingsOverviewViewModel _viewModel;
        public RatingsOverview(RatingsOverviewViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeShortcuts();
        }
        private void InitializeShortcuts()
        {
            PreviewKeyDown += Escape_PreviewKeyDown;
            PreviewKeyDown += Left_PreviewKeyDown;
            PreviewKeyDown += Right_PreviewKeyDown;
        }
        private void Escape_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        private void Left_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                _viewModel.PreviousImage();
            }
        }
        private void Right_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                _viewModel.NextImage();
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.PreviousImage();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.NextImage();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
