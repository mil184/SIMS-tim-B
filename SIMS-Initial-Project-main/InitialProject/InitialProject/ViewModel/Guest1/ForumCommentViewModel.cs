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
                _commentService.Save(comment);
                MessageBox.Show("Comment added successfully.");
                var window = Application.Current.Windows.OfType<View.Guest1.ForumComment>().FirstOrDefault();
                window.Close();
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
