using InitialProject.View.Guide;
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
using InitialProject.Model;

namespace InitialProject.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window
    {
        public User LoggedInUser { get; set; }
        public OwnerWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterAccommodation registerAccommodation = new RegisterAccommodation(LoggedInUser);
            registerAccommodation.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
