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
    }
}
