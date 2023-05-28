using InitialProject.Model;
using InitialProject.View.Owner;
using MenuNavigation.Commands;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.ViewModel.Owner
{
    public class HomeScreenViewModel
    {
        public OwnerMainWindow MainWindow { get; set; }
        public User LoggedInUser { get; set; }

        public RelayCommand NavigateToAccommodationsPageCommand { get; set; }
        public RelayCommand NavigateToRenovationsPageCommand { get; set; }
        public RelayCommand NavigateToGuestReviewPageCommand { get; set; }
        public RelayCommand NavigateToRatingsPageCommand { get; set; }
        public RelayCommand PlayMusicCommand { get; set; }

        private void Execute_NavigateToAccommodationsPageCommand(object obj)
        {
            Page accommodations = new AccommodationsPage(MainWindow, MainWindow.LoggedInUser);
            MainWindow.Main.NavigationService.Navigate(accommodations);
            MainWindow.Title.Content = "Accommodations";
            MainWindow.BackButton.Visibility = Visibility.Visible;
            MainWindow.DotsButton.Visibility = Visibility.Hidden;
        }

        private void Execute_NavigateToRenovationsPageCommand(object obj)
        {
            Page renovations = new RenovationsPage(MainWindow.LoggedInUser);
            MainWindow.Main.NavigationService.Navigate(renovations);
            MainWindow.Title.Content = "Renovations";
            MainWindow.BackButton.Visibility = Visibility.Visible;
            MainWindow.DotsButton.Visibility = Visibility.Hidden;
        }

        private void Execute_NavigateToGuestReviewPageCommand(object obj)
        {
            Page reviews = new GuestReviewPage(MainWindow.LoggedInUser);
            MainWindow.Main.NavigationService.Navigate(reviews);
            MainWindow.Title.Content = "Review a Guest";
            MainWindow.BackButton.Visibility = Visibility.Visible;
            MainWindow.DotsButton.Visibility = Visibility.Hidden;
        }

        private void Execute_NavigateToRatingsPageCommand(object obj)
        {
            Page ratings = new RatingsPage(MainWindow.LoggedInUser);
            MainWindow.Main.NavigationService.Navigate(ratings);
            MainWindow.Title.Content = "Review of You";
            MainWindow.BackButton.Visibility = Visibility.Visible;
            MainWindow.DotsButton.Visibility = Visibility.Hidden;
        }

        private void Execute_PlayMusicCommand(object obj)
        {
            if (!MainWindow._viewModel.IsPlaying)
            {
                MainWindow._viewModel.Player.Play();
                MainWindow._viewModel.IsPlaying = true;
            }
            else
            {
                MainWindow._viewModel.Player.Stop();
                MainWindow._viewModel.IsPlaying = false;
            }
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        public HomeScreenViewModel(OwnerMainWindow window)
        {
            NavigateToAccommodationsPageCommand = new RelayCommand(Execute_NavigateToAccommodationsPageCommand, CanExecute_Command);
            NavigateToRenovationsPageCommand = new RelayCommand(Execute_NavigateToRenovationsPageCommand, CanExecute_Command);
            NavigateToGuestReviewPageCommand = new RelayCommand(Execute_NavigateToGuestReviewPageCommand, CanExecute_Command);
            NavigateToRatingsPageCommand = new RelayCommand(Execute_NavigateToRatingsPageCommand, CanExecute_Command);
            PlayMusicCommand = new RelayCommand(Execute_PlayMusicCommand, CanExecute_Command);

            MainWindow = window;
        }
    }
}
