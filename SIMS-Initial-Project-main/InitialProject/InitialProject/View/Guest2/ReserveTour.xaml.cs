using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Service;
using System.Collections.Generic;
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
        private readonly TourReservationService _tourReservationService;
        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly UserRepository _userRepository;
        private readonly VoucherService _voucherService;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ReserveTour(Guest2TourDTO selectedTour, User user, TourService tourService, TourReservationService tourReservationService, VoucherService voucherService, LocationService locationService, UserRepository userRepository)
        {
            //InitializeComponent();
            DataContext = this;

            SelectedTour = selectedTour;
            LoggedInUser = user;

            _tourReservationService = tourReservationService;
            _tourService = tourService;
            _voucherService = voucherService;
            _locationService = locationService;
            _userRepository = userRepository;

            List<Voucher> UserVouchers = _voucherService.GetUserVouchers(LoggedInUser);
            Vouchers = new ObservableCollection<Voucher>(_voucherService.GetActiveVouchers(UserVouchers));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsInputValid())
            {
                return;
            }

            Tour selectedTour = _tourService.GetById(SelectedTour.TourId);
            int personCount = int.Parse(PersonCount);
            int spacesLeft = selectedTour.MaxGuests - selectedTour.CurrentGuestCount;

            if (!IsVoucherValid())
            {
                return;
            }

            if (personCount > spacesLeft && selectedTour.CurrentGuestCount != selectedTour.MaxGuests)
            {
                ShowSpacesLeftMessage(spacesLeft);
                return;
            }

            if (selectedTour.CurrentGuestCount == selectedTour.MaxGuests)
            {
                //HandleZeroSpacesForReservation(selectedTour);
                return;
            }

            SaveOrUpdateReservation(selectedTour, personCount);
            UpdateSelectedTour(selectedTour, personCount);
            Close();
        }

        private bool IsInputValid()
        {
            return PersonCount != null && AverageAge != null;
        }

        private bool IsVoucherValid()
        {
            //return SelectedVoucher != null ^ NoVoucherBtn.IsChecked == true;
            return true;
        }

        private void ShowSpacesLeftMessage(int spacesLeft)
        {
            if (spacesLeft == 1)
                MessageBox.Show("You've tried adding too many guests. There is only 1 space left.");
            else
                MessageBox.Show("You've tried adding too many guests. There are only " + spacesLeft.ToString() + " spaces left.");
        }

        private void HandleZeroSpacesForReservation(Tour selectedTour)
        {
            //var zeroSpacesForReservation = new ZeroSpacesForReservation(SelectedTour, LoggedInUser, _tourService, _locationService, _userRepository, _voucherService);
            //zeroSpacesForReservation.ShowDialog();
            //Close();
        }

        private void SaveOrUpdateReservation(Tour selectedTour, int personCount)
        {
            int voucherId = -1;
            if (SelectedVoucher != null)
            {
                voucherId = SelectedVoucher.Id;
            }

            TourReservation tourReservation = new TourReservation(
                LoggedInUser.Id,
                SelectedTour.TourId,
                personCount,
                double.Parse(AverageAge),
                voucherId);

            if (CheckIfReservationAlreadyExists(tourReservation))
            {
                UpdateExistingReservation(tourReservation);
            }
            else
            {
                SaveNewReservation(tourReservation);
            }
        }

        private void UpdateExistingReservation(TourReservation tourReservation)
        {
            TourReservation existingReservation = _tourReservationService.GetReservationByGuestIdAndTourId(LoggedInUser.Id, SelectedTour.TourId);
            tourReservation.Id = existingReservation.Id;
            tourReservation.PersonCount = existingReservation.PersonCount + int.Parse(PersonCount);
            tourReservation.AverageAge = (existingReservation.AverageAge + double.Parse(AverageAge)) / 2;

            if (existingReservation.UsedVoucherId == -1 && SelectedVoucher != null)
            {
                tourReservation.UsedVoucherId = SelectedVoucher.Id;
            }
            else
            {
                tourReservation.UsedVoucherId = existingReservation.UsedVoucherId;
            }

            _tourReservationService.Update(tourReservation);
        }

        private void SaveNewReservation(TourReservation tourReservation)
        {
            _tourReservationService.Save(tourReservation);

            if (tourReservation.UsedVoucherId != -1)
            {
                Voucher voucher = _voucherService.GetById(tourReservation.UsedVoucherId);
                voucher.IsActive = false;
                _voucherService.Update(voucher);
            }
        }

        private void UpdateSelectedTour(Tour selectedTour, int personCount)
        {
            selectedTour.CurrentGuestCount += personCount;
            _tourService.Update(selectedTour);
        }

        public bool CheckIfReservationAlreadyExists(TourReservation tourReservation)
        {
            foreach (TourReservation reservation in _tourReservationService.GetAll())
            {
                if (reservation.TourId == tourReservation.TourId && reservation.UserId == tourReservation.UserId)
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