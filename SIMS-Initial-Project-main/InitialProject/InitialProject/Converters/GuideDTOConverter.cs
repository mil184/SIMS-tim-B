using InitialProject.Model.DTO;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Repository;

namespace InitialProject.Converters
{
    public  class GuideDTOConverter
    {
        static public GuideTourDTO ConvertToDTO(Tour tour, LocationService locationService)
        {

            if (tour == null)
                return null;
            return new GuideTourDTO(
            tour.Id,
                    tour.Name,
                    locationService.GetById(tour.LocationId).Country,
                    locationService.GetById(tour.LocationId).City,
                    tour.StartTime,
                    tour.CurrentGuestCount);
        }
        static public List<GuideTourDTO> ConvertToDTO(List<Tour> tours, LocationService locationService)
        {
            List<GuideTourDTO> dtos = new List<GuideTourDTO>();
            foreach (Tour tour in tours) 
            {
                dtos.Add(ConvertToDTO(tour,locationService));
            }
            return dtos;
        }
        static public Tour ConvertToTour(GuideTourDTO dto, TourService tourService)
        {
            if (dto != null)
                return tourService.GetById(dto.Id);
            return null;
        }
        static public List<GuideRequestDTO> ConvertToDTO(List<TourRequest> requests, LocationService locationService)
        {
            List<GuideRequestDTO> dto = new List<GuideRequestDTO>();
            foreach (TourRequest request in requests)
            {
                dto.Add(ConvertToDTO(request, locationService));
            }
            return dto;
        }
        static public GuideRequestDTO ConvertToDTO(TourRequest request, LocationService locationService)
        {

            if (request == null)
                return null;

            return new GuideRequestDTO(
                    request.Id,
                    locationService.GetById(request.LocationId).City + ", " +
                    locationService.GetById(request.LocationId).Country,
                    request.Language,
                    request.MaxGuests.ToString(),
                    request.StartTime,
                    request.EndTime,
                    request.Description
                    );
        }
        static public TourRequest ConvertToRequest(GuideRequestDTO dto, TourRequestService tourRequestService)
        {
            if (dto != null)
                return tourRequestService.GetById(dto.Id);
            return null;
        }
        static public List<UserDTO> ConvertToDTO(List<User> users, TourReservationService tourReservationService, Tour tour, CheckpointService checkpointService)
        {
            List<UserDTO> dto = new List<UserDTO>();
            foreach (User user in users)
            {
                dto.Add(ConvertToDTO(user, tourReservationService, tour, checkpointService));
            }
            return dto;
        }
        static public UserDTO ConvertToDTO(User user, TourReservationService tourReservationService, Tour tour, CheckpointService checkpointService)
        {
            TourReservation reservation = tourReservationService.GetReservationByGuestIdAndTourId(user.Id, tour.Id);

            string currentCheckpoint = "Not Arrived Yet";

            if(reservation.CheckpointArrivalId != -1) 
            {
                currentCheckpoint = checkpointService.GetById(reservation.CheckpointArrivalId).Name;
            }

            return new UserDTO(user.Id, user.Username, currentCheckpoint);
        }
        static public User ConvertToUser(UserDTO dto, UserRepository userRepository)
        {
            if (dto != null)
                return userRepository.GetById(dto.UserId);
            return null;
        }
    }
}
