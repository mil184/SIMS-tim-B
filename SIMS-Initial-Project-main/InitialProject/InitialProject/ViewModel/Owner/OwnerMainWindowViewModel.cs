using InitialProject.View.Owner;
using MenuNavigation.Commands;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace InitialProject.ViewModel.Owner
{
    public class OwnerMainWindowViewModel
    {
        public OwnerMainWindow MainWindow { get; set; }
        public NavigationService NavService { get; set; }
        public SoundPlayer Player { get; set; }
        public bool IsPlaying { get; set; }

        public RelayCommand NavigateToHomePageCommand { get; set; }
        public RelayCommand NavigateBackCommand { get; set; }
        public RelayCommand NavigateToAccommodationsPageCommand { get; set; }
        public RelayCommand NavigateToRenovationsPageCommand { get; set; }
        public RelayCommand NavigateToGuestReviewPageCommand { get; set; }
        public RelayCommand NavigateToRatingsPageCommand { get; set; }
        public RelayCommand PlayMusicCommand { get; set; }
        public RelayCommand LogOutCommand { get; set; }

        private void Execute_NavigateToHomePageCommand(object obj)
        {
            Page home = new HomeScreen(MainWindow, MainWindow.LoggedInUser);
            MainWindow.Main.NavigationService.Navigate(home);
            MainWindow.Title.Content = "DMJM Tours";
            MainWindow.BackButton.Visibility = Visibility.Hidden;
            MainWindow.DotsButton.Visibility = Visibility.Visible;
        }

        private void Execute_NavigateBackCommand(object obj)
        {
            MainWindow.Main.NavigationService?.GoBack();
            MainWindow.Title.Content = "DMJM Tours";
            MainWindow.DotsButton.Visibility = Visibility.Visible;
            MainWindow.BackButton.Visibility = Visibility.Hidden;
        }

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
            if (!IsPlaying)
            {
                Player.Play();
                IsPlaying = true;
            }
            else
            {
                Player.Stop();
                IsPlaying = false;
            }
        }

        private void Execute_LogOutCommand(object obj)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            MainWindow.Close();
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        public OwnerMainWindowViewModel(OwnerMainWindow ownerMainWindow,  NavigationService navService)
        {
            NavigateToHomePageCommand = new RelayCommand(Execute_NavigateToHomePageCommand, CanExecute_Command);
            NavigateBackCommand = new RelayCommand(Execute_NavigateBackCommand, CanExecute_Command);
            NavigateToAccommodationsPageCommand = new RelayCommand(Execute_NavigateToAccommodationsPageCommand, CanExecute_Command);
            NavigateToRenovationsPageCommand = new RelayCommand(Execute_NavigateToRenovationsPageCommand, CanExecute_Command);
            NavigateToGuestReviewPageCommand = new RelayCommand(Execute_NavigateToGuestReviewPageCommand, CanExecute_Command);
            NavigateToRatingsPageCommand = new RelayCommand(Execute_NavigateToRatingsPageCommand, CanExecute_Command);
            PlayMusicCommand = new RelayCommand(Execute_PlayMusicCommand, CanExecute_Command);
            LogOutCommand = new RelayCommand(Execute_LogOutCommand, CanExecute_Command);

            MainWindow = ownerMainWindow;
            NavService = navService;

            IsPlaying = false;
            Player = new SoundPlayer("../../../Resources/Sounds/music.wav");
            Player.Load();
        }
    }
}
