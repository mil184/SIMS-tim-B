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
using System.Windows;

namespace InitialProject.View.Guest2
{
    /// <summary>
    /// Interaction logic for Guest2Window.xaml
    /// </summary>
    public partial class Guest2Window : Window
    {
        public User LoggedInUser { get; set; }
        public Guest2Window(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            //CreateTour createTour = new CreateTour();
            //createTour.Show();
            //Close();
        }
    }
}
