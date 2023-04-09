using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.View.Guest2
{
    public partial class ReserveTour : Window, INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }
        public Guest2TourDTO SelectedTour { get; set; }

        public Voucher SelectedVoucher { get; set; }
        public ObservableCollection<Voucher> Vouchers { get; set; }

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
        private string _averageAge;
        public string AverageAge
        {
            get => _averageAge;
            set
            {
                if (value != _averageAge)
                {
                    _averageAge = value;
                    OnPropertyChanged();
                }
            }
        }
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly TourService _tourService;

        private readonly VoucherRepository _voucherRepository;
        private readonly VoucherService _voucherService;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ReserveTour(Guest2TourDTO selectedTour, User user, TourService tourService)
        {
            InitializeComponent();
            DataContext = this;

            SelectedTour = selectedTour;
            LoggedInUser = user;

            _tourReservationRepository = new TourReservationRepository();
            _tourService = tourService;

            _voucherRepository = new VoucherRepository();
            _voucherService = new VoucherService();

            Vouchers = new ObservableCollection<Voucher>(_voucherService.GetUserVouchers(LoggedInUser));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Tour selectedTour = new Tour();
            selectedTour = _tourService.GetById(SelectedTour.TourId);

            if (PersonCount != null)
            {
                int personCount = int.Parse(PersonCount);
                int spacesLeft = selectedTour.MaxGuests - selectedTour.CurrentGuestCount;

                if (SelectedVoucher != null ^ NoVoucherBtn.IsChecked == true)
                {
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
                            = new ZeroSpacesForReservation(SelectedTour, LoggedInUser, _tourService);
                        zeroSpacesForReservation.ShowDialog();
                        Close();
                    }
                    else
                    {
                        TourReservation tourReservation = new TourReservation(
                                                            LoggedInUser.Id,
                                                            SelectedTour.TourId,
                                                            personCount,
                                                            double.Parse(AverageAge));
                        if (CheckIfReservationAlreadyExists(tourReservation))
                        {
                            tourReservation.Id = _tourReservationRepository.GetReservationByGuestIdAndTourId(LoggedInUser.Id, SelectedTour.TourId).Id;
                            int currentPersonCount = _tourReservationRepository.GetReservationByGuestIdAndTourId(LoggedInUser.Id, SelectedTour.TourId).PersonCount;
                            currentPersonCount += personCount;

                            double currentAverageAge = _tourReservationRepository.GetReservationByGuestIdAndTourId(LoggedInUser.Id, SelectedTour.TourId).AverageAge;
                            currentAverageAge = (currentAverageAge + double.Parse(AverageAge)) / 2;

                            int currentVoucherId = _tourReservationRepository.GetReservationByGuestIdAndTourId(LoggedInUser.Id, SelectedTour.TourId).UsedVoucherId;

                            if(currentVoucherId == -1) 
                            {
                                tourReservation.UsedVoucherId = SelectedVoucher.Id;
                            }
                            else 
                            {
                                tourReservation.UsedVoucherId = currentVoucherId;
                            }

                            tourReservation.PersonCount = currentPersonCount;
                            tourReservation.AverageAge = currentAverageAge;

                            _tourReservationRepository.Update(tourReservation);
                        }
                        else
                        {
                            _tourReservationRepository.Save(tourReservation);
                        }
                        selectedTour.CurrentGuestCount += personCount;
                        _tourService.Update(selectedTour);

                        Close();
                    }
                }
            }

            

            
        }
        public bool CheckIfReservationAlreadyExists(TourReservation tourReservation) 
        {
            foreach(TourReservation reservation in _tourReservationRepository.GetAll()) 
            {
                if(reservation.TourId  == tourReservation.TourId && reservation.UserId == tourReservation.UserId) 
                {
                    return true;
                }
            }
            return false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
