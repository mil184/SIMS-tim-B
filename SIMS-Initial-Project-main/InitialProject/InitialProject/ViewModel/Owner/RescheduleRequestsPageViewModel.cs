using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using InitialProject.View.Owner;
using MenuNavigation.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace InitialProject.ViewModel.Owner
{
    public class RescheduleRequestsPageViewModel : IObserver
    {
        public User LoggedInUser { get; set; }
        public NavigationService navigationService { get; set; }
        public ObservableCollection<RescheduleRequestDTO> Requests { get; set; }
        public RescheduleRequestDTO SelectedRequest { get; set; }

        private AccommodationRepository _repository;
        private readonly AccommodationReservationRepository _reservationRepository;
        private readonly RescheduleRequestService _rescheduleRequestService;

        public RelayCommand DecideCommand { get; set; }

        private void Execute_DecideCommand(object obj)
        {
            Page decide = new RescheduleRequestDecidePage(navigationService, _rescheduleRequestService, SelectedRequest.RescheduleRequestId, SelectedRequest.IsAvailable);
            navigationService.Navigate(decide);
        }

        public bool CanExecute_Command(object obj)
        {
            return true;
        }

        public RescheduleRequestsPageViewModel(NavigationService navService, User user)
        {
            navigationService = navService;
            LoggedInUser = user;

            _repository = new AccommodationRepository();
            _repository.Subscribe(this);
            _reservationRepository = new AccommodationReservationRepository();
            _reservationRepository.Subscribe(this);
            _rescheduleRequestService = new RescheduleRequestService();
            _rescheduleRequestService.Subscribe(this);

            DecideCommand = new RelayCommand(Execute_DecideCommand, CanExecute_Command);

            InitializeRescheduleRequests();
        }

        private void FormRescheduleRequests()
        {
            foreach (RescheduleRequest request in _rescheduleRequestService.GetAll())
            {
                if (request.OwnerId == LoggedInUser.Id && request.Status == Resources.Enums.RescheduleRequestStatus.onhold && request.NewStartDate > DateTime.Now)
                {
                    RescheduleRequestDTO dto = new RescheduleRequestDTO(request.Id, request.AccommodationId, _repository.GetById(request.AccommodationId).Name, request.NewStartDate, request.NewEndDate);
                    SetAvailability(dto);
                    Requests.Add(dto);
                }
            }
        }

        private void SetAvailability(RescheduleRequestDTO dto)
        {
            foreach (AccommodationReservation reservation in _reservationRepository.GetAll())
            {
                if (reservation.AccommodationId == dto.AccommodationId && ((dto.NewStartDate > reservation.StartDate && dto.NewStartDate < reservation.EndDate) || (dto.NewEndDate > reservation.StartDate && dto.NewEndDate < reservation.EndDate)))
                {
                    dto.IsAvailable = false;
                    return;
                }
            }
            dto.IsAvailable = true;
        }

        private void InitializeRescheduleRequests()
        {
            Requests = new ObservableCollection<RescheduleRequestDTO>();
            FormRescheduleRequests();
        }

        void IObserver.Update()
        {
            Requests.Clear();
            FormRescheduleRequests();
        }
    }
}
