using InitialProject.Model;
using InitialProject.Model.DTO;
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

namespace InitialProject.View.Guest2
{
    /// <summary>
    /// Interaction logic for NotEnoughSpacesForReservation.xaml
    /// </summary>
    public partial class NotEnoughSpacesForReservation : Window
    {
        public User LoggedInUser { get; set; }
        public Guest2TourDTO SelectedTour { get; set; }

        public NotEnoughSpacesForReservation(Guest2TourDTO selectedTour, User user)
        {
            InitializeComponent();
            DataContext = this;

            SelectedTour = selectedTour;
            LoggedInUser = user;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
