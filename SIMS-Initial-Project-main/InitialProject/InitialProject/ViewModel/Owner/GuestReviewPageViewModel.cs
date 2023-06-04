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

        private bool _cleanliness1IsChecked;
        public bool Cleanliness1IsChecked
        {
            get => _cleanliness1IsChecked;
            set
            {
                if (value != _cleanliness1IsChecked)
                {
                    _cleanliness1IsChecked = value;
                    OnPropertyChanged();
                    UpdateCleanlinessImage();
                }
            }
        }

        private bool _cleanliness2IsChecked;
        public bool Cleanliness2IsChecked
        {
            get => _cleanliness2IsChecked;
            set
            {
                if (value != _cleanliness2IsChecked)
                {
                    _cleanliness2IsChecked = value;
                    OnPropertyChanged();
                    UpdateCleanlinessImage();
                }
            }
        }

        private bool _cleanliness3IsChecked;
        public bool Cleanliness3IsChecked
        {
            get => _cleanliness3IsChecked;
            set
            {
                if (value != _cleanliness3IsChecked)
                {
                    _cleanliness3IsChecked = value;
                    OnPropertyChanged();
                    UpdateCleanlinessImage();
                }
            }
        }

        private bool _cleanliness4IsChecked;
        public bool Cleanliness4IsChecked
        {
            get => _cleanliness4IsChecked;
            set
            {
                if (value != _cleanliness4IsChecked)
                {
                    _cleanliness4IsChecked = value;
                    OnPropertyChanged();
                    UpdateCleanlinessImage();
                }
            }
        }

        private bool _cleanliness5IsChecked;
        public bool Cleanliness5IsChecked
        {
            get => _cleanliness5IsChecked;
            set
            {
                if (value != _cleanliness5IsChecked)
                {
                    _cleanliness5IsChecked = value;
                    OnPropertyChanged();
                    UpdateCleanlinessImage();
                }
            }
        }

        private string _selectedCleanlinessImage;
        public string SelectedCleanlinessImage
        {
            get => _selectedCleanlinessImage;
            set
            {
                if (value != _selectedCleanlinessImage)
                {
                    _selectedCleanlinessImage = value;
                    OnPropertyChanged();
                }
            }
        }

        private void UpdateCleanlinessImage()
        {
            if (Cleanliness1IsChecked)
                SelectedCleanlinessImage = "/Resources/Images/OwnerIcons/mood1.png";
            else if (Cleanliness2IsChecked)
                SelectedCleanlinessImage = "/Resources/Images/OwnerIcons/mood2.png";
            else if (Cleanliness3IsChecked)
                SelectedCleanlinessImage = "/Resources/Images/OwnerIcons/mood3.png";
            else if (Cleanliness4IsChecked)
                SelectedCleanlinessImage = "/Resources/Images/OwnerIcons/mood4.png";
            else if (Cleanliness5IsChecked)
                SelectedCleanlinessImage = "/Resources/Images/OwnerIcons/mood5.png";
        }

        private bool _behaviour1IsChecked;
        public bool Behaviour1IsChecked
        {
            get => _behaviour1IsChecked;
            set
            {
                if (value != _behaviour1IsChecked)
                {
                    _behaviour1IsChecked = value;
                    OnPropertyChanged();
                    UpdateBehaviourImage();
                }
            }
        }

        private bool _behaviour2IsChecked;
        public bool Behaviour2IsChecked
        {
            get => _behaviour2IsChecked;
            set
            {
                if (value != _behaviour2IsChecked)
                {
                    _behaviour2IsChecked = value;
                    OnPropertyChanged();
                    UpdateBehaviourImage();
                }
            }
        }

        private bool _behaviour3IsChecked;
        public bool Behaviour3IsChecked
        {
            get => _behaviour3IsChecked;
            set
            {
                if (value != _behaviour3IsChecked)
                {
                    _behaviour3IsChecked = value;
                    OnPropertyChanged();
                    UpdateBehaviourImage();
                }
            }
        }

        private bool _behaviour4IsChecked;
        public bool Behaviour4IsChecked
        {
            get => _behaviour4IsChecked;
            set
            {
                if (value != _behaviour4IsChecked)
                {
                    _behaviour4IsChecked = value;
                    OnPropertyChanged();
                    UpdateBehaviourImage();
                }
            }
        }

        private bool _behaviour5IsChecked;
        public bool Behaviour5IsChecked
        {
            get => _behaviour5IsChecked;
            set
            {
                if (value != _behaviour5IsChecked)
                {
                    _behaviour5IsChecked = value;
                    OnPropertyChanged();
                    UpdateBehaviourImage();
                }
            }
        }

        private string _selectedBehaviourImage;
        public string SelectedBehaviourImage
        {
            get => _selectedBehaviourImage;
            set
            {
                if (value != _selectedBehaviourImage)
                {
                    _selectedBehaviourImage = value;
                    OnPropertyChanged();
                }
            }
        }

        private void UpdateBehaviourImage()
        {
            if (Behaviour1IsChecked)
                SelectedBehaviourImage = "/Resources/Images/OwnerIcons/mood1.png";
            else if (Behaviour2IsChecked)
                SelectedBehaviourImage = "/Resources/Images/OwnerIcons/mood2.png";
            else if (Behaviour3IsChecked)
                SelectedBehaviourImage = "/Resources/Images/OwnerIcons/mood3.png";
            else if (Behaviour4IsChecked)
                SelectedBehaviourImage = "/Resources/Images/OwnerIcons/mood4.png";
            else if (Behaviour5IsChecked)
                SelectedBehaviourImage = "/Resources/Images/OwnerIcons/mood5.png";
        }

        private void SetCleanliness()
        {
            if (Cleanliness1IsChecked == true)
            {
                Cleanness = 1;
            }
            else if (Cleanliness2IsChecked == true)
            {
                Cleanness = 2;
            }
            else if (Cleanliness3IsChecked == true)
            {
                Cleanness = 3;
            }
            else if (Cleanliness4IsChecked == true)
            {
                Cleanness = 4;
            }
            else if (Cleanliness5IsChecked == true)
            {
                Cleanness = 5;
            }
        }

        private void SetBehaviour()
        {
            if (Behaviour1IsChecked == true)
            {
                Behaviour = 1;
            }
            else if (Behaviour2IsChecked == true)
            {
                Behaviour = 2;
            }
            else if (Behaviour3IsChecked == true)
            {
                Behaviour = 3;
            }
            else if (Behaviour4IsChecked == true)
            {
                Behaviour = 4;
            }
            else if (Behaviour5IsChecked == true)
            {
                Behaviour = 5;
            }
        }

        private void ResetRadioButtons()
        {
            Cleanliness1IsChecked = false;
            Cleanliness2IsChecked = false;
            Cleanliness3IsChecked = false;
            Cleanliness4IsChecked = false;
            Cleanliness5IsChecked = false;
            Behaviour1IsChecked = false;
            Behaviour2IsChecked = false;
            Behaviour3IsChecked = false;
            Behaviour4IsChecked = false;
            Behaviour5IsChecked = false;
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
                    SetCleanliness();
                    SetBehaviour();
                    GuestReview guestReview = new GuestReview(Reservation.Id, Reservation.AccommodationId, Reservation.GuestId, Comment, Cleanness, Behaviour);
                    _guestReviewService.Save(guestReview);
                    _guestReviewService.NotifyObservers();
                    Comment = "";
                    ResetRadioButtons();
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

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        public GuestReviewPageViewModel(User user)
        {
            LoggedInUser = user;

            _reservations = new AccommodationReservationRepository();
            _reservations.Subscribe(this);
            _guestReviewService = new GuestReviewService();
            _guestReviewService.Subscribe(this);

            IsClicked = false;

            ReviewGuestCommand = new RelayCommand(Execute_ReviewGuestCommand, CanExecute_Command);
            SelectionChangedCommand = new RelayCommand(Execute_SelectionChangedCommand, CanExecute_Command);

            SelectedCleanlinessImage = "/Resources/Images/OwnerIcons/mood3.png";
            SelectedBehaviourImage = "/Resources/Images/OwnerIcons/mood3.png";

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
