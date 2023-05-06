using InitialProject.Model;
using System.Windows;

namespace InitialProject.View.Guest2
{
    public partial class Guest2SplashScreen : Window
    {
        public User LoggedInUser { get; set; }

        public Guest2SplashScreen(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
        }

        private void TourReservationButtonClick(object sender, RoutedEventArgs e)
        {
            Guest2Window guest2Window = new Guest2Window(LoggedInUser);
            guest2Window.Show();
            Close();
        }

        private void LogOutButtonClick(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }

        private void TrackingButton_Click(object sender, RoutedEventArgs e)
        {
            Guest2Window guest2Window = new Guest2Window(LoggedInUser);
            guest2Window.Show();
            guest2Window.tab.SelectedIndex = 1;
            Close();
        }

        private void RatingButton_Click(object sender, RoutedEventArgs e)
        {
            Guest2Window guest2Window = new Guest2Window(LoggedInUser);
            guest2Window.Show();
            guest2Window.tab.SelectedIndex = 2;
            Close();
        }

        private void RequestingButton_Click(object sender, RoutedEventArgs e)
        {
            Guest2Window guest2Window = new Guest2Window(LoggedInUser);
            guest2Window.Show();
            guest2Window.tab.SelectedIndex = 3;
            Close();
        }

        private void ComplexRequestingButton_Click(object sender, RoutedEventArgs e)
        {
            Guest2Window guest2Window = new Guest2Window(LoggedInUser);
            guest2Window.Show();
            guest2Window.tab.SelectedIndex = 4;
            Close();
        }

        private void VouchersButton_Click(object sender, RoutedEventArgs e)
        {
            Guest2Window guest2Window = new Guest2Window(LoggedInUser);
            guest2Window.Show();
            guest2Window.tab.SelectedIndex = 5;
            Close();
        }
    }
}
