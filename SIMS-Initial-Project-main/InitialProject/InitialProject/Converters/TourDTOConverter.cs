using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Converters
{
    public class TourDTOConverter
    {
        private readonly LocationService _locationService = new LocationService();
        private readonly UserService _userService = new UserService();

        public TourDTOConverter(LocationService locationService, UserService userService)
        {
            _locationService = locationService;
            _userService = userService;
        }

        public List<Guest2TourDTO> ConvertToDTOList(List<Tour> tours)
        {
            List<Guest2TourDTO> DTOList = new List<Guest2TourDTO>();

            foreach (Tour tour in tours)
            {
                DTOList.Add(ConvertToDTO(tour));
            }

            return DTOList;
        }

        public Guest2TourDTO ConvertToDTO(Tour tour)
        {
            return new Guest2TourDTO(
                tour.Id,
                tour.Name,
                _locationService.GetById(tour.LocationId).Country,
                _locationService.GetById(tour.LocationId).City,
                tour.Description,
                tour.Language,
                tour.MaxGuests,
                tour.CurrentGuestCount,
                tour.StartTime,
                tour.Duration,
                _userService.GetById(tour.GuideId).Username);
        }
    }
}
