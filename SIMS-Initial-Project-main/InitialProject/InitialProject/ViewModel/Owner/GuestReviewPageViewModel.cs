using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using InitialProject.View.Owner;
using MenuNavigation.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.ViewModel.Owner
{
    public class GuestReviewPageViewModel : INotifyPropertyChanged, IDataErrorInfo, IObserver
    {
        public User LoggedInUser { get; set; }
        public GuestReviewPage CurrentPage { get; set; }
        public ObservableCollection<GuestReviewDTO> UnreviewedGuestsDTO { get; set; }
        public GuestReviewDTO SelectedUnreviewedGuest { get; set; }
        public AccommodationReservation Reservation { get; set; }

        private readonly AccommodationReservationRepository _reservations;
        private readonly GuestReviewService _guestReviewService;

        private bool IsClicked { get; set; }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _cleanness;
        public int Cleanness
        {
            get => _cleanness;
            set
            {
                if (value != _cleanness)
                {
                    _cleanness = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _behaviour;
        public int Behaviour
        {
            get => _behaviour;
            set
            {
                if (value != _behaviour)
                {
                    _behaviour = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand ReviewGuestCommand { get; set; }
        public RelayCommand SelectionChangedCommand { get; set; }
        public RelayCommand CleanlinessSliderChangedCommand { get; set; }
        public RelayCommand BehaviourSliderChangedCommand { get; set; }

        private void Execute_ReviewGuestCommand(object obj)
        {
            if (SelectedUnreviewedGuest != null)
            {
                if (IsValid)
                {
                    IsClicked = true;
                    GuestReview guestReview = new GuestReview(Reservation.Id, Reservation.AccommodationId, Reservation.GuestId, Comment, Cleanness, Behaviour);
                    _guestReviewService.Save(guestReview);
                    _guestReviewService.NotifyObservers();
                }
                else
                {
                    MessageBox.Show("Cannot Review Guest", "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Select a guest to review.", "Slow down!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Execute_SelectionChangedCommand(object obj)
        {
            if (!IsClicked)
                Reservation = _reservations.GetById(SelectedUnreviewedGuest.ReservationId);
        }

        private void Execute_CleanlinessSliderChangedCommand(object obj)
        {
            Cleanness = (int)CurrentPage.CleannessSlider.Value + 1;
        }

        private void Execute_BehaviourSliderChangedCommand(object obj)
        {
            Behaviour = (int)CurrentPage.BehaviourSlider.Value + 1;
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        public GuestReviewPageViewModel(GuestReviewPage page, User user)
        {
            CurrentPage = page;
            LoggedInUser = user;

            _reservations = new AccommodationReservationRepository();
            _reservations.Subscribe(this);
            _guestReviewService = new GuestReviewService();
            _guestReviewService.Subscribe(this);

            IsClicked = false;

            ReviewGuestCommand = new RelayCommand(Execute_ReviewGuestCommand, CanExecute_Command);
            SelectionChangedCommand = new RelayCommand(Execute_SelectionChangedCommand, CanExecute_Command);
            CleanlinessSliderChangedCommand = new RelayCommand(Execute_CleanlinessSliderChangedCommand, CanExecute_Command);
            BehaviourSliderChangedCommand = new RelayCommand(Execute_BehaviourSliderChangedCommand, CanExecute_Command);

            InitializeUnreviewedGuests();
        }

        private void InitializeUnreviewedGuests()
        {
            UnreviewedGuestsDTO = new ObservableCollection<GuestReviewDTO>();
            FormUnreviewedGuests();
        }

        public void FormUnreviewedGuests()
        {
            foreach (GuestReviewDTO guest in _guestReviewService.GetUnreviewedGuests(LoggedInUser.Id))
            {
                UnreviewedGuestsDTO.Add(guest);
            }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Comment")
                {
                    if (SelectedUnreviewedGuest != null)
                        if (string.IsNullOrEmpty(Comment))
                            return "This field is required";
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Comment" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void IObserver.Update()
        {
            UnreviewedGuestsDTO.Clear();
            FormUnreviewedGuests();
        }
    }
}
