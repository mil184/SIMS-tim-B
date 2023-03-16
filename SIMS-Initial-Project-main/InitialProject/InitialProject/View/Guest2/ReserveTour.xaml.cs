using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ReserveTour.xaml
    /// </summary>
    public partial class ReserveTour : Window
    {
        public User LoggedInUser { get; set; }
        public Guest2TourDTO SelectedTour { get; set; }

        private string _personCount;

        private readonly TourReservationRepository _tourReservationRepository;
        private readonly TourRepository _tourRepository;

        

        public string PersonCount
        {
            get => _personCount;
            set
            {
                if (value != _personCount)
                {
                    _personCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ReserveTour(Guest2TourDTO selectedTour, User user, TourRepository tourRepository)
        {
            InitializeComponent();
            DataContext = this;

            SelectedTour = selectedTour;
            LoggedInUser = user;

            _tourReservationRepository = new TourReservationRepository();
            _tourRepository = tourRepository;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Tour selectedTour = new Tour();
            selectedTour = _tourRepository.GetById(SelectedTour.TourId);

            if (int.Parse(PersonCount)>selectedTour.MaxGuests)
            {
                NotEnoughSpacesForReservation notEnoughSpacesForReservation =
                    new NotEnoughSpacesForReservation(SelectedTour, LoggedInUser);
                notEnoughSpacesForReservation.Show();
            }
            else
            {
                TourReservation tourReservation = new TourReservation(
                                                    LoggedInUser.Id,
                                                    SelectedTour.TourId,
                                                    int.Parse(PersonCount));

                _tourReservationRepository.Save(tourReservation);

                //Tour selectedTour = new Tour();
                //selectedTour = _tourRepository.GetById(SelectedTour.TourId);
                selectedTour.CurrentGuestCount += int.Parse(PersonCount);
                _tourRepository.Update(selectedTour);

                Close();
            }
            
        }
    }
}
