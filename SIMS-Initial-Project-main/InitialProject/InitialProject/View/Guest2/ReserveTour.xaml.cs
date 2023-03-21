using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.View.Guest2
{
    public partial class ReserveTour : Window, INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }
        public Guest2TourDTO SelectedTour { get; set; }

        private string _personCount;
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

        private readonly TourReservationRepository _tourReservationRepository;
        private readonly TourRepository _tourRepository;

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

            int personCount = int.Parse(PersonCount);
            int spacesLeft = selectedTour.MaxGuests - selectedTour.CurrentGuestCount;

            if (personCount > spacesLeft && selectedTour.CurrentGuestCount != selectedTour.MaxGuests)
            {
                if (spacesLeft == 1)
                    MessageBox.Show("You've tried adding too many guests. There is only 1 space left.");
                else
                    MessageBox.Show("You've tried adding too many guests. There are only " + spacesLeft.ToString() + " spaces left.");
            }
            else if (selectedTour.CurrentGuestCount == selectedTour.MaxGuests)
            {
                ZeroSpacesForReservation zeroSpacesForReservation
                    = new ZeroSpacesForReservation(SelectedTour, LoggedInUser, _tourRepository);
                zeroSpacesForReservation.ShowDialog();
                Close();
            }
            else
            {
                TourReservation tourReservation = new TourReservation(
                                                    LoggedInUser.Id,
                                                    SelectedTour.TourId,
                                                    personCount);

                _tourReservationRepository.Save(tourReservation);

                selectedTour.CurrentGuestCount += personCount;
                _tourRepository.Update(selectedTour);

                Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
