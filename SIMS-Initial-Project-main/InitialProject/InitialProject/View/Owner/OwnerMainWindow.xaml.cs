using InitialProject.Model;
using InitialProject.ViewModel.Owner;
using System.Windows;

namespace InitialProject.View.Owner
{
    public partial class OwnerMainWindow : Window
    {
        public OwnerMainWindowViewModel _viewModel { get; set; }
        public User LoggedInUser { get; set; }

        public OwnerMainWindow(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            Main.Content = new HomeScreen(this, LoggedInUser);
            _viewModel = new OwnerMainWindowViewModel(this,  Main.NavigationService);
            DataContext = _viewModel;
        }
    }
}
