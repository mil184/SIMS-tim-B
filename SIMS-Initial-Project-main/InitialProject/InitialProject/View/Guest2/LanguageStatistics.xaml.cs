using InitialProject.ViewModel.Guest2;
using InitialProject.ViewModel.Guide;
using System;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.View.Guest2
{
    public partial class LanguageStatistics : Window
    {
        private readonly LanguageStatisticsViewModel _viewModel;

        public LanguageStatistics(LanguageStatisticsViewModel viewModel)
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
