using InitialProject.Commands;
using InitialProject.Converters;
using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.View.Guide;
using MenuNavigation.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InitialProject.ViewModel.Guide
{
    public class ShowComplexTourViewModel
    {
        private readonly ComplexTourService _complexTourService;
        private readonly TourRequestService _tourRequestService;
        private readonly TourService _tourService;
        private readonly ImageRepository _imageRepository;
        private readonly LocationService _locationService;
        private readonly CheckpointService _checkpointService;
        public ComplexTour ComplexTour { get; set; }
        public ObservableCollection<GuideRequestDTO> AvailableRequests { get; set; }
        public GuideRequestDTO SelectedAvailableRequestDTO { get; set; }
        public User LoggedInUser { get; set; }
        public ObservableCollection<DateSlot> DateSlots { get; set; }
        public DateSlot SelectedDateSlot { get; set; }
        public ShowComplexTourViewModel(ComplexTour complexTour, TourRequestService tourRequestService, LocationService locationService, ComplexTourService complexTourService, User user, CheckpointService checkpointService, ImageRepository imageRepository, TourService tourService)
        {
            _tourRequestService = tourRequestService;
            _locationService = locationService;
            _complexTourService = complexTourService;
            _checkpointService = checkpointService;
            _imageRepository = imageRepository;
            _tourService = tourService;

            ComplexTour = complexTour;
            LoggedInUser = user;

            AvailableRequests = new ObservableCollection<GuideRequestDTO>();

            foreach (int id in ComplexTour.AvailableTourRequestIds)
            {
                AvailableRequests.Add(GuideDTOConverter.ConvertToDTO(_tourRequestService.GetById(id), _locationService));
            }
            DateSlots = new ObservableCollection<DateSlot>();
            _checkpointService = checkpointService;

            InitializeCommands();
        }
        public void UpdateTimeSlots() 
        {
            if(SelectedAvailableRequestDTO != null) 
            {
                DateSlots.Clear();

                List<DateOnly> availableTimeSlots = _complexTourService.GetAvailableTimeSlots(LoggedInUser, ComplexTour, GuideDTOConverter.ConvertToRequest(SelectedAvailableRequestDTO, _tourRequestService));

                foreach(DateOnly time in availableTimeSlots) 
                {
                    DateSlot date = new DateSlot(time);
                    DateSlots.Add(date);
                }
            }
        } 
        public void CreateTour() 
        {
            TourRequest selectedRequest = GuideDTOConverter.ConvertToRequest(SelectedAvailableRequestDTO, _tourRequestService);

            CreateTourViewModel createTourViewModel = new CreateTourViewModel(LoggedInUser, _tourService, _locationService, _imageRepository, _checkpointService, _tourRequestService, selectedRequest, ComplexTour, _complexTourService, SelectedDateSlot.Date, CancelCommand);
            CreateTourWindow createTour = new CreateTourWindow(createTourViewModel);
            createTour.ShowDialog();
        }

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand CreateTourCommand { get; private set; }

        public void InitializeCommands()
        {
            CreateTourCommand = new RelayCommand(CreateTour);
        }
        private void CreateTour(object parameter)
        {
            MessageBox.Show("Hello");
            CreateTour();
        }
    }
}
