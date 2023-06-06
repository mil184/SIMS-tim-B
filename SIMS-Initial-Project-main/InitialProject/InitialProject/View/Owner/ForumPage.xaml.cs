using InitialProject.Model;
using InitialProject.ViewModel.Owner;
using System.Windows.Controls;

namespace InitialProject.View.Owner
{
    public partial class ForumPage : Page
    {
        public User LoggedInUser { get; set; }
        public Forum SelectedForum { get; set; }
        public ForumPageViewModel _viewModel { get; set; }

        public ForumPage(User user, Forum forum)
        {
            InitializeComponent();
            LoggedInUser = user;
            SelectedForum = forum;
            _viewModel = new ForumPageViewModel(user, forum);
            DataContext = _viewModel;
        }
    }
}
