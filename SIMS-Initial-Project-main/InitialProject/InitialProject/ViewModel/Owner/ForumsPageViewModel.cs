using InitialProject.Model;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using InitialProject.View.Owner;
using MenuNavigation.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace InitialProject.ViewModel.Owner
{
    public class ForumsPageViewModel : IObserver, INotifyPropertyChanged
    {
        public ObservableCollection<Forum> Forums { get; set; }
        public Forum SelectedForum { get; set; }
        public User LoggedInUser { get; set; }
        public NavigationService navigationService { get; set; }

        private readonly ForumService _forumService;

        public RelayCommand NavigateToForumPageCommand { get; set; }

        private void Execute_NavigateToForumPageCommand(object obj)
        {
            Page forum = new ForumPage(LoggedInUser, SelectedForum);
            navigationService.Navigate(forum);
        }

        public bool CanExecute_Command(object obj)
        {
            return true;
        }

        public ForumsPageViewModel(NavigationService navService, User user)
        {
            LoggedInUser = user;
            navigationService = navService;

            _forumService = new ForumService();

            NavigateToForumPageCommand = new RelayCommand(Execute_NavigateToForumPageCommand, CanExecute_Command);

            InitializeForums();
        }

        private void FormForums()
        {
            foreach (Forum forum in _forumService.GetAll())
            {
                Forums.Add(forum);
            }
        }

        private void InitializeForums()
        {
            Forums = new ObservableCollection<Forum>();
            FormForums();
        }

        public void Update()
        {
            Forums.Clear();
            FormForums();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
