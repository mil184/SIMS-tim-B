using InitialProject.Model;
using InitialProject.ViewModel.Owner;
using System.Windows.Controls;

namespace InitialProject.View.Owner
{
    public partial class RenovationsPage : Page
    {
        public User LoggedInUser { get; set; }
        public RenovationsPageViewModel _viewModel;
        public RenovationsPage(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            _viewModel = new RenovationsPageViewModel(LoggedInUser);
            DataContext = _viewModel;
        }
    }
}
