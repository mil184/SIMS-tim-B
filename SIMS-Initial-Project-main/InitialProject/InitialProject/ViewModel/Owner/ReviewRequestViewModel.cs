using InitialProject.Model;
using InitialProject.Repository;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InitialProject.ViewModel.Owner
{
    public class ReviewRequestViewModel : INotifyPropertyChanged
    {
        private readonly AccommodationReservationRepository _reservationRepository;
        private readonly RescheduleRequestRepository _requestRepository;
        private readonly RescheduleRequest SelectedRequest;

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

        public ReviewRequestViewModel(AccommodationReservationRepository accommodationReservationRepository, RescheduleRequest selectedRescheduleRequest, RescheduleRequestRepository rescheduleRequestRepository)
        {
            _reservationRepository = accommodationReservationRepository;
            _requestRepository = rescheduleRequestRepository;
            SelectedRequest = selectedRescheduleRequest;

            CheckAvailability();
        }

        public string CheckAvailability()
        {

            foreach (AccommodationReservation reservation in _reservationRepository.GetAll())
            {
                if ((SelectedRequest.NewStartDate > reservation.StartDate && SelectedRequest.NewStartDate < reservation.EndDate) || (SelectedRequest.NewEndDate > reservation.StartDate && SelectedRequest.NewEndDate < reservation.EndDate))
                {
                    return "Period isn't available";
                }
            }
            return "Period is available";
        }

        public void Decline()
        {
            SelectedRequest.Status = InitialProject.Resources.Enums.RescheduleRequestStatus.rejected;
            SelectedRequest.IsNotified = false;
            if (Comment != null)
            {
                SelectedRequest.Comment = Comment;
            }
            _requestRepository.Update(SelectedRequest);
        }

        public void Accept()
        {
            SelectedRequest.Status = InitialProject.Resources.Enums.RescheduleRequestStatus.approved;
            SelectedRequest.IsNotified = false;
            if (Comment != null)
            {
                SelectedRequest.Comment = Comment;
            }
            _requestRepository.Update(SelectedRequest);
            UpdateAcceptedDates();
        }

        private void UpdateAcceptedDates()
        {
            AccommodationReservation reservation = _reservationRepository.GetById(SelectedRequest.ReservationId);
            reservation.StartDate = SelectedRequest.NewStartDate;
            reservation.EndDate = SelectedRequest.NewEndDate;
            _reservationRepository.Update(reservation);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
