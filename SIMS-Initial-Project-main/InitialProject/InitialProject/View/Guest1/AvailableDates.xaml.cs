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
    /// Interaction logic for AvailableDates.xaml
    /// </summary>
    public partial class AvailableDates : Window
    {
        private readonly AvailableDatesViewModel _viewModel;
        public AvailableDates(AvailableAccommodationsDTO SelectedAvailableAccommodation,AccommodationService accommodationService, AccommodationReservationService accommodationReservationService, int numberOfDays, int numberOfGuests, User user)
        {
            InitializeComponent();
            _viewModel = new AvailableDatesViewModel(SelectedAvailableAccommodation, accommodationService, accommodationReservationService, numberOfDays, numberOfGuests, user);
            DataContext = _viewModel;
        }
    }
}
