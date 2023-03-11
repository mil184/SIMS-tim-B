using InitialProject.Model;
using System.Windows;

namespace InitialProject.View.Guide
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class GuideWindow : Window
    {
        public User LoggedInUser { get; set; }
        public GuideWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
        }
    }
}
