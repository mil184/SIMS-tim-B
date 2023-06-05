using InitialProject.Model.DTO;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace InitialProject.ViewModel.Guest1
{
    public class ShowCommentsViewModel
    {
        private readonly CommentService _commentService;
        private readonly UserService _userService;
        private readonly AccommodationReservationService _accommodationReservationService;
        public ObservableCollection<CommentDTO> AllComments { get; set; }
        public User LoggedInUser { get; set; }

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

        public ShowCommentsViewModel(ForumDTO selectedForum, CommentService commentService, User user)
        {
            SelectedForum = selectedForum;
            _commentService = commentService;
            LoggedInUser = user;
            _userService = new UserService();
            _accommodationReservationService = new AccommodationReservationService();
            AllComments = new ObservableCollection<CommentDTO>();

            FormComments();
        }

        public CommentDTO ConvertToDTO(Model.ForumComment comment)
        {
            bool wasPresent = CheckGuestPresence(comment.UserId);
            return new CommentDTO(comment.Id, comment.Comment, _userService.GetById(comment.UserId).Username, comment.ForumId, wasPresent);
        }

        public ObservableCollection<CommentDTO> ConvertToDTO(ObservableCollection<Model.ForumComment> comments)
        {
            ObservableCollection<CommentDTO> dto = new ObservableCollection<CommentDTO>();
            foreach (Model.ForumComment comment in comments)
            {
                dto.Add(ConvertToDTO(comment));
            }
            return dto;
        }

        public void FormComments()
        {
            AllComments.Clear();
            foreach (Model.ForumComment comment in _commentService.GetAll())
            {
                bool wasPresent = CheckGuestPresence(comment.UserId);
                if (comment.ForumId == SelectedForum.Id)
                {
                    CommentDTO dto = new CommentDTO(comment.Id, comment.Comment, _userService.GetById(comment.UserId).Username, comment.ForumId, wasPresent);
                    AllComments.Add(dto);
                }
            }
        }

        public bool CheckGuestPresence(int userId)
        {
            var reservations = _accommodationReservationService.GetReservationsByGuestId(userId);
            return reservations.Any(r => r.HasGuestPresence);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
