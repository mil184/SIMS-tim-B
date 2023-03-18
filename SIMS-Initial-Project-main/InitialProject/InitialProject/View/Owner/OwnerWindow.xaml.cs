using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window, INotifyPropertyChanged, IObserver
    {
        public User LoggedInUser { get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public ObservableCollection<GuestReview> GuestReviews { get; set; }
        public GuestReview SelectedGuestReview { get; set; }
        public ObservableCollection<GuestReviewDTO> UnreviewedGuests { get; set; }
        public GuestReviewDTO SelectedUnreviewedGuest { get; set; }

        private AccommodationRepository _repository;
        private readonly LocationRepository _locationRepository;
        private readonly ImageRepository _imageRepository;
        private readonly AccommodationReservationRepository _reservationRepository;
        private readonly GuestReviewRepository _guestReviewRepository;
        private readonly UserRepository _userRepository;

        public OwnerWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;

            _repository = new AccommodationRepository();
            _repository.Subscribe(this);
            _locationRepository = new LocationRepository();
            _locationRepository.Subscribe(this);
            _imageRepository = new ImageRepository();
            _imageRepository.Subscribe(this);
            _reservationRepository = new AccommodationReservationRepository();
            _reservationRepository.Subscribe(this);
            _guestReviewRepository = new GuestReviewRepository();
            _guestReviewRepository.Subscribe(this);
            _userRepository = new UserRepository();
            _userRepository.Subscribe(this);

            FormAccommodations();

            CheckForEligibleReviews();
        }

        public void FormAccommodations()
        {
            Accommodations = new ObservableCollection<Accommodation>();
            foreach (Accommodation accommodation in _repository.GetAll())
            {
                if (accommodation.OwnerId == LoggedInUser.Id)
                {
                    Accommodations.Add(accommodation);
                }
            }
        }

        public ObservableCollection<AccommodationReservation> UnreviewedReservations { get; set; }
        public void CheckForEligibleReviews()
        {
            UnreviewedGuests = new ObservableCollection<GuestReviewDTO>();
            UnreviewedReservations = new ObservableCollection<AccommodationReservation>();
            bool UnreviewedGuestExists = false;

            foreach (AccommodationReservation reservation in _reservationRepository.GetAll())
            {
                if (LessThanFiveDaysPassed(reservation) && !GuestReviewed(reservation))
                {
                    if (Accommodations.Any(item => item.Id == reservation.AccommodationId))
                    {
                        UnreviewedGuests.Add(new GuestReviewDTO(reservation.Id, _userRepository.GetById(reservation.GuestId).Username, _repository.GetById(reservation.AccommodationId).Name));
                        UnreviewedReservations.Add(reservation);
                        UnreviewedGuestExists = true;
                    }
                }
            }

            if (UnreviewedGuestExists)
            {
                MessageBox.Show("You have unreviewed guests!", "Guest Review");
            }
        }

        public bool GuestReviewed(AccommodationReservation reservation)
        {
            return _guestReviewRepository.GetAll().Any(item => item.ReservationId == reservation.Id);
        }

        public bool LessThanFiveDaysPassed(AccommodationReservation reservation)
        {
            TimeSpan timeSpan = DateTime.Now.Subtract(reservation.EndDate);
            return timeSpan.Days >= 0 && timeSpan.Days < 5;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterAccommodation registerAccommodation = new RegisterAccommodation(LoggedInUser, _repository, _locationRepository, _imageRepository);
            registerAccommodation.Show();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                string MessageBoxText = "Are You Sure You Want To Delete Accommodation?";
                string Caption = "Delete Accommodation";
                MessageBoxButton Button = MessageBoxButton.YesNo;
                MessageBoxImage Icon = MessageBoxImage.Warning;
                MessageBoxResult Result;

                Result = MessageBox.Show(MessageBoxText, Caption, Button, Icon, MessageBoxResult.Yes);
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }
        
        private void ReviewGuestButton_Guest(object sender, RoutedEventArgs e)
        {
            if(SelectedUnreviewedGuest != null)
            {
                ReviewGuest reviewGuest = new ReviewGuest(_guestReviewRepository, SelectedUnreviewedGuest, _reservationRepository);
                reviewGuest.Show();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void IObserver.Update()
        {
            Accommodations.Clear();
            foreach (Accommodation accommodation in _repository.GetAll())
            {
                if (accommodation.OwnerId == LoggedInUser.Id)
                {
                    Accommodations.Add(accommodation);
                }
            }

            UnreviewedGuests.Clear();
            foreach (AccommodationReservation reservation in _reservationRepository.GetAll())
            {
                if (LessThanFiveDaysPassed(reservation) && !GuestReviewed(reservation))
                {
                    if (Accommodations.Any(item => item.Id == reservation.AccommodationId))
                    {
                        UnreviewedGuests.Add(new GuestReviewDTO(reservation.Id, _userRepository.GetById(reservation.GuestId).Username, _repository.GetById(reservation.AccommodationId).Name));
                        UnreviewedReservations.Add(reservation);
                    }
                }
            }
        }
    }
}
