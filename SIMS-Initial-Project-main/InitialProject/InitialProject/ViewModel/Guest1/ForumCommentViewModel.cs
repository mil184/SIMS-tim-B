using InitialProject.Model.DTO;
using InitialProject.Model;
using InitialProject.Service;
using MenuNavigation.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;

namespace InitialProject.ViewModel.Guest1
{
    public class ForumCommentViewModel
    {
        private readonly CommentService _commentService;
        private readonly ForumService _forumService;
        private readonly AccommodationService _accommodationService;
        private readonly AccommodationReservationService _accommodationReservationService;
        public User LoggedInUser { get; set; }

        public RelayCommand AddCommentCommand { get; set; }
        public RelayCommand CancelCommentCommand { get; set; }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        private ForumDTO _selectedForum;
        public ForumDTO SelectedForum
        {
            get => _selectedForum;
            set
            {
                if (value != _selectedForum)
                {
                    _selectedForum = value;
                    OnPropertyChanged(nameof(SelectedForum));
                    OnPropertyChanged(nameof(CountryForum));
                    OnPropertyChanged(nameof(CityForum));
                }
            }
        }
        public string CountryForum
        {
            get => SelectedForum?.Country;
        }
        public string CityForum
        {
            get => SelectedForum?.City;
        }

        public ForumCommentViewModel(ForumDTO selectedForum, CommentService commentService, User user)
        {
            SelectedForum = selectedForum;
            _commentService = commentService;
            _forumService = new ForumService();
            _accommodationService = new AccommodationService();
            _accommodationReservationService = new AccommodationReservationService();
            LoggedInUser = user;

            AddCommentCommand = new RelayCommand(Execute_AddCommentCommand, CanExecute_Command);
            CancelCommentCommand = new RelayCommand(Execute_CancelCommentCommand, CanExecute_Command);
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        private void Execute_AddCommentCommand(object obj)
        {
            var messageBoxResult = MessageBox.Show($"Would you like to add comment?", "Adding Comment Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ForumComment comment = new ForumComment(Comment, LoggedInUser.Id, SelectedForum.Id);
                IncreaseComment();
                _commentService.Save(comment);
                MessageBox.Show("Comment added successfully.");
                CheckUsefulForum();
                var window = Application.Current.Windows.OfType<View.Guest1.ForumComment>().FirstOrDefault();
                window.Close();
            }
        }

        private void IncreaseComment()
        {
            int forumId = SelectedForum.Id;
            Forum forum = _forumService.GetForumById(forumId);

            if (LoggedInUser.Type == Resources.Enums.UserType.guest1 || LoggedInUser.Type == Resources.Enums.UserType.superguest)
            {
                forum.GuestCommentsCount++;
            }
            else if (LoggedInUser.Type == Resources.Enums.UserType.owner || LoggedInUser.Type == Resources.Enums.UserType.superowner)
            {
                forum.OwnerCommentsCount++;
            }
            _forumService.Update(forum);
        }

        private void CheckUsefulForum()
        {
            int forumId = SelectedForum.Id;
            Forum forum = _forumService.GetForumById(forumId);

            int guestCommentsCount = forum.GuestCommentsCount;
            int ownerCommentsCount = forum.OwnerCommentsCount;

            int userId = forum.UserId;

            bool hasGuestVisitedLocation = _accommodationReservationService.HasGuestVisitedLocation(userId);
           // bool hasOwnerAccommodationOnLocation = _accommodationService.HasOwnerAccommodationOnLocation(userId);

            if (guestCommentsCount >= 20 && ownerCommentsCount >= 10 && hasGuestVisitedLocation)
            {
                forum.IsVeryUseful = true;
                _forumService.Update(forum);
            }
        }

        private void Execute_CancelCommentCommand(object obj)
        {
            var window = Application.Current.Windows.OfType<View.Guest1.ForumComment>().FirstOrDefault();
            window.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
