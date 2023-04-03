using InitialProject.Model.DTO;
using InitialProject.Repository;
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
    /// Interaction logic for Evaluate.xaml
    /// </summary>
    public partial class Evaluate : Window
    {
        private readonly AccommodationRatingsRepository _accommodationRatingsRepository;
        private readonly AccommodationReservationRepository _accommodationReservationRepository;
        public Evaluate(AccommodationRatingsDTO selectedUnratedAccommodation, AccommodationRatingsRepository accommodationRatingsRepository, AccommodationReservationRepository accommodationReservationRepository)
        {
            InitializeComponent();
        }

        private void AddPicture_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
