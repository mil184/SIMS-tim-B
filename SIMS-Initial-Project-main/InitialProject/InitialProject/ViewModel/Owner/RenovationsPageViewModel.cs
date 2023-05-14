using InitialProject.Model;
using InitialProject.Resources.Enums;
using InitialProject.Service;
using InitialProject.View.Owner;
using MenuNavigation.Commands;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Windows;

namespace InitialProject.ViewModel.Owner
{
    public class RenovationsPageViewModel
    {
        public User LoggedInUser { get; set; }
        public ObservableCollection<AccommodationRenovation> FinishedRenovations { get; set; }
        public ObservableCollection<AccommodationRenovation> UpcomingRenovations { get; set; }
        public AccommodationRenovation SelectedRenovation { get; set; }

        public readonly AccommodationRenovationService _accommodationRenovationService;

        public RelayCommand CancelRenovationCommand { get; set; }

        private void Execute_CancelRenovationCommand(object obj)
        {
            _accommodationRenovationService.Remove(SelectedRenovation.Id);
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

            CancelRenovationCommand = new RelayCommand(Execute_CancelRenovationCommand, CanExecute_Command);

            InitializeRenovations();
        }

        private void FormRenovations()
        {
            foreach (AccommodationRenovation renovation in _accommodationRenovationService.GetFinishedRenovations(LoggedInUser.Id))
            {
                FinishedRenovations.Add(renovation);
            }

            foreach (AccommodationRenovation renovation in _accommodationRenovationService.GetUpcomingRenovations(LoggedInUser.Id))
            {
                UpcomingRenovations.Add(renovation);
            }
        }

        private void InitializeRenovations()
        {
            FinishedRenovations = new ObservableCollection<AccommodationRenovation>();
            UpcomingRenovations = new ObservableCollection<AccommodationRenovation>();
            FormRenovations();
        }
    }
}
