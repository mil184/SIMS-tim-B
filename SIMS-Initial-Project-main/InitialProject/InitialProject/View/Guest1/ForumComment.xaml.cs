using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Service;
using InitialProject.ViewModel.Guest1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.View.Guest1
{
    /// <summary>
    /// Interaction logic for ForumComment.xaml
    /// </summary>
    public partial class ForumComment : Window
    {
        public ForumCommentViewModel _viewModel { get; set; }
        public ForumComment(ForumDTO selectedForum, CommentService commentService, User user)
        {
            InitializeComponent();
            _viewModel = new ForumCommentViewModel(selectedForum, commentService, user);
            DataContext = _viewModel;
        }
    }
}
