using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.ViewModel.Owner
{
    public class ReviewGuestViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly GuestReviewRepository _repository;
        private readonly GuestReviewDTO SelectedUnreviewedGuest;
        private readonly AccommodationReservationRepository _reservations;
        private readonly AccommodationRatingsRepository _ratings;
        public AccommodationReservation Reservation { get; set; }

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

        public ReviewGuestViewModel(GuestReviewRepository repository, GuestReviewDTO selectedUnreviewedGuest, AccommodationReservationRepository reservations, AccommodationRatingsRepository ratings)
        {
            _repository = repository;
            SelectedUnreviewedGuest = selectedUnreviewedGuest;
            _reservations = reservations;
            _ratings = ratings;

            Reservation = _reservations.GetById(SelectedUnreviewedGuest.ReservationId);
        }

        public void Review()
        {
            if (IsValid)
            {
                GuestReview guestReview = new GuestReview(Reservation.Id, Reservation.AccommodationId, Reservation.GuestId, Comment, Cleanness, Behaviour);
                _repository.Save(guestReview);
                _ratings.NotifyObservers();
            }
            else
            {
                MessageBox.Show("Cannot Review Guest", "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Comment")
                {
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
    }
}
