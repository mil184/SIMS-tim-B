using InitialProject.Model;
using System.Windows;

namespace InitialProject.View.Guest2
{
    public partial class Guest2SplashScreen : Window
    {
        public User LoggedInUser { get; set; }

        public int LanguageButtonClickCount { get; set; }
        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";

        public Guest2SplashScreen(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;

            app = (App)Application.Current;
            app.ChangeLanguage(SRB);
            LanguageButtonClickCount = 0;
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

        public void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            LanguageButtonClickCount++;

            if (LanguageButtonClickCount % 2 == 1)
            {
                app.ChangeLanguage(ENG);
                return;
            }

            app.ChangeLanguage(SRB);
        }
    }
}
