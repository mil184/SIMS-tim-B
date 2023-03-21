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
    public partial class ShowTour : Window
    {
        public Guest2TourDTO tourDTO { get; set; }

        public ShowTour(Guest2TourDTO tourDTO)
        {
            InitializeComponent();
            DataContext = this;

            this.tourDTO = tourDTO;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
