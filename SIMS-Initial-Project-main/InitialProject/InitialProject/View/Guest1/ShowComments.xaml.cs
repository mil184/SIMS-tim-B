using InitialProject.Model.DTO;
using InitialProject.Model;
using InitialProject.Service;
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
using InitialProject.ViewModel.Guest1;

namespace InitialProject.View.Guest1
{
    /// <summary>
    /// Interaction logic for ShowComments.xaml
    /// </summary>
    public partial class ShowComments : Window
    {
        public ShowCommentsViewModel _viewModel { get; set; }

        public ShowComments(ForumDTO selectedForum, CommentService commentService, User user)
        {
            InitializeComponent();
            _viewModel = new ShowCommentsViewModel(selectedForum, commentService, user);
            DataContext = _viewModel;
        }
    }
}
