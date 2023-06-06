using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Service;
using MenuNavigation.Commands;
using System;
using System.Collections.ObjectModel;

namespace InitialProject.ViewModel.Owner
{
    public class RenovationsPageViewModel
    {
        public User LoggedInUser { get; set; }
        public ObservableCollection<AccommodationRenovationDTO> FinishedRenovations { get; set; }
        public ObservableCollection<AccommodationRenovationDTO> UpcomingRenovations { get; set; }
        public AccommodationRenovationDTO SelectedRenovation { get; set; }

        private readonly AccommodationRenovationService _accommodationRenovationService;
        private readonly AccommodationService _accommodationService;

        public RelayCommand CancelRenovationCommand { get; set; }

        private void Execute_CancelRenovationCommand(object obj)
        {
            _accommodationRenovationService.Remove(SelectedRenovation.RenovationId);
            FinishedRenovations.Clear();
            UpcomingRenovations.Clear();
            FormRenovations();
        }

        public bool CanExecute_Command(object obj)
        {
            return SelectedRenovation.StartDate.AddDays(-5) > DateTime.Now;
        }

        public RenovationsPageViewModel(User user)
        {
            LoggedInUser = user;
            _accommodationRenovationService = new AccommodationRenovationService();
            _accommodationService = new AccommodationService();

            CancelRenovationCommand = new RelayCommand(Execute_CancelRenovationCommand, CanExecute_Command);

            InitializeRenovations();
        }

        private void FormRenovations()
        {
            foreach (AccommodationRenovation renovation in _accommodationRenovationService.GetFinishedRenovations(LoggedInUser.Id))
            {
                FinishedRenovations.Add(ConvertToDTO(renovation));
            }

            foreach (AccommodationRenovation renovation in _accommodationRenovationService.GetUpcomingRenovations(LoggedInUser.Id))
            {
                UpcomingRenovations.Add(ConvertToDTO(renovation));
            }
        }

        private void InitializeRenovations()
        {
            FinishedRenovations = new ObservableCollection<AccommodationRenovationDTO>();
            UpcomingRenovations = new ObservableCollection<AccommodationRenovationDTO>();
            FormRenovations();
        }

        private AccommodationRenovationDTO ConvertToDTO(AccommodationRenovation renovation)
        {
            return new AccommodationRenovationDTO(renovation.Id, renovation.StartDate, renovation.EndDate, _accommodationService.GetById(renovation.AccommodationId).Name);
        }
    }
}
