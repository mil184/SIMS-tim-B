using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using MenuNavigation.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace InitialProject.ViewModel.Owner
{
    public class ForumPageViewModel : INotifyPropertyChanged, IObserver
    {
        public User LoggedInUser { get; set; }
        public Forum SelectedForum { get; set; }

        public ObservableCollection<OwnerCommentDTO> Comments { get; set; }
        public OwnerCommentDTO SelectedComment { get; set; }

        private readonly CommentService _commentService;
        private readonly ForumService _forumService;
        private readonly LocationService _locationService;
        private readonly UserService _userService;
        private readonly AccommodationReservationService _accommodationReservationService;

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (value != _title)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _userInfo;
        public string UserInfo
        {
            get => _userInfo;
            set
            {
                if (value != _userInfo)
                {
                    _userInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                if (value != _text)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tag;
        public string Tag
        {
            get => _tag;
            set
            {
                if (value != _tag)
                {
                    _tag = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _location;
        public string Location
        {
            get => _location;
            set
            {
                if (value != _location)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _hasAccommodation;
        public bool HasAccommodation
        {
            get => _hasAccommodation;
            set
            {
                if (value != _hasAccommodation)
                {
                    _hasAccommodation = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand ReportCommentCommand { get; set; }
        public RelayCommand EnterCommand { get; set; }

        private void Execute_ReportCommentCommand(object obj)
        {
            ForumComment comment = _commentService.GetById(SelectedComment.Id);
            comment.ReportCount++;
            _commentService.Update(comment);
        }

        private void Execute_EnterCommand(object obj)
        {
            ForumComment comment = new ForumComment(Text, LoggedInUser.Id, SelectedForum.Id);
            _commentService.Save(comment);
            Comments.Clear();
            FormComments();
        }

        public bool CanExecute_Command(object obj)
        {
            return true;
        }

        public bool CanExecute_EnterCommand(object obj)
        {
            return !string.IsNullOrEmpty(Text);
        }

        public ForumPageViewModel(User user, Forum forum)
        {
            LoggedInUser = user;
            SelectedForum = forum;

            _commentService = new CommentService();
            _commentService.Subscribe(this);
            _forumService = new ForumService();
            _locationService = new LocationService();
            _userService = new UserService();
            _accommodationReservationService = new AccommodationReservationService();

            ReportCommentCommand = new RelayCommand(Execute_ReportCommentCommand, CanExecute_Command);
            EnterCommand = new RelayCommand(Execute_EnterCommand, CanExecute_EnterCommand);

            HasAccommodation = _userService.HasAccommodation(LoggedInUser.Id, SelectedForum.LocationId) && _forumService.GetById(SelectedForum.Id).IsOpened;

            InitializeData();
            InitializeComments();
        }

        private void InitializeData()
        {
            Title = SelectedForum.Comment;
            Location = _locationService.GetById(SelectedForum.LocationId).City + ", " + _locationService.GetById(SelectedForum.LocationId).Country;

            if (HasAccommodation)
            {
                Tag = "Share your opinion about this location";
            }
            else
            {
                Tag = "You don't have an accommodation here";
            }
        }

        private void FormComments()
        {
            foreach (ForumComment comment in _commentService.GetCommentsByForumId(SelectedForum.Id))
            {
                Comments.Add(ConvertToDTO(comment));
            }
        }

        private OwnerCommentDTO ConvertToDTO(ForumComment comment)
        {
            bool wasPresent = CheckGuestPresence(comment.UserId);
            string user = _userService.GetById(comment.UserId).Username + ", ";
            if (_userService.GetById(comment.UserId).Type.ToString() == "owner")
            {
                user += "Owner";
            }
            else
            {
                user += "Guest";
            }
            return new OwnerCommentDTO(comment.Id, comment.Comment, user, comment.ForumId, wasPresent, comment.ReportCount);
        }

        public bool CheckGuestPresence(int userId)
        {
            var reservations = _accommodationReservationService.GetReservationsByGuestId(userId);
            return reservations.Any(r => r.HasGuestPresence);
        }

        private void InitializeComments()
        {
            Comments = new ObservableCollection<OwnerCommentDTO>();
            FormComments();
        }

        void IObserver.Update()
        {
            Comments.Clear();
            FormComments();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
