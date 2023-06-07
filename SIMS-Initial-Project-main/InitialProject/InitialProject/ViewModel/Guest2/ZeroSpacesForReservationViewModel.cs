using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using InitialProject.Converters;
using MenuNavigation.Commands;
using InitialProject.View.Guest2;

namespace InitialProject.ViewModel.Guest2
{
    public class ZeroSpacesForReservationViewModel
    {
        public Action CloseAction { get; set; }

        public Guest2TourDTO PreviousSelectedTour { get; set; }
        public Guest2TourDTO SelectedTour { get; set; }

        public List<Tour> ToursByCity;
        public List<Tour> ToursByCityWithoutSelected;

        public ObservableCollection<Guest2TourDTO> Tours { get; set; }

        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly UserService _userService;

        private readonly TourDTOConverter _tourDTOConverter;

        public RelayCommand ChangeLanguageCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand ShowMoreCommand { get; set; }

        public int LanguageButtonClickCount { get; set; }
        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";


        public ZeroSpacesForReservationViewModel(Guest2TourDTO selectedTour, TourService tourService, LocationService locationService, UserService userService, string lang)
        {
            PreviousSelectedTour = selectedTour;

            _tourService = tourService;
            _locationService = locationService;
            _userService = userService;

            _tourDTOConverter = new TourDTOConverter(_locationService, _userService);

            ToursByCity = _tourService.GetByCityName(selectedTour.City);
            ToursByCityWithoutSelected = _tourService.RemoveFromListById(ToursByCity, selectedTour.TourId);

            Tours = new ObservableCollection<Guest2TourDTO>(_tourDTOConverter.ConvertToDTOList(ToursByCity));

            ChangeLanguageCommand = new RelayCommand(Execute_ChangeLanguageCommand);
            ExitCommand = new RelayCommand(Execute_ExitCommand);
            ShowMoreCommand = new RelayCommand(Execute_ShowMoreCommand);

            app = (App)Application.Current;
            app.ChangeLanguage(lang);
            InitializeLanguageButton(lang);
        }

        public void InitializeLanguageButton(string lang)
        {
            if (lang == SRB)
            {
                LanguageButtonClickCount = 0;
                return;
            }

            LanguageButtonClickCount = 1;
        }

        public void Execute_ShowMoreCommand(object obj)
        {
            ShowTour showTour = new ShowTour(SelectedTour);
            showTour.Show();
        }

        private void Execute_ExitCommand(object obj)
        {
            CloseAction();
        }

        private void Execute_ChangeLanguageCommand(object obj)
        {
            LanguageButtonClickCount++;

            if (LanguageButtonClickCount % 2 == 1)
            {
                app.ChangeLanguage(ENG);
                return;
            }

            app.ChangeLanguage(SRB);
        }

        public void Update()
        {
            Tours.Clear();
            foreach (Tour tour in _tourService.GetAll())
            {
                Tours.Add(_tourDTOConverter.ConvertToDTO(tour));
            }
        }
    }
}
