﻿using InitialProject.Model;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using InitialProject.View.Owner;
using MenuNavigation.Commands;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace InitialProject.ViewModel.Owner
{
    public class AccommodationsPageViewModel : IObserver
    {
        public OwnerMainWindow MainWindow { get; set; }
        public User LoggedInUser { get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        private readonly AccommodationService _accommodationService;

        public RelayCommand AddAccommodationCommand { get; set; }

        private void Execute_NavigateAddAccommodationPageCommand(object obj)
        {
            Page addAccommodation = new AddAccommodationPage(MainWindow, _accommodationService);
            MainWindow.Main.NavigationService.Navigate(addAccommodation);
            MainWindow.Title.Content = "Register an Accommodation";
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        public AccommodationsPageViewModel(OwnerMainWindow window, User user)
        {
            MainWindow = window;
            LoggedInUser = user;

            _accommodationService = new AccommodationService();
            _accommodationService.Subscribe(this);

            AddAccommodationCommand = new RelayCommand(Execute_NavigateAddAccommodationPageCommand, CanExecute_Command);

            InitializeAccommodations();
        }

        private void InitializeAccommodations()
        {
            Accommodations = new ObservableCollection<Accommodation>();
            FormAccommodations();
        }

        private void FormAccommodations()
        {
            foreach (Accommodation accommodation in _accommodationService.GetByUser(LoggedInUser.Id))
            {
                Accommodations.Add(accommodation);
            }
        }

        public void Update()
        {
            Accommodations.Clear();
            FormAccommodations();
        }
    }
}
