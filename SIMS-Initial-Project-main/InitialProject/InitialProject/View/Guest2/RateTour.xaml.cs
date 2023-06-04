using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.ViewModel.Guest2;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Image = InitialProject.Model.Image;

namespace InitialProject.View.Guest2
{
    public partial class RateTour : Window
    {
        private readonly RateTourViewModel _viewModel;

        public RateTour(RateTourViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            if (_viewModel.CloseAction == null)
            {
                _viewModel.CloseAction = new Action(this.Close);
            }
        }

        //private void SetRatingForGuideKnowledge(object sender, RoutedEventArgs e)
        //{
        //    if (knowledge1.IsChecked == true)
        //        _viewModel.GuideKnowledge = 1;
        //    else if (knowledge2.IsChecked == true)
        //        _viewModel.GuideKnowledge = 2;
        //    else if (knowledge3.IsChecked == true)
        //        _viewModel.GuideKnowledge = 3;
        //    else if (knowledge4.IsChecked == true)
        //        _viewModel.GuideKnowledge = 4;
        //    else if (knowledge5.IsChecked == true)
        //        _viewModel.GuideKnowledge = 5;
        //}

        //private void SetRatingForInterestingness(object sender, RoutedEventArgs e)
        //{
        //    if (interestingness1.IsChecked == true)
        //        _viewModel.Interestingness = 1;
        //    else if (interestingness2.IsChecked == true)
        //        _viewModel.Interestingness = 2;
        //    else if (interestingness3.IsChecked == true)
        //        _viewModel.Interestingness = 3;
        //    else if (interestingness4.IsChecked == true)
        //        _viewModel.Interestingness = 4;
        //    else if (interestingness5.IsChecked == true)
        //        _viewModel.Interestingness = 5;
        //}

        //private void SetRatingForGuideLanguage(object sender, RoutedEventArgs e)
        //{
        //    if (language1.IsChecked == true)
        //        _viewModel.GuideLanguage = 1;
        //    else if (language2.IsChecked == true)
        //        _viewModel.GuideLanguage = 2;
        //    else if (language3.IsChecked == true)
        //        _viewModel.GuideLanguage = 3;
        //    else if (language4.IsChecked == true)
        //        _viewModel.GuideLanguage = 4;
        //    else if (language5.IsChecked == true)
        //        _viewModel.GuideLanguage = 5;
        //}

        //private void Submit_Click(object sender, RoutedEventArgs e)
        //{
        //    _viewModel.Submit();
        //    Close();
        //}

        //private void Cancel_Click(object sender, RoutedEventArgs e)
        //{
        //    Close();
        //}

        //private void AddImage_Click(object sender, RoutedEventArgs e)
        //{
        //    _viewModel.AddImage(UrlTextBox.Text);
        //    UrlTextBox.Text = string.Empty;
        //}

        //public void LanguageButton_Click(object sender, RoutedEventArgs e)
        //{
        //    _viewModel.LanguageButton_Click(sender, e);
        //}
    }
}
