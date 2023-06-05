using InitialProject.Model;
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
    /// Interaction logic for CreateForum.xaml
    /// </summary>
    public partial class CreateForum : Window
    {
        private readonly CreateForumViewModel _viewModel;
        public CreateForum(ForumService forumService, LocationService locationService, User user)
        {
            InitializeComponent();
            _viewModel = new CreateForumViewModel(forumService, locationService, user);
            DataContext = _viewModel;
        }

        private void SelectedCountryChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.UpdateCityComboBox();
        }
    }
}
